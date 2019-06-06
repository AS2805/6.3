using System;
using SwitchLink.ProtocalFactory.AS2805;

namespace SwitchLink.ProtocalFactory.HostNode.Models
{
    public class ReversalRequestModel : ReversalModel
    {
        public ReversalRequestModel() { }

        public ReversalRequestModel(string mti, string processingCode, int transactionAmount, DateTime transactionDate,
            int stan, DateTime timeLocal, DateTime dateLocation, DateTime dateSettlement, string posConditionCode, int transactionFee, int acqInstIdCode, int fwdInstIdCode,
            string track2, long retRefNo, string terminalId, string cardAcptIdCode, string nameLocation,
            string addtlDataNat, int secControlInfo, int amountCash, string mac128, 
            string miscellaneousX)
        {
            Mti = mti;
            ProcessingCode = processingCode;
            AmountTran = transactionAmount;
            TranDate = transactionDate;
            Stan = stan;
            TimeLocalTran = timeLocal;
            DateLocalTran = dateLocation;
            DateSettlement = dateSettlement;
            PosConditionCode = posConditionCode;
            AmtTranFee = transactionFee;
            AcqInstIdCode = acqInstIdCode;
            FwdInstIdCode = fwdInstIdCode;
            Track2 = track2;
            TerminalId = terminalId;
            CardAcptIdCode = cardAcptIdCode;
            NameLocation = nameLocation;
            AddtlDataNat = addtlDataNat;
            SecControlInfo = secControlInfo;
            AmountCash = amountCash;
            Mac128 = mac128;
            TranSeqNo = retRefNo;
            MiscellaneousX = miscellaneousX;
        }

        public ReversalRequestModel Create(As2805 builder)
        {
            if (builder.mti != "0420")
                throw new InvalidOperationException("Response can be only built from Request with MTI: 0420");

            As2805Extensions helper = new As2805Extensions();
            
            Mti = builder.mti;
            ProcessingCode = helper.Parse_TransactionCode(builder.de3_ProcCode);
            AmountTran = helper.Parse_Amount1(builder.de4_AmtTxn);
            TranDate = helper.Parse_de7_TransDttm_ToDateTime(builder.de7_TransDttm);
            Stan = int.Parse(builder.de11_STAN);
            AmtTranFee = helper.Parse_Amount2(builder.de28_AmtTxnFee);
            TerminalId = builder.de41_CardAcptTermId;
            CardAcptIdCode = helper.Parse_Card_acceptor_id(builder.de42_CardAcptIdCode);
            SecControlInfo = string.IsNullOrEmpty(builder.de53_SecControlInfo) ? 0 : int.Parse(builder.de53_SecControlInfo);
            AmountCash = helper.Parse_AmountCash(builder.de57_AmtCash);

            return this;
        }

        public override string ToString()
        {
            return string.Format("MTI: {0}, Stan: {1}", Mti, Stan);
        }
    }
}
