using System;
using Common.Logging;
using SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Request;
using SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Request;

namespace SwitchLink.CoreNode.Services
{
    interface IAuthorizationService
    {
        TranHostNodeRequestDto AuthorizationRequest(TranTritonNodeRequestDto req);
    }

    class AuthorizationService : IAuthorizationService
    {
        private readonly ILog _log = LogManager.GetLogger<AuthorizationService>();

        public TranHostNodeRequestDto AuthorizationRequest(TranTritonNodeRequestDto req)
        {
            TranHostNodeRequestDto requestDto;
            ICryptographyService svr = new CryptographyService(req.TerminalId);
            var translateResult = svr.TranslatePin(req.PinBlock, req.Track2);
            //string nameLocation = "800 LANGDON ST,          MADISON      AU";
            string nameLocation = svr.GetTerminalLocation();
            string merchantType = "5811";

            if (translateResult != null && translateResult.ErrorCode == "00")
            //if (translateResult != null)
            {
                _log.Debug("Constructing Core Request Object");
                requestDto = new TranHostNodeRequestDto
                {
                    P3ProcessingCode = req.TransactionCode, // from triton
                    P4AmountTran = req.Amount1, // from triton
                    P7TransmitDt = DateTime.Now,
                    P12TimeLocalTran = DateTime.Now,
                    P13DateLocalTran = DateTime.Now,
                    P15DateSettlement = DateTime.Now.AddDays(+1),
                    P18MerchantType = merchantType,
                    P25PosConditionCode = req.PosCondCode,// from triton
                    P28AmtTranFee = req.Amount2, // from triton
                    P35Track2 = req.Track2, // from triton  
                    P41TerminalId = req.TerminalId, // from triton
                    P43NameLocation = nameLocation,
                    P52PinBlock = translateResult.PinBlock, // translated pin
                    P57AmountCash =
                        req.TransactionCode == "11" || req.TransactionCode == "12" ||
                        req.TransactionCode == "15"
                            ? req.Amount1
                            : 0, // from triton
                    TranSeqNo = req.TranSeqNo, // from triton
                    MiscellaneousX = req.MiscellaneousX, // 55
                };
            }
            else
            {
                requestDto = null;
            }

            return requestDto;
        }
    }
}
