using System;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Request
{
    [Serializable]
    public class ReversalTritonNodeRequestDto : BaseCoreNodeRequestDto
    {
        public string Track2 { get; protected set; }
        public int Amount1 { get; protected set; }
        public int Amount2 { get; protected set; } // Surcharge
        public int Amount3 { get; protected set; } // Surcharge
        public string PinBlock { get; protected set; }
        public int AuthorizationNum { get; protected set; }
        public ReversalTritonNodeRequestDto(string terminalId, string transactionCode, int amount1, int amount2, int amount3, string track2, long tranSeqNo, string miscellaneousX, string posCondCode, int authorizationNum)
        {
            TerminalId = terminalId;
            TransactionCode = transactionCode;
            Amount1 = amount1;
            Amount2 = amount2;
            Amount3 = amount3;
            Track2 = track2;
            TranSeqNo = tranSeqNo;
            MiscellaneousX = miscellaneousX;
            PosCondCode = posCondCode;
            AuthorizationNum = authorizationNum;
        }
    }
}
