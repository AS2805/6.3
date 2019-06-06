using Hik.Communication.ScsServices.Service;
using SwitchLink.HostNode.Messaging.Handler;

namespace SwitchLink.HostNode.Messaging.Interface
{
    /// <summary>
    /// This interface defines methods of Core Node Request service that can be called by Triton Node.
    /// </summary>
    [ScsService]
    public interface IHostNodeBaseRequestService
    {
        BaseHostNodeMessageHandler HostNodeMessageFactory(BaseHostNodeMessageHandler data);
    }
}
