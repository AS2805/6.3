using System;
using System.Collections.Generic;
using AtmSimulatorWinForm;
using Common.Logging;
using UnitTest.Helper;

namespace AtmSimulatorConsApp
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger<Program>();

        static void Main(string[] args)
        {
            AtmRequestHelper atmRequestHelper = new AtmRequestHelper();
            string ip = "127.0.0.1";
            int port = 8000;
            Log.Debug(string.Format("Connect to {0}, Port {1}", ip, port));
            string isContinue = "y";

            while (isContinue == "y")
            {
                Console.WriteLine(@"How many ATM ?:");
                string input = Console.ReadLine(); // Read string from console

                int count;

                if (int.TryParse(input, out count)) // Try to parse the string as an integer
                {
                    string m1 = "<STX>00000000td2W0       <FS>S9111111       <FS>11<FS>";
                    string m3 = "<FS>4089670000392726=17112011000017980000<FS>00008900<FS>00000200<FS>76728398F76ED27D<FS><FS><FS>VA6.00.12WV02.70.10 V06.01.12 0  0T  00 100     00000302K0288000000002K028800000000000000000000000000000000000000<FS><FS><FS><ETX>";
                    for (int i = 1; i <= count; i++)
                    {
                        string m2 = i.ToString("D4");
                        byte[] rawMessageBytes = atmRequestHelper.BuildMessageBytesForAtmRequest(m1 + m2 + m3); // Bulid byte
                        ConnectionServices svc = new ConnectionServices();
                        svc.Connect(i, ip, port);
                        svc.Send(rawMessageBytes);
                    }
                }
                else
                {
                    Console.WriteLine("Not an integer!");
                }
                Console.WriteLine("Input y to continue");
                isContinue = Console.ReadLine();
                Console.Clear();
            }
           
        }
    }
}
