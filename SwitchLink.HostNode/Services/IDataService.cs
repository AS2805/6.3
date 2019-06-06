using SwitchLink.Data;
using SwitchLink.ProtocalFactory.Helper;

namespace SwitchLink.HostNode.Services
{
    interface IDataService
    {
        string RetrieveCardAcceptorId();
        string RetrieveAddtlDataNat();
        int RetrieveSecurityBlock();
        int RetrieveAcqInstIdCode();
        int RetrieveFwdInstIdCode();
    }

    class DataService : IDataService
    {
        private readonly MsgHelper _helper = new MsgHelper();
        private readonly string _terminal;
        private readonly long _tranSeqNo;
        private readonly int _authorizationNum;

        public DataService(string terminal, long tranSeqNo, int authorizationNum)
        {
            _terminal = terminal;
            _tranSeqNo = tranSeqNo;
            _authorizationNum = authorizationNum;
        }

        public string RetrieveCardAcceptorId()
        {
            using (var tran = new TransactionData())
            {
                string val = tran.RetrieveCardAcceptorIdForReversal(_terminal, _tranSeqNo, _authorizationNum);
                return val.Trim();
            }
        }

        public string RetrieveAddtlDataNat()
        {
            using (var tran = new TransactionData())
            {
                string val = tran.RetrieveAddtlDataNatForReversal(_terminal, _tranSeqNo, _authorizationNum);
                return _helper.HexStringToString(val);
            }
        }

        public int RetrieveSecurityBlock()
        {
            using (var tran = new TransactionData())
            {
                string val = tran.RetrieveRetrieveSecurityBlockForReversal(_terminal, _tranSeqNo, _authorizationNum);
                return int.Parse(val);
            }
        }

        public int RetrieveAcqInstIdCode()
        {
            using (var tran = new TransactionData())
            {
                string val = tran.RetrieveAcqInstIdCodeForReversal(_terminal, _tranSeqNo, _authorizationNum);
                return int.Parse(val);
            }
        }

        public int RetrieveFwdInstIdCode()
        {
            using (var tran = new TransactionData())
            {
                string val = tran.RetrieveFwdInstIdCodeForReversal(_terminal, _authorizationNum);
                return int.Parse(val);
            }
        }
    }
}
