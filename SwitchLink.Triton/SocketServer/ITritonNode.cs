using System;
using System.Collections.Generic;
using Common.Logging;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.Scs.Server;
using SwitchLink.TritonNode.Communication;
using SwitchLink.TritonNode.Communication.Controller;

namespace SwitchLink.TritonNode.SocketServer
{
    public interface ITritonNode
    {
        void StartListening(string ip = "", int port = 8000);
        void CloseSockets();
    }

    class TritonNode : ITritonNode
    {
        private readonly ILog _log = LogManager.GetLogger<TritonNode>();
        private readonly IDictionary<string, IAtmCommunicationController> _clientLists = new Dictionary<string, IAtmCommunicationController>();
        private IScsServer _server;

        public void StartListening(string ip, int port)
        {
            //Create a server that listens 8000 TCP port for incoming connections
            _server = ScsServerFactory.CreateServer(new ScsTcpEndPoint(ip, port));
            _server.WireProtocolFactory = new AtmProtocolFactory();
            //Register events of the server to be informed about clients
            _server.ClientConnected += Server_ClientConnected;
            _server.ClientDisconnected += Server_ClientDisconnected;
            _server.Start(); //Start the server

            _log.Info(String.Format("Server Triton Node Sevice Listening at: ip {0} : port {1}", ip, port));
            _log.Info("Switchlink.Switch.Service => Triton Node Sevice Running...");
        }

        public void CloseSockets()
        {
            _server.Stop();
        }

        private void Server_ClientConnected(object sender, ServerClientEventArgs e)
        {
            _log.Info(string.Format("A new client is connected. Client Id: {0}, Address:{1} ", e.Client.ClientId, e.Client.RemoteEndPoint));
            IAtmCommunicationController atm = new AtmCommunicationController(e.Client);
            atm.StartConnectionTimeOut(60000);
            atm.SendInit();
            _clientLists.Add(e.Client.ClientId, atm);

        }

        private void Server_ClientDisconnected(object sender, ServerClientEventArgs e)
        {
            _clientLists.Remove(e.Client.ClientId);
            _log.Info(string.Format("A client is disconnected!. Client Id: {0}, Address:{1} ", e.Client.ClientId, e.Client.RemoteEndPoint));
        }
    }
}
