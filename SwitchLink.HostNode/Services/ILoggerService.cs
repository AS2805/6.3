using System.Linq;
using System.Threading.Tasks;
using SwitchLink.Data;
using SwitchLink.Data.Models;
using SwitchLink.ProtocalFactory.AS2805;

namespace SwitchLink.HostNode.Services
{
    interface ILoggerService
    {
        void LogMessageToPartner(byte[] bytes);
        void LogMessageFromPartner(byte[] bytes);
    }

    class LoggerService : ILoggerService
    {
        public async void LogMessageToPartner(byte[] bytes)
        {
            if (bytes != null)
            {
                await Task.Run(() => { Save(bytes, "Message to Partner"); });
            }
        }

        public async void LogMessageFromPartner(byte[] bytes)
        {
            if (bytes != null)
            {
                await Task.Run(() => { Save(bytes, "Message from Partner"); });
            }
        }

        private void Save(byte[] bytes, string tranType)
        {
            var msg = bytes.Skip(2).ToArray();
            As2805Extensions helper = new As2805Extensions();
            As2805 as2805 = new As2805(msg);
            HostNode_Transaction model = new HostNode_Transaction
            {
                mti = as2805.mti,
                tran_type = tranType,
                p2_pan = as2805.de2_PAN,
                p3_processing_code = as2805.de3_ProcCode,
                p4_amount_tran = as2805.de4_AmtTxn,
                p5_amount_settlement = as2805.de5_AmtSettle,
                p6_amount_billing = as2805.de6_AmtCardhBill,
                p7_transmit_dt = as2805.de7_TransDttm,
                p9_conversion_rate_settlement = as2805.de9_ConvRateSettle,
                p10_conversion_rate_billing = as2805.de10_ConvRateCardhBill,
                p11_stan = as2805.de11_STAN,
                p12_time_local_tran = as2805.de12_TimeLocal,
                p13_date_local_tran = as2805.de13_DateLocal,
                p14_date_expiration = as2805.de14_DateExpiry,
                p15_date_settlement = as2805.de15_DateSetl,
                p16_date_conversion = as2805.de16_DateConv,
                p18_merchant_type = as2805.de18_MerchType,
                p22_pos_entry_mode = as2805.de22_PosEntryMode,
                p23_card_seq_no = as2805.de23_CardSeqNo,
                p25_pos_condition_code = as2805.de25_PosCondCode,
                p28_amt_tran_fee = as2805.de28_AmtTxnFee,
                p32_acq_inst_id = as2805.de32_AcqInstIdCode,
                p33_fwd_inst_id = as2805.de33_FwdInstIdCode,
                p35_track2 = as2805.de35_Track2,
                p37_ret_ref_no = as2805.de37_RetRefNo,
                p38_auth_id_response = as2805.de38_AuthIdentResp,
                p39_response_code = as2805.de39_RespCode,
                p41_terminal_id = as2805.de41_CardAcptTermId,
                p42_card_acceptor_id = as2805.de42_CardAcptIdCode,
                p43_name_location = as2805.de43_CardAcptNameLoc,
                p44_additional_response_data = helper.Parse_Bytes_ToStringHex(as2805.de44_AddtRespData),
                p47_additional_response_national = helper.Parse_Bytes_ToStringHex(as2805.de47_AddtlDataNat),
                p48_additional_response_private = helper.Parse_Bytes_ToStringHex(as2805.de48_AddtlDataPriv),
                p49_currency = as2805.de49_CurCodeTxn,
                p50_currency_settle = as2805.de50_CurCodeSettle,
                p51_currency_biling = as2805.de51_CurCodeCardhBill,
                p53_security_block = as2805.de53_SecControlInfo,
                p57_amount_cash = as2805.de57_AmtCash,
                p58_ledger_balance = as2805.de58_BalanceLedger,
                p59_account_balance = as2805.de59_BalanceCleared,
                p66_settlement_code = as2805.de66_SettleCode,
                p70_network_mgt_info_code = as2805.de70_NetMgtInfoCode,
                p74_credits_no = as2805.de74_CreditsNo,
                p75_credits_rev_no = as2805.de75_CreditRevsNo,
                p76_debits_no = as2805.de76_DebitsNo,
                p77_debits_rev_no = as2805.de77_DebitRevsNo,
                p78_transfers_no = as2805.de78_TransfersNo,
                p79_transfers_rev_no = as2805.de79_TransferRevsNo,
                p80_inquiries_no = as2805.de80_InquiriesNo,
                p81_auths_no = as2805.de81_AuthsNo,
                p83_credits_tran_fee = as2805.de83_CreditsTxnFeeAmt,
                p85_debits_tran_fee = as2805.de85_DebitsTxnFeeAmt,
                p86_credits_amt = as2805.de86_CreditsAmt,
                p87_credits_rev_amt = as2805.de87_CreditRevsAmt,
                p88_debits_amt = as2805.de88_DebitsAmt,
                p89_debits_rev_amt = as2805.de89_DebitRevsAmt,
                p90_original_data = as2805.de90_OrigDataElem,
                p95_replacement_amounts = as2805.de95_ReplAmts,
                p97_amt_net_settle = as2805.de97_AmtNetSetl,
                p99_settle_id_code = as2805.de99_SettleInstIdCode,
                p100_receving_id_code = as2805.de100_RecvInstIdCode,
                p112_key_mng_data = helper.Parse_Bytes_ToStringHex(as2805.de112_ResvNat),
                p118_cash_no = as2805.de118_TotalCashNo,
                p119_cash_amount = as2805.de119_TotalCashAmt,
                p128_mac_extended = helper.Parse_Bytes_ToStringHex(as2805.de128_MAC),
                text = as2805.ToString()
            };

            using (HostData context = new HostData())
            {
                context.InsertHostLogRecord(model);
            }
        }
    }
}
