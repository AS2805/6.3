using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Request
{
    [Serializable]
    public class BaseCoreNodeRequestDto
    {
        public string TerminalId { get; protected set; }
        public string TransactionCode { get; protected set; }
        public long TranSeqNo { get; protected set; }
        public string MiscellaneousX { get; protected set; }
        public string PosCondCode { get; protected set; }
    }
}
