using System;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;

namespace BankSimulator.Configuration
{
    class Config
    {
        public static string ServerIpAddress
        {
            get
            {
                string addrString = ConfigurationManager.AppSettings["app:IpAddress"];
                if (string.IsNullOrEmpty(addrString))
                {
                    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    addrString = ipHostInfo.AddressList[0].ToString();
                }
                if (IsValidIp(addrString))
                {
                    //Valid IP, with address containing the IP
                    return addrString;
                }
                else
                {
                    //Invalid IP
                    throw new InvalidOperationException("Invalid server IP address");
                }
            }
        }

        public static int ServerPort
        {
            get
            {
                string port = ConfigurationManager.AppSettings["app:Port"];
                if (string.IsNullOrEmpty(port))
                    port = "0";
                return int.Parse(port);
            }
        }

        private static bool IsValidIp(string address)
        {
            if (!Regex.IsMatch(address, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b"))
                return false;

            IPAddress dummy;
            return IPAddress.TryParse(address, out dummy);
        }
    }
}
