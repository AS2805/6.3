using System;
using System.Collections.Generic;
using Common.Logging;
using SwitchLink.Data;
using SwitchLink.HostNode.Communication.Messages;
using SwitchLink.HostNode.Communication.States;
using SwitchLink.ProtocalFactory.Helper;
using SwitchLink.ProtocalFactory.HostNode;
using SwitchLink.ProtocalFactory.HostNode.Helper;
using SwitchLink.ProtocalFactory.HostNode.Models;

namespace SwitchLink.HostNode.Communication.Services
{
    public interface IBankingCoreService
    {
        //query 
        BaseModel GetCoreResponse();
        HostState CurrentState { get; }
        HostRawMessage GetBankResponse();

        //commands
        void BeginTransaction();
        void CommitTransaction();
        void HandleIncomingMessageFromBank(HostRawMessage message);
        void BankResponseSent();
        void Connect();
        void Disconnect();
    }

    public class BankingCoreService : IBankingCoreService
    {
        private enum EventType
        {
            MessageReceivedFromBank,
            ResponseSentToBank,
            MessageReceivedFromCore,
            ResponseSentToCore
        }
        
        private readonly ILog _log = LogManager.GetLogger<BankingCoreService>();
        private readonly RRN_Sequence _rrnSequence = new RRN_Sequence(); 
        private HostWorkflowState _workFlowState;
        private BaseModel _lastReceivedMessageFromBank;
        private readonly IFactory _factory;
        //private readonly BankMessageFactory _responseBuilder;
        private Dictionary<string, string> _zoneKeySet1 = new Dictionary<string, string>();
        //key data
        private readonly string _keKs;
        private readonly string _keKr;
        private HostState _currentState;
        private readonly BankMessageBuilder _responseBuilder;

        public BankingCoreService()
        {
            CurrentState = HostState.Disconnected;
            _factory = new Factory();
            _responseBuilder = new BankMessageBuilder();
            using (Session2805Data data = new Session2805Data())
            {
                _keKs = data.GetKeks_Send().Keks;
                _keKr = data.GetKekr_Send().Kekr;
            }
        }

        public BaseModel GetCoreResponse()
        {
            return _lastReceivedMessageFromBank;
        }

        public HostState CurrentState
        {
            get { return _currentState; }
            private set
            {
                _log.Info(string.Format("Host State changed {0} --> {1}", _currentState, value));
                _currentState = value;
            }
        }

        public void Connect()
        {
            _log.Debug("Host Connected To Partner..");
            CurrentState = HostState.Connected;
        }

        public void Disconnect()
        {
            _log.Warn("Host Disonnected From Partner..");
            CurrentState = HostState.Disconnected;
        }


        public void BeginTransaction()
        {
            CurrentState = HostState.Transacting;
        }
        public void CommitTransaction()
        {
            CurrentState = HostState.SignedOn;
        }

        public void HandleIncomingMessageFromBank(HostRawMessage message)
        {
            if (message == null)
                throw new ArgumentException("incoming message cannot be null");

            _lastReceivedMessageFromBank = _factory.Create(message.RawBytes);
            TransitionState(EventType.MessageReceivedFromBank);
        }

        public HostRawMessage GetBankResponse()
        {
            if (_workFlowState == HostWorkflowState.ReceivedSignOnRequest &&
                _lastReceivedMessageFromBank is SignonRequestModel)
            {
                var response = (_lastReceivedMessageFromBank as SignonRequestModel).GetValdationResponse(_keKr);
                return new HostRawMessage(_responseBuilder.NetworkManagementResponse(response));
            }
            else if (_workFlowState == HostWorkflowState.SendSignOnRequest)
            {
                var model = (_lastReceivedMessageFromBank as SignonRequestModel);
                if (model != null)
                {
                    model.ValidateKeys(_keKs, _rrnSequence.GetStan);
                    return new HostRawMessage(_responseBuilder.NetworkManagementRequest(model));
                }

                return null;
            }
            else if (_workFlowState == HostWorkflowState.ReceivedSignOnReply)
            {
                return null;
            }
            else if (_workFlowState == HostWorkflowState.RecievedKeyExchangeRequest && _lastReceivedMessageFromBank is KeyChangeRequest)
            {
                var req = _lastReceivedMessageFromBank as KeyChangeRequest;
                var response = req.GetTranslatedResponse(_keKr);
                return new HostRawMessage(_responseBuilder.NetworkManagementAdviceResponse(response));
            }
            else if (_workFlowState == HostWorkflowState.SendKeyExchangeRequest)
            {
                var lastReq = _lastReceivedMessageFromBank as KeyChangeRequest;
                _zoneKeySet1 = lastReq.GenerateZoneKeys(_keKs);
                lastReq.SetZoneKeys(_zoneKeySet1, _rrnSequence.GetStan);

                return new HostRawMessage(_responseBuilder.NetworkManagementAdviceRequest(lastReq));
            }
            else
            {
                return null;
            }
        }

        public void BankResponseSent()
        {
            TransitionState(EventType.ResponseSentToBank);
        }

        private void TransitionState(EventType evtType)
        {
            if (evtType == EventType.MessageReceivedFromBank)
            {
                if (_lastReceivedMessageFromBank is NetworkManagementModel)
                {
                    var model = (_lastReceivedMessageFromBank as NetworkManagementModel);
                    if (model.Mti == "0800" && model.NetMgtInfoCode == "001")
                    {
                        _log.Debug("Receieved Logon Request from Partner MTI: " + model.Mti);
                        ChangeInternalState(HostWorkflowState.ReceivedSignOnRequest);
                    }
                    else if (model.Mti == "0810" && model.NetMgtInfoCode == "001")
                    {
                        _log.Debug("Receieved Logon Response from Partner MTI: " + model.Mti);
                        ChangeInternalState(HostWorkflowState.ReceivedSignOnReply);
                        this.FinaliseLogon();
                    }
                    else if (model.Mti == "0820" && model.NetMgtInfoCode == "101")
                    {
                        _log.Debug("Receieved Key Exchange Request from Partner MTI: " + model.Mti);
                        ChangeInternalState(HostWorkflowState.RecievedKeyExchangeRequest);
                    }
                    else if (model.Mti == "0830" && model.NetMgtInfoCode == "101")
                    {
                        _log.Debug("Receieved Key Exchange Response from Partner MTI: " + model.Mti);
                        ChangeInternalState(HostWorkflowState.ReceivedKeyExchangeReply);
                        _log.Debug("Finalizing Key Exchange...");
                        if (this.FinaliseKeyExchange())
                        {
                            CurrentState = HostState.SignedOn;
                            _log.Info("Host is now Signed ON");
                        }
                    }
                }
            }
            else if (evtType == EventType.ResponseSentToBank)
            {
                if (_workFlowState == HostWorkflowState.ReceivedSignOnRequest)
                    ChangeInternalState(HostWorkflowState.SendSignOnRequest);
                else if (_workFlowState == HostWorkflowState.SendSignOnRequest)
                    ChangeInternalState(HostWorkflowState.WaitForSignOnReply);
                else if (_workFlowState == HostWorkflowState.RecievedKeyExchangeRequest)
                    ChangeInternalState(HostWorkflowState.SendKeyExchangeRequest);
                else if (_workFlowState == HostWorkflowState.SendKeyExchangeRequest)
                    ChangeInternalState(HostWorkflowState.WaitForKeyExchangeReply);
                else if (_workFlowState == HostWorkflowState.ReceivedKeyExchangeReply)
                    ChangeInternalState(HostWorkflowState.Ready);
            }
        }

        private void ChangeInternalState(HostWorkflowState newState)
        {
            _log.Info(string.Format("Host Workflow State changed {0} --> {1}", _workFlowState, newState));
            if (newState == HostWorkflowState.ReceivedSignOnRequest)
                CurrentState = HostState.SigningOn;
            else if (newState == HostWorkflowState.RecievedKeyExchangeRequest)
                CurrentState = HostState.KeyExchangeInProcess;

            _workFlowState = newState;
        }


        private bool FinaliseKeyExchange()
        {
            NetworkManagementModel model = _lastReceivedMessageFromBank as NetworkManagementModel;
            using (Session2805Data data = new Session2805Data())
            {
                return ValidateKeyExchangeResponse(model, data);
            }
        }

        private void FinaliseLogon()
        {
            NetworkManagementModel model = _lastReceivedMessageFromBank as NetworkManagementModel;
            ValidateKeyLogOnResponse(model);
        }

        void ValidateKeyLogOnResponse(NetworkManagementModel model)
        {
            if (model.RespCode == "00")
            {
                _log.Debug("Logon Completed");
            }

        }
        private bool ValidateKeyExchangeResponse(NetworkManagementModel network, Session2805Data data)
        {
            string value = network.AddtlDataPriv;
            string KMACs_KVC = value.Substring(0, 6);
            string KPEs_KVC = value.Substring(6);

            int node_number = network.SecControlInfo;
            _log.Info("KMACs_KVC : " + KMACs_KVC + " KPEs_KVC : " + KPEs_KVC);

            if (KMACs_KVC == _zoneKeySet1["ZAK Check Value"] && KPEs_KVC == _zoneKeySet1["ZPK Check Value"])
            {
                _log.Info("Key exchange successfull. Check values match. ZAK Check value :" + _zoneKeySet1["ZAK Check Value"] + " ZPK Check value : " + _zoneKeySet1["ZPK Check Value"]);
                data.UpdateSession_Send_as2805(_zoneKeySet1["ZPK(LMK)"], _zoneKeySet1["ZPK(ZMK)"], _zoneKeySet1["ZPK Check Value"], _zoneKeySet1["ZAK(LMK)"], _zoneKeySet1["ZAK(ZMK)"], _zoneKeySet1["ZAK Check Value"], _zoneKeySet1["ZEK(LMK)"], _zoneKeySet1["ZEK(ZMK)"], _zoneKeySet1["ZEK Check Value"], node_number.ToString());
                return true;
            }

            return false;
        }
    }
}
