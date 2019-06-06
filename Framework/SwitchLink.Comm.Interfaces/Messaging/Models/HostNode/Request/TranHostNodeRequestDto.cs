using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Request
{
    [Serializable]
    public class TranHostNodeRequestDto : BaseHostNodeRequestDto
    {
        public string P3ProcessingCode { get; set; } // from triton
        public int P4AmountTran { get; set; } // from triton
        public DateTime P7TransmitDt { get; set; }
        public DateTime P12TimeLocalTran { get; set; }
        public DateTime P13DateLocalTran { get; set; }
        public DateTime P15DateSettlement { get; set; }
        public string P18MerchantType { get; set; }
        public string P25PosConditionCode { get; set; }
        public int P28AmtTranFee { get; set; } // from triton
        public string P35Track2 { get; set; } // from triton  
        public string P41TerminalId { get; set; } // from triton
        public string P43NameLocation { get; set; }
        public string P52PinBlock { get; set; } // from triton
        public int P57AmountCash { get; set; } // from triton
    }
}
