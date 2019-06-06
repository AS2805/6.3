using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AS2805Helper = UnitTest.AS2805.Helper.AS2805Helper;
using BaseMessageHelper = UnitTest.AS2805.Helper.BaseMessageHelper;

namespace UnitTest
{
    [TestClass]
    public class AS2805_Message
    {
        readonly BaseMessageHelper _baseMessageHelper = new BaseMessageHelper();
        public AS2805Helper AS2805Helper
        {
            get
            {
                return new AS2805Helper(); 
            }
        }

        [TestMethod]
        public void TestAS2805MessageBankResponseWhenConnect()
        {
            
            string msg = "0800822000008001080004000000100000003132303331363333303832333030393831383433373538363030323033323241323542414134313634373439463930303030303030303030303030303031303031313436313030303136";
            byte[] bytes = _baseMessageHelper.StringToBytes(msg);

            byte[] length = BitConverter.GetBytes((short)bytes.Length);
            var header = length.Reverse();
            IList<byte> completeMsg = header.ToList();
            foreach (byte t in bytes)
            {
                completeMsg.Add(t);
            }

        }

        [TestMethod]
        public void TestAS2805MessageSendToBank0200()
        {
            string results = "";
            string MTI = "0200";
            
            string[] DE = new string[130];

            DE[3] = AS2805Helper.Build_p3_processing_code("12");
            DE[4] = AS2805Helper.Build_p4_amount_tran("00002000");
            DE[7] = AS2805Helper.Build_p7_transmit_dt();
            DE[11] = AS2805Helper.Build_p11_stan(0);
            DE[12] = AS2805Helper.Build_p12_time_local_tran();
            DE[13] = AS2805Helper.Build_p13_date_local_tran();
            DE[15] = AS2805Helper.Build_p15_date_settlement();
            DE[18] = AS2805Helper.Build_p18_merchant_type("5811");
            DE[22] = AS2805Helper.Build_p22_pos_entry_mode("021");
            DE[25] = AS2805Helper.Build_p25_pos_condition_code("41");
            DE[28] = AS2805Helper.Build_p28_amt_tran_fee("00000200");
            DE[32] = AS2805Helper.Build_p32_acq_inst_id("560258");
            DE[35] = AS2805Helper.Build_p35_track2("5188680100002932=15122015076719950000");
            DE[37] = AS2805Helper.Build_p37_ret_ref_no("500810470506");
            DE[41] = AS2805Helper.Build_p41_terminal_id("S9218163");
            DE[42] = AS2805Helper.Build_p42_card_acceptor_id("437586000      ");
            DE[43] = AS2805Helper.Build_p43_name_location("800 LANGDON ST,          MADISON      AU");
            DE[47] = AS2805Helper.Build_p47_additional_response_national(@"TCC01\EFC00000000\CCI0\FBKV\");
            DE[52] = "A8FB4E47EACB0FA1";
            DE[53] = "0000000000000002";
            DE[57] = "000000000000";
            DE[64] = "29365A0400000000";
            results += MTI;
            results += AS2805Helper.BuildPrimaryBitmap(DE)[0];

            for (int i = 0; i < DE.Length; i++)
            {
                if (!string.IsNullOrEmpty(DE[i]))
                {
                    results += DE[i];
                }
            }

            byte[] bytes = _baseMessageHelper.StringToBytes(results);


            short v = (short) 4048;
            byte[] length = BitConverter.GetBytes(v);
            var correct = length.Reverse();
           
            //var results = AS2805.NET.AS2805.GetPrimaryBitmap(DE);
            var text = string.Concat(bytes.Select(b => b.ToString("X2")));



            //As2805Message m = new As2805Message(bytes);


            //string NewISOmsg = as2805.Build(DE, MTI);
        }

        [TestMethod]
        public void AS2805MessageFunction_p4_amount_tran()
        {
            string amount1 = "00008900";
            string p4_amount_tran = AS2805Helper.Build_p4_amount_tran(amount1);
            amount1 = AS2805Helper.Parse_Amount1(p4_amount_tran);

        }

        [TestMethod]
        public void AS2805MessageFunction_p3_processing_code()
        {
            string processing = "11";
            string p3_processing_code = AS2805Helper.Build_p3_processing_code(processing);
            processing = AS2805Helper.Parse_TransactionCode(p3_processing_code);
        }

        [TestMethod]
        public void AS2805MessageFunction_p7_transmit_dt()
        {
            string p7_transmit_dt = AS2805Helper.Build_p7_transmit_dt();
        }

        [TestMethod]
        public void AS2805MessageFunction_p11_stan()
        {
            string p11_stan = AS2805Helper.Build_p11_stan(0);
        }

        [TestMethod]
        public void AS2805MessageFunction_p22_pos_entry_mode()
        {
            string p22_pos_entry_mode = AS2805Helper.Build_p22_pos_entry_mode();

            var result = AS2805Helper.Parse_posEntryMode(p22_pos_entry_mode);
        }

        [TestMethod]
        public void AS2805MessageFunction_p28_amt_tran_fee()
        {
            string p28_amt_tran_fee = AS2805Helper.Build_p28_amt_tran_fee("0000100");

            var result = AS2805Helper.Parse_Amount2(p28_amt_tran_fee);
        }

        [TestMethod]
        public void AS2805MessageFunction_p32_acq_inst_id()
        {
            string p32_acq_inst_id = AS2805Helper.Build_p32_acq_inst_id();

            var result = AS2805Helper.Parse_acq_inst_id(p32_acq_inst_id);
        }

        [TestMethod]
        public void AS2805MessageFunction_p35_track2()
        {
            var val = "4089670000392726=17112011000017980000";

            string p35_track2 = AS2805Helper.Build_p35_track2(val);

            var result = AS2805Helper.Parse_Track2(p35_track2);
        }

        [TestMethod]
        public void AS2805MessageFunction_p37_ret_ref_no()
        {
            string p37_ret_ref_no = AS2805Helper.Build_p37_ret_ref_no("0");
            var t1 = AS2805Helper.Parse_ret_ref_no(p37_ret_ref_no);
        }

        [TestMethod]
        public void AS2805MessageFunction_p41_terminal_id()
        {
            var tr = "S9218163       ";
            string p41_terminal_id = AS2805Helper.Build_p41_terminal_id(tr);
            var t1 = AS2805Helper.Parse_TerminalId(p41_terminal_id);
        }

        [TestMethod]
        public void AS2805MessageFunction_p43_name_location()
        {
            var tr = "800 LANGDON ST,          MADISON      AU";
            string p43_name_location = AS2805Helper.Build_p43_name_location(tr);
            var t1 = AS2805Helper.Parse_name_location(p43_name_location);
        }

        [TestMethod]
        public void AS2805MessageFunction_p47_additional_response_national()
        {
            var tr = @"TCC01\EFC00000000\CCI0\FBKV\";
            string p47_additional_response_national = AS2805Helper.Build_p47_additional_response_national(tr);
            var t1 = AS2805Helper.Parse_NationalData(p47_additional_response_national);
        }
    }
}
