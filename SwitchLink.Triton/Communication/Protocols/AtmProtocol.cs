using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.Scs.Communication.Protocols;
using SwitchLink.TritonNode.Communication.Messages;

namespace SwitchLink.TritonNode.Communication.Protocols
{
    class AtmProtocol : IScsWireProtocol
    { 
        private const byte Eot = 0x04;
        private const byte Enq = 0x05;
        private const byte Etx = 0x03;
        private const byte Stx = 0x02;
        private const byte Fs = 0x1c;
        private const byte Lrc = 0x56;
        private const byte Ack = 0x06;

        private MemoryStream _receiveMemoryStream;

        public AtmProtocol()
        {
            _receiveMemoryStream = new MemoryStream();
        }

        public byte[] GetBytes(IScsMessage message)
        {
            return ((AtmRawMessage) message).RawBytes;
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

            if (receivedBytes[0] == Stx)
            {
                if (receivedBytes.Any(x => x == Etx))
                {
                    messages.Add(DeserializeMessage(receivedBytes));
                    return false;
                }
            }
            else if (receivedBytes[0] == Ack)
            {
                messages.Add(DeserializeMessage(receivedBytes));
                return false;
            }
            else if (receivedBytes[0] == Eot)
            {
                messages.Add(DeserializeMessage(receivedBytes));
                return false;
            }            
            else
            {
                throw new Exception("Incorrect message format");
            }
            return true;
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
