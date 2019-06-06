using System;
using System.ServiceProcess;
using Common.Logging;
using SwitchLink.HostNode.Configuration;
using SwitchLink.HostNode.SocketServer;

namespace SwitchLink.HostNode
{
    partial class EntryService : ServiceBase
    {
        private readonly IHostNode _sc = new SocketServer.HostNode();
        private readonly ILog _log = LogManager.GetLogger<EntryService>();

        public EntryService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _sc.StartListening();
            }
            catch (Exception ex)
            {
                //_log.Error(ex.Message);
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
