using System;
using Hik.Communication.ScsServices.Service;
using log4net;
using SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Request;
using SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Response;
using SwitchLink.Comm.Interfaces.Services;
using SwitchLink.HostNode.Controller;
using SwitchLink.HostNode.Services;
using SwitchLink.ProtocalFactory.HostNode.Helper;
using SwitchLink.ProtocalFactory.HostNode.Models;

namespace SwitchLink.HostNode.Communication.Services
{
    public class HostNodeRequestService : ScsService, IHostNodeRequestService
    {
        private readonly BankMessageBuilder _responseBuilder;
        private readonly BankingController _bank;
        private readonly ILog _log = LogManager.GetLogger(typeof(HostNodeRequestService));

        public HostNodeRequestService()
        {
            _bank = new BankingController();
            _responseBuilder = new BankMessageBuilder();
        }

        public TranHostNodeResponseDto RequestTransaction(TranHostNodeRequestDto req)
        {
            ITransactionService svc = new TransactionService();
            _log.Debug("Creating Transaction Request");
            AuthorizationRequestModel model = svc.RequestTranAuthorization(req);
            byte[] rawBytes = _responseBuilder.RequestTranAuthorization(model);
            _log.Debug("Get Transaction Response");

            try
            {
                BaseModel res = _bank.Send(rawBytes, model.Stan);

                if (res is TransactionModel)
                {
                    TransactionModel tran = res as TransactionModel;
                    TranHostNodeResponseDto responseDto = new TranHostNodeResponseDto
                    {
                        Stan = tran.Stan,
                        ResponseCode = tran.RespCode,
                        ResponseDescription = tran.RespDescription,
                        ResponseAction = tran.RespAction,
                        AmountTran = tran.AmountTran,
                        AmtTranFee = tran.AmtTranFee,
                        AmountCash = tran.AmountCash,
                        TranSeqNo = model.TranSeqNo// set from request model
                    };
                    return responseDto;
                }
                else
                {
                    throw new InvalidOperationException("incorrect responsed from Bank");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ConfigHostNodeResponseDto RequestConfig(ConfigHostNodeRequestDto req)
        {
            throw new InvalidOperationException("incorrect responsed from Bank");
        }

        public HostTotalHostNodeResponseDto RequestHostTotal(HostTotalHostNodeRequestDto req)
        {
            throw new InvalidOperationException("incorrect responsed from Bank");
        }

        public ReversalHostNodeResponseDto RequestReversal(ReversalHostNodeRequestDto req)
        {
            BaseModel res;
            ITransactionService svc = new TransactionService();
            var model = svc.RequestAdviceReversal(req);

            byte[] rawBytes = _responseBuilder.ReversalAdviceRequest(model);
            
            try
            {
                res = _bank.Send(rawBytes, model.Stan);
            }
            catch (Exception ex)
            {
                if (ex is TimeoutException)
                {
                    model = svc.RequestAdviceReversal(req, true);
                    rawBytes = _responseBuilder.ReversalAdviceRequest(model);
                    try
                    {
                        res = _bank.Send(rawBytes, model.Stan);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

            if (!(res is ReversalModel))
            {
                throw new InvalidOperationException("incorrect responsed from Bank");
            }
            else
            {
                ReversalModel reversal = res as ReversalModel;
                ReversalHostNodeResponseDto responseDto = new ReversalHostNodeResponseDto
                {
                    Stan = reversal.Stan,
                    ResponseCode = reversal.RespCode,
                    ResponseDescription = reversal.RespDescription,
                    ResponseAction = reversal.RespAction,
                    AmountTran = reversal.AmountTran,
                    AmtTranFee = reversal.AmtTranFee,
                    AmountCash = reversal.AmountCash,
                    TranSeqNo = model.TranSeqNo // set from request model
                };
                return responseDto;
            }
        }
    }
}
