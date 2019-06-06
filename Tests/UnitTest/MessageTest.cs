using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwitchLink.ProtocalFactory.Helper;

namespace UnitTest
{
    /// <summary>
    /// Summary description for MessageTest
    /// </summary>
    [TestClass]
    public class MessageTest
    {
        private AsyncCallback _pfnCallBack;
        private Socket _clientSocket;
        private int _stage = 0;
        private readonly MsgHelper _baseMessageHelper = new MsgHelper();

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

        private Socket Connection()
        {
            var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect((EndPoint)new IPEndPoint(IPAddress.Parse(GetIp()), (int)Convert.ToInt16(8000)));
            
            return clientSocket;
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMessageHelper()
        {
            const string str = "<STX>00000000td2W0       <FS>S9218163       <FS>11<FS>4892<FS>4089670000392726=17112011000017980000<FS>00008900<FS>00000200<FS>76728398F76ED27D<FS><FS><FS>VA6.00.12WV02.70.10 V06.01.12 0  0T  00 100     00000302K0288000000002K028800000000000000000000000000000000000000<FS><FS><FS><ETX>";
            byte[] rawMessageBytes = _baseMessageHelper.BuildMessageBytes(str); // Bulid byte



            string[] results = _baseMessageHelper.BuildMsgFromAtm(rawMessageBytes);

            var t1 = results[0].Substring(0, 8);
            var t2 = results[0].Substring(8, 2);
            var t3 = results[0].Substring(10, 2);
            var t4 = results[0].Substring(12, 1);
            var t5 = results[0].Substring(13, 7);
        }

        [TestMethod]
        public void BuildResponse()
        {
            var InformationHeader = "            ";
            var  _encryptedPinKey = "5D54825F2FD76C53";

            //9U00009D       60~5D54825F2FD76C53{B0CF814842FDC9C5mFA85F143D984EBA6}1336264E03A394ADV
           
            string builder = "";

            builder += "<STX>";
            builder += InformationHeader;
            builder += "<FS>";
            builder += "9U00009D       ";
            builder += "<FS>";
            builder += "60";
            builder += "<FS>";
            builder += "~"; // field Id Code
            builder += "<FS>";
            builder += _encryptedPinKey;
            builder += "<FS>";
            builder += "!"; // field Id Code
            builder += "<FS>";
            builder += "00000200"; // field Id Code
            builder += "<FS>";
            builder += ""; // field Id Code
            builder += "<FS>";
            builder += "<ETX>";

            byte[] decode = _baseMessageHelper.BuildMessageBytes(builder.ToString());

        }



        [TestMethod]
        public void TestSendConfigMessage()
        {
            try
            {
                if (_pfnCallBack == null)
                    _pfnCallBack = OnDataReceived;
                _clientSocket = Connection();
                SocketPacket socketPacket = new SocketPacket();
                socketPacket.thisSocket = Connection();
               _clientSocket.BeginReceive(socketPacket.dataBuffer, 0, socketPacket.dataBuffer.Length, SocketFlags.None, _pfnCallBack, socketPacket);
                _stage = 1;
            }
            catch (SocketException ex)
            {
            }
        }

        private void OnDataReceived(IAsyncResult asyn)
        {
            SocketPacket socketPacket = (SocketPacket)asyn.AsyncState;
            //int byteCount = socketPacket.thisSocket.EndReceive(asyn);
            var byteArray = socketPacket.dataBuffer.Where(x => x != 0x00).ToArray();

            if (_stage == 1)
            {
                var assert = new byte[] {0x05};
                Assert.AreEqual(assert[0], byteArray[0]);
                const string str = "<STX>000000  td3W0       <FS>9V00000000     <FS>60<FS>VA5.00.03WV02.70.10 V04.00.19 0  0T  00 000     00000002K0111000100005K000002220060000000000000000000000000000000<FS><FS><ETX>";
                byte[] rawMessageBytes = _baseMessageHelper.BuildMessageBytes(str); // Bulid byte

                _clientSocket.Send(rawMessageBytes);
                _stage = 2;
            }
            else if (_stage == 2)
            {
                var assert = new byte[] { 0x02 };
                Assert.AreEqual(assert, byteArray[0]);
                _clientSocket.Send(new byte[] { 0x06 });
                _stage = 3;
            }
            else if (_stage == 3)
            {
                var assert = new byte[] { 0x04 };
                Assert.AreEqual(assert, byteArray[0]);
                _clientSocket.Send(new byte[] { 0x04 });
                _stage = 4;
            }


        }



        
    }
    public class SocketPacket
    {
        public byte[] dataBuffer = new byte[1024];
        public Socket thisSocket;
    }
}
