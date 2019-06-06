using Hik.Communication.Scs.Communication.Protocols;

namespace BankSimulator.Communication.Protocols
{
    class BankProtocolFactory : IScsWireProtocolFactory
    {
        public IScsWireProtocol CreateWireProtocol()
        {
            return new BankProtocol();
        }
    }
}
