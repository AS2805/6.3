using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Response
{
    [Serializable]
    public class HostTotalCoreNodeResponseDto : BaseCoreNodeResponseDto
    {
        public int Amount1 { get; set; }
        public int Amount2 { get; set; }
    }
}
