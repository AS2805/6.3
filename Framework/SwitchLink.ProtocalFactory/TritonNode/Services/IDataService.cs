using SwitchLink.Data;

namespace SwitchLink.ProtocalFactory.TritonNode.Services
{
    interface IDataService
    {
        long GetRrnForReversal(string terminal, int atmSeqNo);
        int GetAuthorizationNumForReversal(string terminal, int atmSeqNo);
    }

    class DataService : IDataService
    {
        public long GetRrnForReversal(string terminal, int atmSeqNo)
        {
            using (var tran = new TransactionData())
            {
                return tran.RetrieveAuthorizationRrnForReversal(terminal, atmSeqNo);
            }
        }

        public int GetAuthorizationNumForReversal(string terminal, int atmSeqNo)
        {
            using (var tran = new TransactionData())
            {
                return tran.RetrieveAuthorizationNumForReversal(terminal, atmSeqNo);
            }
        }
    }
}
