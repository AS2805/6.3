using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Request
{
    [Serializable]
    public class BaseHostNodeRequestDto
    {
        public long TranSeqNo { get; set; }// from triton
        public string MiscellaneousX { get; set; }// from triton
    }
}
