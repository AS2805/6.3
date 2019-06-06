using System;
using BankSimulator.Communication.Messages;
using BankSimulator.Communication.Services;
using BankSimulator.Communication.States;
using Common.Logging;
using Hik.Communication.Scs.Communication.Messages;
using Hik.Communication.Scs.Server;

namespace BankSimulator.Communication.Controllers
{
    class SimulatorController
    {
        private readonly IScsServerClient _scsServerClient;
        private readonly Object _syncLock = new Object();
        private readonly ISimulatorCoreService _service;
        private readonly ILog _log = LogManager.GetLogger<SimulatorController>();

        public SimulatorController(IScsServerClient scsServerClient)
        {
            _scsServerClient = scsServerClient;
            _service = new SimulatorCoreService();
        }

        internal void Start()
        {
            _scsServerClient.SendMessage(_service.GenerateSignOnRequest());
            _scsServerClient.Disconnected += SwitchLinkDisconnected;
            _scsServerClient.MessageReceived += MessageFromSwitchLink;
        }

        private void MessageFromSwitchLink(object sender, MessageEventArgs e)
        {
            lock (_syncLock)
            {
                try
                {
                    var client = (IScsServerClient) sender;
                    var message = e.Message as BankRawMessage; //Server only accepts Raw messages

                    _service.HandleResponse(message);

                    switch (_service.GetState())
                    {
                        case SimulatorStates.RecievedSignOnRequest:
                        {
                            _log.Debug("sending Sign on Response to Host Node");
                            client.SendMessage(_service.GenerateSignOnResponse());
                            _log.Debug("sending Sign on Request to Host Node");
                            client.SendMessage(_service.GenerateKeyExchangeRequest());

                        }
                            break;
                        case SimulatorStates.RecievedSignOnResponse:
                        {
                            _service.FinaliseLogon();

                        }
                            break;
                        case SimulatorStates.RecievedKeyExchangeRequest:
                        {
                            _log.Debug("Sending Key Exchange Response to Host Node");
                            client.SendMessage(_service.GenerateKeyExchangeResponse());
                            _log.Debug("Simulator Ready to Accept Transactions");
                            _service.SetState(SimulatorStates.Ready);

                        }
                            break;
                        case SimulatorStates.RecievedKeyExchangeResponse:
                        {
                            _service.FinaliseKeyExchange();

                        }
                            break;
                        //Non network messages ie. auths ect

                        case SimulatorStates.AuthorizationReceived:
                        {
                            client.SendMessage(_service.GenerateAuthorizationResponse());
                        }
                            break;
                        case SimulatorStates.ReversalRecieved:
                        {
                            client.SendMessage(_service.GenerateReversalResponse());
                        }
                            break;

                    }
                }
                catch (Exception ex)
                {
                    _log.Error("Error Occured " + ex.Message);
                }
            }
        }

        //private void MessageFromSwitchLink(object sender, MessageEventArgs e)
        //{
        //    lock (_syncLock)
        //    {
        //        try
        //        {
        //            var client = (IScsServerClient) sender;
        //            var message = e.Message as BankRawMessage; //Server only accepts Raw messages

        //            _service.HandleResponse(message);

        //            if (!_service.IsReady)
        //            {
        //                switch (_service.GetState())
        //                {
        //                    case SimulatorStates.RecievedSignOnRequest:
        //                    {
        //                        _log.Debug("sending Sign on Response to Host Node");
        //                        client.SendMessage(_service.GenerateSignOnResponse());
        //                        _log.Debug("sending Sign on Request to Host Node");
        //                        client.SendMessage(_service.GenerateKeyExchangeRequest());

        //                    }
        //                        break;
        //                    case SimulatorStates.RecievedSignOnResponse:
        //                    {
        //                        _service.FinaliseLogon();

        //                    }
        //                        break;
        //                    case SimulatorStates.RecievedKeyExchangeRequest:
        //                    {
        //                        _log.Debug("Sending Key Exchange Response to Host Node");
        //                        client.SendMessage(_service.GenerateKeyExchangeResponse());
        //                        _log.Debug("Simulator Ready to Accept Transactions");
        //                        _service.SetState(SimulatorStates.Ready);

        //                    }
        //                        break;
        //                    case SimulatorStates.RecievedKeyExchangeResponse:
        //                    {
        //                        _service.FinaliseKeyExchange();

        //                    }
        //                        break;
        //                }
        //            }
        //            else
        //            {
        //                //Non network messages ie. auths ect
        //                switch (_service.GetState())
        //                {
        //                    case SimulatorStates.AuthorizationReceived:
        //                    {
        //                        client.SendMessage(_service.GenerateAuthorizationResponse());
        //                    }
        //                        break;
        //                    case SimulatorStates.ReversalRecieved:
        //                    {
        //                        client.SendMessage(_service.GenerateReversalResponse());
        //                    }
        //                        break;

        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            _log.Error("Error Occured " + ex.Message);
        //        }
        //    }
        //}

        private void SwitchLinkDisconnected(object sender,EventArgs e)
        {
            _log.Warn("HostNode is disconnected!");
        }
    }
}
