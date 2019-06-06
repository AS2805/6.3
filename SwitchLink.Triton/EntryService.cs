using System;
using System.ServiceProcess;
using Common.Logging;
using SwitchLink.TritonNode.Configuration;
using SwitchLink.TritonNode.SocketServer;

namespace SwitchLink.TritonNode
{
    partial class EntryService : ServiceBase
    {
        private readonly ITritonNode _sc = new SocketServer.TritonNode();
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
