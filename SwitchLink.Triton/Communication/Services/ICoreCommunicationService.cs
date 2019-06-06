using System;
using Common.Logging;
using Hik.Communication.ScsServices.Client;
using SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Request;
using SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Response;
using SwitchLink.Comm.Interfaces.Services;
using SwitchLink.ProtocalFactory.TritonNode.Models;
using SwitchLink.TritonNode.Services;

namespace SwitchLink.TritonNode.Communication.Services
{
    public interface ICoreCommunicationService
    {
        TransactionModel AuthorizationRequest(TransactionModel model);
        ConfigModel ConfigRequest(ConfigModel configModel);
        HostTotalModel HostTotalRequest(HostTotalModel hostTotalModel);
        ReversalModel ReversalRequest(ReversalModel reversalModel);
    }

    public class CoreCommunicationService : ICoreCommunicationService
    {        
        private readonly IScsServiceClient<ICoreNodeRequestService> _proxyClient;
        private readonly ILog _log = LogManager.GetLogger(typeof(CoreCommunicationService));

        public CoreCommunicationService(IScsServiceClient<ICoreNodeRequestService> proxyClient)
        {
            _proxyClient = proxyClient;
        }

        public TransactionModel AuthorizationRequest(TransactionModel atm)
        {
            using (var proxy = _proxyClient)
            {
                proxy.Connect();
                TranTritonNodeRequestDto req = new TranTritonNodeRequestDto
                    (
                        atm.TerminalId, 
                        atm.TransactionCode,
                        atm.Amount1, 
                        atm.Amount2, 
                        atm.Track2, 
                        atm.PinBlock, 
                        atm.TranSeqNo, 
                        atm.MiscellaneousX,
                        atm.PosCondCode
                    );

                _log.Debug("Sending Request to Core Node: " + req);
                TranCoreNodeResponseDto responseDto = proxy.ServiceProxy.RequestTransaction(req); // Send to CoreNode
                _log.Debug("Recieved Response from Core Node: " + responseDto);

                atm.AuthorizationCode = responseDto.ResponseCode;
                atm.AuthorizationDesc = responseDto.ResponseDescription;
                atm.AuthorizationAction = responseDto.ResponseAction;

                atm.TranSeqNo = responseDto.TranSeqNo;
                atm.Balance = responseDto.AmountCash;
                atm.AuthorizationNum = responseDto.Stan;
                return atm;
            }
        }

        public ConfigModel ConfigRequest(ConfigModel atm)
        {
            ICryptographyService svr = new CryptographyService(atm.TerminalId);
            svr.CreateSessionKeys();

            if (svr.GetResponseCode() == "00")
            {
                // Save the keys in database
                svr.UpdateSessionKeys();
                atm.EncryptedPinKey1 = svr.GetEncryptedPinKey1();
                atm.EncryptedPinKey2 = svr.GetEncryptedPinKey2();
                return atm;
            }
            _log.Error(string.Format("Response Code:{0}", svr.GetResponseCode()));
            throw new InvalidOperationException(string.Format("Response Code:{0}", svr.GetResponseCode()));

        }

        public HostTotalModel HostTotalRequest(HostTotalModel atm)
        {
            // wait for get information form database 
            return atm;
        }

        public ReversalModel ReversalRequest(ReversalModel atm)
        {
            using (var proxy = _proxyClient)
            {
                proxy.Connect();
                ReversalTritonNodeRequestDto req = new ReversalTritonNodeRequestDto 
                    (
                        atm.TerminalId, 
                        atm.TransactionCode,
                        atm.AuthorizationTranAmount, 
                        atm.AuthorizationSurAmount, 
                        atm.AuthorizationDispensedAmount,
                        atm.Track2,
                        atm.TranSeqNo, 
                        atm.MiscellaneousX,
                        atm.PosCondCode,
                        atm.AuthorizationNum
                    );

                ReversalCoreNodeResponseDto responseDto = proxy.ServiceProxy.RequestReversal(req);

                atm.AuthorizationCode = responseDto.ResponseCode;
                atm.AuthorizationDesc = responseDto.ResponseDescription;
                atm.AuthorizationAction = responseDto.ResponseAction;

                atm.AuthorizationNum = responseDto.Stan;
                atm.TranSeqNo = responseDto.TranSeqNo;
                return atm;
            }
        }
    }
}
