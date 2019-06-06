using System;
using System.Linq;
using Common.Logging;
using SwitchLink.ProtocalFactory.AS2805;
using SwitchLink.ProtocalFactory.Helper;
using SwitchLink.ProtocalFactory.HostNode.Models;

namespace SwitchLink.ProtocalFactory.HostNode
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
                case "0800": // Network management: SignON request 
                    return new SignonRequestModel(builder);
                case "0810": // Network management: SignON response 
                    return new SignonResponseModel(builder);
                case "0820": // Network management: KeyChange advice request 
                    return new KeyChangeRequest(builder);
                case "0830": // Network management: KeyChange advice response 
                    return new KeyChangeResponse(builder);
                case "0210":
                    return new AuthorizationResponseModel().Create(builder);
                case "0430":
                    return new ReversalResponseModel().Create(builder);
                default:
                    throw new InvalidOperationException(string.Format("Request message error => mti code not found ({0})", builder.mti));
            }
        }
    }
}
