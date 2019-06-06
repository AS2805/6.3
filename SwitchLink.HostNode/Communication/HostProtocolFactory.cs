using Hik.Communication.Scs.Communication.Protocols;
using SwitchLink.HostNode.Communication.Protocols;

namespace SwitchLink.HostNode.Communication
{
    class HostProtocolFactory : IScsWireProtocolFactory
    {
        public IScsWireProtocol CreateWireProtocol()
        {
            return new HostProtocol();
        }
    }
}
