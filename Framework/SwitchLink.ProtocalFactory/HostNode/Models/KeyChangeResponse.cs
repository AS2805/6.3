using System;
using System.Collections.Generic;
using Common.Logging;
using SwitchLink.Cryptography.HostCryptography;
using SwitchLink.Data;
using SwitchLink.ProtocalFactory.AS2805;

namespace SwitchLink.ProtocalFactory.HostNode.Models
{
    public class KeyChangeResponse : NetworkManagementModel
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(KeyChangeResponse));
        public KeyChangeResponse(As2805 builder)
        {
            try
            {
                As2805Extensions helper = new As2805Extensions();

                Mti = "0830";
                NetMgtInfoCode = "101";

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


        public void TranslateZoneKeys(string kekr)
        {
            var value = this.AddtlDataPriv;
            string zak = value.Substring(0, 32);
            string zpk = value.Substring(32);
            int node_number = this.SecControlInfo;

            _log.Info("Keys received under ZMK. ZAK = " + zak + " and ZPK = " + zpk);

            var hostCrypt=new HostCryptography();
            _log.Debug("Translating Zone Keys");
            Dictionary<string, string> zoneKeySet2 = hostCrypt.TranslateSetOfZoneKeys(kekr, zpk, zak, "11111111111111111111111111111111");
            using (Session2805Data data = new Session2805Data())
            {
                if (zoneKeySet2["ErrorCode"] == "00")
                {
                    data.UpdateSession_Recieve_as2805(zoneKeySet2["ZPK(LMK)"], zpk, zoneKeySet2["ZPK Check Value"], zoneKeySet2["ZAK(LMK)"], zak, zoneKeySet2["ZAK Check Value"], zoneKeySet2["ZEK(LMK)"], zoneKeySet2["ZEK Check Value"], node_number.ToString());
                    _log.Info("Sent keys under LMK : ZAK : " + zoneKeySet2["ZAK(LMK)"] + " ZAK check value : " + zoneKeySet2["ZAK Check Value"] + "ZPK : " + zoneKeySet2["ZPK(LMK)"] + "ZPK Check Value : " + zoneKeySet2["ZPK Check Value"]);
                    this.Mti = "0830";
                    this.NetMgtInfoCode = "101";
                    this.RespCode = "00";
                    this.AddtlDataPriv = zoneKeySet2["ZAK Check Value"] + zoneKeySet2["ZPK Check Value"];
                    _log.Info("Sending key exchange response");                    
                }
                else
                {
                    _log.Error("Error Translating Set of Zone Keys");
                    zoneKeySet2.Clear();
                    this.RespCode = "30";                    
                }                               
            } 
        }
        
    }
}