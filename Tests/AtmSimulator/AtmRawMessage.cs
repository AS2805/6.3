using Hik.Communication.Scs.Communication.Messages;

namespace AtmSimulator
{
    class AtmRawMessage : IScsMessage
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
