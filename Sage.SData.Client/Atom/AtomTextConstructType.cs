﻿/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
12/11/2007	brian.kuhn	Created AtomTextConstructType Enumeration
****************************************************************************/
using System;

using Sage.SData.Client.Common;

namespace Sage.SData.Client.Atom
{
    /// <summary>
    /// Represents the entity encoding utilized by human-readable text constructs. 
    /// </summary>
    /// <seealso cref="AtomTextConstruct"/>
    /// <remarks>
    ///     
    /// </remarks>
    [Serializable()]
    public enum AtomTextConstructType
    {
        /// <summary>
        /// No entity-encoding type specified.
        /// </summary>
        [EnumerationMetadata(DisplayName = "", AlternateValue = "")]
        None    = 0,

        /// <summary>
        /// Indicates that the human-readable text is Hyper-Text Markup Language (HTML) encoded.
        /// </summary>
        [EnumerationMetadata(DisplayName = "HTML", AlternateValue = "html")]
        Html    = 1,

        /// <summary>
        /// Indicates that the human-readable text is not encoded per a specific entity scheme.
        /// </summary>
        [EnumerationMetadata(DisplayName = "Text", AlternateValue = "text")]
        Text    = 2,

        /// <summary>
        /// Indicates that the human-readable text is Extensible Hyper-Text Markup Language (XHTML) encoded.
        /// </summary>
        [EnumerationMetadata(DisplayName = "XHTML", AlternateValue = "xhtml")]
        Xhtml   = 3
    }
}