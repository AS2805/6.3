using System;
using Common.Logging;
using SwitchLink.ProtocalFactory.Helper;

namespace SwitchLink.ProtocalFactory.TritonNode.Models
{
    public class HostTotalModel : BaseModel
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(HostTotalModel));
        private readonly RRN_Sequence _rrnSequence = new RRN_Sequence(); 
        public int NoOfWithdrawals { get; set; }
        public int NoOfInquiries { get; set; }
        public int NoOfTransfers { get; set; }
        public int Settlement { get; set; }
        public string AuthorizationCode { get; set; }
        public int AuthorizationNum { get; set; }

        internal HostTotalModel Create(string[] results, string request)
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
                NoOfWithdrawals = int.Parse(results[4].Substring(0, 4)); //4 Numeric Total number of withdrawals since the last request for totals. Zero filled to the left.
                NoOfInquiries = int.Parse(results[4].Substring(4, 4)); //4 Numeric Total number of inquiries since the last request for totals. Zero filled to the left.
                NoOfTransfers = int.Parse(results[4].Substring(8, 4)); //4 Numeric Total number of transfers since the last request for totals. Zero filled to the left. 
                Settlement = int.Parse(results[4].Substring(12, 12)); //12 Numeric Total amount of all withdrawals. Zero filled to left, right justified. Represents the amount in the smallest possible unit of currency. 
                if (results.Length > 5 && results[5].Contains("ud"))
                    MiscellaneousX = results[5];

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
