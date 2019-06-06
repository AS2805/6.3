namespace SwitchLink.TritonNode.Communication.States
{
    /// <summary>
    /// Triton message states
    /// </summary>
    public enum AtmStates
    {
        /// <summary>
        /// ST_INIT
        /// </summary>
        ST_INIT = 0,
        /// <summary>
        /// ST_SEND_ENQ
        /// </summary>
        ST_SEND_ENQ = 1,
        /// <summary>
        /// </summaST_WAIT_FOR_REQry>
        /// 
        /// </summary>
        ST_WAIT_FOR_REQ = 2,
        /// <summary>
        /// ST_PROCESSING_REQ
        /// </summary>
        ST_PROCESSING_REQ = 3,
        /// <summary>
        /// ST_SEND_RESP
        /// </summary>
        ST_SEND_RESP = 4,
        /// <summary>
        /// ST_WAIT_FOR_ACK_NAK
        /// </summary>
        ST_WAIT_FOR_ACK_NAK = 5,
        /// <summary>
        /// ST_PROCESSING_ACK
        /// </summary>
        ST_SEND_ACK = 6,
        /// <summary>
        /// ST_SEND_EOT
        /// </summary>
        ST_SEND_EOT = 7,
        /// <summary>
        /// ST_DISCONNECT
        /// </summary>
        ST_DISCONNECT = 8,
        /// <summary>
        /// ST_WAIT_FOR_EOT
        /// </summary>
        ST_WAIT_FOR_EOT = 10
    }
}
