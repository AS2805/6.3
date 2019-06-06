using System;

namespace UnitTest.AS2805.Helper
{
    public class AS2805Helper
    {
        private readonly UnitTest.AS2805.Helper.BaseMessageHelper _baseMessageHelper = new UnitTest.AS2805.Helper.BaseMessageHelper();

        internal string BuildPrimaryBitmap(string[] DE)
        {
            string newDE1 = "";

            for (int I = 2; I <= 64; I++)
            {
                if (DE[I] != null)
                {
                    newDE1 += "1";
                }
                else
                {
                    newDE1 += "0";
                }
            }

            string newDE2 = "";
            for (int I = 65; I <= 128; I++)
            {
                if (DE[I] != null)
                {
                    newDE2 += "1";
                }
                else
                {
                    newDE2 += "0";
                }
            }

            if (newDE2 == "0000000000000000000000000000000000000000000000000000000000000000")
            {
                newDE1 = "0" + newDE1;
            }
            else
            {
                newDE1 = "1" + newDE1;
            }

            string DE1Hex = String.Format("{0:X1}", Convert.ToInt64(newDE1, 2));
            DE1Hex = DE1Hex.PadLeft(16, '0'); //Pad-Left
            var result = DE1Hex;

            string DE2Hex = String.Format("{0:X1}", Convert.ToInt64(newDE2, 2));
            DE2Hex = DE2Hex.PadLeft(16, '0'); //Pad-Left

            if (DE2Hex == "0000000000000000")
            {
                DE[1] = null;
            }
            else
            {
                result += DE2Hex;
            }

            return result;
        }

        public string Build_p3_processing_code(string transactionCode)
        {
            switch (transactionCode)
            {
                case "11": // Cash Withdrawal from primary cheque account
                    return "012000";
                case "12": // Cash Withdrawal from primary savings account 
                    return "011000";
                case "15": // Cash Withdrawal from primary credit card account
                    return "013000";
                case "31": // Primary cheque account balance inquiry
                    return "302000";
                case "32": // Primary Savings account balance inquiry
                    return "301000";
                case "35": // Primary Credit card balance inquiry
                    return "303000";
                default:
                    throw new Exception("Request message error => Transaction code not found");
            }
        }

        public string Parse_TransactionCode(string p3_processing_code)
        {
            switch (p3_processing_code)
            {
                case "012000": // Cash Withdrawal from primary cheque account
                    return "11";
                case "011000": // Cash Withdrawal from primary savings account 
                    return "12";
                case "013000": // Cash Withdrawal from primary credit card account
                    return "15";
                case "302000": // Primary cheque account balance inquiry
                    return "31";
                case "301000": // Primary Savings account balance inquiry
                    return "32";
                case "303000": // Primary Credit card balance inquiry
                    return "35";
                default:
                    throw new Exception("Request message error => Transaction code not found");
            }
        }

        public string Build_p4_amount_tran(string amount1)
        {
            int DEFixLen = 12;
            return amount1.PadLeft(Math.Abs(DEFixLen), '0');
        }

        public string Parse_Amount1(string p4_amount_tran)
        {
            return p4_amount_tran.Substring(4);
        }

        public string Build_p7_transmit_dt(DateTime dateTime = default(DateTime))
        {
            if (dateTime == DateTime.MinValue)
                dateTime = DateTime.Now;
            return dateTime.ToString("MMddHHmmss");
        }

        public string Build_p11_stan(int seq)
        {
            return seq.ToString("D6");
        }

        public string Build_p12_time_local_tran(DateTime dateTime = default(DateTime))
        {
            if (dateTime == DateTime.MinValue)
                dateTime = DateTime.Now;
            return dateTime.ToString("HHmmss");
        }

        public string Build_p13_date_local_tran(DateTime dateTime = default(DateTime))
        {
            if (dateTime == DateTime.MinValue)
                dateTime = DateTime.Now;
            return dateTime.ToString("MMdd");
        }

        public string Build_p15_date_settlement(DateTime dateTime = default(DateTime))
        {
            if (dateTime == DateTime.MinValue)
                dateTime = DateTime.Now;
            return dateTime.AddDays(+1).ToString("MMdd");
        }

        public string Build_p18_merchant_type(string merchant_type = "5811")
        {
            return merchant_type;
        }

        public string Build_p22_pos_entry_mode(string posEntryMode = "021")
        {
            int DEFixLen = 3;
            if (posEntryMode.Length != DEFixLen)
                throw new Exception("Invalid Format");
            DEFixLen++;
            string BMPadded = posEntryMode.PadLeft(DEFixLen, '0');
            string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(DEFixLen), Math.Abs(DEFixLen));
            return sBM;
        }

        public string Parse_posEntryMode(string p22_pos_entry_mode)
        {
            int DEFixLen = 3;
            if (p22_pos_entry_mode.Length != DEFixLen + 1)
                throw new Exception("Invalid Format");

            string BMPadded = p22_pos_entry_mode.Substring(1, DEFixLen);
            string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(DEFixLen), Math.Abs(DEFixLen));
            return sBM;
        }

        public string Build_p25_pos_condition_code(string posCondCode = "41")// Cash Dispenser Machine (ATM)
        {
            return posCondCode;
        }

        public string Build_p28_amt_tran_fee(string amount2, string fix = "D")
        {
            if (amount2.Length > 8)
                throw new Exception("Invalid Format");
            fix = _baseMessageHelper.BuildStringMessageHexString(fix);
            if (amount2.Length < 8)
                amount2 = amount2.PadLeft(8, '0');
            string sBM = amount2.Substring(amount2.Length - Math.Abs(amount2.Length), Math.Abs(amount2.Length));
            string result = fix + sBM;
            return result;
        }

        public string Parse_Amount2(string p28_amt_tran_fee)
        {
            string BMPadded = p28_amt_tran_fee.Substring(2);
            string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(BMPadded.Length), Math.Abs(BMPadded.Length));
            return sBM;
        }

        public string Build_p32_acq_inst_id(string acq_inst_id = "437586002")
        {
            /*  
             * Insitution Codes
                           # Insitution Codes
                           #Cashpoint 437586002
                           #CUSCAL  6100016
             * */
            int length = acq_inst_id.Length;
            if (length % 2 != 0)
                acq_inst_id = acq_inst_id.PadLeft(acq_inst_id.Length + 1, '0');
            return length.ToString("D2") + acq_inst_id;
        }

        public string Parse_acq_inst_id(string p32_acq_inst_id)
        {
            int length = int.Parse(p32_acq_inst_id.Substring(0, 2));
            p32_acq_inst_id = StringExtensions.Right(p32_acq_inst_id, length);
            return p32_acq_inst_id;
        }

        public string Build_p35_track2(string track2)
        {
            int length = track2.Length;
            if (length%2 != 0)
            {
                track2 += "F";
            }
                //track2 = track2.PadLeft(track2.Length + 1, '0');
            
            track2 = track2.Replace("=", "D");
            return length.ToString("D2") + track2;
        }

        public string Parse_Track2(string p35_track2)
        {
            if (p35_track2.EndsWith("F"))
                p35_track2 = p35_track2.Replace("F", "");
            p35_track2 = p35_track2.Replace("D", "=");
            int length = int.Parse(p35_track2.Substring(0, 2));
            p35_track2 = StringExtensions.Right(p35_track2, length);
            return p35_track2;
        }

        public string Build_p37_ret_ref_no(string retRefNo)
        {
            return _baseMessageHelper.BuildStringMessageHexString(retRefNo);
        }

        public string Parse_ret_ref_no(string p37_ret_ref_no)
        {
            return _baseMessageHelper.HexStringToString(p37_ret_ref_no);
        }

        public string Build_p41_terminal_id(string terminalId)
        {
            return _baseMessageHelper.BuildStringMessageHexString(terminalId.Trim());
        }

        public string Parse_TerminalId(string p41_terminal_id)
        {
            return _baseMessageHelper.HexStringToString(p41_terminal_id);
        }

        public string Build_p42_card_acceptor_id(string card_acceptor_id)
        {
            if (card_acceptor_id.Length != 15)
                throw new Exception("Invalid Format");
            return _baseMessageHelper.BuildStringMessageHexString(card_acceptor_id);
        }

        public string Parse_Card_acceptor_id(string p42_card_acceptor_id)
        {
            return _baseMessageHelper.HexStringToString(p42_card_acceptor_id);
        }

        public string Build_p43_name_location(string atm_location)
        {
            return _baseMessageHelper.BuildStringMessageHexString(atm_location);
        }

        public string Parse_name_location(string p43_name_location)
        {
            return _baseMessageHelper.HexStringToString(p43_name_location);
        }

        public string Build_p47_additional_response_national(string nationalData = @"TCC01\EFC00000000\CCI0\FBKV\")
        {
            string result = _baseMessageHelper.BuildStringMessageHexString(nationalData);
            string length = _baseMessageHelper.BuildStringMessageHexString(nationalData.Length.ToString("D3"));
            return length + result;
        }

        public string Parse_NationalData(string p47_additional_response_national)
        {
            string result = _baseMessageHelper.HexStringToString(p47_additional_response_national);
            int length = int.Parse(result.Substring(0, 3));
            result = StringExtensions.Right(result, length);
            return result;
        }

        public string Build_p52_pin_block(string pinBlock)
        {
            return pinBlock;
        }

        public string Parse_PinBlock(string p52_pin_block)
        {
            return p52_pin_block;
        }

        public string Build_p57_amount_cash(string transactionCode, string p4_amount_tran)
        {
            switch (transactionCode)
            {
                case "11": // Cash Withdrawal from primary cheque account
                case "12": // Cash Withdrawal from primary savings account 
                case "15": // Cash Withdrawal from primary credit card account
                    return p4_amount_tran;
                case "31": // Primary cheque account balance inquiry
                case "32": // Primary Savings account balance inquiry
                case "35": // Primary Credit card balance inquiry
                    return "000000000000";
                default:
                    return "000000000000";
            }
        }

    }
}
