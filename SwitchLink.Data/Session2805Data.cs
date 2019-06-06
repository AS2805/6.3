using System.Linq;
using Dapper;
using SwitchLink.Data.Models;

namespace SwitchLink.Data
{
   public class Session2805Data : BaseData
    {
        public Trnaslate_Zpk_Lmk GetZpk_Lmk()
        {
            return Connection.Query<Trnaslate_Zpk_Lmk>(" Select ZPK_LMK FROM sessions_as2805 WHERE keyset_description = @command", new { command = "Send" }).FirstOrDefault();
        }

        public GetKeks GetKeks_Send()
        {
            return Connection.Query<GetKeks>(" Select Keks FROM sessions_as2805 WHERE keyset_description = @command", new { command = "Send" }).FirstOrDefault();
        }
        public GetKeks GetKeks_Recieve()
        {
            return Connection.Query<GetKeks>(" Select Keks FROM sessions_as2805 WHERE keyset_description = @command", new { command = "Recieve" }).FirstOrDefault();
        }

        public GetKekr GetKekr_Send()
        {
            return Connection.Query<GetKekr>(" Select Kekr FROM sessions_as2805 WHERE keyset_description = @command", new { command = "Send" }).FirstOrDefault();
        }

        public GetKekr GetKekr_Recieve()
       {
           using (Connection)
           {
               return
                   Connection.Query<GetKekr>(" Select Kekr FROM sessions_as2805 WHERE keyset_description = @command",
                       new {command = "Recieve"}).FirstOrDefault();
           }
       }

       public void UpdateSession_Recieve_as2805(string ZPK_LMK, string ZPK_ZMK,string ZPK_CHECK, string ZAK_LMK,string ZAK_ZMK,string ZAK_CHECK,string ZEK_LMK,string ZEK_CHECK,string KEYSET_NUMBER )
        {
            Connection.Execute("UPDATE sessions_as2805 SET ZPK_LMK = @ZPK_LMK , ZPK_ZMK = @ZPK_ZMK, ZPK_CHECK = @ZPK_CHECK, ZAK_LMK = @ZAK_LMK ,ZAK_ZMK = @ZAK_ZMK , ZAK_CHECK = @ZAK_CHECK,ZEK_LMK = @ZEK_LMK,ZEK_CHECK = @ZEK_CHECK,ZAK_ZMK = @ZAK_ZMK,KEYSET_NUMBER=@KEYSET_NUMBER WHERE host_id = '1' AND  keyset_description = 'Recieve'",
            new { ZPK_LMK, ZPK_ZMK, ZPK_CHECK, ZAK_LMK, ZAK_ZMK, ZAK_CHECK , ZEK_LMK, ZEK_CHECK, KEYSET_NUMBER }
                );
        }

        public void UpdateSession_Send_as2805(string ZPK_LMK, string ZPK_ZMK, string ZPK_CHECK, string ZAK_LMK, string ZAK_ZMK, string ZAK_CHECK, string ZEK_LMK, string ZEK_ZMK, string ZEK_CHECK, string KEYSET_NUMBER)
        {
            Connection.Execute("UPDATE sessions_as2805 SET ZPK_LMK = @ZPK_LMK , ZPK_ZMK = @ZPK_ZMK, ZPK_CHECK = @ZPK_CHECK, ZAK_LMK = @ZAK_LMK ,ZAK_ZMK = @ZAK_ZMK , ZAK_CHECK = @ZAK_CHECK,ZEK_LMK = @ZEK_LMK,ZEK_ZMK = @ZEK_ZMK,ZEK_CHECK = @ZEK_CHECK,ZAK_ZMK = @ZAK_ZMK,KEYSET_NUMBER=@KEYSET_NUMBER WHERE host_id = '1' AND  keyset_description = 'Send'",
            new { ZPK_LMK, ZPK_ZMK, ZPK_CHECK, ZAK_LMK, ZAK_ZMK, ZAK_CHECK, ZEK_LMK,ZEK_ZMK, ZEK_CHECK, KEYSET_NUMBER }
                );
        }
    }
}
