using System;

namespace SwitchLink.ProtocalFactory.HostNode.Models
{
    public class TransactionModel : BaseModel
    {
        public DateTime DateSettlement { get; protected set; }
        public string MerchantType { get; protected set; }
        public string PosConditionCode { get; protected set; }
        public DateTime TimeLocalTran { get; protected set; }
        public DateTime DateLocalTran { get; protected set; }
        public string ProcessingCode { get; protected set; } // from triton
        public int AmountTran { get; protected set; } // from triton        
        public int AmtTranFee { get; protected set; } // from triton
        public string Track2 { get; protected set; } // from triton  
        public string TerminalId { get; protected set; } // from triton
        public string NameLocation { get; protected set; }
        public string PinBlock { get; protected set; } // from triton
        public int AmountCash { get; protected set; } // from triton
        public int AcqInstIdCode { get; protected set; }
        public string CardAcptIdCode { get; protected set; }
        public string AddtlDataNat { get; protected set; }
        public string Mac64 { get; protected set; }
        public long TranSeqNo { get; protected set; }// from triton
        public string MiscellaneousX { get; protected set; }        
    }
}
