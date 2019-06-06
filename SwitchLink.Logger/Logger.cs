using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;

namespace SwitchLink.Logger
{

   
        public static class Logger
        {
            private static readonly ILog Log = LogManager.GetLogger(typeof(Logger));
            private const string SourceIdIdReference = "SwitchId";
            public static readonly int SourceId;

            public static ILog LoggingInstance
            {
                get { return Log; }
            }

            static Logger()
            {
                SourceId = GetAndValidateAppId();

                GlobalContext.Properties[SourceIdIdReference] = SourceId;
                log4net.Config.XmlConfigurator.Configure();
            }

            private static int GetAndValidateAppId()
            {
                var clientId = "1";

                int appIdPlaceholder;
                if (string.IsNullOrEmpty(clientId) || !int.TryParse(clientId, out appIdPlaceholder))
                {
                    throw new ArgumentException();
                }
                return appIdPlaceholder;

            }
        }
    }

