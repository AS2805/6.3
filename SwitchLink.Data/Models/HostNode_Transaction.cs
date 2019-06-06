using System;
using System.Reflection;
using System.Text;

namespace SwitchLink.Data.Models
{
    public class HostNode_Transaction
    {
        public string mti { get; set; }
        public string tran_type { get; set; }
        public string p2_pan { get; set; }
        public string p3_processing_code { get; set; }
        public string p4_amount_tran { get; set; }
        public string p5_amount_settlement { get; set; }
        public string p6_amount_billing { get; set; }
        public string p7_transmit_dt { get; set; }
        public string p9_conversion_rate_settlement { get; set; }
        public string p10_conversion_rate_billing { get; set; }
        public string p11_stan { get; set; }
        public string p12_time_local_tran { get; set; }
        public string p13_date_local_tran { get; set; }
        public string p14_date_expiration { get; set; }
        public string p15_date_settlement { get; set; }
        public string p16_date_conversion { get; set; }
        public string p18_merchant_type { get; set; }
        public string p22_pos_entry_mode { get; set; }
        public string p23_card_seq_no { get; set; }
        public string p25_pos_condition_code { get; set; }
        public string p28_amt_tran_fee { get; set; }
        public string p32_acq_inst_id { get; set; }
        public string p33_fwd_inst_id { get; set; }
        public string p35_track2 { get; set; }
        public string p37_ret_ref_no { get; set; }
        public string p38_auth_id_response { get; set; }
        public string p39_response_code { get; set; }
        public string p41_terminal_id { get; set; }
        public string p42_card_acceptor_id { get; set; }
        public string p43_name_location { get; set; }
        public string p44_additional_response_data { get; set; }
        public string p47_additional_response_national { get; set; }
        public string p48_additional_response_private { get; set; }
        public string p49_currency { get; set; }
        public string p50_currency_settle { get; set; }
        public string p51_currency_biling { get; set; }
        public string p53_security_block { get; set; }
        public string p57_amount_cash { get; set; }
        public string p58_ledger_balance { get; set; }
        public string p59_account_balance { get; set; }
        public string p66_settlement_code { get; set; }
        public string p70_network_mgt_info_code { get; set; }
        public string p74_credits_no { get; set; }
        public string p75_credits_rev_no { get; set; }
        public string p76_debits_no { get; set; }
        public string p77_debits_rev_no { get; set; }
        public string p78_transfers_no { get; set; }
        public string p79_transfers_rev_no { get; set; }
        public string p80_inquiries_no { get; set; }
        public string p81_auths_no { get; set; }
        public string p83_credits_tran_fee { get; set; }
        public string p85_debits_tran_fee { get; set; }
        public string p86_credits_amt { get; set; }
        public string p87_credits_rev_amt { get; set; }
        public string p88_debits_amt { get; set; }
        public string p89_debits_rev_amt { get; set; }
        public string p90_original_data { get; set; }
        public string p95_replacement_amounts { get; set; }
        public string p97_amt_net_settle { get; set; }
        public string p99_settle_id_code { get; set; }
        public string p100_receving_id_code { get; set; }
        public string p112_key_mng_data { get; set; }
        public string p118_cash_no { get; set; }
        public string p119_cash_amount { get; set; }
        public string p128_mac_extended { get; set; }
        public string text { get; set; }

        public override string ToString()
        {
            StringBuilder queryBuilder = new StringBuilder();

            queryBuilder.Append("INSERT INTO transaction_host_node SET ");
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


