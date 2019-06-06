using System;
using System.ServiceProcess;
using Common.Logging;
using SwitchLink.CoreNode.Configuration;
using SwitchLink.CoreNode.SocketServer;

namespace SwitchLink.CoreNode
{
    partial class EntryService : ServiceBase
    {
        private readonly ICoreNode _sc = new SocketServer.CoreNode();
        private readonly ILog _log = LogManager.GetLogger<EntryService>();

        public EntryService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _sc.StartListening(Config.ServerIpAddress, Config.ServerPort);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                Environment.Exit(0);
            }
        }

        protected override void OnStop()
        {
            _sc.CloseSockets();
        }

        public void StartConsole(string[] args)
        {
            OnStart(args);
        }

        public void StopConsole()
        {
            OnStop();
        }
    }
}
