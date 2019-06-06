using Dapper;
using SwitchLink.Data.Models;

namespace SwitchLink.Data
{
    public class HostData: BaseData
    {
        public void InsertHostLogRecord(HostNode_Transaction transaction)
        {
            Connection.Execute(transaction.ToString());
        }
    }
}
