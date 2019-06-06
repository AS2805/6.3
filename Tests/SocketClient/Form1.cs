using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using AtmSimulatorWinForm.Builder;
using UnitTest.AS2805.Helper;
using UnitTest.Helper;

namespace AtmSocketClient
{
    public partial class Form1 : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                // Activate double buffering at the form level.  All child controls will be double buffered as well.
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                return cp;
            }
        }

        private readonly BaseMessageHelper _baseMessageHelper = new BaseMessageHelper();
        private Socket _clientSocket;
        private AsyncCallback _pfnCallBack;
        readonly AtmRequestHelper _atmRequestHelper = new AtmRequestHelper();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxIP.Text = GetIp();
            radioHeaderLength.Checked = true;
            radioHexFormat.Checked = true;
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (_clientSocket == null)
                return;
            _clientSocket.Close();
            _clientSocket = null;
            UpdateControls(false);
        }

        private void UpdateControls(bool connected)
        {
            buttonConnect.Enabled = !connected;
            buttonDisconnect.Enabled = connected;
            textBoxConnectStatus.Text = connected ? "Connected" : "Not Connected";
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (_clientSocket != null)
            {
                _clientSocket.Close();
                _clientSocket = null;
            }
            Close();
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

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (textBoxIP.Text != "")
            {
                if (textBoxPort.Text != "")
                {
                    try
                    {
                        UpdateControls(false);
                        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        _clientSocket.Connect((EndPoint)new IPEndPoint(IPAddress.Parse(this.textBoxIP.Text), (int)Convert.ToInt16(this.textBoxPort.Text)));
                        if (!_clientSocket.Connected)
                            return;
                        UpdateControls(true);
                        WaitForData();
                        return;
                    }
                    catch (SocketException ex)
                    {
                        int num = (int)MessageBox.Show("\nConnection failed, is the server running?\n" + ex.Message);
                        UpdateControls(false);
                        return;
                    }
                }
            }
            int num1 = (int)MessageBox.Show("IP Address and Port Number are required to connect to the Server\n");
        }

        private void WaitForData()
        {
            try
            {
                if (_pfnCallBack == null)
                    _pfnCallBack = OnDataReceived;
                SocketPacket socketPacket = new SocketPacket();
                socketPacket.thisSocket = _clientSocket;
                try
                {
                    var _result = _clientSocket.BeginReceive(socketPacket.dataBuffer, 0, socketPacket.dataBuffer.Length, SocketFlags.None, _pfnCallBack, socketPacket);
                }
                catch (Exception)
                {
                }
                
            }
            catch (SocketException ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                SocketPacket socketPacket = (SocketPacket)asyn.AsyncState;
                int byteCount = socketPacket.thisSocket.EndReceive(asyn);
                //char[] chars = new char[byteCount + 1];

                //Encoding.UTF8.GetDecoder().GetChars(socketPacket.dataBuffer, 0, byteCount, chars, 0);
                //var data = Encoding.ASCII.GetString(socketPacket.dataBuffer, 0, byteCount+1);
                var byteArray = socketPacket.dataBuffer.Where(x => x != 0x00).ToArray();
                string encode = _baseMessageHelper.AsciiOctets2String(byteArray);

                if (richTextRxMessage.InvokeRequired)
                    richTextRxMessage.Invoke(new MethodInvoker(delegate { richTextRxMessage.Text = richTextRxMessage.Text + encode + "\n"; }));
                else
                    richTextRxMessage.Text = richTextRxMessage.Text + encode + "\n";

             
                WaitForData();
            }
            catch (ObjectDisposedException ex)
            {
                Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            richTextRxMessage.Clear();
        }

        private void buttonSendMessage_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (!radioHexFormat.Checked)
                {
                    string text = richTextTxMessage.Text;
                    StreamWriter streamWriter = new StreamWriter(new NetworkStream(_clientSocket));
                    streamWriter.WriteLine(text);
                    streamWriter.Flush();
                }
                else
                {
                    byte[] b = {};
                    if (radioHeaderLength.Checked)
                    {
                        b = _baseMessageHelper.BuildMessageWithHeaderLength(richTextTxMessage.Text);
                    }
                    else
                    {
                        b = _baseMessageHelper.StringToBytes(richTextTxMessage.Text);
                    }
                    BinaryWriter writer = new BinaryWriter(new NetworkStream(_clientSocket));
                    writer.Write(b);
                    writer.Flush();
                }
                
            }
            catch (SocketException ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void btnStx_Click(object sender, EventArgs e)
        {
            //ClientMessageBuilder builder = new ClientMessageBuilder("S9111111");
            //string str = builder.GetRequestMsg();
            const string str = "<STX>PILOT   td8D2       <FS>SM200004       <FS>11<FS>1682<FS>4902370000002348=121210111234123<FS>00003000<FS>00000250<FS>EECCBD2A57B822E7<FS><FS><FS>VA6.00.18DV02.70.10 V06.01.12 0  0T  00 000     03300002K1571003200005K181800010000000000000000000000000000000000<FS>ub093A<FS><ETX>V"; // transaction
            //const string str = "<STX>PILOT   td8D2       <FS>S9111111       <FS>60<FS>VA6.00.18DV02.70.10 V06.01.12 0  0T  00 000     D0000002K2000000000005K200000000000000000000000000000000000000000<FS>ub25AF<FS><ETX><US>"; // config 9V00000000
            //const string str = "<STX>00000000td2W0       <FS>S9218163       <FS>51<FS>HostTotals<FS>111122223333444444444444<FS><FS><ETX> "; // Host Totals
            //const string str = "<STX>PILOT   td8D2       <FS>S9111111       <FS>29<FS>0151<FS>4902370000002348=121210111234123<FS>00005000<FS>00000250<FS>00000000<FS>VA6.00.18DV02.70.10 V06.01.12 0  0T  00 000     01800002K1561005100005K157500010000000000000000000000000000000000<FS>ub93DC<FS><ETX>w";
            //const string str = "<STX>PILOT   td8D2       <FS>S9111111       <FS>50<FS>VA6.00.18DV02.70.10 V06.01.12 0  0T  00 000     00000002K0060000200005K007500000000000000000000000000000000000000<FS>002200030000000000028000<FS>ub320B<FS><ETX><BS>"; // host total
            
            //byte[] msg = Encoding.ASCII.GetBytes("\u0002033w130,02,1,0000003444,0000000000,18000810709859900000,\u0003G"); 
            //_clientSocket.Send(msg);
            byte[] rawMessageBytes = _atmRequestHelper.BuildMessageBytesForAtmRequest(str); // Bulid byte
            _clientSocket.Send(rawMessageBytes);
        }

        private void btnAck_Click(object sender, EventArgs e)
        {
            var data = new byte[] { 06 };
            _clientSocket.Send(data);
        }

        private void btnEot_Click(object sender, EventArgs e)
        {
            var data = new byte[] { 04 };
            _clientSocket.Send(data);
        }
    }

    public class SocketPacket
    {
        public byte[] dataBuffer = new byte[1024];
        public Socket thisSocket;
    }
}
