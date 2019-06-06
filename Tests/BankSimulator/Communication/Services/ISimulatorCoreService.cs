using System;
using System.Collections.Generic;
using BankSimulator.Communication.Messages;
using BankSimulator.Communication.States;
using Common.Logging;
using SwitchLink.Cryptography.HostCryptography;
using SwitchLink.Data;
using SwitchLink.ProtocalFactory.Helper;
using SwitchLink.ProtocalFactory.HostNode.Models;
using SwitchLink.ProtocalFactory.PartnerSimulator;
using SwitchLink.ProtocalFactory.PartnerSimulator.Helper;

namespace BankSimulator.Communication.Services
{
    public interface ISimulatorCoreService
    {
        //query
        bool IsReady { get; }
        //commands
        void HandleResponse(BankRawMessage message);
        BankRawMessage GenerateSignOnResponse();
        BankRawMessage GenerateSignOnRequest();
        BankRawMessage GenerateKeyExchangeRequest();
        BankRawMessage GenerateKeyExchangeResponse();
        BankRawMessage GenerateAuthorizationResponse();
        BankRawMessage GenerateReversalResponse();
        SimulatorStates GetState();
        void SetState(SimulatorStates state);
        void FinaliseLogon();
        void FinaliseKeyExchange();
        void Connect();
        void Disconnect();
    }

    public class SimulatorCoreService : ISimulatorCoreService
    {
        //Key data
        private string KEKr;
        private string KEKs;

        private readonly ILog _log = LogManager.GetLogger<SimulatorCoreService>();
        private readonly RRN_Sequence _rrnSequence = new RRN_Sequence(); 
        private SimulatorStates _partnerState;
        private BaseModel _baseModel;
        private readonly IFactory _factory;
        private readonly PartnerMessageBuilder _responseBuilder;
        private Dictionary<string, string> _zoneKeySet1 = new Dictionary<string, string>();


        public SimulatorCoreService()
        {
            //_hostState = SimulatorStates.Disconnect;
            _factory = new Factory();
            _responseBuilder = new PartnerMessageBuilder();

            using (Session2805Data data = new Session2805Data())
            {
                KEKs = data.GetKeks_Send().Keks;
                KEKr = data.GetKekr_Send().Kekr;
            }
        }

        public bool IsBankConfig { get; private set; }

        public bool IsReady
        {
            get { return _partnerState == SimulatorStates.Ready; }
        }

        public SimulatorStates GetState()
        {
            return _partnerState;
        }

        public void Connect()
        {
            SetState(SimulatorStates.Connected);
        }

        public void Disconnect()
        {
            SetState(SimulatorStates.Disconnect);
        }

        public BaseModel GetCoreResponse()
        {
            return _baseModel;
        }

        public void HandleResponse(BankRawMessage message)
        {
            try
            {
                _log.Debug("Geting Incomming Message");
                if (message != null)
                {
                    BaseModel model = _factory.Create(message.RawBytes);
                   

                    if (model is NetworkManagementModel)
                    {
                        SetWorkflow((NetworkManagementModel) model);
                    }

                    else if (model.Mti == "0200")
                    {
                        _log.Debug("Recieved Key Auth Request from Host Node");
                        SetState(SimulatorStates.AuthorizationReceived);
                    }
                    else if (model.Mti == "0420")
                    {
                        _log.Debug("Recieved Key Reversal Request from Host Node");
                        SetState(SimulatorStates.ReversalRecieved);
                    }

                    _baseModel = model;
                }
                else
                {
                    _log.Error("Response message null");

                }
            }
            catch (Exception e)
            {
                _log.Error("Error in HandleResponse: " + e.Message);
            }
        }


        public void SetWorkflow(NetworkManagementModel model)
        {
            if (_partnerState != SimulatorStates.Disconnect)
            {
                if (model.Mti == "0800" && model.NetMgtInfoCode == "001")
                {
                    _log.Debug("Recieved Log On Request from Host Node");
                    SetState(SimulatorStates.RecievedSignOnRequest);
                }
                else if (model.Mti == "0810" && model.NetMgtInfoCode == "001")
                {
                    _log.Debug("Recieved Log On Response from Host Node");
                    SetState(SimulatorStates.RecievedSignOnResponse);
                }
                else if (model.Mti == "0820" && model.NetMgtInfoCode == "101")
                {
                    _log.Debug("Recieved Key Exchange Request from Host Node");
                    SetState(SimulatorStates.RecievedKeyExchangeRequest);
                }
                else if (model.Mti == "0830" && model.NetMgtInfoCode == "101")
                {
                    _log.Debug("Recieved Key Exchange Response from Host Node");
                    SetState(SimulatorStates.RecievedKeyExchangeResponse);
                }
         
            }
        }

        public BankRawMessage GenerateAuthorizationResponse()
        {
            TransactionModel model = _baseModel as TransactionModel;      
            return new BankRawMessage(_responseBuilder.ResponseTranAuthorization(model));
        }

        public BankRawMessage GenerateReversalResponse()
        {
            ReversalRequestModel model = _baseModel as ReversalRequestModel;
            return new BankRawMessage(_responseBuilder.ReversalAdviceResponse(model));
        }

        public BankRawMessage GenerateSignOnResponse()
        {
            NetworkManagementModel model = _baseModel as NetworkManagementModel;
            using (Session2805Data data = new Session2805Data())
            {
                HostCryptography hostCrypt = new HostCryptography();
                _log.Debug("Generating Validation Response");
                return GenerateValidationResponse(model, data, hostCrypt);
            }
        }

        public BankRawMessage GenerateSignOnRequest()
        {
            //NetworkManagementModel model = _baseModel as NetworkManagementModel;
            NetworkManagementModel model = new NetworkManagementModel();

            using (Session2805Data data = new Session2805Data())
            {
                HostCryptography hostCrypt = new HostCryptography();
                _log.Debug("Generating Validation Request");
                return GenerateValidationRequest(model, data, hostCrypt);
            }
        }
        public BankRawMessage GenerateKeyExchangeRequest()
        {
            NetworkManagementModel model = _baseModel as NetworkManagementModel;
           
            using (Session2805Data data = new Session2805Data())
            {
                HostCryptography hostCrypt = new HostCryptography();
                _log.Debug("Generating Zone Keys");
                return GenerateZoneKeys(model, data, hostCrypt);
            }
        }
        public BankRawMessage GenerateKeyExchangeResponse()
        {
            NetworkManagementModel model = _baseModel as NetworkManagementModel;
            using (Session2805Data data = new Session2805Data())
            {
                HostCryptography hostCrypt = new HostCryptography();
                _log.Debug("Translating Zone Keys");

                return TranslateZoneKeys(model, data, hostCrypt);
            }
        }

        public void FinaliseKeyExchange()
        {
            NetworkManagementModel model = _baseModel as NetworkManagementModel;
            using (Session2805Data data = new Session2805Data())
            {
                ValidateKeyExchangeResponse(model, data);
            }
        }

        public void FinaliseLogon()
        {
            NetworkManagementModel model = _baseModel as NetworkManagementModel;
            ValidateKeyLogOnResponse(model);
        }

        void ValidateKeyLogOnResponse(NetworkManagementModel model)
        {
            if (model.RespCode == "00")
            {
                _log.Info("Log on Successfull");
            }
        }

        private void ValidateKeyExchangeResponse(NetworkManagementModel network, Session2805Data data)
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
               _log.Info("Log on Successfull");
            }
            SetState(SimulatorStates.Connected);
        }

        private BankRawMessage GenerateValidationResponse(NetworkManagementModel model, Session2805Data data,
            HostCryptography hostCrypt)
        {

            _log.Debug("Building 0810");
            //build 0810


            var krsReceived = model.AddtlDataPriv;
            var validationResponse = hostCrypt.Generate_KEKr_Validation_Response(KEKr, krsReceived);

            if (validationResponse["ErrorCode"] == "00")
            {
                model.Mti = "0810";
                model.AddtlDataPriv = validationResponse["KRr"];
                model.RespCode = "00";
                model.NetMgtInfoCode = "001";
                return new BankRawMessage(_responseBuilder.NetworkManagementResponse(model));
            }

            _log.Error("0810 KRr Response Error");
            model.RespCode = "30"; //format error
            return new BankRawMessage(_responseBuilder.NetworkManagementResponse(model));
        }

        private BankRawMessage GenerateValidationRequest(NetworkManagementModel model, Session2805Data data, HostCryptography hostCrypt)
        {
            _log.Debug("Building 0800");
            //build 0800
           
            var keksValidationRequest = hostCrypt.Generate_KEKs_Validation_Request(KEKs);

            if (keksValidationRequest["ErrorCode"] == "00")
            {
                string krsSent;
                keksValidationRequest.TryGetValue("KRs", out krsSent);
                model.Mti = "0800";
                model.Stan = _rrnSequence.GetStan;
                model.FwdInstIdCode = 579944;
                model.NetMgtInfoCode = "001";
                model.RecvInstIdCode = 61001001;
                model.AddtlDataPriv = krsSent;


                return new BankRawMessage(_responseBuilder.NetworkManagementRequest(model));
            }
            _log.Error("0800 KEKs Validation Request Error");
            throw new InvalidOperationException("0800 KEKs Validation Request Error");
        }

        private BankRawMessage TranslateZoneKeys(NetworkManagementModel model, Session2805Data data, HostCryptography hostCrypt)
        {
            var value = model.AddtlDataPriv;
            string zak = value.Substring(0, 32);
            string zpk = value.Substring(32);
            int node_number = model.SecControlInfo;
          
            _log.Info("Keys received under ZMK. ZAK = " + zak + " and ZPK = " + zpk);

            Dictionary<string, string> zoneKeySet2 = hostCrypt.TranslateSetOfZoneKeys(KEKr,zpk, zak, "11111111111111111111111111111111");
            if (zoneKeySet2["ErrorCode"] == "00")
            {
                data.UpdateSession_Recieve_as2805(zoneKeySet2["ZPK(LMK)"], zpk, zoneKeySet2["ZPK Check Value"], zoneKeySet2["ZAK(LMK)"], zak, zoneKeySet2["ZAK Check Value"], zoneKeySet2["ZEK(LMK)"], zoneKeySet2["ZEK Check Value"], node_number.ToString());

                _log.Info("Sent keys under LMK : ZAK : " + zoneKeySet2["ZAK(LMK)"] + " ZAK check value : " + zoneKeySet2["ZAK Check Value"] + "ZPK : " + zoneKeySet2["ZPK(LMK)"] + "ZPK Check Value : " + zoneKeySet2["ZPK Check Value"]);

                model.Mti = "0830";
                model.RespCode = "00";
                model.NetMgtInfoCode = "101";
                model.AddtlDataPriv = zoneKeySet2["ZAK Check Value"] + zoneKeySet2["ZPK Check Value"];

                _log.Info("Sending key exchange response");
                return new BankRawMessage(_responseBuilder.NetworkManagementAdviceResponse(model));
            }

            _log.Error("Error Translating Set of Zone Keys");
            zoneKeySet2.Clear();
            SetState(SimulatorStates.Disconnect);
            model.RespCode = "30";
            return new BankRawMessage(_responseBuilder.NetworkManagementAdviceResponse(model));
        }

        private BankRawMessage GenerateZoneKeys(NetworkManagementModel model, Session2805Data data, HostCryptography hostCrypt)
        {
            _zoneKeySet1 = hostCrypt.GenerateSetOfZoneKeys(KEKs);
            if (_zoneKeySet1["ErrorCode"] == "00")
            {
                model.Mti = "0820";
                model.Stan = _rrnSequence.GetStan;
                model.AddtlDataPriv = _zoneKeySet1["ZAK(ZMK)"].Substring(1) +
                                      _zoneKeySet1["ZPK(ZMK)"].Substring(1);
                model.Mti = "0820";
                model.FwdInstIdCode = 579944;
                model.RecvInstIdCode = 61100016;
                model.NetMgtInfoCode = "101";
                int node_number = model.SecControlInfo;
                data.UpdateSession_Send_as2805(_zoneKeySet1["ZPK(LMK)"], _zoneKeySet1["ZPK(ZMK)"], _zoneKeySet1["ZPK Check Value"], _zoneKeySet1["ZAK(LMK)"], _zoneKeySet1["ZAK(ZMK)"], _zoneKeySet1["ZAK Check Value"], _zoneKeySet1["ZEK(LMK)"], _zoneKeySet1["ZEK(ZMK)"], _zoneKeySet1["ZEK Check Value"], node_number.ToString());
                _log.Info("GenerateZoneKeys under LMK : ZAK : " + _zoneKeySet1["ZAK(LMK)"] + " ZAK check value : " + _zoneKeySet1["ZAK Check Value"] + "ZPK : " + _zoneKeySet1["ZPK(LMK)"] + "ZPK Check Value : " + _zoneKeySet1["ZPK Check Value"]);


                return new BankRawMessage(_responseBuilder.NetworkManagementAdviceRequest(model));
            }
            _log.Error("0820 KEKs Validation Request Error");
            throw new InvalidOperationException("0820 KEKs Validation Request Error");
        }

        public void SetState(SimulatorStates states)
        {
            _log.Info(string.Format("Bank Connection state changed from {0} -> {1}", _partnerState, states));
            _partnerState = states;
        }
    }
}
