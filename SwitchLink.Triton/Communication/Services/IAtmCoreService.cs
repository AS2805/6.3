using System;
using System.IO;
using Common.Logging;
using SwitchLink.ProtocalFactory.TritonNode;
using SwitchLink.ProtocalFactory.TritonNode.Helpers;
using SwitchLink.ProtocalFactory.TritonNode.Models;
using SwitchLink.TritonNode.Communication.Messages;
using SwitchLink.TritonNode.Communication.States;
using SwitchLink.TritonNode.Controller;

namespace SwitchLink.TritonNode.Communication.Services
{
    public interface IAtmCoreService
    {
        //query
        bool IsConnected { get; }
        bool RetryResponse { get; }
        AtmRawMessage GetAtmResponse();
        //commands
        void Connect(DateTime atmConnected, string atmId, string atmIp);
        void DisConnect();
        void HandleIncomingMessage(AtmRawMessage message);
        void ResponseSent();
  }

    public class AtmCoreService : IAtmCoreService
    {
        private readonly ICoreCommunicationService _coreComm;
        private readonly IFactory _atmFactory = new Factory();
        private readonly AtmStateMachine _pState = new AtmStateMachine();
        private BaseModel _atmBaseModel;
        private byte[] _responseMsg = null;
        private ILoggerController _conLogger;
        private int _retryCount = 1;
        private readonly ILog _log = LogManager.GetLogger<AtmCoreService>();
        private BaseModel _response;
        private readonly AtmResponseBuilder _responseBuilder = new AtmResponseBuilder();
        private DateTime _responseFromCoreNode;

        public AtmCoreService(ICoreCommunicationService coreComm)
        {
            _coreComm = coreComm;
            _pState.CurrentState = AtmStates.ST_DISCONNECT;
        }

        public bool IsConnected
        {
            get { return _pState.CurrentState != AtmStates.ST_DISCONNECT; }
        }

        public bool RetryResponse { get; private set; }

        public AtmRawMessage GetAtmResponse()
        {
            if (_pState.CurrentState == AtmStates.ST_INIT)
            {
                return new AtmRawMessage(new[] { _atmFactory.Enq });
            }
            if (_pState.CurrentState == AtmStates.ST_PROCESSING_REQ)
            {
                return new AtmRawMessage(new[] { _atmFactory.Ack });
            }
            if (_pState.CurrentState == AtmStates.ST_SEND_ACK)
            {
                if (_responseMsg != null)
                {
                    return new AtmRawMessage(_responseMsg);
                }
                throw new InvalidOperationException("Invalid response from host node");
            }
            if (_pState.CurrentState == AtmStates.ST_SEND_EOT)
            {
                return new AtmRawMessage(new[] { _atmFactory.Eot });
            }
            throw new InvalidOperationException("No message to response for this stage");
        }

        public void Connect(DateTime atmConnected, string atmId, string atmIp)
        {
            _pState.CurrentState = AtmStates.ST_INIT;
            _conLogger = new LoggerController(atmConnected: atmConnected, atmId: atmId, atmIp: atmIp);
        }

        public void HandleIncomingMessage(AtmRawMessage message)
        {
            if (message != null && message.RawBytes.Length > 0)
            {
                if (message.RawBytes[0] == _atmFactory.Stx && _pState.CurrentState == AtmStates.ST_WAIT_FOR_REQ)
                {
                    RetryResponse = true;
                    _pState.CurrentState = AtmStates.ST_PROCESSING_REQ;

                    _atmBaseModel = _atmFactory.CreateAtmRequest(message.RawBytes);
                    // logging request into database && day total
                    _conLogger.LogTritonRequest(_atmBaseModel, DateTime.Now);
                    //
                    if (_atmBaseModel is TransactionModel)
                    {
                        _response = _coreComm.AuthorizationRequest(_atmBaseModel as TransactionModel);
                        _responseMsg = _responseBuilder.TranResponse(_response as TransactionModel);
                    }
                    else if (_atmBaseModel is ConfigModel)
                    {
                        _response = _coreComm.ConfigRequest(_atmBaseModel as ConfigModel);
                        _responseMsg = _responseBuilder.ConfigResponse(_response as ConfigModel);
                    }
                    else if (_atmBaseModel is HostTotalModel)
                    {
                        _response = _coreComm.HostTotalRequest(_atmBaseModel as HostTotalModel);
                        _responseMsg = _responseBuilder.HostTotalResponse(_response as HostTotalModel);
                    }
                    else if (_atmBaseModel is ReversalModel)
                    {
                        _response = _coreComm.ReversalRequest(_atmBaseModel as ReversalModel);
                        _responseMsg = _responseBuilder.ReversalResponse(_response as ReversalModel);
                    }
                    _responseFromCoreNode = DateTime.Now;
                }
                else if (message.RawBytes[0] == _atmFactory.Ack && _pState.CurrentState == AtmStates.ST_SEND_ACK)
                {
                    RetryResponse = false;
                    _pState.CurrentState = AtmStates.ST_SEND_EOT;
                    _log.Debug("Recieved Ack");
                }
                else
                {
                    throw new InvalidDataException("Invalid stage");
                }
            }
            else
            {
                throw new InvalidDataException("Response message null");
            }
        }

        public void ResponseSent()
        {
            if (_pState.CurrentState == AtmStates.ST_INIT)
            {
                _pState.CurrentState = _pState.GetNext(SendMsg.Enq);
            }
            else if (_pState.CurrentState == AtmStates.ST_PROCESSING_REQ)
            {
                _pState.CurrentState = _pState.GetNext(SendMsg.Ack);
            }
            else if (_pState.CurrentState == AtmStates.ST_SEND_ACK)
            {
                if (_retryCount < 3)
                {
                    if (_retryCount == 1)
                    {
                        // logging into database
                        _conLogger.LogTritonResponse(_response, DateTime.Now, _responseFromCoreNode);
                    }
                    _retryCount = _retryCount + 1;
                    RetryResponse = true;
                }
                else
                    RetryResponse = false;
            }
            else if (_pState.CurrentState == AtmStates.ST_SEND_EOT)
            {
                _pState.CurrentState = _pState.GetNext(SendMsg.Eot);
            }
        }

        public void DisConnect()
        {
            if (_pState.CurrentState == AtmStates.ST_SEND_ACK && _atmBaseModel is TransactionModel)
            {
                TransactionModel autReverse = _atmBaseModel as TransactionModel;
                if (autReverse.AuthorizationCode == "00")
                {
                    _log.Warn("Triton process reversal message to bank");
                    // sent reversal to bank
                    
                    ReversalModel reversal = new ReversalModel().Create(autReverse);
                    // logging request into database
                    _conLogger.LogTritonRequest(reversal, DateTime.Now);
                    //
                    var response = _coreComm.ReversalRequest(reversal);
                    // logging request into database
                    _conLogger.LogTritonResponse(response, DateTime.Now, DateTime.Now);  // logging into database
                    //
                }
                else
                {
                    // Wait to confirm 
                }
            }
            _pState.CurrentState = AtmStates.ST_DISCONNECT;
        }
    }
}
