using System;
using Common.Logging;
using SwitchLink.Comm.Interfaces.Messaging.Models.CoreNode.Request;
using SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Request;

namespace SwitchLink.CoreNode.Services
{
    interface IReversalService
    {
        ReversalHostNodeRequestDto ReversalRequest(ReversalTritonNodeRequestDto req);
    }

    class ReversalService : IReversalService
    {
        private readonly ILog _log = LogManager.GetLogger<ReversalService>();

        public ReversalHostNodeRequestDto ReversalRequest(ReversalTritonNodeRequestDto req)
        {
            ICryptographyService svr = new CryptographyService(req.TerminalId);
            //string nameLocation = "800 LANGDON ST,          MADISON      AU";
            string nameLocation = svr.GetTerminalLocation();

            _log.Debug("Constructing Core Request Object");
            var requestDto = new ReversalHostNodeRequestDto
            {
                P3ProcessingCode = req.TransactionCode, // from triton
                P4AmountTran = req.Amount1, // from triton /* Authorization transaction amount */
                P7TransmitDt = DateTime.Now,
                P12TimeLocalTran = DateTime.Now,
                P13DateLocalTran = DateTime.Now,
                P15DateSettlement = DateTime.Now.AddDays(+1),
                P25PosConditionCode = req.PosCondCode,// from triton
                P28AmtTranFee = req.Amount2, // from triton /* Surcharge amount */
                P35Track2 = req.Track2, // from triton  
                P41TerminalId = req.TerminalId, // from triton
                P43NameLocation = nameLocation,
                P57AmountCash = req.Amount3, // from triton /* Dispensed amount */
                TranSeqNo = req.TranSeqNo,// from triton 
                MiscellaneousX = req.MiscellaneousX,// from triton 
                AuthorizationNum = req.AuthorizationNum
            };

            return requestDto;
        }
    }
}
