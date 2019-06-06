using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Response
{
    [Serializable]
    public class ConfigHostNodeResponseDto : BaseHostNodeResponseDto
    {
        public int Amount2 { get; set; } // Surcharge
    }
}
