using System;
using SwitchLink.ProtocalFactory.AS2805;

namespace SwitchLink.ProtocalFactory.HostNode.Models
{
    public class ReversalResponseModel : ReversalModel
    {
        public ReversalResponseModel Create(As2805 builder)
        {
            if (builder.mti != "0430")
                throw new InvalidOperationException("Response can be only built from Request with MTI: 0420");

            As2805Extensions helper = new As2805Extensions();
            Mti = builder.mti;
            ProcessingCode = helper.Parse_TransactionCode(builder.de3_ProcCode);
            Stan = int.Parse(builder.de11_STAN);
            TranDate = helper.Parse_de7_TransDttm_ToDateTime(builder.de7_TransDttm);
            FwdInstIdCode = int.Parse(builder.de33_FwdInstIdCode);
            RespCode = builder.de39_RespCode;
            SecControlInfo = string.IsNullOrEmpty(builder.de53_SecControlInfo) ? 0 : int.Parse(builder.de53_SecControlInfo);
            AmountTran = helper.Parse_Amount1(builder.de4_AmtTxn);
            AmtTranFee = helper.Parse_Amount2(builder.de28_AmtTxnFee);
            AmountCash = helper.Parse_AmountCash(builder.de57_AmtCash);

            return this;
        }

        public override string ToString()
        {
            return string.Format("MTI: {0}, Stan: {1}, RespCode: {2}", Mti, Stan, RespCode);
        }
    }
}
