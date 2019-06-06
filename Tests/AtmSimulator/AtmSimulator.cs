using System;
using System.Net;
using System.Windows.Forms;
using AtmSimulatorWinForm.Builder;
using Common.Logging;
using UnitTest.Helper;

namespace AtmSimulatorWinForm
{
    public partial class AtmSimulator : Form
    {
        readonly AtmRequestHelper _helper = new AtmRequestHelper();
        private readonly ILog _log = LogManager.GetLogger<AtmSimulator>();
        private Timer _timer;
        private string _atmId;
 
        public AtmSimulator()
        {
            InitializeComponent();
            _log.Debug("Start Atm Simulator");
            textBoxIP.Text = GetIp();
        }

        private int _i = 0;

        private void btnStart_Click(object sender, EventArgs e)
        {
            richTextResults.Text = @"Start Simulator";
            _atmId = textTerminalId.Text.Trim();

            // set init ATM remaining
            AtmRequestHelper.CasA = 1253;
            AtmRequestHelper.CasB = 1500;

            int inter = int.Parse(interval.Text);

            if (textBoxIP.Text != "" && textBoxPort.Text != "" && textAtm.Text != "" && _atmId != "")
            {
                var rnd=new Random();
                richTextResults.Text = "";
                if (_timer != null)
                    DisposeTimer();
                _timer = new Timer { Interval = inter };
                _timer.Tick += (o, args) =>
                {
                    if(rnd.Next(100)<30)
                        return;
                    
                    int atm = int.Parse(textAtm.Text);
                    for (int i = 1; i <= atm; i++)
                    {

                        var builder = new ClientMessageBuilder(_atmId);
                        byte[] rawMessageBytes = _helper.BuildMessageBytesForAtmRequest(builder.GetRequestMsg());
                      
                        ConnectionServices svc = new ConnectionServices(this);
                        svc.Connect(i, textBoxIP.Text, int.Parse(textBoxPort.Text));
                        svc.Send(rawMessageBytes);
                    }
                    if (_i == 10)
                    {
                        richTextResults.Text = "";
                        _i = 0;
                    }

                    _i++;
                };
                _timer.Start();
            }
            else
            {
                MessageBox.Show(@"Invalid values");
            }
        }

        private void DisposeTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }

        private string GetIp()
        {
            IPHostEntry hostByName = Dns.GetHostByName(Dns.GetHostName());
            string str = "";
            IPAddress[] addressList = hostByName.AddressList;
            int index = 0;
            if (index < addressList.Length)
                return addressList[index].ToString();
            return str;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            richTextResults.Text = "";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            var result = "Stop by user" + DateTime.Now + "/n" + richTextResults.Text;
            richTextResults.Text = result;
            DisposeTimer();
        }
    }
}
