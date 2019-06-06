using System;
using Common.Logging;
using SwitchLink.Cryptography.HostCryptography;
using SwitchLink.ProtocalFactory.AS2805;

namespace SwitchLink.ProtocalFactory.HostNode.Models
{
    public class SignonRequestModel : NetworkManagementModel
    {
        private readonly As2805 _builder;
        private readonly ILog _log = LogManager.GetLogger(typeof(SignonRequestModel));
        public SignonRequestModel(As2805 builder)
        {
            _builder = builder;
            try
            {
                As2805Extensions helper = new As2805Extensions();
                Mti = "0800";
                NetMgtInfoCode = "001";

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


        public SignonResponseModel GetValdationResponse(string kekr)
        {
            var responseModel=new SignonResponseModel(_builder);

            responseModel.ValidateKeys(kekr);

            return responseModel;
        }

        public void ValidateKeys(string keks, int stan)
        {
            var hostCrypt = new HostCryptography();
            var keksValidationRequest = hostCrypt.Generate_KEKs_Validation_Request(keks);

            if (keksValidationRequest["ErrorCode"] == "00")
            {
                string krsSent;
                keksValidationRequest.TryGetValue("KRs", out krsSent);                
                this.Stan = stan;
                this.FwdInstIdCode = 579944;
                this.NetMgtInfoCode = "001";
                this.RecvInstIdCode = 61100016;
                this.AddtlDataPriv = krsSent;
            }
            else
            {
                _log.Error("0800 KEKs Validation Request Error");    
            }            
        }
    }
}