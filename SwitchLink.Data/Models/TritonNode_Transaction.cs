using System;
using System.Reflection;
using System.Text;

namespace SwitchLink.Data.Models
{
    public class TritonNode_Transaction
    {
        public string tran_gid { get; set; }
        public string tran_ip { get; set; }
        public string tran_type { get; set; }
        public string req_type { get; set; }
        public string information_header { get; set; }
        public string terminal_id { get; set; }
        public string transaction_code { get; set; }
        public long? transaction_seq_no { get; set; }
        public string dtconnected { get; set; }
        public string dtTakenByCore { get; set; }
        public string dtResponseFromCore { get; set; }
        public string dtResponseSent { get; set; }
        public string rev_match { get; set; }
        public string settlement_data { get; set; }
        public string track2 { get; set; }
        public string processing_code { get; set; }
        public int? switch_no { get; set; }
        public string communications_identifier { get; set; }
        public string terminal_identifier { get; set; }
        public string software_version_number { get; set; }
        public string encryption_mode_flag { get; set; }
        public decimal transaction_amount { get; set; }
        public string status_monitoring { get; set; }
        public string response_code { get; set; }
        public int? auth_no { get; set; }
        public string tran_date { get; set; }
        public string tran_time { get; set; }
        public string business_date { get; set; }
        public decimal? account_balance { get; set; }
        public decimal surcharge_amount { get; set; }
        public decimal dispense_amount { get; set; }
        public string miscellaneous_1 { get; set; }
        public string miscellaneous_2 { get; set; }
        public string miscellaneous_X { get; set; }
        public string multi_block_indicator { get; set; }
        public string pin_block { get; set; }
        public string text { get; set; }
        public int? terminal_tran_seq { get; set; }

        public override string ToString()
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO transaction_triton_node SET ");

            foreach (PropertyInfo i in GetType().GetProperties())
            {
                object val = i.GetValue(this);
                if (val != null)
                {
                    queryBuilder.Append(i.Name + " = '" + val.ToString().Replace(@"'", string.Empty).Replace(@"\", string.Empty).Replace("\"", string.Empty) + "', ");
                }
            }
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            queryBuilder.Append("CreateBy = 'System', LastUpdateBy = 'System', CreateAt='" + date + "', LastUpdateAt='" + date + "'");
            string result = queryBuilder.ToString();
            return result;
        }
    }
}