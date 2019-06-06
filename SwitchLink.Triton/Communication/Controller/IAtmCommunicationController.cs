using System;
using System.Timers;
using Common.Logging;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.Scs.Server;
using Hik.Communication.ScsServices.Client;
using SwitchLink.Comm.Interfaces.Services;
using SwitchLink.TritonNode.Communication.Messages;
using SwitchLink.TritonNode.Communication.Services;
using SwitchLink.TritonNode.Configuration;

namespace SwitchLink.TritonNode.Communication.Controller
{
    interface IAtmCommunicationController
    {
        void SendInit();
        void StartConnectionTimeOut(int timeout);
    }

    class AtmCommunicationController : IAtmCommunicationController
    {
        #region Private fields

        private readonly IScsServerClient _client;

        private readonly ILog _log = LogManager.GetLogger(typeof(AtmCommunicationController));
        private readonly ICoreCommunicationService _svc = new CoreCommunicationService(ScsServiceClientBuilder.CreateClient<ICoreNodeRequestService>(new ScsTcpEndPoint(Config.ServerIpAddress, 8001)));
        private readonly IAtmCoreService _coreSvc;

        private Timer _replyTimer;
        private Timer _connTimeOut;
        #endregion

        #region Public methods

        public AtmCommunicationController(IScsServerClient client)
        {
            _client = client;
            _client.MessageReceived += Client_MessageReceived;
            _client.Disconnected += Client_Disconnected;

            _coreSvc = new AtmCoreService(_svc);
        }

        public void SendInit()
        {
            _coreSvc.Connect(DateTime.Now, _client.ClientId, _client.RemoteEndPoint.ToString());
            _client.SendMessage(_coreSvc.GetAtmResponse());
            _coreSvc.ResponseSent();
            _log.Debug(string.Format("Sent Enq to Client Id = {0}", _client.ClientId));
        }

        public void StartConnectionTimeOut(int timeout)
        {
            if (_connTimeOut != null)
            {
                _connTimeOut.Stop();
                _connTimeOut.Close();
                _connTimeOut.Dispose();
                _connTimeOut = null;
            }
            _connTimeOut = new Timer(timeout);
            _connTimeOut.Elapsed += (sender, args) =>
            {
                _client.Disconnect();
            };
            _connTimeOut.Start();
        }

        #endregion

        #region Private methods

        private void Client_MessageReceived(object sender, MessageEventArgs e)
        {
            _coreSvc.HandleIncomingMessage((AtmRawMessage)e.Message);
            if (!_coreSvc.IsConnected)
                _client.Disconnect();
            else
            {
                if (_coreSvc.RetryResponse)
                {
                    int count = 1;
                    _client.SendMessage(_coreSvc.GetAtmResponse()); // sent Ack
                    _coreSvc.ResponseSent();
                    _log.Info("Response Ack sent");

                    _client.SendMessage(_coreSvc.GetAtmResponse()); // sent Stx
                    _coreSvc.ResponseSent();
                    _log.Info(string.Format("Response Stx Sent : count ({0})", count));

                    _replyTimer = new Timer(3000);
                    _replyTimer.Elapsed += (o, args) =>
                    {
                        count++;
                        if (!_coreSvc.IsConnected || !_coreSvc.RetryResponse)
                        {
                            _replyTimer.Stop();
                            _log.Warn("ATM not ACK within 3 times");
                            _client.Disconnect();
                        }
                        else
                        {
                            _client.SendMessage(_coreSvc.GetAtmResponse());
                            _coreSvc.ResponseSent();
                            _log.Info(string.Format("Response Stx Sent : count ({0})", count));
                        }
                    };
                    _replyTimer.Start();
                }
                else
                {
                    _client.SendMessage(_coreSvc.GetAtmResponse()); // sent Eot
                    _coreSvc.ResponseSent();
                    _log.Info("Response Eot Sent");
                }
            }
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
            DisposeTimer();
            _coreSvc.DisConnect();
        }

        private void DisposeTimer()
        {
            if (_replyTimer != null)
            {
                _replyTimer.Stop();
                _replyTimer.Close();
                _replyTimer.Dispose();
                _replyTimer = null;
            }
            if (_connTimeOut != null)
            {
                _connTimeOut.Stop();
                _connTimeOut.Close();
                _connTimeOut.Dispose();
                _connTimeOut = null;
            }
        }

        #endregion
    }
}
