using System;
using System.Collections.Generic;
using Common.Logging;
using SwitchLink.Cryptography.HostCryptography;
using SwitchLink.Data;
using SwitchLink.ProtocalFactory.AS2805;

namespace SwitchLink.ProtocalFactory.HostNode.Models
{
    public class KeyChangeRequest : NetworkManagementModel
    {
        private readonly As2805 _builder;
        private readonly ILog _log = LogManager.GetLogger(typeof(KeyChangeRequest));
        public KeyChangeRequest(As2805 builder)
        {
            _builder = builder;
            try
            {
                As2805Extensions helper = new As2805Extensions();
                Mti = "0820";
                NetMgtInfoCode = "101";
                this.FwdInstIdCode = 579944;
                this.RecvInstIdCode = 61100016;

                TranDate = helper.Parse_de7_TransDttm_ToDateTime(builder.de7_TransDttm);
                Stan = int.Parse(builder.de11_STAN);
                RespCode = builder.de39_RespCode;
                FwdInstIdCode = int.Parse(builder.de33_FwdInstIdCode);
                AddtlDataPriv = helper.Parse_AddtlDataPrivToASCII(builder.de48_AddtlDataPriv);
                SecControlInfo = string.IsNullOrEmpty(builder.de53_SecControlInfo) ? 0 : int.Parse(builder.de53_SecControlInfo);
                
                RecvInstIdCode = int.Parse(builder.de100_RecvInstIdCode);                
            }
            catch (Exception e)
            {
                _log.Error(e.Message + " Stack: " + e.StackTrace);
                throw new ArgumentException(e.Message + " Stack: " + e.StackTrace);
            }
        }


        public KeyChangeResponse GetTranslatedResponse(string kekr)
        {
            var responseModel=new KeyChangeResponse(_builder);
            responseModel.TranslateZoneKeys(kekr);

            return responseModel;
        }

        public void SetZoneKeys(Dictionary<string,string> zoneKeySet1, int stan)
        {
            if (zoneKeySet1["ErrorCode"] == "00")
            {
                this.Stan = stan;                
                this.AddtlDataPriv = zoneKeySet1["ZAK(ZMK)"].Substring(1) +
                                      zoneKeySet1["ZPK(ZMK)"].Substring(1);
                using (Session2805Data data = new Session2805Data())
                {
                    int node_number = this.SecControlInfo;
                    data.UpdateSession_Send_as2805(zoneKeySet1["ZPK(LMK)"], zoneKeySet1["ZPK(ZMK)"],
                        zoneKeySet1["ZPK Check Value"], zoneKeySet1["ZAK(LMK)"], zoneKeySet1["ZAK(ZMK)"],
                        zoneKeySet1["ZAK Check Value"], zoneKeySet1["ZEK(LMK)"], zoneKeySet1["ZEK(ZMK)"],
                        zoneKeySet1["ZEK Check Value"], node_number.ToString());
                    _log.Info("Sent keys under LMK : ZAK : " + zoneKeySet1["ZAK(LMK)"] + " ZAK check value : " +
                              zoneKeySet1["ZAK Check Value"] + "ZPK : " + zoneKeySet1["ZPK(LMK)"] + "ZPK Check Value : " +
                              zoneKeySet1["ZPK Check Value"]);    
                }                               
            }
        }
    }
}