using System;
using Common.Logging;
using SwitchLink.ProtocalFactory.Helper;

namespace SwitchLink.ProtocalFactory.TritonNode.Models
{
    public class TransactionModel : BaseModel
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(TransactionModel));
        private readonly RRN_Sequence _rrnSequence = new RRN_Sequence(); 
        public string Track2 { get; set; }
        public int Amount1 { get; set; }
        public int Amount2 { get; set; }
        public string PinBlock { get; set; }
        public string Miscellaneous1 { get; set; }
        public string Miscellaneous2 { get; set; }
        public string AuthorizationCode { get; set; }
        public string AuthorizationDesc { get; set; }
        public string AuthorizationAction { get; set; }
        public int AuthorizationNum { get; set; }
        public int Balance { get; set; }
        public int AtmSeqNo { get; set; }
        public string PosCondCode { get; set; }

        internal TransactionModel Create(string[] results, string request)
        {
            try
            {
                CommunicationIdentifier = results[0].Substring(0, 8);// 8 ASCII Char
                TerminalIdentifier = results[0].Substring(8, 2);// 2 ASCII Char
                SoftwareVerionNo = results[0].Substring(10, 2);// 2 ASCII Char
                EncryptionModeFlag = results[0].Substring(12, 1);// 1 ASCII Char
                InformationHeader = results[0].Substring(13, 7);// 7 ASCII Char
                TerminalId = results[1];
                TransactionCode = results[2];
                TranSeqNo = _rrnSequence.TranSeqNo;
                AtmSeqNo = int.Parse(results[3]);
                Track2 = results[4];
                Amount1 = int.Parse(results[5]);
                Amount2 = int.Parse(results[6]);
                PinBlock = results[7];
                Miscellaneous1 = results[8];
                Miscellaneous2 = results[9];
                StatusMonitoringField = results[10];
                PosCondCode = "41";  // Cash Dispenser Machine (ATM)
                if (results.Length > 11 && results[11].Contains("ud"))
                    MiscellaneousX = results[11];

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
