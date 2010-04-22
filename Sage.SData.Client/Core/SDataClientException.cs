using System;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Exception for SDataClient
    /// </summary>
    public class SDataClientException : Exception
    {
        // Base Exception class constructors.
        /// <summary>
        /// constructor
        /// </summary>
        public SDataClientException()
        {
        }

        /// <summary>
        /// costructor
        /// </summary>
        /// <param name="message"></param>
        public SDataClientException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SDataClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}