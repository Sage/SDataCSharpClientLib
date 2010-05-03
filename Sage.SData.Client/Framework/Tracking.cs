// Copyright (c) Sage (UK) Limited 2010. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use this
// code. Please contact Sage (UK) if you do not have such a licence. Sage will take
// appropriate legal action against those who make unauthorised use of this code.

using System.Xml.Serialization;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Represents tracking information used to track the progress of an
    /// asynchronous operation.
    /// </summary>
    [XmlRoot(Namespace = Common.SData.Namespace)]
    [XmlType(TypeName = "tracking", Namespace = Common.SData.Namespace)]
    public class Tracking
    {
        /// <summary>
        /// The current phase of the operation.
        /// </summary>
        [XmlElement("phase")]
        public string Phase { get; set; }

        /// <summary>
        /// More detailed information about the current phase of the operation.
        /// </summary>
        [XmlElement("phaseDetail")]
        public string PhaseDetail { get; set; }

        /// <summary>
        /// Percentage of the operation which is completed.
        /// </summary>
        [XmlElement("progress")]
        public decimal Progress { get; set; }

        /// <summary>
        /// Time elapsed since operation started, in seconds.
        /// </summary>
        [XmlElement("elapsedSeconds")]
        public int ElapsedSeconds { get; set; }

        /// <summary>
        /// Expected remaining time, in seconds
        /// </summary>
        [XmlElement("remainingSeconds")]
        public int RemainingSeconds { get; set; }

        /// <summary>
        /// Delay (in milliseconds) that the consumer should use 
        /// before polling the service again.
        /// </summary>
        [XmlElement("pollingMillis")]
        public int PollingMillis { get; set; }
    }
}