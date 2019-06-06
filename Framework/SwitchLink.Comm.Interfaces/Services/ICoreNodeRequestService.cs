using Hik.Communication.ScsServices.Service;
using SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Request;
using SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Response;

namespace SwitchLink.Comm.Interfaces.Services
{
    /// <summary>
    /// This interface defines methods of Core Node Request service that can be called by Triton Node.
    /// </summary>
    [ScsService]
    public interface ICoreNodeRequestService
    {
        TranCoreNodeResponseDto RequestTransaction(TranTritonNodeRequestDto req);
        ConfigCoreNodeResponseDto RequestConfig(ConfigTritonNodeRequestDto req);
        HostTotalCoreNodeResponseDto RequestHostTotal(HostTotalCoreNodeRequestDto req);
        ReversalCoreNodeResponseDto RequestReversal(ReversalTritonNodeRequestDto req);
    }
}
