using System;
using BankSimulator.Communication.Messages;
using BankSimulator.Communication.Services;
using Common.Logging;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.Scs.Server;
using SwitchLink.ProtocalFactory.HostNode.Models;
using SwitchLink.ProtocalFactory.PartnerSimulator;

namespace BankSimulator.Communication.Controllers
{
    interface ICoreController
    {
        void Start();
    }

    class CoreController : ICoreController
    {
        private readonly ILog _log = LogManager.GetLogger<CoreController>();
        private readonly Object _syncLock = new Object();
        private readonly IScsServerClient _client;
        private readonly ICoreService _svc;
        private readonly IFactory _factory;

        public CoreController(IScsServerClient scsServerClient)
        {
            _client = scsServerClient;
            _factory = new Factory();
            _svc = new CoreService();
        }

        public void Start()
        {
            _client.SendMessage(_svc.GenerateSignOnRequest());
            _log.Info("Sent :: GenerateSignOnRequest");
            _client.Disconnected -= Disconnected;
            _client.MessageReceived -= MessageReceived;

            _client.Disconnected += Disconnected;
            _client.MessageReceived += MessageReceived;
        }

        private void MessageReceived(object sender, MessageEventArgs e)
        {
            lock (_syncLock)
            {
                _log.Debug("Geting Incomming Message");
                BankRawMessage message = e.Message as BankRawMessage; //Server only accepts Raw messages
                if (message != null)
                {
                    BaseModel baseModel = _factory.Create(message.RawBytes);

                    if (baseModel is NetworkManagementModel)
                    {
                        NetworkManagementModel model = baseModel as NetworkManagementModel;
                        if (model.Mti == "0800" && model.NetMgtInfoCode == "001")
                        {
                            _log.Debug("Recieved Log On Request from Host Node");

                            _log.Debug("sending Sign on Response to Host Node");
                            _client.SendMessage(_svc.GenerateSignOnResponse(model));

                            _log.Debug("sending Sign on Request to Host Node");
                            _client.SendMessage(_svc.GenerateKeyExchangeRequest(model));
                        }
                        else if (model.Mti == "0810" && model.NetMgtInfoCode == "001")
                        {
                            _log.Debug("Recieved Log On Response from Host Node");
                            _svc.FinaliseLogon(model);
                        }
                        else if (model.Mti == "0820" && model.NetMgtInfoCode == "101")
                        {
                            _log.Debug("Recieved Key Exchange Request from Host Node");
                            _log.Debug("Sending Key Exchange Response to Host Node");
                            _client.SendMessage(_svc.GenerateKeyExchangeResponse(model));
                            _log.Debug("Simulator Ready to Accept Transactions");
                        }
                        else if (model.Mti == "0830" && model.NetMgtInfoCode == "101")
                        {
                            _log.Debug("Recieved Key Exchange Response from Host Node");
                            _svc.FinaliseKeyExchange(model);
                        }
                    }
                    else if (baseModel.Mti == "0200")
                    {
                        _log.Debug("Recieved Key Auth Request from Host Node");
                        _client.SendMessage(_svc.GenerateAuthorizationResponse(baseModel as AuthorizationRequestModel));
                    }
                    else if (baseModel.Mti == "0420")
                    {
                        _log.Debug("Recieved Key Reversal Request from Host Node");
                        _client.SendMessage(_svc.GenerateReversalResponse(baseModel as ReversalRequestModel));
                    }
                }
                else
                {
                    _log.Error("Incomming Message:: NULL");
                }
            }
        }

        private void Disconnected(object sender, EventArgs e)
        {
            _log.Warn("Lost Connection with Switch");
        }
    }
}
