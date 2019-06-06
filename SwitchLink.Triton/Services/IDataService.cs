using SwitchLink.Data;

namespace SwitchLink.TritonNode.Services
{
    interface IDataService
    {
        long GetRrnForReversal();
        int GetAuthorizationNumForReversal();
    }

    class DataService : IDataService
    {
        private readonly string _terminal;
        private readonly int _atmSeqNo;

        public DataService(string terminal, int atmSeqNo)
        {
            _terminal = terminal;
            _atmSeqNo = atmSeqNo;
        }

        public long GetRrnForReversal()
        {
            using (var tran = new TransactionData())
            {
                return tran.RetrieveAuthorizationRrnForReversal(_terminal, _atmSeqNo);
            }
        }

        public int GetAuthorizationNumForReversal()
        {
            using (var tran = new TransactionData())
            {
                return tran.RetrieveAuthorizationNumForReversal(_terminal, _atmSeqNo);
            }
        }
    }
}
