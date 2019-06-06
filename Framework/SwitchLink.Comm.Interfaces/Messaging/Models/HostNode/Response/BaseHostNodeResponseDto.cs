using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Response
{
    [Serializable]
    public class BaseHostNodeResponseDto
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string ResponseAction { get; set; }
        public int Stan { get; set; }
        public long TranSeqNo { get; set; }
    }
}
