using SwitchLink.Cryptography.Communication.Services;

namespace SwitchLink.Cryptography
{
    public abstract class Cryptography
    {
        private const string HsmIp = "203.213.124.34";
        private const int HsmPort = 1500;

        public string SendMessage(string message)
        {
            using (var svr = new ConnectionServices())
            {
                svr.Connect(HsmIp, HsmPort);
                string hsmResponse = svr.SendCommand(message);
                return hsmResponse;
            }
        }
    }
}
