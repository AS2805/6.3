using System;
using System.Text;

namespace SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Response
{
    [Serializable]
    public class TranCoreNodeResponseDto : BaseCoreNodeResponseDto
    {
        public int AmountTran { get; set; }
        public int AmtTranFee { get; set; }
        public int AmountCash { get; set; }

        public override string ToString()
        {
           var builder = new StringBuilder();
            builder.AppendLine("Stan: " + Stan.ToString());
            builder.AppendLine("AmountTran: " + AmountTran.ToString());
            builder.AppendLine("AmtTranFee: " + AmtTranFee.ToString());
            builder.AppendLine("AmountCash: " + AmountCash.ToString());

            return builder.ToString();

        }

       
    }
}
