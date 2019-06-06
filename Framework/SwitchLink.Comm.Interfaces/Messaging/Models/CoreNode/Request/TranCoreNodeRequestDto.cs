using System;
using System.Text;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Request
{
    [Serializable]
    public class TranTritonNodeRequestDto : BaseCoreNodeRequestDto
    {
        public string Track2 { get; protected set; }
        public int Amount1 { get; protected set; }
        public int Amount2 { get; protected set; } // Surcharge
        public string PinBlock { get; protected set; }

        public TranTritonNodeRequestDto(string terminalId, string transactionCode, int amount1, int amount2, string track2, string pinBlock, long tranSeqNo, string miscellaneousX, string posCondCode)
        {
            TerminalId = terminalId;
            TransactionCode = transactionCode;
            Amount1 = amount1;
            Amount2 = amount2;
            Track2 = track2;
            PinBlock = pinBlock;
            TranSeqNo = tranSeqNo;
            MiscellaneousX = miscellaneousX;
            PosCondCode = posCondCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Track2 :" + Track2);
            builder.AppendLine("Amount1 :" + Amount1);
            builder.AppendLine("Amount2 :" + Amount2);
            builder.AppendLine("PinBlock :" + PinBlock);
            return builder.ToString();
        }
    }
}
