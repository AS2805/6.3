using System;
using SwitchLink.Data;

namespace SwitchLink.ProtocalFactory.Helper
{
    public class RRN_Sequence
    {
       
        public int GetStan
        {
            get
            {
                using (var tran = new TransactionData())
                {
                    return Convert.ToInt32(tran.GetSequenceNumbers("tran_stan"));
                }
            }
        }

        public Int64 TranSeqNo
        {
            get
            {
                using (var tran = new TransactionData())
                {
                    return tran.GetSequenceNumbers("tran_seq");
                }
            }
        }
    }
    //public static class RRN_Sequence
    //{
    //    private static int _lastSeqNo;
    //    private static int _retRefNo;
    //    private static readonly object StanLock = new object();
    //    private static readonly object TranSeqNoLock = new object();

    //    public static int GetStan
    //    {
    //        get
    //        {
    //            lock (StanLock)
    //            {
    //                if (_lastSeqNo == 999999)
    //                    _lastSeqNo = 0;
    //            }
    //            return Interlocked.Increment(ref _lastSeqNo);
    //        }
    //    }

    //    public static int TranSeqNo
    //    {
    //        get
    //        {
    //            lock (TranSeqNoLock)
    //            {
    //                if (_retRefNo == 999999999999)
    //                    _retRefNo = 0;
    //            }
    //            return Interlocked.Increment(ref _retRefNo);
    //        }
    //    }
    //}
}
