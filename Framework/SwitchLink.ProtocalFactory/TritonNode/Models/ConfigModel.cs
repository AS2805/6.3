using System;
using Common.Logging;
using SwitchLink.ProtocalFactory.Helper;

namespace SwitchLink.ProtocalFactory.TritonNode.Models
{
    public class ConfigModel : BaseModel
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(ConfigModel));
        private readonly RRN_Sequence _rrnSequence = new RRN_Sequence(); 
        public string EncryptedPinKey1 { get; set; }
        public string EncryptedPinKey2 { get; set; }
        public int Amount2 { get; set; }

        internal ConfigModel Create(string[] results, string request)
        {
            try
            {
                CommunicationIdentifier = results[0].Substring(0, 8);// 8 ASCII Char
                TerminalIdentifier = results[0].Substring(8, 2);// 2 ASCII Char
                SoftwareVerionNo = results[0].Substring(10, 2);// 2 ASCII Char
                EncryptionModeFlag = results[0].Substring(12, 1);// 1 ASCII Char
                InformationHeader = results[0].Substring(13, 7);// 7 ASCII Char
                TranSeqNo = _rrnSequence.TranSeqNo;
                TerminalId = results[1];
                TransactionCode = results[2];
                StatusMonitoringField = results[3];
                if (results.Length > 4 && results[4].Contains("ud"))
                    MiscellaneousX = results[4];

                Text = request; // ATM transaction request log

                return this;
            }
            catch (Exception e)
            {
                _log.Error(e.Message + " Stack: " + e.StackTrace);
                throw new ArgumentException(e.Message + " Stack: " + e.StackTrace);
            }
           
        }
    }
}
