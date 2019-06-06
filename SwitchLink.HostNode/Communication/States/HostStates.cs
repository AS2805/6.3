namespace SwitchLink.HostNode.Communication.States
{
    internal enum HostWorkflowState
    {
        WaitForSignOnRequest,
        ReceivedSignOnRequest,
        SendSignOnRequest,        
        WaitForSignOnReply,
        ReceivedSignOnReply,
        RecievedKeyExchangeRequest,
        SendKeyExchangeRequest,
        WaitForKeyExchangeReply,
        ReceivedKeyExchangeReply,
        //GenerateZoneKeys,
        //KeyExchangeCompleted,
        Ready
    }


    public enum HostState
    {
        Connected,
        Disconnected,
        SigningOn, 
        KeyExchangeInProcess,
        SignedOn,
        Transacting
    }
}