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
    /// Defines the sync mode.
    /// </summary>
    public enum SyncMode
    {
        /// <summary>
        /// An alternative version of the resource.
        /// </summary>
        [XmlEnum("catchUp")] CatchUp,

        /// <summary>
        /// A resource related to this resource.
        /// </summary>
        [XmlEnum("immediate")] Immediate
    }
}