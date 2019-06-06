using System;
using UnitTest.Helper;

namespace AtmSimulatorWinForm.Builder
{
    public class ClientMessageBuilder
    {
        private readonly string _atmId;
        //"<STX>PILOT   td8D2       <FS>S9111111       <FS>12<FS>0109<FS>4902370000002348=121210111234123<FS>00002000<FS>00000250<FS>EECCBD2A57B822E7<FS><FS><FS>VA6.00.18DV02.70.10 V06.01.12 0  0T  00 000     00000002K1560003200005K157500010000000000000000000000000000000000<FS>ub093A<FS><ETX>q"
        public ClientMessageBuilder(string atmId)
        {
            _atmId = atmId;
        }

        public string GetRequestMsg()
        {
            string val = "";
            val += "<STX>";
            val += "PILOT   td8D2       ";
            val += "<FS>";
            val += _atmId.PadRight(15, ' ');
            val += "<FS>";
            val += RandomTranCode();
            val += "<FS>";
            val += AtmRequestHelper.GetSeq.ToString("D4");
            val += "<FS>";
            //val += RandomCardNo() + "=121210111234123";
            val += "4902370000002348=121210111234123";
            val += "<FS>";
            val += TranAmount();
            val += "<FS>";
            val += "00000250";
            val += "<FS>";
            val += "EECCBD2A57B822E7";
            val += "<FS>";
            val += "<FS>";
            val += "<FS>";
            val += Status();
            val += "<FS>ub093A<FS><ETX>q";

            return val;
        }

        private string RandomTranCode()
        {
            string[] val = {"11", "12", "15"};
            return val[new Random().Next(val.Length)];
        }

        private string RandomCardNo()
        {
            string result = "4";
            if (new Random().Next(3) == 2)
                result = "5";
            result += new Random().Next(0, 99999999);
            result += new Random().Next(0, 9999999);
            return result;
        }

        private string TranAmount()
        {
            string r1 = new Random().Next(2, 40).ToString("D2");
            string val = "000" + r1 + "000";
            return val;
        }

        private string Status()
        {
            string casA = AtmRequestHelper.GetRemainingCasA.ToString("D4");
            string casB = AtmRequestHelper.GetRemainingCasB.ToString("D4"); 

            string status = "VA6.00.18D";
            status += "V02.70.10 ";
            status += "V06.01.12 ";
            status += "0  0T  00 ";
            status += new Random().Next(0, 2).ToString();//   #TODO: change status automatically - Dispenser
            status += new Random().Next(0, 2).ToString();//   #TODO: change status automatically - Comms System
            status += "0     ";
            status += RandomTerminalErrorCode();// #TODO: change status automatically - Terminal Error Code
            //status += "000";// #TODO: change status automatically - Terminal Error Code
            status += new Random().Next(0, 10).ToString("D3");// #TODO: change status automatically - Comms failures
            //status += "02K1560003200005K157500010000000000000000000000000000000000";
            status += "02K" + casA + "003200005K" + casB + "00010000000000000000000000000000000000";
            return status;
        }

        private string RandomTerminalErrorCode()
        {
            string result = "000";
            string[] val = {
                "018", "033", "033", "033", "034", "037",
                "042", "043", "043", "043", "052", "061", "063", "068", "068", "073", "073", "078", "094", "103", "103",
                "121", "136", "136", "138", "138", "139", "139", "139", "139", "139", "139", "139", "139", "141", "141", "185", "189",
                "190", "191", "195", "195", "197", "201", "204", "302", "307", "307", "322", "327", "327", "330", "343", "343", "343", "343",
                "343", "343", "343", "343", "343", "343", "366", "366", "367", "367", "367", "367", "367", "375", "377", "378", "380", "565"
            };
            if (new Random().Next(100) < 10)
            {
                result = val[new Random().Next(val.Length - 1)];
            }
            return result;
        }
    }
}
