using System;
using SwitchLink.ProtocalFactory.AS2805;

namespace SwitchLink.ProtocalFactory.HostNode.Models
{
    public class AuthorizationResponseModel : TransactionModel
    {
        internal AuthorizationResponseModel Create(As2805 builder)
        {
            if (builder.mti != "0210")
                throw new InvalidOperationException("Response can be only built from Request with MTI: 0210");

            As2805Extensions helper = new As2805Extensions();
            Mti = builder.mti;
            ProcessingCode = helper.Parse_TransactionCode(builder.de3_ProcCode);
            AmountTran = helper.Parse_Amount1(builder.de4_AmtTxn);
            TranDate = helper.Parse_de7_TransDttm_ToDateTime(builder.de7_TransDttm);
            Stan = int.Parse(builder.de11_STAN);
            FwdInstIdCode = int.Parse(string.IsNullOrEmpty(builder.de33_FwdInstIdCode) ? "0" : builder.de33_FwdInstIdCode);
            RespCode = builder.de39_RespCode;
            AmtTranFee = helper.Parse_Amount2(builder.de28_AmtTxnFee);
            TerminalId = builder.de41_CardAcptTermId;
            CardAcptIdCode = helper.Parse_Card_acceptor_id(builder.de42_CardAcptIdCode);
            SecControlInfo = string.IsNullOrEmpty(builder.de53_SecControlInfo) ? 0 : int.Parse(builder.de53_SecControlInfo);
            AmountCash = helper.Parse_AmountCash(builder.de57_AmtCash);
            Mac64 = helper.Parse_Bytes_ToStringHex(builder.de64_MAC);

            return this;
        }

        public override string ToString()
        {
            return string.Format("MTI: {0}, Stan: {1}, RespCode: {2}", Mti, Stan, RespCode);
        }
    }
}