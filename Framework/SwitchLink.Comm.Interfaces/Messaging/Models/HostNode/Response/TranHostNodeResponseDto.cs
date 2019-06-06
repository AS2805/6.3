using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Response
{
    [Serializable]
    public class TranHostNodeResponseDto : BaseHostNodeResponseDto
    {
        public int AmountTran { get; set; }
        public int AmtTranFee { get; set; }
        public int AmountCash { get; set; }
    }
}
