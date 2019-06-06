using System.Linq;
using Dapper;
using SwitchLink.Data.Models;

namespace SwitchLink.Data
{
    public class TerminalData: BaseData
    {
        public Sessions_Triton_TMK GetTMKByTerminalId(string terminalID)
        {
           return Connection.Query<Sessions_Triton_TMK>(" Select TMK, TMK_CHK FROM sessions_triton WHERE atm_id = @atm_id", new {atm_id= terminalID}).FirstOrDefault();
        }

        //Arthurs method
        //public void UpdateSessionKeysByTerminalId(string terminalID, string TPK_TMK, string TPK_CHK, string TPK_LMK, string TAK)
        //{
        //    base.Connection.Execute(
        //        "UPDATE sessions_triton SET TPK_TMK = @TPK_TMK , TPK_CHK = @TPK_CHK, TPK_LMK = @TPK_LMK, TAK =@TAK  WHERE atm_id = @atm_id",
        //    new {TPK_TMK, TPK_CHK, TPK_LMK, TAK, terminalID}
        //        );
        //}

        public void UpdateSessionKeysByTerminalId(string terminalID, string TPK_TMK, string TPK_CHK, string TPK_LMK, string TAK,string TAK_CHK)
        {
            Connection.Execute(
                "UPDATE sessions_triton SET TPK_TMK = @TPK_TMK , TPK_CHK = @TPK_CHK, TPK_LMK = @TPK_LMK, TAK = @TAK ,TAK_CHK = @TAK_CHK WHERE atm_id = @atm_id",
            new { TPK_TMK, TPK_CHK, TPK_LMK, TAK,TAK_CHK, atm_id = terminalID }
                );
        }

        public Sessions_Core_ZPK_to_TPK GetTPKZPKByTerminalId(string terminalID)
        {
           return Connection.Query<Sessions_Core_ZPK_to_TPK>("SELECT ZPK_LMK, (SELECT TPK_LMK FROM sessions_triton WHERE atm_id = @atm_id) as TPK_LMK FROM sessions_as2805 WHERE host_id = '1' AND keyset_description = 'Send' LIMIT 1", new { atm_id=terminalID}).FirstOrDefault();
        }


        public Sessions_Triton_Location GetNameLocationByTerminalId(string terminalID)
        {
            return Connection.Query<Sessions_Triton_Location>("SELECT name_location_name, name_location_city, name_location_state, name_location_country  FROM sessions_triton WHERE atm_id = @atm_id",
               new {atm_id= terminalID}).FirstOrDefault();
        }

        public TerminalDetails GetTerminalKeys(string terminalID)
        {
            return Connection.Query<TerminalDetails>(" Select TPK_TMK FROM sessions_triton WHERE atm_id = @atm_id", new { atm_id = terminalID }).FirstOrDefault();
        }

        public void InsertTritonLogRecord(TritonNode_Transaction transaction)
        {
            Connection.Execute(transaction.ToString());
        }

        public void InsertTritonDayTotals(Terminals_Day_Totals dayTotals)
        {
            Connection.Execute(dayTotals.ToString());
        }
    }
}
