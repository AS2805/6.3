using System;

namespace Hik.Communication.Scs.Communication.Messages
{
    /// <summary>
    /// Stores message to be used by an event.
    /// </summary>
    public class DisconnectEventArgs : EventArgs
    {
        private readonly Exception _exception;

        /// <summary>
        /// gets the exception 
        /// </summary>
        public Exception Error { get { return _exception; } }

        /// <summary>
        /// Creates a new MessageEventArgs object.
        /// </summary>        
        /// <param name="exception">optional exception</param>
        public DisconnectEventArgs(Exception exception=null)
        {
            _exception = exception;
        }
    }
}
