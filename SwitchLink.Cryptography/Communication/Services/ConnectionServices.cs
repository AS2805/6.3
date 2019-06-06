using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Hik.Communication.Scs.Client;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Communication.Messages;
using log4net;
using SwitchLink.Cryptography.Communication.Messages;
using SwitchLink.Cryptography.Communication.Protocols;

namespace SwitchLink.Cryptography.Communication.Services
{
    public class ConnectionServices : IDisposable
    {
        private readonly ILog _log = LogManager.GetLogger("HSMConnectionLogger");
        private IScsClient _tcpClient;
        private TaskCompletionSource<byte[]> _tcsHsm;
        private Timer _responseTimer;
        private int _responseTimerPeriod = 3000;

        public void Connect(string ip, int port)
        {
            _tcpClient = ScsClientFactory.CreateClient(new ScsTcpEndPoint(ip, port));
            _tcpClient.WireProtocol = new HsmProtocol();

            _tcpClient.MessageReceived -= OnMessageReceived;

            _tcpClient.MessageReceived += OnMessageReceived;
            _tcpClient.Connect();
        }

        internal string SendCommand(string message)
        {
            //  logger.Info("Message to be sent to the HSM : " + message);
            byte[] msg = Encoding.UTF8.GetBytes("HEAD" + message);
            byte[] len = BitConverter.GetBytes((short)msg.Length);
            byte[] constMsg = len.Reverse().Concat(msg).ToArray();

            byte[] response = Send(constMsg).Result;
            string result = Encoding.ASCII.GetString(response);

            return result;
        }

        private Task<byte[]> Send(byte[] message)
        {
            _tcsHsm = new TaskCompletionSource<byte[]>();
            if (_responseTimer != null)
                DisposeResponsetimer();
            _responseTimer = new Timer(_responseTimerPeriod) {AutoReset = false};
            _responseTimer.Elapsed += (sender, args) =>
            {
                var log = string.Format("Timeout => HSM not responding request in {0} sec.", TimeSpan.FromMilliseconds(_responseTimerPeriod).TotalSeconds);
                DisposeResponsetimer();
                _tcsHsm.SetException(new TimeoutException(log));
                _log.Error(log);
            };

            _tcpClient.SendMessage(new HsmRawMessage(message));

            return _tcsHsm.Task;
        }

        private void DisposeResponsetimer()
        {
            if (_responseTimer != null)
            {
                _responseTimer.Stop();
                _responseTimer.Close();
                _responseTimer.Dispose();
                _responseTimer = null;
            }
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            var response = e.Message as HsmRawMessage;
            if (response != null)
                _tcsHsm.SetResult(response.RawBytes);
            else
                _tcsHsm.SetException(new InvalidDataException("NULL responsed from HSM"));
        }

        public void Dispose()
        {
            DisposeResponsetimer();
            _tcpClient.Disconnect();
        }
    }
}
