using System;
using BankSimulator.Communication.Controllers;
using BankSimulator.Communication.Protocols;
using BankSimulator.Configuration;
using Common.Logging;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Server;

namespace BankSimulator.SocketServer
{
    public interface IBankSimulator
    {
        void StartListening();
        void CloseSockets();
    }

    class BankSimulator : IBankSimulator
    {
        private IScsServer _server;
        private readonly ILog _log = LogManager.GetLogger<BankSimulator>();

        public void StartListening()
        {
            //Create a server that listens 8000 TCP port for incoming connections
            _server = ScsServerFactory.CreateServer(new ScsTcpEndPoint(Config.ServerIpAddress, Config.ServerPort));
            _server.WireProtocolFactory = new BankProtocolFactory();
            //Register events of the server to be informed about clients
            _server.ClientConnected += Server_ClientConnected;
            _server.Start(); //Start the server

            _log.Info(String.Format("Bank Simulator Listening at: ip {0} : port {1}", Config.ServerIpAddress, Config.ServerPort));
            _log.Info("Bank Simulator Running...");
        }

        private void Server_ClientConnected(object sender, ServerClientEventArgs e)
        {
            //var simulator = new SimulatorController(e.Client); // Old
            ICoreController simulator = new CoreController(e.Client);
            simulator.Start();
        }

        public void CloseSockets()
        {
            _server.Stop();
        }
    }
}
