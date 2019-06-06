using Hik.Communication.Scs.Communication.Protocols;
using SwitchLink.TritonNode.Communication.Protocols;

namespace SwitchLink.TritonNode.Communication
{
    class AtmProtocolFactory : IScsWireProtocolFactory
    {
        public IScsWireProtocol CreateWireProtocol()
        {
            return new AtmProtocol();
        }
    }
}
