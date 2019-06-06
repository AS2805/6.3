using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SwitchLink.ProtocalFactory.Helper;

namespace SwitchLink.ProtocalFactory.AS2805
{
    public class As2805Extensions
    {
        private readonly MsgHelper _helper = new MsgHelper();

        public string BuildPrimaryBitmap(string[] DE)
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

        public string Parse_AddtlDataPrivToASCII(byte[] de48_AddtlDataPriv)
        {
            var strB = Parse_Bytes_ToStringHex(de48_AddtlDataPriv);
            return _helper.HexStringToString(strB);
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
                case "29": // Reverse Previous Withdrawal
                    return "200000";
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
                case "200000": // Reverse Previous Withdrawal
                    return "29";
                default:
                    throw new Exception("Request message error => Transaction code not found");
            }
        }

        public string Build_p4_amount_tran(int amount1)
        {
            //int DEFixLen = 12;
            //return amount1.PadLeft(Math.Abs(DEFixLen), '0');
            return amount1.ToString("D12");
        }

        public int Parse_Amount1(string p4_amount_tran)
        {
            return int.Parse(p4_amount_tran.Substring(4));
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

        public string Build_p22_pos_entry_mode(string miscellaneousX = "")
        {
            string posEntryMode = "0021";

            if (!string.IsNullOrEmpty(miscellaneousX) && miscellaneousX.Contains("ud"))
                    posEntryMode = "0051";
            return posEntryMode;
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

        public string Build_p25_pos_condition_code(string posCondCode = "41") // Cash Dispenser Machine (ATM)
        {
            return posCondCode;
        }

        public string Build_p28_amt_tran_fee(int amount2, string fix = "D")
        {
            string amt = amount2.ToString();
            if (amt.Length > 8)
                throw new Exception("Invalid Format");
            fix = _helper.BuildStringMessageHexString(fix);
            if (amt.Length < 8)
                amt = amt.PadLeft(8, '0');
            string sBM = amt.Substring(amt.Length - Math.Abs(amt.Length), Math.Abs(amt.Length));
            string result = fix + sBM;
            return result;
        }

        public int Parse_Amount2(string p28_amt_tran_fee)
        {
            string BMPadded = p28_amt_tran_fee.Substring(2);
            string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(BMPadded.Length), Math.Abs(BMPadded.Length));
            return int.Parse(sBM);
        }

        public string Build_p32_acq_inst_id(int acq_inst_id = 437586002)
        {
            /*  
             * Insitution Codes
                           # Insitution Codes
                           #Cashpoint 437586002
                           #CUSCAL  6100016
             * */
            var val = acq_inst_id.ToString();
            
            int length = val.Length;
            if (length%2 != 0)
                val = val.PadLeft(length + 1, '0');
            return length.ToString("D2") + val;
        }

        public string Parse_acq_inst_id(string p32_acq_inst_id)
        {
            int length = int.Parse(p32_acq_inst_id.Substring(0, 2));
            p32_acq_inst_id = p32_acq_inst_id.Right(length);
            return p32_acq_inst_id;
        }

        public string Build_de33_FwdInstIdCode(int fwdInstIdCode)
        {
            /*  
             * Insitution Codes
                           # Insitution Codes
                           #Cashpoint 437586002
                           #CUSCAL  6100016
             * */
            string re = fwdInstIdCode.ToString();
            int length = re.Length;
            if (length%2 != 0)
                re = re.PadLeft(re.Length + 1, '0');
            return length.ToString("D2") + re;
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
            p35_track2 = p35_track2.Right(length);
            return p35_track2;
        }

        public string Build_p37_ret_ref_no(long retRefNo)
        {
            string refn = retRefNo.ToString("D12");
            return _helper.BuildStringMessageHexString(refn);
        }

        public string Parse_ret_ref_no(string p37_ret_ref_no)
        {
            return _helper.HexStringToString(p37_ret_ref_no);
        }

        public string Build_p41_terminal_id(string terminalId)
        {
            return _helper.BuildStringMessageHexString(terminalId.Trim());
        }

        public string Parse_TerminalId(string p41_terminal_id)
        {
            return _helper.HexStringToString(p41_terminal_id);
        }

        public string Build_p42_card_acceptor_id(string card_acceptor_id)
        {
            if (card_acceptor_id.Length > 15)
                throw new Exception("Invalid Format");

            card_acceptor_id = card_acceptor_id.PadRight(15, ' ');
            return _helper.BuildStringMessageHexString(card_acceptor_id);
        }

        public string Parse_Card_acceptor_id(string p42_card_acceptor_id)
        {
            return p42_card_acceptor_id;
            //return _helper.HexStringToString(p42_card_acceptor_id);
        }

        public string Build_p43_name_location(string atm_location)
        {
            string val = "";
            if (atm_location.Length < 40)
            {
                val = atm_location.PadRight(40, ' ');
            }
            else if (atm_location.Length > 40)
            {
                val = atm_location.Substring(0, 40);
            }
            else
            {
                val = atm_location;
            }


            return _helper.BuildStringMessageHexString(val);
        }

        public string Parse_name_location(string p43_name_location)
        {
            return _helper.HexStringToString(p43_name_location);
        }

        public string Build_p47_additional_response_national(string nationalData = @"TCC01\EFC00000000\CCI0\FBKV\")
        {
            string result = _helper.BuildStringMessageHexString(nationalData);
            string length = _helper.BuildStringMessageHexString(nationalData.Length.ToString("D3"));
            return length + result;
        }

        //public string Parse_NationalData(string p47_additional_response_national)
        //{
        //    string result = _helper.HexStringToString(p47_additional_response_national);
        //    int length = int.Parse(result.Substring(0, 3));
        //    result = result.Right(length);
        //    return result;
        //}

        public string Parse_Bytes_ToStringHex(byte[] bytes)
        {
            if(bytes == null)
                return null;
            string st = _helper.ByteArrayToString(bytes);
            return st;
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

        public string Build_de57_AmtCash(int amountCash)
        {
            return amountCash.ToString("D12");
        }

        public int Parse_AmountCash(string de57_AmtCash)
        {
            return int.Parse(de57_AmtCash);
        }

        public string Build_de100_RecvInstIdCode(int instIdCode)
        {
            /*  
             * Insitution Codes
                           # Insitution Codes
                           #Cashpoint 437586002
                           #CUSCAL  6100016
             * */
            string re = instIdCode.ToString();
            int length = re.Length;
            if (length%2 != 0)
                re = re.PadLeft(re.Length + 1, '0');
            return length.ToString("D2") + re;
        }

        public int Parse_InstIdCode(string de100_RecvInstIdCode)
        {
            int length = int.Parse(de100_RecvInstIdCode.Substring(0, 2));
            de100_RecvInstIdCode = de100_RecvInstIdCode.Right(length);
            return int.Parse(de100_RecvInstIdCode);
        }

        public string Build_de70_NetMgtInfoCode(string netMgtInfoCode)
        {
            string d = netMgtInfoCode.ToString();
            int DEFixLen = 3;
            if (d.Length != DEFixLen)
                throw new Exception("Invalid Format");
            DEFixLen++;
            string BMPadded = d.PadLeft(DEFixLen, '0');
            string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(DEFixLen), Math.Abs(DEFixLen));
            return sBM;
        }

        public string Build_de39_RespCode(string respCode)
        {
            return _helper.BuildStringMessageHexString(respCode);
        }

        public string Build_de48_AddtlDataPriv(string addtlDataPriv)
        {
            string result = _helper.BuildStringMessageHexString(addtlDataPriv);
            string length = _helper.BuildStringMessageHexString(addtlDataPriv.Length.ToString("D3"));
            return length + result;
        }

        public string Build_de53_SecControlInfo(int secControlInfo)
        {
            return secControlInfo.ToString("D16");
        }

        public DateTime Parse_de7_TransDttm_ToDateTime(string strFdate, string format = "MMddHHmmss")
        {
            var r = DateTime.ParseExact(strFdate, format, CultureInfo.InvariantCulture);
            return r;
        }

        public DateTime Parse_de15_date_settlement(string p, string format = "MMdd")
        {
            var r = DateTime.ParseExact(p, format, CultureInfo.InvariantCulture);
            return r;
        }

        public byte[] GetBytesWithHeaderLength(string msg)
        {
            byte[] bytes = StringToBytes(msg);
            byte[] length = BitConverter.GetBytes((short)bytes.Length);
            IList<byte> completeMsg = length.Reverse().ToList();
            foreach (byte t in bytes)
                completeMsg.Add(t);
            return completeMsg.ToArray();
        }

        private byte[] StringToBytes(string text) { return (StringToBytes(text, -1)); }

        private byte[] StringToBytes(string text, int len)
        {

            int p = 0, l = text.Length, s = l < 2 ? 0 : text[0] == '0' && text[1] == 'x' ? 2 : 0, lr = len < 0 ? (l - s) / 2 + (l - s) % 2 : len;

            byte[] r = new byte[lr];

            for (int i = s; i < l & p < lr * 2; i++)
            {

                if (text[i] >= '0' && text[i] <= '9') { r[p / 2] |= ((p % 2 != 0) ? (byte)(text[i] - '0') : (byte)((text[i] - '0') << 4)); p++; }

                else if (text[i] >= 'A' && text[i] <= 'F') { r[p / 2] |= ((p % 2 != 0) ? (byte)(text[i] - 'A' + 10) : (byte)((text[i] - 'A' + 10) << 4)); p++; }

                else if (text[i] >= 'a' && text[i] <= 'f') { r[p / 2] |= ((p % 2 != 0) ? (byte)(text[i] - 'a' + 10) : (byte)((text[i] - 'a' + 10) << 4)); p++; }

            }

            if (len < 0) Array.Resize(ref r, p / 2 + p % 2);

            return (r);

        }

        internal string Build_de55_ResIso(string p)
        {
            string results = null;
            if (!string.IsNullOrEmpty(p))
            {
                results = p.Substring(p.IndexOf("ud", StringComparison.Ordinal) + 2);
                if (results.Length % 2 != 0)
                    results += "0";
            }
            return results;
        }

        public string Build_de90_OrigDataElem(int originalTranStan, DateTime originalTranDate, int originalAcqInstIdCode = 560258, int originalFwdInstIdCode = 61100016)
        {
            string mti = "0200";
            string orgStan = Build_p11_stan(originalTranStan);
            string orgTranDate = Build_p7_transmit_dt(originalTranDate);
            string orgAcqInstId = originalAcqInstIdCode.ToString("D11");
            string orgFwdInstId = originalFwdInstIdCode.ToString("D11");
            return mti + orgStan + orgTranDate + orgAcqInstId + orgFwdInstId;
        }
        
    }
}
