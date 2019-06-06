using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common.Logging;

namespace SwitchLink.Switch.SocketServer
{
    public interface IAsynchronousSocketListener
    {
        void StartListening(IPAddress ipAddress = null, int port = 8000);
        void CloseSockets();
    }

    class AsynchronousSocketListener_bkp : IAsynchronousSocketListener
    {
        private readonly ILog _log = LogManager.GetLogger<AsynchronousSocketListener_bkp>();
        private Socket _mainSocket;
        private string _clientId = "";
        private readonly ConcurrentDictionary<string, Socket> _workerSocketList = new ConcurrentDictionary<string, Socket>();
        private AsyncCallback _pfnWorkerCallBack;

        public void StartListening(IPAddress ipAddress, int port)
        {
            if (ipAddress == null)
            {
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                ipAddress = ipHostInfo.AddressList[0];
            }
            // Create a TCP/IP socket.
            _mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _mainSocket.Bind(new IPEndPoint(ipAddress, port));
            _mainSocket.Listen(100);
            _mainSocket.BeginAccept(OnClientConnect, null);
            _log.Info(String.Format("server listening at: {0}", ipAddress));
            _log.Info("Switchlink.Switch.Service Running...");
        }

        void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                _log.Info("active connections: " + _workerSocketList.Count);
                Socket soc = _mainSocket.EndAccept(asyn);
               
                //Interlocked.Increment(ref _clientCount);             
                lock (new object())
                {
                    _clientId = Guid.NewGuid().ToString();
                }   
                _workerSocketList.AddOrUpdate(_clientId, soc, (s, socket) => { return null; });
                ResponseMsgToClient("Welcome client " + _clientId + "\n This is response message from server", _clientId);
                WaitForData(soc, _clientId);
                _mainSocket.BeginAccept(OnClientConnect, null);
               
            }
            catch (ObjectDisposedException ex)
            {
                Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
                _log.Error(String.Format("ObjectDisposedException - name:{0} msg:{1}", ex.ObjectName, ex.Message));
            }
            catch (SocketException ex)
            {
                Debugger.Log(0, "SocketException", "\n " + ex.Message + "\n");
                _log.Error(String.Format("SocketException - code:{0} msg:{1} - {2}", ex.ErrorCode, ex.Message, ex.SocketErrorCode));
            }
        }

        public void WaitForData(Socket soc, string clientNumber)
        {
            try
            {
                if (_pfnWorkerCallBack == null)
                    _pfnWorkerCallBack = OnDataReceived;
                SocketPacket socketPacket = new SocketPacket(soc, clientNumber);
                soc.BeginReceive(socketPacket.DataBuffer, 0, socketPacket.DataBuffer.Length, SocketFlags.None, _pfnWorkerCallBack, socketPacket);
            }
            catch (SocketException ex)
            {
                Debugger.Log(0, "SocketException", "\n " + ex.Message + "\n");
                _log.Error(String.Format("SocketException - code:{0} msg:{1} - {2}", ex.ErrorCode, ex.Message, ex.SocketErrorCode));

            }
           
        }

        public void OnDataReceived(IAsyncResult asyn)
        {
            SocketPacket socketPacket = (SocketPacket)asyn.AsyncState;
            try
            {
                int byteCount = socketPacket.CurrentSocket.EndReceive(asyn);
                char[] chars = new char[byteCount + 1];
                Encoding.UTF8.GetDecoder().GetChars(socketPacket.DataBuffer, 0, byteCount, chars, 0);
                string str = new string(chars);

                UpdateServerLog(socketPacket.ClientNumber + ":" + str);

                byte[] bytes = Encoding.ASCII.GetBytes("Server Reply:" + str.ToUpper());
                socketPacket.CurrentSocket.Send(bytes);
                WaitForData(socketPacket.CurrentSocket, socketPacket.ClientNumber);
            }
            catch (ObjectDisposedException ex)
            {
                Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
                _log.Error(String.Format("ObjectDisposedException - name:{0} msg:{1}", ex.ObjectName, ex.Message));
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode == 10054)
                {
                    UpdateServerLog("Client " + socketPacket.ClientNumber + " Disconnected\n");
                    Socket soc;
                    _workerSocketList.TryRemove(socketPacket.ClientNumber, out soc);
                }
                else
                {
                    Debugger.Log(0, "SocketException", "\n " + ex.Message + "\n");
                    _log.Error(String.Format("SocketException - code:{0} msg:{1} - {2}", ex.ErrorCode, ex.Message, ex.SocketErrorCode));
                }
            }
        }

        private void UpdateServerLog(string s)
        {
            _log.Info("active connections: " + _workerSocketList.Count);
            _log.Info(s);
        }

        private void ResponseMsgToClient(string msg, string clientNumber)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            Socket client;
            _workerSocketList.TryGetValue(clientNumber,  out client);     
            client.Send(bytes);           
        }

        public void CloseSockets()
        {
            if (_mainSocket != null)
            {
                _mainSocket.Disconnect(true);
                _mainSocket.Close();
                _mainSocket.Dispose();
            }
            for (int index = 0; index < _workerSocketList.Count; ++index)
            {
                Socket workerSocket = _workerSocketList.ElementAt(index).Value;
                if (workerSocket != null)
                {
                    workerSocket.Disconnect(true);
                    workerSocket.Close();
                    workerSocket.Dispose();
                }
            }
        }
    }

    public class SocketPacket
    {
        //public byte[] DataBuffer = new byte[1024];
        public byte[] DataBuffer = new byte[4096];
        private readonly Socket _currentSocket;
        private readonly string _clientNumber;

        public SocketPacket(Socket socket, string clientNumber)
        {
            _currentSocket = socket;
            _clientNumber = clientNumber;
        }

        public Socket CurrentSocket
        {
            get { return _currentSocket; }
        }

        public string ClientNumber
        {
            get { return _clientNumber; }
        }
    }
}
