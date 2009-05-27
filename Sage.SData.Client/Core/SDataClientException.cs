using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Exception for SDataClient
    /// </summary>
    public class SDataClientException : System.Exception
    {
         // Base Exception class constructors.
        /// <summary>
        /// constructor
        /// </summary>
        public SDataClientException()
            :base() {}
        /// <summary>
        /// costructor
        /// </summary>
        /// <param name="message"></param>
        public SDataClientException(String message)
            :base(message) {}
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SDataClientException(String message, Exception innerException)
            :base(message, innerException) {}

    }
}
