using System;
using Common.Logging;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Service;
using SwitchLink.Comm.Interfaces.Services;
using SwitchLink.CoreNode.Services;

namespace SwitchLink.CoreNode.SocketServer
{
    public interface ICoreNode
    {
        void StartListening(string ip = "", int port = 8001);
        void CloseSockets();
    }

    class CoreNode : ICoreNode
    {
        private IScsServiceApplication _serviceApplication;
        private readonly ILog _log = LogManager.GetLogger<CoreNode>();

        public void StartListening(string ip, int port)
        {

            //Create a service application that runs on 8001 TCP port
            _serviceApplication = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(ip, port));

            //Create a CoreNode RMI Service and add it to service application
            _serviceApplication.AddService<ICoreNodeRequestService, CoreNodeRequestService>(new CoreNodeRequestService());
            //Start service application
            _serviceApplication.Start();

            _serviceApplication.ClientConnected += Server_ClientConnected;
            _serviceApplication.ClientDisconnected += Server_ClientDisconnected;


            _log.Info(String.Format("Server CoreNode Sevice Listening at: ip {0} : port {1}", ip, port));
            _log.Info("Switchlink.Switch.Service => CoreNode Sevice Running...");
        }

        private void Server_ClientDisconnected(object sender, ServiceClientEventArgs e)
        {
            _log.Info("A client is disconnected! Client Id = " + e.Client.ClientId);
        }

        private void Server_ClientConnected(object sender, ServiceClientEventArgs e)
        {
            _log.Info("A new client is connected. Client Id = " + e.Client.ClientId);
        }

        public void CloseSockets()
        {
            //Stop service application
            _serviceApplication.Stop();
        }

       
    }
}
