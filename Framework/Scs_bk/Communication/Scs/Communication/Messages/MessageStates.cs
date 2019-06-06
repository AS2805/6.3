using System.Collections.Generic;

namespace Hik.Communication.Scs.Communication.Messages
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageStates
    {
        class StateTransition
        {
            readonly Status _currentState;
            readonly SendCommand _command;

            public StateTransition(Status currentState, SendCommand command)
            {
                _currentState = currentState;
                _command = command;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * _currentState.GetHashCode() + 31 * _command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = obj as StateTransition;
                return other != null && _currentState == other._currentState && _command == other._command;
            }
        }

        readonly Dictionary<StateTransition, Status> _transitions;
        /// <summary>
        /// 
        /// </summary>
        public Status CurrentState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MessageStates()
        {
            _transitions = new Dictionary<StateTransition, Status>
            {
                { new StateTransition(Status.ST_INIT, SendCommand.Enq), Status.ST_WAIT_FOR_REQ },
                { new StateTransition(Status.ST_PROCESSING_REQ, SendCommand.Stx), Status.ST_WAIT_FOR_ACK_NAK },
                { new StateTransition(Status.ST_PROCESSING_ACK, SendCommand.Eot), Status.ST_WAIT_FOR_EOT },
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Status GetNext(SendCommand command)
        {
            var transition = new StateTransition(CurrentState, command);
            Status nextState;
            if (!_transitions.TryGetValue(transition, out nextState))
            {
                throw new CommunicationStateException("Incorrect State");
            }
            return nextState;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Status MoveNext(SendCommand command)
        {
            CurrentState = GetNext(command);
            return CurrentState;
        }


    }

    /// <summary>
    /// Message States. 
    /// </summary>
    public enum Status
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
        ST_PROCESSING_ACK = 6,
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

    /// <summary>
    /// 
    /// </summary>
    public enum SendCommand
    {
        /// <summary>
        /// 
        /// </summary>
        Enq = 0,
        /// <summary>
        /// 
        /// </summary>
        Stx = 1,
        /// <summary>
        /// 
        /// </summary>
        Eot = 2,

        /// <summary>
        /// 
        /// </summary>
        RawData = 3,
        /// <summary>
        /// 
        /// </summary>
        None = 99
    }
}