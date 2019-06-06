using System.Collections.Generic;
using System.IO;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.Scs.Communication.Protocols;

namespace AtmSimulator
{
    class AtmProtocol : IScsWireProtocol
    { 
        private MemoryStream _receiveMemoryStream;

        public AtmProtocol()
        {
            _receiveMemoryStream = new MemoryStream();
        }

        public byte[] GetBytes(IScsMessage message)
        {
            return ((AtmRawMessage)message).RawBytes;
        }

        public IEnumerable<IScsMessage> CreateMessages(byte[] receivedBytes)
        {
            _receiveMemoryStream.Write(receivedBytes, 0, receivedBytes.Length);
            var messages = new List<IScsMessage>();

            while (ReadSingleMessage(messages, receivedBytes)) { }

            return messages;
        }

        private bool ReadSingleMessage(List<IScsMessage> messages, byte[] receivedBytes)
        {
            messages.Add(DeserializeMessage(receivedBytes));
            return false;
        }

        private IScsMessage DeserializeMessage(byte[] receivedBytes)
        {
            return new AtmRawMessage(receivedBytes);
        }
      
        public void Reset()
        {
            if (_receiveMemoryStream.Length > 0)
            {
                _receiveMemoryStream = new MemoryStream();
            }
        }
    }
}
