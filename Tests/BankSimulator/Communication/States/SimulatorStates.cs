namespace BankSimulator.Communication.States
{
    public enum SimulatorStates
    {
        Connected,
        Disconnect,
        RecievedSignOnRequest,
        GenerateValidationRequest,
        RecievedKeyExchangeResponse,
        RecievedKeyExchangeRequest,
        SignOnResponseSent,
        Ready,
        GenerateZoneKeys,
        RecievedSignOnResponse,
        KeyExchangeCompleted,
        AuthorizationReceived,
        ReversalRecieved
    }
}
