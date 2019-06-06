using Hik.Communication.Scs.Communication.Messages;

namespace SwitchLink.TritonNode.Communication.Messages
{
    public class AtmRawMessage : IScsMessage
    {
        public string MessageId { get; private set; }
        public string RepliedMessageId { get; set; }
        public byte[] RawBytes { get; private set; }

        public AtmRawMessage(byte[] rawBytes)
        {
            RawBytes = rawBytes;
        }
    }
}
