using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Response
{
    [Serializable]
    public class ConfigCoreNodeResponseDto : BaseCoreNodeResponseDto
    {
        public int Amount2 { get; set; } // Surcharge
    }
}
