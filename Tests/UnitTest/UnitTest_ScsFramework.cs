using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Hik.Communication.Scs.Client;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwitchLink.HostNode.Communication.Messages;
using SwitchLink.HostNode.Communication.Protocols;
using SwitchLink.ProtocalFactory.AS2805;

namespace UnitTest
{
    [TestClass]
    public class UnitTest_ScsFramework
    {
        private string _bankIp = "127.0.0.1";
        private int _bankPort = 8000;
        private readonly As2805Extensions _helper = new As2805Extensions();
        [TestMethod]
        public void TestWithScsSentMultipleMessage()
        {
            using (var tcpClient = ScsClientFactory.CreateClient(new ScsTcpEndPoint(_bankIp, _bankPort)))
            {
                //bool sent;
                tcpClient.WireProtocol = new HostProtocol();
                tcpClient.Connect();
                //tcpClient.MessageSent += (sender, args) =>
                //{
                //    sent = true;
                //};

                for (int i = 0; i < 10; i++)
                {
                    //sent = false;
                    Thread.Sleep(10);
                    Console.WriteLine("Start sending : " + i);
                    string b = "08008220000080000000040000001000000001041018080084380861100016030106579944";
                    Byte[] result = _helper.GetBytesWithHeaderLength(b);
                    tcpClient.SendMessage(new HostRawMessage(result));
                    Console.WriteLine("Sent : " + i);

                    //while (!sent) { }
                }

            }
        }

        [TestMethod]
        public void TestWithNormalSocketSentMultipleMessage()
        {
            using (var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                clientSocket.Connect((EndPoint)new IPEndPoint(IPAddress.Parse(_bankIp), _bankPort));
                for (int i = 0; i < 10; i++)
                {
                   Thread.Sleep(10);
                    Console.WriteLine("Start sending : " + i);
                    string b = "08008220000080000000040000001000000001041018080084380861100016030106579944";
                    Byte[] result = _helper.GetBytesWithHeaderLength(b);
                    
                    using(NetworkStream stream = new NetworkStream(clientSocket))
                    {
                        stream.Write(result, 0, result.Length);    
                        stream.Flush();
                    }
                    
                    
                    
                    
                    Console.WriteLine("Sent : " + i);

                }

                //Thread.Sleep(100);
            }
            
        }
    }
}
