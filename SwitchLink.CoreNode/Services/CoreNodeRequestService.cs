using System;
using Common.Logging;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using Hik.Communication.ScsServices.Service;
using SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Request;
using SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Response;
using SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Request;
using SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Response;
using SwitchLink.Comm.Interfaces.Services;
using SwitchLink.CoreNode.Configuration;

namespace SwitchLink.CoreNode.Services
{
    public class CoreNodeRequestService : ScsService, ICoreNodeRequestService
    {
        private readonly ILog _log = LogManager.GetLogger<CoreNodeRequestService>();

        public TranCoreNodeResponseDto RequestTransaction(TranTritonNodeRequestDto req)
        {
            using (var client = ScsServiceClientBuilder.CreateClient<IHostNodeRequestService>(new ScsTcpEndPoint(Config.HostIpAddress, Config.HostPort)))
            {
                client.Connect();
                TranCoreNodeResponseDto autResponseDto;
                IAuthorizationService svr = new AuthorizationService();
                TranHostNodeRequestDto autReq = svr.AuthorizationRequest(req);
                if (autReq != null)
                {
                    _log.Debug("Sending Core Request Object");
                    TranHostNodeResponseDto res = client.ServiceProxy.RequestTransaction(autReq);
                    // Send to HostNode
                    _log.Debug("Getting Host Response Object");
                    autResponseDto = new TranCoreNodeResponseDto
                    {
                        Stan = res.Stan,
                        ResponseCode = res.ResponseCode,
                        ResponseDescription = res.ResponseDescription,
                        ResponseAction = res.ResponseAction,
                        AmountTran = res.AmountTran,
                        AmtTranFee = res.AmtTranFee,
                        AmountCash = res.AmountCash,
                        TranSeqNo = res.TranSeqNo
                    };
                }
                else
                {
                    _log.Debug("Pin Translation failed, sending response.");
                    //send response back to terminal for decline
                    autResponseDto = new TranCoreNodeResponseDto
                    {
                        ResponseCode = "99",
                        ResponseDescription = "Pin Translation Error",
                        ResponseAction = "Decline transaction",
                        TranSeqNo = req.TranSeqNo,
                        AmountTran = req.Amount1,
                        AmtTranFee = req.Amount2
                    };
                }

                return autResponseDto;
            }
        }


        public ConfigCoreNodeResponseDto RequestConfig(ConfigTritonNodeRequestDto req)
        {
            throw new NotImplementedException();
        }

        public HostTotalCoreNodeResponseDto RequestHostTotal(HostTotalCoreNodeRequestDto req)
        {
            throw new NotImplementedException();
        }

        public ReversalCoreNodeResponseDto RequestReversal(ReversalTritonNodeRequestDto req)
        {
            using (var client = ScsServiceClientBuilder.CreateClient<IHostNodeRequestService>(new ScsTcpEndPoint(Config.HostIpAddress, Config.HostPort)))
            {
                client.Connect();

                ReversalCoreNodeResponseDto autResponseDto;
                IReversalService svr = new ReversalService();
                var reversalReq = svr.ReversalRequest(req);
                if (reversalReq != null)
                {
                    _log.Debug("Sending Core Request Object");
                    ReversalHostNodeResponseDto res = client.ServiceProxy.RequestReversal(reversalReq);
                    // Send to HostNode
                    _log.Debug("Getting Host Response Object");
                    autResponseDto = new ReversalCoreNodeResponseDto
                    {
                        Stan = res.Stan,
                        ResponseCode = res.ResponseCode,
                        ResponseDescription = res.ResponseDescription,
                        ResponseAction = res.ResponseAction,
                        AmountTran = res.AmountTran,
                        AmtTranFee = res.AmtTranFee,
                        AmountCash = res.AmountCash,
                        TranSeqNo = res.TranSeqNo
                    };
                }
                else
                {
                    _log.Debug("Pin Translation failed, sending response.");
                    //send response back to terminal for decline
                    autResponseDto = new ReversalCoreNodeResponseDto
                    {
                        ResponseCode = "99",
                        ResponseDescription = "Pin Translation Error",
                        ResponseAction = "Decline transaction",
                        TranSeqNo = req.TranSeqNo,
                        AmountTran = req.Amount1,
                        AmtTranFee = req.Amount2
                    };
                }

                return autResponseDto;
            }
        }
    }
}

