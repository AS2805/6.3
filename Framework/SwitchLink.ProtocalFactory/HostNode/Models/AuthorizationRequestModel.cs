using System;
using SwitchLink.ProtocalFactory.AS2805;

namespace SwitchLink.ProtocalFactory.HostNode.Models
{
    public class AuthorizationRequestModel : TransactionModel
    {
        public AuthorizationRequestModel() { }

        public AuthorizationRequestModel(string mti, string processingCode, int transactionAmount, DateTime transactionDate,
            int stan, DateTime timeLocal, DateTime dateLocation, DateTime dateSettlement, string merchantType, string posConditionCode, int transactionFee, int acqInstIdCode, 
            string track2, long retRefNo, string terminalId, string cardAcptIdCode, string nameLocation,
            string addtlDataNat, string pinBlock, int secControlInfo, int amountCash, string mac64, 
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
            MerchantType = merchantType;
            PosConditionCode = posConditionCode;
            AmtTranFee = transactionFee;
            AcqInstIdCode = acqInstIdCode;
            Track2 = track2;
            TerminalId = terminalId;
            CardAcptIdCode = cardAcptIdCode;
            NameLocation = nameLocation;
            AddtlDataNat = addtlDataNat;
            PinBlock = pinBlock;
            SecControlInfo = secControlInfo;
            AmountCash = amountCash;
            Mac64 = mac64;
            TranSeqNo = retRefNo;
            MiscellaneousX = miscellaneousX;
        }

        internal AuthorizationRequestModel Create(As2805 builder)
        {
            if (builder.mti != "0200")
                throw new InvalidOperationException("Response can be only built from Request with MTI: 0200");

            As2805Extensions helper = new As2805Extensions();
            Mti = builder.mti;
            ProcessingCode = helper.Parse_TransactionCode(builder.de3_ProcCode);
            AmountTran = helper.Parse_Amount1(builder.de4_AmtTxn);
            TranDate = helper.Parse_de7_TransDttm_ToDateTime(builder.de7_TransDttm);
            Stan = int.Parse(builder.de11_STAN);
            DateSettlement = helper.Parse_de15_date_settlement(builder.de15_DateSetl);
            AmtTranFee = helper.Parse_Amount2(builder.de28_AmtTxnFee);
            AcqInstIdCode = int.Parse(builder.de32_AcqInstIdCode);
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