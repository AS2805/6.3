using System;
using SwitchLink.ProtocalFactory.AS2805.Model;
using SwitchLink.ProtocalFactory.HostNode.Models;

namespace SwitchLink.ProtocalFactory.PartnerSimulator.Helper
{
    public class PartnerMessageBuilder
    {
       public byte[] NetworkManagementRequest(NetworkManagementModel msg)
        {
            if(string.IsNullOrEmpty(msg.Mti) || msg.Mti != "0800")
                throw new ArgumentException("Mti is not valid for NetworkManagementRequest(0800)");
            var model=new AS2805Model(msg.Mti);
            model.SetField(7, DateTime.Now);
            model.SetField(11, msg.Stan);
            model.SetField(33, msg.FwdInstIdCode);
            model.SetField(48, msg.AddtlDataPriv);
            model.SetField(70, msg.NetMgtInfoCode);
            model.SetField(100, msg.RecvInstIdCode);         

            return model.ToBytes();
        }

        public byte[] NetworkManagementResponse(NetworkManagementModel msg)
        {
            if (string.IsNullOrEmpty(msg.Mti) || msg.Mti != "0810")
                throw new ArgumentException("Mti is not valid for NetworkManagementResponse(0810)");
            var model = new AS2805Model(msg.Mti);
            model.SetField(7, DateTime.Now);
            model.SetField(11, msg.Stan);
            model.SetField(33, msg.FwdInstIdCode);
            model.SetField(39, msg.RespCode);
            model.SetField(48, msg.AddtlDataPriv);
            model.SetField(70, msg.NetMgtInfoCode);
            model.SetField(100, msg.RecvInstIdCode);

            return model.ToBytes();
        }

        public byte[] NetworkManagementAdviceRequest(NetworkManagementModel msg)
        {
            if (string.IsNullOrEmpty(msg.Mti) || msg.Mti != "0820")
                throw new ArgumentException("Mti is not valid for NetworkManagementAdviceRequest(0820)");
            var model = new AS2805Model(msg.Mti);
            model.SetField(7, DateTime.Now);
            model.SetField(11, msg.Stan);
            model.SetField(33, msg.FwdInstIdCode);
            model.SetField(48, msg.AddtlDataPriv);
            model.SetField(53, msg.SecControlInfo);
            model.SetField(70, msg.NetMgtInfoCode);
            model.SetField(100, msg.RecvInstIdCode);

            return model.ToBytes();
        }

        public byte[] NetworkManagementAdviceResponse(NetworkManagementModel msg)
        {
            if (string.IsNullOrEmpty(msg.Mti) || msg.Mti != "0830")
                throw new ArgumentException("Mti is not valid for NetworkManagementAdviceResponse(0830)");
            var model = new AS2805Model(msg.Mti);
            model.SetField(7, DateTime.Now);
            model.SetField(11, msg.Stan);
            model.SetField(33, msg.FwdInstIdCode);
            model.SetField(39, msg.RespCode);
            model.SetField(48, msg.AddtlDataPriv);
            model.SetField(53, msg.SecControlInfo);
            model.SetField(70, msg.NetMgtInfoCode);
            model.SetField(100, msg.RecvInstIdCode);

            return model.ToBytes();
        }

        public byte[] ResponseTranAuthorization(TransactionModel msg)
        {
            if (string.IsNullOrEmpty(msg.Mti) || msg.Mti != "0200")
                throw new ArgumentException("Mti is not valid for ResponseTranAuthorization(0210)");

            var model = new AS2805Model("0210");
            model.SetField(3, msg.ProcessingCode);
            model.SetField(4, msg.AmountTran);
            model.SetField(7, msg.TranDate);
            model.SetField(11, msg.Stan);
            model.SetField(15, msg.DateSettlement);
            model.SetField(28, msg.AmtTranFee);
            model.SetField(32, msg.AcqInstIdCode);
            model.SetField(33, 61001001); // FwdInstIdCode
            //model.SetField(39, "00"); // RespCode
            model.SetField(39, Response.GetRandomResponseCode(msg.TerminalId)); // RespCode
            model.SetField(41, msg.TerminalId);
            model.SetField(42, msg.CardAcptIdCode);
            model.SetField(53, msg.SecControlInfo);
            //model.SetField(57, msg.AmountCash); // AmountCash
            model.SetField(57, new Random().Next(2000, 100000)); // AmountCash
            model.SetField(64, msg.Mac64);
            return model.ToBytes();
        }

        public byte[] ReversalAdviceResponse(ReversalRequestModel msg)
        {
            if (string.IsNullOrEmpty(msg.Mti) || msg.Mti != "0420")
                throw new ArgumentException("Mti is not valid for ReversalAdviceResponse(0420)");
            var model = new AS2805Model("0430");

            model.SetField(3, msg.ProcessingCode);
            model.SetField(4, msg.AmountTran);
            model.SetField(7, msg.TranDate);
            model.SetField(11, msg.Stan);
            model.SetField(15, msg.DateSettlement);
            model.SetField(28, msg.AmtTranFee);
            model.SetField(32, msg.AcqInstIdCode);
            model.SetField(33, 61001001); // FwdInstIdCode
            //model.SetField(39, "00"); // RespCode
            model.SetField(39, Response.GetRandomResponseCode(msg.TerminalId)); // RespCode
            model.SetField(41, msg.TerminalId);
            model.SetField(42, msg.CardAcptIdCode);
            model.SetField(53, msg.SecControlInfo);
            model.SetField(57, msg.AmountCash); // AmountCash
            model.SetField(64, msg.Mac64);
            return model.ToBytes();
        }
    }
}
