using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Common.Logging;
using Hik.Communication.Scs.Client;
using Hik.Communication.Scs.Communication;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Communication.Messages;
using SwitchLink.HostNode.Communication.Messages;
using SwitchLink.HostNode.Communication.Protocols;
using SwitchLink.HostNode.Communication.Services;
using SwitchLink.HostNode.Communication.States;
using SwitchLink.HostNode.Configuration;
using SwitchLink.HostNode.Services;
using SwitchLink.ProtocalFactory.HostNode.Models;
using Timer = System.Timers.Timer;

namespace SwitchLink.HostNode.Communication.Controller
{
    public interface IBankingCommunicationController
    {
        void CreateConnetionToPartner();
        Task<BaseModel> SendCommand(byte[] bytes, int stan);
        void Close();
    }

    public class BankingCommunicationController : IBankingCommunicationController
    {
        private Timer _signOntimer;
        private int _signOnPeriod = 600000;

        private Timer _transactionTimer;
        private int _transactionTimerPeriod = 6000;

        private IScsClient _tcpClient;

        private readonly Object _receivedLock = new Object();
        private readonly Object _sentLock = new Object();
        private readonly Object _connectionLock = new Object();
        private readonly Object _myLock = new Object();
        private readonly Object _logonLock = new Object();

        private readonly ILog _log = LogManager.GetLogger<BankingCommunicationController>();
        private int _msgRefNo;
        private TaskCompletionSource<BaseModel> _res;
        private readonly ILoggerService _svrLog;

        private readonly IBankingCoreService _coreSvc;

        public BankingCommunicationController()
        {
            _coreSvc = new BankingCoreService();
            _svrLog = new LoggerService();
        }

        public void CreateConnetionToPartner()
        {
            lock (_connectionLock)
            {
                Task.Run(() => ConnectAsync());
            }
        }

        private void ConnectAsync()
        {
            while (true)
            {
                try
                {
                    if (_tcpClient == null)
                    {
                        _tcpClient = ScsClientFactory.CreateClient(new ScsTcpEndPoint(Config.PartnerIpAddress, Config.PartnerPort));
                        _tcpClient.WireProtocol = new HostProtocol();
                    }
                    if (_tcpClient.CommunicationState == CommunicationStates.Disconnected)
                    {
                        _coreSvc.Disconnect();
                        _log.Warn("Connecting to Partner...");
                        _tcpClient.Connected -= Connected;
                        _tcpClient.Disconnected -= Disconnected;
                        _tcpClient.MessageReceived -= OnMessageReceived;
                        _tcpClient.MessageSent -= MessageSent;

                        _tcpClient.Connected += Connected;
                        _tcpClient.Disconnected += Disconnected;
                        _tcpClient.MessageReceived += OnMessageReceived;
                        _tcpClient.MessageSent += MessageSent;
                        _tcpClient.Connect();
                    }
                    else if (_tcpClient.CommunicationState == CommunicationStates.Connected)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                    Close();
                }
            }
        }

        public Task<BaseModel> SendCommand(byte[] bytes, int stan)
        {
            lock (_sentLock)
            {
                if (_coreSvc.CurrentState == HostState.SignedOn)
                {
                    _msgRefNo = stan;
                    _res = new TaskCompletionSource<BaseModel>();
                    _tcpClient.SendMessage(new HostRawMessage(bytes));
                    _coreSvc.BeginTransaction();

                    if (_transactionTimer != null)
                        DisposeTransactionOntimer();
                    _transactionTimer = new Timer(_transactionTimerPeriod) { AutoReset = false };
                    _transactionTimer.Elapsed += (sender, args) =>
                    {
                        var msg = string.Format("Transaction ref:{0} timeout => Partner not responding request in {1} sec.", _msgRefNo, TimeSpan.FromMilliseconds(_transactionTimerPeriod).TotalSeconds);
                        DisposeTransactionOntimer();
                        _coreSvc.CommitTransaction(); // Change Stage to Signon
                        _res.SetException(new TimeoutException(msg));
                        _log.Error(msg);
                    };
                    _transactionTimer.Start();
                    return _res.Task;
                }
                _log.Error("Login Process have not completed::Current Stage:" + _coreSvc.CurrentState);
                throw new CommunicationException("Login Process have not completed:: Current Stage:" + _coreSvc.CurrentState);
            }
        }

        private void DisposeTransactionOntimer()
        {
            if (_transactionTimer != null)
            {
                _transactionTimer.Stop();
                _transactionTimer.Close();
                _transactionTimer.Dispose();
                _transactionTimer = null;
            }
        }

        public void Close()
        {
            lock (_myLock)
            {
                DisposeSignOntimer();
                _tcpClient.Disconnect();
                _tcpClient.Dispose();
            }
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            lock (_receivedLock)
            {
                var message = (HostRawMessage) e.Message;
                if (message != null)
                {
                    _log.Info(string.Format("Received a message: {0}", string.Concat(((HostRawMessage)e.Message).RawBytes.Select(c => c.ToString("X2")))));
                    _coreSvc.HandleIncomingMessageFromBank(message);

                    if (_coreSvc.CurrentState != HostState.Transacting)
                    {
                        var currentReply = _coreSvc.GetBankResponse();
                        while (currentReply != null)
                        {
                            _tcpClient.SendMessage(currentReply);
                            _coreSvc.BankResponseSent();
                            currentReply = _coreSvc.GetBankResponse();
                        }
                    }
                    else
                    {
                        _coreSvc.CommitTransaction();
                        BaseModel results = _coreSvc.GetCoreResponse();
                        DisposeTransactionOntimer(); // Transaction timeout.
                        if (_msgRefNo == results.Stan)
                        {
                            _log.Info(string.Format("Received transaction ref: {0} from Partner", _msgRefNo));
                            _res.SetResult(results);
                        }
                        else
                        {
                            _log.Error(string.Format("Reference number not match {0} : {1}", _msgRefNo, results.Stan));
                            _res.SetException(new InvalidOperationException(string.Format("Reference number not match {0} : {1}", _msgRefNo, results.Stan)));
                        }

                    }
                    _svrLog.LogMessageFromPartner(message.RawBytes); // Logging message from partner to database
                }

                //    if (_signOntimer != null)
                //        DisposeSignOntimer();
                //    _signOntimer = new Timer(_signOnPeriod) { AutoReset = false };
                //    _signOntimer.Elapsed += (o, args) =>
                //    {
                //        if (_coreSvc.IsReady)
                //            DisposeSignOntimer();
                //        else
                //            _tcpClient.Disconnect();

                //        throw new TimeoutException("Bank is not responding");
                //    };
                //    _signOntimer.Start();                
            }
        }

        private void MessageSent(object sender, MessageEventArgs e)
        {
            lock (_logonLock)
            {
                var message = (HostRawMessage) e.Message;
                if (message != null)
                    _svrLog.LogMessageToPartner(message.RawBytes); // Logging message to partner to database
            }
        }

        private void Disconnected(object sender, EventArgs e)
        {
            _coreSvc.Disconnect();

            if (e is DisconnectEventArgs)
            {
                var arg = e as DisconnectEventArgs;

                if (arg.Error is SocketException) //disconnected from Partner end
                {
                    CreateConnetionToPartner();
                    DisposeSignOntimer();
                    DisposeTransactionOntimer();
                }
                else
                {
                    _log.Error("Lost Connection to bank due to exception...", arg.Error);
                }
            }
        }

        private void Connected(object sender, EventArgs e)
        {
            _coreSvc.Connect();
            _log.Info(string.Format("Created Bank Connection: ip:{0} port:{1}", Config.ServerIpAddress, Config.ServerPort));
        }

        private void DisposeSignOntimer()
        {
            if (_signOntimer != null)
            {
                _signOntimer.Stop();
                _signOntimer.Close();
                _signOntimer.Dispose();
                _signOntimer = null;
            }
        }
    }
}
