using System;
using System.Linq;

namespace SwitchLink.ProtocalFactory.AS2805.Model
{
    public class AS2805Model
    {
        private readonly string _mti;
        private readonly string[] _de = new string[130];
        private readonly As2805Extensions _helper = new As2805Extensions();

        public AS2805Model(string mti)
        {
            _mti = mti;
        }

        public string this[int i]
        {
            get
            {
                return _de[i];
            }           
        }


        internal void SetField(int index, DateTime val)
        {
            var validIndexes = new int[] { 7, 12, 13, 15 };
            if(!validIndexes.Contains(index))
                throw new ArgumentException("index is not valid for datetime value", index.ToString());

            if (index == 7)
                _de[index]=_helper.Build_p7_transmit_dt(val);
            if (index == 12)
                _de[index] = _helper.Build_p12_time_local_tran(val);
            if (index == 13)
                _de[index] = _helper.Build_p13_date_local_tran(val);
            if (index == 15)
                _de[index] = _helper.Build_p15_date_settlement(val);
        }

        internal void SetField(int index, long val)
        {
            var validIndexes = new int[] { 37 };
            if (!validIndexes.Contains(index))
                throw new ArgumentException("index is not valid for long value", index.ToString());
            if (index == 37)
                _de[index] = _helper.Build_p37_ret_ref_no(val);
        }

        internal void SetField(int index, int val)
        {
            var validIndexes = new int[] { 11, 33, 100, 53, 4, 28, 32, 37, 57};
            if (!validIndexes.Contains(index))
                throw new ArgumentException("index is not valid for int value", index.ToString());

            if (index == 4)
                _de[index] = _helper.Build_p4_amount_tran(val);
            if (index == 11)
                _de[index] = _helper.Build_p11_stan(val);
            if (index == 28)
                _de[index] = _helper.Build_p28_amt_tran_fee(val);
            if (index == 32)
                _de[index] = _helper.Build_p32_acq_inst_id(val);
            if (index == 33)
                _de[index] = _helper.Build_de33_FwdInstIdCode(val);
            if (index == 53)
                _de[index] = _helper.Build_de53_SecControlInfo(val);
            if (index == 57)
                _de[index] = _helper.Build_de57_AmtCash(val);
            if (index == 100)
                _de[index] = _helper.Build_de100_RecvInstIdCode(val);
        }

        internal void SetField(int index, string val)
        {
            var validIndexes = new int[] { 3, 22, 48, 70, 39, 3, 18, 25, 35, 41, 42, 43, 47, 52, 55, 64, 128 };

            if (!validIndexes.Contains(index))
                throw new ArgumentException("index is not valid for int value", index.ToString());
            //if (string.IsNullOrEmpty(val))
            //    throw new ArgumentException("value is null or empty", index.ToString());

            if (index == 3)
                _de[index] = _helper.Build_p3_processing_code(val);
            if (index == 18)
                _de[index] = _helper.Build_p18_merchant_type(val);
            if (index == 25)
                _de[index] = _helper.Build_p25_pos_condition_code(val);
            if (index == 35)
                _de[index] = _helper.Build_p35_track2(val);
            if (index == 39)
                _de[index] = _helper.Build_de39_RespCode(val);
            if (index == 22)
                _de[index] = _helper.Build_p22_pos_entry_mode(val);
            if (index == 41)
                _de[index] = _helper.Build_p41_terminal_id(val);
            if (index == 42)
                _de[index] = _helper.Build_p42_card_acceptor_id(val);
            if (index == 43)
                _de[index] = _helper.Build_p43_name_location(val);
            if (index == 47)
                _de[index] = _helper.Build_p47_additional_response_national(val);
            if (index == 48)
                _de[index] = _helper.Build_de48_AddtlDataPriv(val);
            if (index == 52)
                _de[index] = val;
            if (index == 55)
                _de[index] = _helper.Build_de55_ResIso(val);
            if (index == 64)
                _de[index] = val;
            if (index == 70)
                _de[index] = _helper.Build_de70_NetMgtInfoCode(val);
            if (index == 128)
                _de[index] = val;

        }

        internal void SetField(int index, int p2, DateTime dateTime, int p3, int p4)
        {
            var validIndexes = new int[] { 90 };

            if (!validIndexes.Contains(index))
                throw new ArgumentException("index is not valid for int value", index.ToString());

            if (index == 90)
                _de[index] = _helper.Build_de90_OrigDataElem(p2, dateTime, p3, p4);

        }

        public override string ToString()
        {
            return string.Join("",_de);
        }

        public byte[] ToBytes()
        {
            var strBuilder = _mti + _helper.BuildPrimaryBitmap(_de);
            strBuilder+=string.Join("", _de.Where(x=>!string.IsNullOrEmpty(x)));

            return _helper.GetBytesWithHeaderLength(strBuilder);
        }
    }
}
