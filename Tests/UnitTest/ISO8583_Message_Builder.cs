using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class ISO8583_Message_Builder
    {
        [TestMethod]
        public void TestMethod1()
        {
            /// <summary>
            /// Developed by     : Bim Garcia
            /// Email address    : bimbo_garcia@yahoo.ie
            /// Telephone        : +639154132083
            /// Remarks:         : For help and suggestions, please feel free to call or email me from 09:00-15:00, GMT.
            /// </summary>



            //Set Data
            string MTI = "0200";                  //Message Type Identifier (Financial Message Request)
            string PAN = "60197105032103634";      //Primary Account No or Card No. [DE #2]
            string ProCode = "001000";            //Processing Code.[DE #3] (Purchase Transaction, Savings)
            string Amount = "15075";              //Transaction Amount.[DE #4] (X100) (ex: $150.75 = 15075 or 000000015075)  or simply remove the decimal.
            string DateTime = "0429104720";       //Transmission Date and Time.[DE #7] (format: MMDDhhmmss)
            string STAN = "456";             //System Trace Audit No.[DE #11] (456 or 000456)
            string TID = "44449999";               //Terminal ID. [DE #41]    
            string POSEM = "021";                 //Point of Service Entry Mode. [DE #22] (02 - Magnetic Stripe)



            string[] DE = new string[130];
            DE[2] = PAN;
            DE[3] = ProCode;
            DE[4] = Amount;
            DE[7] = DateTime;
            DE[11] = STAN;
            DE[22] = POSEM;
            DE[41] = TID;



            
            Console.WriteLine("Build ISO8583 Message");
            Console.WriteLine("Output:");
            Console.ReadLine();
        }


        [TestMethod]
        public void TestISO8583Message()
        {
            string MTI = "0200";           
            BIM_ISO8583.NET.ISO8583 iso8583 = new BIM_ISO8583.NET.ISO8583();
            string[] DE = new string[130];
            DE[22] = "021";
            //DE[32] = "437586002";

            string NewISOmsg = iso8583.Build(DE, MTI);

            string test = "0200323a448128e21881013000000000";
            test +="00200001090438520005961538520109";
            test +="01095811002141080043758637518868";
            test +="0100002858d15122015301010540000f";
            test +="35303039313533383035393653393231";
            test +="38313633343337353836303030202020";
            test +="202020383030204c414e47444f4e2053";
            test +="542c202020202020202020204d414449";
            test +="534f4e20202020202041553032385443";
            test +="4330315c45464330303030303030305c";
            test +="434349305c46424b565cab702f27a85e";
            test +="87130000000000000001000000000000";
            test +="cf74bdce00000000";
            string[] decode = iso8583.Parse(test);
        }

       
    }
}
