using Hik.Communication.Scs.Communication.Messages;

namespace SwitchLink.HostNode.Communication.Messages
{
    public class HostRawMessage : IScsMessage
    {
        public string MessageId { get; private set; }
        public string RepliedMessageId { get; set; }
        public byte[] RawBytes { get; private set; }

        public HostRawMessage(byte[] rawBytes)
        {
            RawBytes = rawBytes;
        }
    }
}
