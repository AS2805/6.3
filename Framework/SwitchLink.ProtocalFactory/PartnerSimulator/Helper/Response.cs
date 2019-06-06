using System;

namespace SwitchLink.ProtocalFactory.PartnerSimulator.Helper
{
    class Response
    {
        private static readonly object Lock= new object();

        public static string GetRandomResponseCode(string terminalId)
        {
            lock (Lock)
            {
                string val = "00";

                if (terminalId.Contains("SM"))
                {
                    if (new Random().Next(100) < 10)
                    {
                        string[] responseCode = { "05", "06", "08", "12", "13", "14", "15", "19", "21", "30", "31", "33", "34", "36", "38", "40", "41",
                            "43", "44", "51", "52", "53", "54", "55", "56", "57", "58", "61", "64", "65", "67", "91", "92", "94", "95", "96", "97", "98" };
                        int index = new Random().Next(responseCode.Length-1);
                        val = responseCode[index];
                    }
                }

                return val;
            }    
        }
    }
}
