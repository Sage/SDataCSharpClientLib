/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
12/06/2007	brian.kuhn	Created IAtomCommonObjectAttributes Interface
****************************************************************************/
using System;
using System.Globalization;

namespace Sage.SData.Client.Atom
{
    /// <summary>
    /// Allows an object to implement common Atom entity attributes by representing a set of properties, methods, indexers and events common to Atom syndication resources.
    /// </summary>
    /// <seealso cref="Sage.SData.Client.Atom.AtomEntry"/>
    /// <seealso cref="Sage.SData.Client.Atom.AtomFeed"/>
    interface IAtomCommonObjectAttributes
    {
        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region BaseUri
        /// <summary>
        /// Gets or sets the base URI other than the base URI of the document or external entity.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents a base URI other than the base URI of the document or external entity. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     <para>
        ///         The value of this property is interpreted as a URI Reference as defined in <a href="http://www.ietf.org/rfc/rfc2396.txt">RFC 2396: Uniform Resource Identifiers</a>, 
        ///         after processing according to <a href="http://www.w3.org/TR/xmlbase/#escaping">XML Base, Section 3.1 (URI Reference Encoding and Escaping)</a>.</para>
        /// </remarks>
        Uri BaseUri
        {
            get;
            set;
        }
        #endregion

        #region Language
        /// <summary>
        /// Gets or sets the natural or formal language in which the content is written.
        /// </summary>
        /// <value>A <see cref="CultureInfo"/> that represents the natural or formal language in which the content is written. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     <para>
        ///         The value of this property is a language identifier as defined by <a href="http://www.ietf.org/rfc/rfc3066.txt">RFC 3066: Tags for the Identification of Languages</a>, or its successor.
        ///     </para>
        /// </remarks>
        CultureInfo Language
        {
            get;
            set;
        }
        #endregion
    }
}
