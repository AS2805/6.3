using System;
using System.Reflection;
using System.Text;

namespace SwitchLink.Data.Models
{
    public class Transaction_Completed
    {
        public string tran_gid { get; set; }
        public string tran_ip { get; set; }
        public int? switch_no { get; set; }
        public string request_timestamp { get; set; }
        public string request_type { get; set; }
        public string terminal_id { get; set; }
        public long? transaction_seq_no { get; set; }
        public string card_no { get; set; }
        public decimal transaction_amount { get; set; }
        public decimal surcharge_amount { get; set; }
        public decimal dispense_amount { get; set; }
        public string response_code { get; set; }
        public string response_desc { get; set; }
        public string response_action { get; set; }
        public int auth_no { get; set; }
        public string tran_date { get; set; }
        public string tran_time { get; set; }
        public decimal? account_balance { get; set; }
        public string transaction_speed { get; set; }
        public string transaction_code { get; set; }
        public string status_monitoring { get; set; }
        public int terminal_tran_seq { get; set; }
        
        public override string ToString()
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO transaction_completed SET ");

            foreach (PropertyInfo i in GetType().GetProperties())
            {
                object val = i.GetValue(this);
                if (val != null)
                {
                    queryBuilder.Append(i.Name + "='" + val + "',");
                }
            }
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            queryBuilder.Append("CreateBy = 'System', LastUpdateBy = 'System', CreateAt='" + date + "', LastUpdateAt='" + date + "'");
            string result = queryBuilder.ToString();
            return result;
        }
    }
}
