using Hik.Communication.Scs.Communication.Messages;

namespace BankSimulator.Communication.Messages
{
    public class BankRawMessage : IScsMessage
    {
        public string MessageId { get; private set; }
        public string RepliedMessageId { get; set; }
        public byte[] RawBytes { get; private set; }

        public BankRawMessage(byte[] rawBytes)
        {
            RawBytes = rawBytes;
        }
    }
}
