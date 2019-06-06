using System;
using System.Threading.Tasks;
using SwitchLink.ProtocalFactory.TritonNode.Models;
using SwitchLink.TritonNode.Services;

namespace SwitchLink.TritonNode.Controller
{
    interface ILoggerController
    {
        void LogTritonRequest(BaseModel atm, DateTime takenByCore);
        void LogTritonResponse(BaseModel atm, DateTime responseSent, DateTime responseFromCoreNode);
    }

    class LoggerController : ILoggerController
    {
        private readonly DateTime _atmConnected;
        private readonly string _atmId;
        private readonly string _atmIp;
        private readonly ILoggerService _srvTransaction;

        private DateTime _takenByCore;
        private DateTime _responseFromCoreNode;
        private DateTime _responseSent;
        
        public LoggerController(DateTime atmConnected, string atmId, string atmIp)
        {
            _atmConnected = atmConnected;
            _atmId = atmId;
            _atmIp = atmIp;
            _srvTransaction = new LoggerService();
        }

        public async void LogTritonRequest(BaseModel atm, DateTime takenByCore)
        {
            _takenByCore = takenByCore;

            if (atm is TransactionModel)
                await Task.Run(() => { _srvTransaction.LogTritonAuthorizationRequest(atm as TransactionModel, _atmConnected, _takenByCore, _atmId, _atmIp); });
            else if(atm is ConfigModel)
                await Task.Run(() => { _srvTransaction.LogTritonConfigRequest(atm as ConfigModel, _atmConnected, _takenByCore, _atmId, _atmIp); });
            else if (atm is ReversalModel)
                await Task.Run(() => { _srvTransaction.LogTritonReversalRequest(atm as ReversalModel, _atmConnected, _takenByCore, _atmId, _atmIp); });
            else if (atm is HostTotalModel)
            {
                await Task.Run(() => { _srvTransaction.LogTritonHostTotalRequest(atm as HostTotalModel, _atmConnected, _takenByCore, _atmId, _atmIp); });
                await Task.Run(() => { _srvTransaction.LogTritonDayTotal(atm as HostTotalModel, _atmId); });
            }
        }

        public async void LogTritonResponse(BaseModel atm, DateTime responseSent, DateTime responseFromCoreNode)
        {
            _responseSent = responseSent;
            _responseFromCoreNode = responseFromCoreNode;

            if (atm is TransactionModel)
            {
                await Task.Run(() => { _srvTransaction.LogTritonAuthorizationResponse(atm as TransactionModel, _responseSent, _responseFromCoreNode, _atmConnected, _takenByCore, _atmId, _atmIp); });
            }
            else if (atm is ReversalModel)
            {
                await Task.Run(() => { _srvTransaction.LogTritonReversalResponse(atm as ReversalModel, _responseSent, _responseFromCoreNode, _atmConnected, _takenByCore, _atmId, _atmIp); });
            }
            else if (atm is ConfigModel)
            {
                await Task.Run(() => { _srvTransaction.LogTritonConfigResponse(atm as ConfigModel, _responseSent, _responseFromCoreNode, _atmConnected, _takenByCore, _atmId, _atmIp); });
            } 
            else if (atm is HostTotalModel)
            {
                await Task.Run(() => { _srvTransaction.LogTritonHostTotalResponse(atm as HostTotalModel, _responseSent, _responseFromCoreNode, _atmConnected, _takenByCore, _atmId, _atmIp); });
            }
        }
    }
}
