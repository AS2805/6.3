using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Response
{
    [Serializable]
    public class ReversalCoreNodeResponseDto : BaseCoreNodeResponseDto
    {
        public int AmountTran { get; set; }
        public int AmtTranFee { get; set; }
        public int AmountCash { get; set; }
    }
}
