using Hik.Communication.Scs.Communication.Protocols;

namespace AtmSimulator
{
    class AtmProtocolFactory : IScsWireProtocolFactory
    {
        public IScsWireProtocol CreateWireProtocol()
        {
            return new AtmProtocol();
        }
    }
}
