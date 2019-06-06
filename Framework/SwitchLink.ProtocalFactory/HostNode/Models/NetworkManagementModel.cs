using System;
using System.Collections.Generic;
using Common.Logging;
using SwitchLink.Cryptography.HostCryptography;
using SwitchLink.ProtocalFactory.AS2805;

namespace SwitchLink.ProtocalFactory.HostNode.Models
{
    public class NetworkManagementModel : BaseModel
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(NetworkManagementModel));
        public int RecvInstIdCode { get; set; }
        public string AddtlDataPriv { get; set; }
        public string NetMgtInfoCode { get; set; } 

        public NetworkManagementModel Create(As2805 builder)
        {
            try
            {
                As2805Extensions helper = new As2805Extensions();
                
                Mti = builder.mti;
                TranDate = helper.Parse_de7_TransDttm_ToDateTime(builder.de7_TransDttm);
                Stan = int.Parse(builder.de11_STAN);
                RespCode = builder.de39_RespCode;
                FwdInstIdCode = int.Parse(builder.de33_FwdInstIdCode);
                AddtlDataPriv = helper.Parse_AddtlDataPrivToASCII(builder.de48_AddtlDataPriv);
                SecControlInfo = string.IsNullOrEmpty(builder.de53_SecControlInfo) ? 0 : int.Parse(builder.de53_SecControlInfo);
                NetMgtInfoCode = builder.de70_NetMgtInfoCode;
                RecvInstIdCode = int.Parse(builder.de100_RecvInstIdCode);

                return this;
            }
            catch (Exception e)
            {
                _log.Error(e.Message + " Stack: " + e.StackTrace);
                throw new ArgumentException(e.Message + " Stack: " + e.StackTrace);
            }
        }



        public Dictionary<string, string> GenerateZoneKeys(string keks)
        {
            var hostCrypt = new HostCryptography();
            return hostCrypt.GenerateSetOfZoneKeys(keks);
        } 
    }
}
