using log4net;
using SwitchLink.Comm.Interfaces.Messaging.Models.HostNode.Request;
using SwitchLink.ProtocalFactory.Helper;
using SwitchLink.ProtocalFactory.HostNode.Models;

namespace SwitchLink.HostNode.Services
{
    public interface ITransactionService
    {
        AuthorizationRequestModel RequestTranAuthorization(TranHostNodeRequestDto dto);
        ReversalModel RequestAdviceReversal(ReversalHostNodeRequestDto dto, bool isRepeat = false);
    }

    public class TransactionService : ITransactionService
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(TransactionService));
        private readonly RRN_Sequence _rrnSequence = new RRN_Sequence(); 
        public AuthorizationRequestModel RequestTranAuthorization(TranHostNodeRequestDto dto)
        {
            _log.Debug("Creating TransactionModel Request");
            string cardAcceptorId = "437586000      "; // p42_card_acceptor_id
            string nationalData = @"TCC01\EFC00000000\CCI0\FBKV\"; // p47_additional_response_national
            int securityBlock = 2; // 53
            string destKeyRow = "29365A0400000000"; // 64
            int acq_inst_id = 560258; // 32
            string mti = "0200";

            AuthorizationRequestModel trn = new AuthorizationRequestModel
                (
                mti: mti,
                processingCode: dto.P3ProcessingCode,
                transactionAmount: dto.P4AmountTran,
                transactionDate: dto.P7TransmitDt,
                stan: _rrnSequence.GetStan,
                timeLocal: dto.P12TimeLocalTran,
                dateLocation: dto.P13DateLocalTran,
                dateSettlement: dto.P15DateSettlement,
                merchantType: dto.P18MerchantType,
                posConditionCode: dto.P25PosConditionCode,
                transactionFee: dto.P28AmtTranFee,
                acqInstIdCode: acq_inst_id,
                track2: dto.P35Track2,
                retRefNo: dto.TranSeqNo,
                terminalId: dto.P41TerminalId,
                cardAcptIdCode: cardAcceptorId,
                nameLocation: dto.P43NameLocation,
                addtlDataNat: nationalData,
                pinBlock: dto.P52PinBlock,
                secControlInfo: securityBlock,
                amountCash: dto.P57AmountCash,
                mac64: destKeyRow,
                miscellaneousX: dto.MiscellaneousX
                );
            return trn;
        }

        public ReversalModel RequestAdviceReversal(ReversalHostNodeRequestDto dto, bool isRepeat)
        {
            IDataService svcData = new DataService(dto.P41TerminalId, dto.TranSeqNo, dto.AuthorizationNum);

            string cardAcceptorId = svcData.RetrieveCardAcceptorId();
            string nationalData = svcData.RetrieveAddtlDataNat();
            int securityBlock = svcData.RetrieveSecurityBlock();
            int acqInstId = svcData.RetrieveAcqInstIdCode();  // 32
            int fwdInstIdCode = svcData.RetrieveFwdInstIdCode(); // 33

            //string cardAcceptorId = "437586000      "; // p42_card_acceptor_id
            //string nationalData = @"TCC01\EFC00000000\CCI0\FBKV\"; // p47_additional_response_national
            //int securityBlock = 2; // 53
            //int acqInstId = 560258; // 32
            //int fwdInstIdCode = 61100016; // 33
            string mac128 = "7001DC4E00000000"; // 90

            string mti = isRepeat != true ? "0420" : "0421";

            ReversalRequestModel model = new ReversalRequestModel
                (
                mti: mti,
                processingCode: dto.P3ProcessingCode,
                transactionAmount: dto.P4AmountTran,
                transactionDate: dto.P7TransmitDt,
                stan: dto.AuthorizationNum,
                timeLocal: dto.P12TimeLocalTran,
                dateLocation: dto.P13DateLocalTran,
                dateSettlement: dto.P15DateSettlement,
                posConditionCode: dto.P25PosConditionCode,
                transactionFee: dto.P28AmtTranFee,
                acqInstIdCode: acqInstId,
                fwdInstIdCode: fwdInstIdCode,
                track2: dto.P35Track2,
                retRefNo: dto.TranSeqNo,
                terminalId: dto.P41TerminalId,
                cardAcptIdCode: cardAcceptorId,
                nameLocation: dto.P43NameLocation,
                addtlDataNat: nationalData,
                secControlInfo: securityBlock,
                amountCash: dto.P57AmountCash,
                mac128: mac128,
                miscellaneousX: dto.MiscellaneousX
                );
            return model;
        }
    }
}
