using System.Linq;
using Dapper;
using SwitchLink.Data.Models;

namespace SwitchLink.Data
{
    public class TransactionData : BaseData
    {
        public void InsertTransactionCompletedRecord(Transaction_Completed transaction)
        {
            Connection.Execute(transaction.ToString());
        }

        public long RetrieveAuthorizationRrnForReversal(string terminalId, int atmSeqNo)
        {
            string script = "select transaction_seq_no from transaction_completed where terminal_id = @terminal_id and terminal_tran_seq = @terminal_tran_seq and request_type = 'Authorization' and response_code = '00' order by id desc";
            return Connection.Query<long>(script, new { terminal_id = terminalId, terminal_tran_seq = atmSeqNo }).FirstOrDefault();
        }

        public int RetrieveAuthorizationNumForReversal(string terminalId, int atmSeqNo)
        {
            string script = "select auth_no from transaction_completed where terminal_id = @terminal_id and terminal_tran_seq = @terminal_tran_seq and request_type = 'Authorization' and response_code = '00' order by id desc";
            return Connection.Query<int>(script, new { terminal_id = terminalId, terminal_tran_seq = atmSeqNo }).FirstOrDefault();
        }

        public string RetrieveCardAcceptorIdForReversal(string terminal, long tranSeqNo, int authorizationNum)
        {
            string script = "select p42_card_acceptor_id from transaction_host_node where p41_terminal_id = @terminal_id and p37_ret_ref_no = @tran_seq and p11_stan = @stan and mti = '0200' order by auto_id desc";
            return Connection.Query<string>(script, new { terminal_id = terminal, tran_seq = tranSeqNo.ToString("D12"), stan = authorizationNum.ToString("D6") }).FirstOrDefault();
        }

        public string RetrieveAddtlDataNatForReversal(string terminal, long tranSeqNo, int authorizationNum)
        {
            string script = "select p47_additional_response_national from transaction_host_node where p41_terminal_id = @terminal_id and p37_ret_ref_no = @tran_seq and p11_stan = @stan and mti = '0200' order by auto_id desc";
            return Connection.Query<string>(script, new { terminal_id = terminal, tran_seq = tranSeqNo.ToString("D12"), stan = authorizationNum.ToString("D6") }).FirstOrDefault();
        }

        public string RetrieveRetrieveSecurityBlockForReversal(string terminal, long tranSeqNo, int authorizationNum)
        {
            string script = "select p53_security_block from transaction_host_node where p41_terminal_id = @terminal_id and p37_ret_ref_no = @tran_seq and p11_stan = @stan and mti = '0200' order by auto_id desc";
            return Connection.Query<string>(script, new { terminal_id = terminal, tran_seq = tranSeqNo.ToString("D12"), stan = authorizationNum.ToString("D6") }).FirstOrDefault();
        }

        public string RetrieveAcqInstIdCodeForReversal(string terminal, long tranSeqNo, int authorizationNum)
        {
            string script = "select p32_acq_inst_id from transaction_host_node where p41_terminal_id = @terminal_id and p37_ret_ref_no = @tran_seq and p11_stan = @stan and mti = '0200' order by auto_id desc";
            return Connection.Query<string>(script, new { terminal_id = terminal, tran_seq = tranSeqNo.ToString("D12"), stan = authorizationNum.ToString("D6") }).FirstOrDefault();
        }

        public string RetrieveFwdInstIdCodeForReversal(string terminal, int authorizationNum)
        {
            string script = "select p33_fwd_inst_id from transaction_host_node where p41_terminal_id = @terminal_id and p11_stan = @stan and mti = '0210' order by auto_id desc";
            return Connection.Query<string>(script, new { terminal_id = terminal, stan = authorizationNum.ToString("D6") }).FirstOrDefault();
        }

        public long GetSequenceNumbers(string seq_type)
        {
            string script = "select sequence_numbers(@type)";
            return Connection.Query<long>(script, new { type = seq_type }).FirstOrDefault();
        }
    }
}
