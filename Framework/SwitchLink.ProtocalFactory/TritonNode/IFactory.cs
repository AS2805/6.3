using System;
using Common.Logging;
using SwitchLink.ProtocalFactory.Helper;
using SwitchLink.ProtocalFactory.TritonNode.Models;

namespace SwitchLink.ProtocalFactory.TritonNode
{
    public interface IFactory
    {
        byte Enq { get; }
        byte Stx { get; }
        byte Eot { get; }
        byte Ack { get; }
        byte Etx { get; }
        BaseModel CreateAtmRequest(byte[] bytes);
    }

    public class Factory : IFactory
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(Factory));
        private readonly MsgHelper _helper = new MsgHelper();

        public byte Enq
        {
            get { return 0x05; }
        }

        public byte Stx
        {
            get { return 0x02; }
        }

        public byte Eot
        {
            get { return 0x04; }
        }

        public byte Ack
        {
            get { return 0x06; }
        }

        public byte Etx
        {
            get { return 0x03; }
        }

        public BaseModel CreateAtmRequest(byte[] bytes)
        {
            try
            {
                string request = _helper.AsciiOctets2String(bytes);
                _log.Debug(request);
                string[] results = _helper.BuildMsgFromAtm(bytes);

                switch (results[2])
                {
                    case "11": // Cash Withdrawal from primary checking account
                    case "12": // Cash Withdrawal from primary savings account 
                    case "15": // Cash Withdrawal from primary credit card account
                    case "31": // Primary cheque account balance inquiry
                    case "32": // Primary Savings account balance inquiry
                    case "35": // Primary Credit card balance inquiry
                        return new TransactionModel().Create(results, request);
                    case "60": // Download configuration table
                        return new ConfigModel().Create(results, request);
                    case "50": // Get host totals (do not change business day or reset totals)
                    case "51": // Get host totals (Change business date and reset totals)
                        return new HostTotalModel().Create(results, request);
                    case "29": // Reverse Previous Withdrawal (Reversal Message )
                        return new ReversalModel().Create(results, request);
                    default:
                        throw new InvalidOperationException("Request message error => Transaction code not found");
                }
            }
            catch (Exception)
            {
                throw new FormatException("Invalid ATM Request format");
            }
            
        }
    }
}
