using System;
using Common.Logging;
using SwitchLink.ProtocalFactory.TritonNode.Services;

namespace SwitchLink.ProtocalFactory.TritonNode.Models
{
    public class ReversalModel : BaseModel
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(ReversalModel));
        private readonly IDataService _svcData = new DataService();

        public string Track2 { get; set; }
        public int AuthorizationTranAmount { get; set; }
        public int AuthorizationSurAmount { get; set; }
        public int AuthorizationDispensedAmount { get; set; }
        public string Miscellaneous1 { get; set; }
        public string Miscellaneous2 { get; set; }
        public string OriginalDataElements { get; set; }
        public string MessageAuthenticationCode { get; set; }
        public string AuthorizationCode { get; set; }
        public string AuthorizationDesc { get; set; }
        public string AuthorizationAction { get; set; }
        public int AuthorizationNum { get; set; }
        public int AtmSeqNo { get; set; }
        public string PosCondCode { get; set; }

        internal ReversalModel Create(string[] results, string request)
        {
            try
            {
                CommunicationIdentifier = results[0].Substring(0, 8);// 8 ASCII Char
                TerminalIdentifier = results[0].Substring(8, 2);// 2 ASCII Char
                SoftwareVerionNo = results[0].Substring(10, 2);// 2 ASCII Char
                EncryptionModeFlag = results[0].Substring(12, 1);// 1 ASCII Char
                InformationHeader = results[0].Substring(13, 7);// 7 ASCII Char
                AtmSeqNo = int.Parse(results[3]);
                TerminalId = results[1];
                TransactionCode = results[2];
                Track2 = results[4];
                AuthorizationTranAmount = int.Parse(results[5]);
                AuthorizationSurAmount = int.Parse(results[6]);
                AuthorizationDispensedAmount = int.Parse(results[7]);
                StatusMonitoringField = results[8];
                PosCondCode = "41";  // Cash Dispenser Machine (ATM)
                if (results.Length > 9 && results[9].Contains("ud"))
                    MiscellaneousX = results[9];

                Text = request; // ATM transaction request log

                TranSeqNo = _svcData.GetRrnForReversal(TerminalId, AtmSeqNo);
                AuthorizationNum = _svcData.GetAuthorizationNumForReversal(TerminalId, AtmSeqNo);

                return this;
            }
            catch (Exception e)
            {
                _log.Error(e.Message + " Stack: " + e.StackTrace);
                throw new ArgumentException(e.Message + " Stack: " + e.StackTrace);
            }
        }

        public ReversalModel Create(TransactionModel tran)
        {
            try
            {
                CommunicationIdentifier = tran.CommunicationIdentifier;
                TerminalIdentifier = tran.TerminalIdentifier;
                SoftwareVerionNo = tran.SoftwareVerionNo;
                EncryptionModeFlag = tran.EncryptionModeFlag;
                InformationHeader = tran.InformationHeader;
                TerminalId = tran.TerminalId;
                TransactionCode = "29";
                AtmSeqNo = tran.AtmSeqNo;
                Track2 = tran.Track2;
                AuthorizationTranAmount = tran.Amount1;
                AuthorizationSurAmount = tran.Amount2;
                Miscellaneous1 = tran.Miscellaneous1;
                Miscellaneous2 = tran.Miscellaneous2;
                StatusMonitoringField = tran.StatusMonitoringField;
                MiscellaneousX = tran.MiscellaneousX;
                PosCondCode = "41";  // Cash Dispenser Machine (ATM)

                TranSeqNo = _svcData.GetRrnForReversal(TerminalId, AtmSeqNo);
                AuthorizationNum = _svcData.GetAuthorizationNumForReversal(TerminalId, AtmSeqNo);

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
