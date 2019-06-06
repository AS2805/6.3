using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Request
{
    [Serializable]
    public class ReversalHostNodeRequestDto : BaseHostNodeRequestDto
    {
        public string P3ProcessingCode { get; set; }
        public int P4AmountTran { get; set; }
        public DateTime P7TransmitDt { get; set; }
        public DateTime P12TimeLocalTran { get; set; }
        public DateTime P13DateLocalTran { get; set; }
        public DateTime P15DateSettlement { get; set; }
        public string P25PosConditionCode { get; set; }
        public int P28AmtTranFee { get; set; }
        public string P35Track2 { get; set; }
        public string P41TerminalId { get; set; }
        public string P43NameLocation { get; set; }
        public int P57AmountCash { get; set; }
        public int AuthorizationNum { get; set; }
    }
}
