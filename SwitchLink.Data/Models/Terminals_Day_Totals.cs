using System;
using System.Reflection;
using System.Text;

namespace SwitchLink.Data.Models
{
    public class Terminals_Day_Totals
    {
        public string tran_gid { get; set; }
        public string terminal_id { get; set; }
        public string business_date { get; set; }
        public string settlement_code { get; set; }
        public string processed_date { get; set; }
        public int num_of_cw { get; set; }
        public int num_of_tf { get; set; }
        public int num_of_bi { get; set; }
        public decimal total_dispensed { get; set; }


        public override string ToString()
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO terminals_day_totals SET ");

            foreach (PropertyInfo i in GetType().GetProperties())
            {
                object val = i.GetValue(this);
                if (val != null)
                {
                    queryBuilder.Append(i.Name + " = '" +
                                        val.ToString()
                                            .Replace(@"'", string.Empty)
                                            .Replace(@"\", string.Empty)
                                            .Replace("\"", string.Empty) + "', ");
                }
            }
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            queryBuilder.Append("CreateBy = 'System', LastUpdateBy = 'System', CreateAt='" + date + "', LastUpdateAt='" +
                                date + "'");
            string result = queryBuilder.ToString();
            return result;
        }
    }
}
