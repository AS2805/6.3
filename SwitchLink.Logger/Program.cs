using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SwitchLink.Logger
{
    public class Program
    {
        private static ILog log;
         static void Main(string[] args)
        {
            try
            {
                var log = Logger.LoggingInstance;

                log.Debug("Debug Log");
                log.Debug("Debug Log");
                //log.Debug("Debug Log");
                log.Debug("Debug Log");
                log.Debug("Debug Log");
                log.Debug("Debug Log");
                log.Debug("Debug Log");
                log.Debug("Debug Log");
              
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }
    }
}
