// Copyright (c) Sage (UK) Limited 2007. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use
// this code. Please contact [email@sage.com] if you do not have such a licence.
// Sage will take appropriate legal action against those who make unauthorised use of this
// code.

using System.Xml.Serialization;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Defines the severity of an error.
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// Informational message, does not require any special attention.
        /// </summary>
        [XmlEnum("info")] Info,

        /// <summary>
        /// Warning message: does not prevent operation from succeeding but may require attention.
        /// </summary>
        [XmlEnum("warning")] Warning,

        /// <summary>
        /// Transient error, operation failed but may succeed later in the same condition.
        /// </summary>
        [XmlEnum("transient")] Transient,

        /// <summary>
        /// Error, operation failed, request should be modified before resubmitting.
        /// </summary>
        [XmlEnum("error")] Error,

        /// <summary>
        /// Severe error, operation should not be reattempted (and other operations are likely to fail too).
        /// </summary>
        [XmlEnum("fatal")] Fatal,
    }
}