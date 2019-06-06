using System;

namespace SwitchLink.ProtocalFactory.TritonNode.Models
{
    public class BaseModel
    {
        public string CommunicationIdentifier { get; set; }
        public string TerminalIdentifier { get; set; }
        public string SoftwareVerionNo { get; set; }
        public string EncryptionModeFlag { get; set; }
        public string InformationHeader { get; set; }
        public string TerminalId { get; set; }
        public string TransactionCode { get; set; }
        public string StatusMonitoringField { get; set; }
        public string MiscellaneousX { get; set; }
        public long TranSeqNo { get; set; }

        public string Text { get; set; }
    }
}
