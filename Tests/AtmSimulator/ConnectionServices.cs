using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using AtmSimulator;
using Common.Logging;
using Hik.Communication.Scs.Client;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Communication.Messages;
using UnitTest.AS2805.Helper;
using UnitTest.Helper;

namespace AtmSimulatorWinForm
{
    public class ConnectionServices
    {
        private IScsClient _tcpClient;
        private int _clientId;
        private readonly ILog _log = LogManager.GetLogger<ConnectionServices>();
        private readonly AtmSimulator _atmSimulator;
        private readonly BaseMessageHelper _baseMessageHelper = new BaseMessageHelper();
        readonly AtmRequestHelper _atmRequestHelper = new AtmRequestHelper();
        private readonly Stopwatch _stopWatch;
        private readonly Object _syncRoot = new object();

        public ConnectionServices()
        {
            _stopWatch = new Stopwatch();
        }

        public ConnectionServices(AtmSimulator mainForm) : this()
        {
            _atmSimulator = mainForm;
        }

        public void Connect(int clientId, string ip, int port)
        {
            _clientId = clientId;
            _tcpClient = ScsClientFactory.CreateClient(new ScsTcpEndPoint(ip, port));
            _tcpClient.WireProtocol = new AtmProtocol();
            _tcpClient.MessageReceived -= OnMessageReceived;
            _tcpClient.MessageReceived += OnMessageReceived;
            _tcpClient.Connect();
        }

        public void Send(Byte[] bytes)
        {
            UpdateResults(String.Format("Atm Id : {0} Sent", _clientId));
           
            _tcpClient.SendMessage(new AtmRawMessage(bytes));
            _stopWatch.Start();
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            var msg = e.Message as AtmRawMessage;
            string encode = _baseMessageHelper.AsciiOctets2String(msg.RawBytes);
            if (encode.Contains("<STX>"))
            {
                _stopWatch.Stop();
                UpdateResults(String.Format("Atm Id : {0} Received reponse from Bank, Time elapsed: {1}", _clientId, _stopWatch.Elapsed));
                if (new Random().Next(10) > 5) // test reversal
                {
                    UpdateResults(String.Format("Atm Id : {0} : Responsed ACK", _clientId));
                    _tcpClient.SendMessage(new AtmRawMessage(new[] {_atmRequestHelper.Ack}));
                }
                else
                {
                    UpdateResults(String.Format("Atm Id : {0} : Not Responsed ACK", _clientId));
                }
                  
            }
            else if (encode.Contains("<EOT>"))
            {
                _tcpClient.SendMessage(new AtmRawMessage(new[] { _atmRequestHelper.Eot }));
            }
        }

        private void UpdateResults(string encode)
        {
           
           _log.Debug(encode);

            if (_atmSimulator != null)
            {
                if (_atmSimulator.richTextResults.InvokeRequired)
                    _atmSimulator.richTextResults.Invoke(new MethodInvoker(delegate { _atmSimulator.richTextResults.Text = _atmSimulator.richTextResults.Text + encode + "\n"; }));
                else
                    _atmSimulator.richTextResults.Text = _atmSimulator.richTextResults.Text + encode + "\n";

            }
        }
       
    }
}
