using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwitchLink.ProtocalFactory.AS2805;

namespace UnitTest
{
    [TestClass]
    public class Format
    {
        private static object _locker = new object();
        private static int _test;
        private const int _max = 10000000;

        [TestMethod]
        public void FormatTest()
        {
            var date = DateTime.Now;
            var d = DateTime.Now.AddDays(+1);
        }

        [TestMethod]
        public void SeqNum()
        {
        //    var s1 = Stopwatch.StartNew();
        //    for (int i = 0; i < _max; i++)
        //    {
        //        lock (_locker)
        //        {
        //            _test++;
        //        }
        //    }
        //    s1.Stop();
            var s2 = Stopwatch.StartNew();
            for (int i = 0; i < _max; i++)
            {
                if (i == 1000)
                {
                    _test = 0;
                }

                Interlocked.Increment(ref _test);
            }
            s2.Stop();
            Console.WriteLine(_test);
            //Console.WriteLine(((double) (s1.Elapsed.TotalMilliseconds*1000000)/ _max).ToString("0.00 ns"));
            Console.WriteLine(((double) (s2.Elapsed.TotalMilliseconds*1000000)/ _max).ToString("0.00 ns"));
            Console.Read();

        }

        [TestMethod]
        public void DateTimeExtensions()
        {
            string data = "0104102141";
           // var result = data.Parse_de7_TransDttm_ToDateTime();
        }

        [TestMethod]
        public void Parse_LLVAR_ToInt()
        {
        //    //string data = "06579944";
        //    //var result = data.Parse_LLVAR_ToInt();
        //    String a = "ffffud5285+365894";
        //    String b = a.Substring(a.IndexOf("ud", StringComparison.Ordinal) + 2);
            string track2 = "4902370000002348=121210111234123";
            string accNumber = track2.Substring(0, track2.IndexOf("=", StringComparison.Ordinal));

            string accNumber1 = track2.Substring(track2.IndexOf("=", StringComparison.Ordinal), 16);
            var ss = accNumber.Substring(0, 4) + "************" + accNumber.Substring(accNumber.Length - 4, 4);

            
        }

        [TestMethod]
        public void Parse_de15_date_settlement()
        {
            var helper = new As2805Extensions();
            string data = "0217";
            var result = helper.Parse_de15_date_settlement(data);
        }

        [TestMethod]
        public void CardNo()
        {
            string status = "VA6.00.12W";
            status += "V02.70.10 ";
            status += "V06.01.12 ";
            status += "0  0T  00 ";
            status += new Random().Next(0, 2).ToString();//   #TODO: change status automatically - Dispenser
            status += new Random().Next(0, 2).ToString();//   #TODO: change status automatically - Comms System
            status += "0     ";
            status += "000";// #TODO: change status automatically - Terminal Error Code
            status += new Random().Next(0, 10).ToString("D3");// #TODO: change status automatically - Comms failures
            status += "02K0288000000002K028800000000000000000000000000000000000000";
            
        }

        [TestMethod]
        public void RandomTranCode()
        {
            string[] val = { "11", "12", "15" };
            var result = val[new Random().Next(val.Length)];
        }

    }
}
