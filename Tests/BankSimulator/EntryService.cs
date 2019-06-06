using System;
using System.ServiceProcess;
using BankSimulator.SocketServer;
using Common.Logging;

namespace BankSimulator
{
    partial class EntryService : ServiceBase
    {
        private readonly IBankSimulator _sc = new SocketServer.BankSimulator();
        private readonly ILog _log = LogManager.GetLogger<SocketServer.BankSimulator>();
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
