using Hik.Communication.ScsServices.Service;
using SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Request;
using SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Response;

namespace SwitchLink.Comm.Interfaces.Services
{
    /// <summary>
    /// This interface defines methods of Core Node Request service that can be called by Triton Node.
    /// </summary>
    [ScsService]
    public interface IHostNodeRequestService
    {
        TranHostNodeResponseDto RequestTransaction(TranHostNodeRequestDto req);
        ConfigHostNodeResponseDto RequestConfig(ConfigHostNodeRequestDto req);
        HostTotalHostNodeResponseDto RequestHostTotal(HostTotalHostNodeRequestDto req);
        ReversalHostNodeResponseDto RequestReversal(ReversalHostNodeRequestDto req);
    }
}
