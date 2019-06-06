using System;
using SwitchLink.ProtocalFactory.HostNode.Helper;

namespace SwitchLink.ProtocalFactory.HostNode.Models
{
    public class BaseModel
    {
        public string Mti { get; set; }
        public DateTime TranDate { get; set; }
        public int Stan { get; set; }
        public int FwdInstIdCode { get; set; }
        public string RespCode { get; set; }                

        public string RespDescription
        {
            get { return BankResponseHelper.GetRespDescription(RespCode); }
        }
        public string RespAction
        {
            get { return BankResponseHelper.GetRespAction(RespCode); }
        }
        public int SecControlInfo { get; set; }
    }
}
