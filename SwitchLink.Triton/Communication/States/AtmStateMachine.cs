using System.Collections.Generic;
using Hik.Communication.Scs.Communication;

namespace SwitchLink.TritonNode.Communication.States
{
    class AtmStateMachine
    {
        class StateTransition
        {
            readonly AtmStates _currentState;
            readonly SendMsg _command;

            public StateTransition(AtmStates currentState, SendMsg command)
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

        readonly Dictionary<StateTransition, AtmStates> _transitions;
        /// <summary>
        /// 
        /// </summary>
        public AtmStates CurrentState { get; set; }

        public AtmStateMachine()
        {
            _transitions = new Dictionary<StateTransition, AtmStates>
            {
                { new StateTransition(AtmStates.ST_INIT, SendMsg.Enq), AtmStates.ST_WAIT_FOR_REQ },
                { new StateTransition(AtmStates.ST_PROCESSING_REQ, SendMsg.Ack), AtmStates.ST_SEND_ACK },
                { new StateTransition(AtmStates.ST_SEND_ACK, SendMsg.Stx), AtmStates.ST_WAIT_FOR_ACK_NAK },
                { new StateTransition(AtmStates.ST_SEND_EOT, SendMsg.Eot), AtmStates.ST_DISCONNECT }
            };
        }

        public AtmStates GetNext(SendMsg command)
        {
            var transition = new StateTransition(CurrentState, command);
            AtmStates nextState;
            if (!_transitions.TryGetValue(transition, out nextState))
            {
                throw new CommunicationStateException("Incorrect State");
            }
            return nextState;
        }

        public AtmStates MoveNext(SendMsg command)
        {
            CurrentState = GetNext(command);
            return CurrentState;
        }
    }

    public enum SendMsg
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
        Ack = 3,
        /// <summary>
        /// 
        /// </summary>
        None = 99
    }
}
