using System;

namespace Hik.Communication.Scs.Server
{
    /// <summary>
    /// Provides some functionality that are used by servers.
    /// </summary>
    internal static class ScsServerManager
    {
        /// <summary>
        /// Used to set an auto incremential unique identifier to clients.
        /// </summary>
        private static long _lastClientId;

        /// <summary>
        /// Gets an unique number to be used as idenfitier of a client.
        /// </summary>
        /// <returns></returns>
        public static string GetClientId()
        {
            lock (new object())
            {
                return Guid.NewGuid().ToString();
            }
            //return Interlocked.Increment(ref _lastClientId);
        }
    }
}
