using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Common.Logging;
using Hik.Communication.Scs.Communication.Messages;

namespace SwitchLink.HostNode.Services
{
    public interface IBankingCommunicationService_bk
    {
        void Connect();
        byte[] SendCommand(byte[] bytes);
        void Close();
        /// <summary>
        /// This event is raised when a new message is received.
        /// </summary>
        event EventHandler<MessageEventArgs> MessageReceived;
        event EventHandler OnConnected;
        event EventHandler OnDisconnected;
    }

    public class BankingCommunicationService_bk : IBankingCommunicationService_bk, IDisposable
    {
        private static TcpClient _tcpClient;
        private static readonly Object Mylock = new Object();
        private readonly ILog _log = LogManager.GetLogger<BankingCommunicationService_bk>();
        //private string _ip = "10.125.254.102";
        private string _ip = "127.0.0.1";
        private int _bankPort = 7000;

        public event EventHandler<MessageEventArgs> MessageReceived;
        public event EventHandler OnConnected;
        public event EventHandler OnDisconnected;

        public void Connect()
        {
            lock (Mylock)
            {
                _tcpClient = new TcpClient(AddressFamily.InterNetwork);
                Task.Run(() => ConnectAsync());
            }
        }

        private async Task ConnectAsync()
        {
            bool bClosed = false;
            while (true)
            {
                if (_tcpClient.Client.Poll(0, SelectMode.SelectRead))
                {
                    // Client disconnected
                    bClosed = true;
                    if (OnDisconnected != null)
                        OnDisconnected(this, new EventArgs());
                }

                if (!_tcpClient.Connected || bClosed)
                {
                    _log.Info("Connecting to bank...");

                    try
                    {
                        _tcpClient = new TcpClient(AddressFamily.InterNetwork);
                        await _tcpClient.ConnectAsync(IPAddress.Parse(_ip), Convert.ToInt16(_bankPort));
                        await Task.Delay(TimeSpan.FromSeconds(2));
                        _log.Info(string.Format("Created Bank Connection: ip:{0} port:{1}", _ip, _bankPort));
                        bClosed = false;
                        if (OnConnected != null)
                            OnConnected(this, new EventArgs());
                    }
                    catch (SocketException e)
                    {
                        _log.Error(e);
                    }
                }
                else
                {
                    //_log.Info("Already Connected");
                }
            }
        }

        public byte[] SendCommand(byte[] bytes)
        {
            lock (Mylock)
            {
                byte[] fullServerReply = null;
                int retry = 10;
                try
                {
                    NetworkStream nwStream = _tcpClient.GetStream();
                    while (retry --> 0)
                    {
                        if (nwStream.CanWrite)
                        {
                            nwStream.Write(bytes, 0, bytes.Length);
                            fullServerReply = ReadAsync(_tcpClient);
                            break;
                        }
                    }
                }
                catch (SocketException e)
                {
                    _log.Error(e);
                }
                //string ascii = Encoding.UTF8.GetString(fullServerReply);
                return fullServerReply;
            }
        }
        
        private byte[] ReadAsync(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            ns.ReadTimeout = 2000;
            //byte[] readBuffer = new byte[client.ReceiveBufferSize];
            byte[] readHeaderBuffer = new byte[2];
            byte[] msReply;
            while (true)
            {
                if (ns.CanRead)
                {
                    int dataSize;
                    using (var headerWriter = new MemoryStream())
                    {
                        int numberOfBytesRead = ns.Read(readHeaderBuffer, 0, readHeaderBuffer.Length);
                        headerWriter.Write(readHeaderBuffer, 0, numberOfBytesRead);
                        byte[] header = headerWriter.ToArray();
                        dataSize = BitConverter.ToInt16(header.Reverse().ToArray(), 0);
                    }

                    using (var detailWriter = new MemoryStream())
                    {
                        byte[] readBuffer = new byte[dataSize];
                        int bytesRead = ns.Read(readBuffer, 0, readBuffer.Length);
                        detailWriter.Write(readBuffer, 0, bytesRead);
                        msReply = detailWriter.ToArray();
                    }
                    //do
                    //{
                    //    int numberOfBytesRead = ns.Read(readBuffer, 0, readBuffer.Length);
                    //    if (numberOfBytesRead <= 0)
                    //    {
                    //        break;
                    //    }
                    //    writer.Write(readBuffer, 0, numberOfBytesRead);
                    //} while (ns.DataAvailable);
                    ////fullServerReply = Encoding.UTF8.GetString(writer.ToArray());
                    break;
                }
            }
            if (MessageReceived != null)
            {
                MessageEventArgs args = new MessageEventArgs(new ScsRawDataMessage(msReply));
                MessageReceived(this, args);
            }
            return msReply;
        }

        public void Close()
        {
            lock (Mylock)
            {
                try
                {
                    _tcpClient.Close();
                    _tcpClient = new TcpClient(AddressFamily.InterNetwork);
                    _log.Warn(string.Format("Closed Bank Connection: ip:{0} port:{1}", _ip, _bankPort));

                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }
        }

        public void Dispose()
        {
            
        }
    }
}
