using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Response
{
    [Serializable]
    public class HostTotalHostNodeResponseDto : BaseHostNodeResponseDto
    {
        public int Amount1 { get; set; }
        public int Amount2 { get; set; }
    }
}
