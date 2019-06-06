using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UnitTest.Helper
{
    public class AtmRequestHelper
    {
        public byte Eot = 0x04;
        public byte Enq = 0x05;
        public byte Etx = 0x03;
        public byte Stx = 0x02;
        public byte Fs = 0x1c;
        public byte Lrc = 0x56;
        public byte Ack = 0x06;

        public byte[] BuildMessageBytesForAtmRequest(string msg)
        {
            byte Lrc = 0x56;
            byte Fs = 0x1c;
            IList<byte> bytes = new List<byte>();
            bytes.Add(Stx);

            foreach (var s in StringArrayForAtmRequest(msg))
            {
                Encoding.ASCII.GetBytes(s).ToList().ForEach(bytes.Add);
                bytes.Add(Fs);
            }
            bytes.Add(Etx);
            bytes.Add(Lrc);
            return bytes.ToArray();
        }

        private string[] StringArrayForAtmRequest(string msg)
        {
            msg = msg.Replace("<STX>", "").Replace("<ETX>", "");
            string[] parts = msg.Split(new[] { "<FS>" }, StringSplitOptions.None);
            parts = parts.Take(parts.Length - 1).ToArray();
            return parts;
        }

        private static int _lastSeqNo;

        public static int CasA;
        public static int CasB;

        private static readonly object Lock = new object();

        public static int GetSeq
        {
            get
            {
                lock (Lock)
                {
                    if (_lastSeqNo == 9999)
                        _lastSeqNo = 0;
                }
                return Interlocked.Increment(ref _lastSeqNo);
            }
        }

        public static int GetRemainingCasA
        {
            get
            {
                lock (Lock)
                {
                    if (CasA == 0)
                        CasA = 2000;
                }
                return Interlocked.Decrement(ref CasA);
            }
        }

        public static int GetRemainingCasB
        {
            get
            {
                lock (Lock)
                {
                    if (CasB == 0)
                        CasB = 2000;
                }
                return Interlocked.Decrement(ref CasB);
            }
        }
    }
}
