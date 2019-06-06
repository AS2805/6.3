using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Response
{
    [Serializable]
    public class BaseCoreNodeResponseDto
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string ResponseAction { get; set; }
        public int Stan { get; set; }
        public long TranSeqNo { get; set; }
    }
}
