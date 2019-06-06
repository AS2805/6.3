using System;
using System.Collections.Generic;
using BankSimulator.Communication.Messages;
using Common.Logging;
using SwitchLink.Cryptography.HostCryptography;
using SwitchLink.Data;
using SwitchLink.ProtocalFactory.Helper;
using SwitchLink.ProtocalFactory.HostNode.Models;
using SwitchLink.ProtocalFactory.PartnerSimulator.Helper;

namespace BankSimulator.Communication.Services
{
    interface ICoreService
    {
        BankRawMessage GenerateSignOnRequest();

        BankRawMessage GenerateSignOnResponse(NetworkManagementModel model);
        BankRawMessage GenerateKeyExchangeRequest(NetworkManagementModel model);
        BankRawMessage GenerateKeyExchangeResponse(NetworkManagementModel model);
        BankRawMessage GenerateAuthorizationResponse(AuthorizationRequestModel model);
        BankRawMessage GenerateReversalResponse(ReversalRequestModel model);
        void FinaliseLogon(NetworkManagementModel model);
        void FinaliseKeyExchange(NetworkManagementModel model);
    }

    class CoreService : ICoreService
    {
        //Key data
        private readonly string _keKr;
        private readonly string _keKs;
        private Dictionary<string, string> _zoneKeySet1 = new Dictionary<string, string>();

        private readonly PartnerMessageBuilder _responseBuilder;
        private readonly ILog _log = LogManager.GetLogger<CoreService>();
        private readonly RRN_Sequence _rrnSequence = new RRN_Sequence(); 

        public CoreService()
        {
            _responseBuilder = new PartnerMessageBuilder();
            using (Session2805Data data = new Session2805Data())
            {
                _keKs = data.GetKeks_Send().Keks;
                _keKr = data.GetKekr_Send().Kekr;
            }
        }

        public BankRawMessage GenerateAuthorizationResponse(AuthorizationRequestModel model)
        {
            BankRawMessage result = new BankRawMessage(_responseBuilder.ResponseTranAuthorization(model));
            return result;
        }

        public BankRawMessage GenerateReversalResponse(ReversalRequestModel model)
        {
            BankRawMessage result = new BankRawMessage(_responseBuilder.ReversalAdviceResponse(model));
            return result;
        }

        private BankRawMessage GenerateValidationResponse(NetworkManagementModel model, HostCryptography hostCrypt)
        {
            _log.Debug("Building 0810");
            //build 0810
            var krsReceived = model.AddtlDataPriv;
            var validationResponse = hostCrypt.Generate_KEKr_Validation_Response(_keKr, krsReceived);

            if (validationResponse["ErrorCode"] == "00")
            {
                model.Mti = "0810";
                model.AddtlDataPriv = validationResponse["KRr"];
                model.RespCode = "00";
                model.NetMgtInfoCode = "001";
            }
            else
            {
                _log.Error("0810 KRr Response Error");
                model.RespCode = "30"; //format error
            }

            return new BankRawMessage(_responseBuilder.NetworkManagementResponse(model));
        }

        public BankRawMessage GenerateSignOnRequest()
        {
            NetworkManagementModel model = new NetworkManagementModel();

            HostCryptography hostCrypt = new HostCryptography();
            _log.Debug("Generating Validation Request");
            return GenerateValidationRequest(model, hostCrypt);

        }

        public BankRawMessage GenerateSignOnResponse(NetworkManagementModel model)
        {
            HostCryptography hostCrypt = new HostCryptography();
            _log.Debug("Generating Validation Response");
            return GenerateValidationResponse(model, hostCrypt);

        }

        private BankRawMessage GenerateValidationRequest(NetworkManagementModel model, HostCryptography hostCrypt)
        {
            _log.Debug("Building 0800");
            //build 0800

            var keksValidationRequest = hostCrypt.Generate_KEKs_Validation_Request(_keKs);

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
            _log.Error("0800 _keKs Validation Request Error");
            throw new InvalidOperationException("0800 _keKs Validation Request Error");
        }

        public BankRawMessage GenerateKeyExchangeRequest(NetworkManagementModel model)
        {
            HostCryptography hostCrypt = new HostCryptography();
            _log.Debug("Generating Zone Keys");
            return GenerateZoneKeys(model, hostCrypt);

        }

        public void FinaliseLogon(NetworkManagementModel model)
        {
            if (model.RespCode == "00")
                _log.Info("Log on Successfull");
        }

        public void FinaliseKeyExchange(NetworkManagementModel model)
        {
            string value = model.AddtlDataPriv;
            string kmaCsKvc = value.Substring(0, 6);
            string kpEsKvc = value.Substring(6);

            int nodeNumber = model.SecControlInfo;
            _log.Info("KMACs_KVC : " + kmaCsKvc + " KPEs_KVC : " + kpEsKvc);

            if (kmaCsKvc == _zoneKeySet1["ZAK Check Value"] && kpEsKvc == _zoneKeySet1["ZPK Check Value"])
            {
                _log.Info("Key exchange successfull. Check values match. ZAK Check value :" + _zoneKeySet1["ZAK Check Value"] + " ZPK Check value : " + _zoneKeySet1["ZPK Check Value"]);
                using (Session2805Data data = new Session2805Data())
                {
                    data.UpdateSession_Send_as2805(_zoneKeySet1["ZPK(LMK)"], _zoneKeySet1["ZPK(ZMK)"], _zoneKeySet1["ZPK Check Value"], _zoneKeySet1["ZAK(LMK)"], _zoneKeySet1["ZAK(ZMK)"], _zoneKeySet1["ZAK Check Value"], _zoneKeySet1["ZEK(LMK)"], _zoneKeySet1["ZEK(ZMK)"], _zoneKeySet1["ZEK Check Value"], nodeNumber.ToString());
                }
                _log.Info("Log on Successfull");
            }
        }

        private BankRawMessage GenerateZoneKeys(NetworkManagementModel model, HostCryptography hostCrypt)
        {
            _zoneKeySet1 = hostCrypt.GenerateSetOfZoneKeys(_keKs);
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
                int nodeNumber = model.SecControlInfo;
                using (Session2805Data data = new Session2805Data())
                {
                    data.UpdateSession_Send_as2805(_zoneKeySet1["ZPK(LMK)"], _zoneKeySet1["ZPK(ZMK)"],
                        _zoneKeySet1["ZPK Check Value"], _zoneKeySet1["ZAK(LMK)"], _zoneKeySet1["ZAK(ZMK)"],
                        _zoneKeySet1["ZAK Check Value"], _zoneKeySet1["ZEK(LMK)"], _zoneKeySet1["ZEK(ZMK)"],
                        _zoneKeySet1["ZEK Check Value"], nodeNumber.ToString());
                    _log.Info("GenerateZoneKeys under LMK : ZAK : " + _zoneKeySet1["ZAK(LMK)"] + " ZAK check value : " +
                              _zoneKeySet1["ZAK Check Value"] + "ZPK : " + _zoneKeySet1["ZPK(LMK)"] +
                              "ZPK Check Value : " + _zoneKeySet1["ZPK Check Value"]);
                }

                return new BankRawMessage(_responseBuilder.NetworkManagementAdviceRequest(model));
            }
            _log.Error("0820 _keKs Validation Request Error");
            throw new InvalidOperationException("0820 _keKs Validation Request Error");
        }

        public BankRawMessage GenerateKeyExchangeResponse(NetworkManagementModel model)
        {
            HostCryptography hostCrypt = new HostCryptography();
            _log.Debug("Translating Zone Keys");

            return TranslateZoneKeys(model, hostCrypt);
        }

        private BankRawMessage TranslateZoneKeys(NetworkManagementModel model, HostCryptography hostCrypt)
        {
            var value = model.AddtlDataPriv;
            string zak = value.Substring(0, 32);
            string zpk = value.Substring(32);
            int nodeNumber = model.SecControlInfo;

            _log.Info("Keys received under ZMK. ZAK = " + zak + " and ZPK = " + zpk);

            Dictionary<string, string> zoneKeySet2 = hostCrypt.TranslateSetOfZoneKeys(_keKr, zpk, zak, "11111111111111111111111111111111");
            if (zoneKeySet2["ErrorCode"] == "00")
            {
                using (Session2805Data data = new Session2805Data())
                {
                    data.UpdateSession_Recieve_as2805(zoneKeySet2["ZPK(LMK)"], zpk, zoneKeySet2["ZPK Check Value"],
                        zoneKeySet2["ZAK(LMK)"], zak, zoneKeySet2["ZAK Check Value"], zoneKeySet2["ZEK(LMK)"],
                        zoneKeySet2["ZEK Check Value"], nodeNumber.ToString());
                }
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
            model.RespCode = "30";
            return new BankRawMessage(_responseBuilder.NetworkManagementAdviceResponse(model));
        }

    }
}
