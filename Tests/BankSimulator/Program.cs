using System;
using System.ServiceProcess;

namespace BankSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                var svc = new EntryService();
                svc.StartConsole(args);
                Console.ReadKey();
                svc.StopConsole();
            }
            else
            {
                var servicesToRun = new ServiceBase[]
                {
                    new EntryService()
                };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
