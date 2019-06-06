using System;
using System.Linq;
using Common.Logging;
using SwitchLink.ProtocalFactory.AS2805;
using SwitchLink.ProtocalFactory.Helper;
using SwitchLink.ProtocalFactory.HostNode.Models;

namespace SwitchLink.ProtocalFactory.PartnerSimulator
{
    public interface IFactory
    {
        BaseModel Create(byte[] bytes);
    }

    public class Factory : IFactory
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (Factory));
        private readonly MsgHelper _helper = new MsgHelper();

        public BaseModel Create(byte[] bytes)
        {
            byte[] msg = bytes.Skip(2).ToArray();
            _log.Debug(_helper.AsciiOctets2String(msg));
            As2805 builder = new As2805(msg);

            switch (builder.mti)
            {
                case "0800": // Network management request 
                case "0810": // Network management response 
                case "0820": // Network management advice request 
                case "0830": // Network management advice response 
                    return new NetworkManagementModel().Create(builder);
                case "0200":
                    return new AuthorizationRequestModel().Create(builder);
                case "0420":
                    return new ReversalRequestModel().Create(builder);
                default:
                    throw new InvalidOperationException(string.Format("Request message error => mti code not found ({0})", builder.mti));
            }
        }
    }
}
