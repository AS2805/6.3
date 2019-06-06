using System;
using Common.Logging;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using SwitchLink.Comm.Interfaces.Services;
using SwitchLink.HostNode.Communication.Services;
using SwitchLink.HostNode.Configuration;
using SwitchLink.HostNode.Controller;

namespace SwitchLink.HostNode.SocketServer
{
    public interface IHostNode
    {
        void StartListening();
        void CloseSockets();
    }

    class HostNode : IHostNode
    {
        private IScsServiceApplication _serviceApplication;
        private readonly ILog _log = LogManager.GetLogger<HostNode>();
        private readonly BankingController _bank = new BankingController();

        public void StartListening()
        {
            //Create a service application that runs on 8002 TCP port
            _serviceApplication = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(Config.ServerIpAddress, Config.ServerPort));

            //Create a HostNode RMI Service and add it to service application
            _serviceApplication.AddService<IHostNodeRequestService, HostNodeRequestService>(new HostNodeRequestService());
            //Start service application
            _serviceApplication.Start();

            _bank.CreatePartnerConnection();

            _serviceApplication.ClientConnected += Server_ClientConnected;
            _serviceApplication.ClientDisconnected += Server_ClientDisconnected;

            _log.Info(String.Format("Server HostNode Sevice Listening at: ip {0} : port {1}", Config.ServerIpAddress, Config.ServerPort));
            _log.Info("Switchlink.Switch.Service => HostNode Sevice Running...");
        }
      
        private void Server_ClientDisconnected(object sender, ServiceClientEventArgs e)
        {
            _log.Info("A client is disconnected! Client Id = " + e.Client.ClientId);
        }

        private void Server_ClientConnected(object sender, ServiceClientEventArgs e)
        {
            _log.Info("A new client is Connected. Client Id = " + e.Client.ClientId);
        }

        public void CloseSockets()
        {
            //Stop service application
            _serviceApplication.Stop();
            _bank.CloseBankConnection();
        }
    }
}
