using System;
using log4net;
using SwitchLink.ProtocalFactory.AS2805.Model;
using SwitchLink.ProtocalFactory.HostNode.Models;

namespace SwitchLink.ProtocalFactory.HostNode.Helper
{
    public class BankMessageBuilder
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(BankMessageBuilder));

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

        public byte[] RequestTranAuthorization(AuthorizationRequestModel msg)
        {
            if (string.IsNullOrEmpty(msg.Mti) || msg.Mti != "0200")
                throw new ArgumentException("Mti is not valid for RequestTranAuthorization(0200)");
            var model = new AS2805Model(msg.Mti);

            model.SetField(3, msg.ProcessingCode);
            model.SetField(4, msg.AmountTran);
            model.SetField(7, msg.TranDate);
            model.SetField(11, msg.Stan);
            model.SetField(12, msg.TimeLocalTran);
            model.SetField(13, msg.DateLocalTran);
            model.SetField(15, msg.DateSettlement);
            model.SetField(18, msg.MerchantType);
            model.SetField(22, msg.MiscellaneousX);
            model.SetField(25, msg.PosConditionCode);
            model.SetField(28, msg.AmtTranFee);
            model.SetField(32, msg.AcqInstIdCode);
            model.SetField(35, msg.Track2);
            model.SetField(37, msg.TranSeqNo);
            model.SetField(41, msg.TerminalId);
            model.SetField(42, msg.CardAcptIdCode);
            model.SetField(43, msg.NameLocation);
            model.SetField(47, msg.AddtlDataNat);
            model.SetField(52, msg.PinBlock);
            model.SetField(53, msg.SecControlInfo);
            model.SetField(55, msg.MiscellaneousX);
            model.SetField(57, msg.AmountCash);
            model.SetField(64, msg.Mac64);

            _log.Info(string.Format("host node build (0200) request transaction authorization ==> {0}", model));
            return model.ToBytes();
        }

        public byte[] ReversalAdviceRequest(ReversalModel msg)
        {
            if (string.IsNullOrEmpty(msg.Mti) || msg.Mti != "0420")
                throw new ArgumentException("Mti is not valid for ReversalAdviceRequest(0420)");
            var model = new AS2805Model(msg.Mti);

            model.SetField(3, msg.ProcessingCode);
            model.SetField(4, msg.AmountTran);
            model.SetField(7, msg.TranDate);
            model.SetField(11, msg.Stan);
            model.SetField(12, msg.TimeLocalTran);
            model.SetField(13, msg.DateLocalTran);
            model.SetField(15, msg.DateSettlement);
            model.SetField(22, msg.MiscellaneousX);
            model.SetField(25, msg.PosConditionCode);
            model.SetField(28, msg.AmtTranFee);
            model.SetField(32, msg.AcqInstIdCode);
            model.SetField(35, msg.Track2);
            model.SetField(37, msg.TranSeqNo);
            model.SetField(41, msg.TerminalId);
            model.SetField(42, msg.CardAcptIdCode);
            model.SetField(43, msg.NameLocation);
            model.SetField(47, msg.AddtlDataNat);
            model.SetField(53, msg.SecControlInfo);
            model.SetField(55, msg.MiscellaneousX);
            model.SetField(57, msg.AmountCash);
            model.SetField(90, msg.Stan, msg.TranDate, msg.AcqInstIdCode, msg.FwdInstIdCode);
            model.SetField(128, msg.Mac128);

            return model.ToBytes();
        }
    }
}
