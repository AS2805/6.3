using System;
using Common.Logging;
using SwitchLink.Cryptography.HostCryptography;
using SwitchLink.ProtocalFactory.AS2805;

namespace SwitchLink.ProtocalFactory.HostNode.Models
{
    public class SignonResponseModel : NetworkManagementModel
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(SignonResponseModel));
        public SignonResponseModel(As2805 builder)
        {
            try
            {
                As2805Extensions helper = new As2805Extensions();
                Mti = "0810";
                TranDate = helper.Parse_de7_TransDttm_ToDateTime(builder.de7_TransDttm);
                Stan = int.Parse(builder.de11_STAN);
                RespCode = builder.de39_RespCode;
                FwdInstIdCode = int.Parse(builder.de33_FwdInstIdCode);
                AddtlDataPriv = helper.Parse_AddtlDataPrivToASCII(builder.de48_AddtlDataPriv);
                SecControlInfo = string.IsNullOrEmpty(builder.de53_SecControlInfo) ? 0 : int.Parse(builder.de53_SecControlInfo);
                NetMgtInfoCode = builder.de70_NetMgtInfoCode;
                RecvInstIdCode = int.Parse(builder.de100_RecvInstIdCode);                                               
            }
            catch (Exception e)
            {
                _log.Error(e.Message + " Stack: " + e.StackTrace);
                throw new ArgumentException(e.Message + " Stack: " + e.StackTrace);
            }
        }

        public void ValidateKeys(string kekr)
        {
            var krsReceived = this.AddtlDataPriv;
            var hostCrypt = new HostCryptography();
            var validationResponse = hostCrypt.Generate_KEKr_Validation_Response(kekr, krsReceived);

            if (validationResponse["ErrorCode"] == "00")
            {
                this.AddtlDataPriv = validationResponse["KRr"];
                this.Mti = "0810";
                this.RespCode = "00";                
            }
            else
            {
                _log.Error("0810 KRr Response Error");
                this.RespCode = "30"; //format error
            }
        }
    }
}