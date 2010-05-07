/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
12/06/2007	brian.kuhn	Created ISyndicationExtension Interface
****************************************************************************/
using System;
using System.Xml;
using System.Xml.XPath;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Allows an object to implement a syndication extension by representing a set of properties, methods, indexers and events common to web content syndication extensions.
    /// </summary>
    /// <seealso cref="SyndicationExtension"/>
    public interface ISyndicationExtension
    {
        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Description
        /// <summary>
        /// Gets a human-readable description of the syndication extension.
        /// </summary>
        /// <value>A human-readable description for the syndication extension.</value>
        string Description
        {
            get;
        }
        #endregion

        #region Documentation
        /// <summary>
        /// Gets a <see cref="Uri"/> that points to documentation for the syndication extension.
        /// </summary>
        /// <value>A <see cref="Uri"/> that points to the documentation or implementation details for the syndication extension.</value>
        Uri Documentation
        {
            get;
        }
        #endregion

        #region Name
        /// <summary>
        /// Gets the human-readable name of the syndication extension.
        /// </summary>
        /// <value>A human-readable name for the syndication extension.</value>
        string Name
        {
            get;
        }
        #endregion

        #region Version
        /// <summary>
        /// Gets the <see cref="Version"/> of the specification that the syndication extension conforms to.
        /// </summary>
        /// <value>The <see cref="Version"/> of the specification that the syndication extension conforms to.</value>
        Version Version
        {
            get;
        }
        #endregion

        #region XmlNamespace
        /// <summary>
        /// Gets the XML namespace that is used when qualifying the syndication extension's element and attribute names.
        /// </summary>
        /// <value>The XML namespace that is used when qualifying the syndication extension's element and attribute names.</value>
        string XmlNamespace
        {
            get;
        }
        #endregion

        #region XmlPrefix
        /// <summary>
        /// Gets the prefix used to associate the syndication extension's element and attribute names with the syndication extension's XML namespace.
        /// </summary>
        /// <value>The prefix used to associate the syndication extension's element and attribute names with the syndication extension's <see cref="XmlNamespace">XML namespace</see>.</value>
        string XmlPrefix
        {
            get;
        }
        #endregion

        //============================================================
        //	PUBLIC EVENTS
        //============================================================
        #region Loaded
        /// <summary>
        /// Occurs when the syndication extension state has been changed by a load operation.
        /// </summary>
        /// <seealso cref="ISyndicationExtension.Load(IXPathNavigable)"/>
        /// <seealso cref="ISyndicationExtension.Load(XmlReader)"/>
        event EventHandler<SyndicationExtensionLoadedEventArgs> Loaded;
        #endregion

        //============================================================
        //	UTILITY METHODS
        //============================================================
        #region ExistsInSource(XPathNavigator source)
        /// <summary>
        /// Determines if the <see cref="ISyndicationExtension"/> exists in the XML data in the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <see cref="XPathNavigator"/> to parse.</param>
        /// <returns><b>true</b> if the <see cref="ISyndicationExtension"/>  elements or attributes are present in the <paramref name="source"/>; otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     <para>
        ///         <b>Notes to Implementers:</b>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                     This method should be as lightweight as possible when determining if the <see cref="ISyndicationExtension"/> or its related entities are present in the <paramref name="source"/>. 
        ///                     It is recommended that implementers utilize the <see cref="XPathNavigator.GetNamespacesInScope(XmlNamespaceScope)"/> method to determine if the <paramref name="source"/> contains 
        ///                     the expected namespace(s) for the <see cref="ISyndicationExtension"/>.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        bool ExistsInSource(XPathNavigator source);
        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================
        #region CreateNamespaceManager(XmlNameTable nameTable)
        /// <summary>
        /// Initializes a <see cref="XmlNamespaceManager"/> object for resolving prefixed XML namespaces utilized by this <see cref="SyndicationExtension"/>.
        /// </summary>
        /// <param name="navigator">Provides a cursor model for navigating syndication extension data.</param>
        /// <returns>A <see cref="XmlNamespaceManager"/> that resolves prefixed XML namespaces and provides scope management for these namespaces.</returns>
        /// <remarks>
        ///     This method will return a <see cref="XmlNamespaceManager"/> that has a namespace added to it using the <see cref="XmlPrefix"/> and <see cref="XmlNamespace"/> 
        ///     of the extension unless the supplied <see cref="XPathNavigator"/> already has an XML namespace associated to the <see cref="XmlPrefix"/>, in which case 
        ///     the associated XML namespace is used instead. This is to prevent collisions and is an attempt to gracefully handle the case where a XML namespace that 
        ///     is not per the extension's specification has been declared on the syndication resource.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        XmlNamespaceManager CreateNamespaceManager(XPathNavigator navigator);
        #endregion

        #region Load(IXPathNavigable source)
        /// <summary>
        /// Initializes the syndication extension using the supplied <see cref="IXPathNavigable"/>.
        /// </summary>
        /// <param name="source">The <b>IXPathNavigable</b> used to load the syndication extension.</param>
        /// <returns><b>true</b> if the syndication extension was able to be initialized using the supplied <paramref name="source"/>; otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     <para>
        ///         <b>Notes to Implementers:</b>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="ISyndicationExtension.Loaded"/> event <b>must</b> be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        bool Load(IXPathNavigable source);
        #endregion

        #region Load(XmlReader reader)
        /// <summary>
        /// Initializes the syndication extension using the supplied <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to load the syndication extension.</param>
        /// <returns><b>true</b> if the syndication extension was able to be initialized using the supplied <paramref name="reader"/>; otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     <para>
        ///         <b>Notes to Implementers:</b>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                     When implementing this method, the <see cref="XmlReader"/> should be used to create a <see cref="IXPathNavigable"/> 
        ///                     that is then passed to the <see cref="ISyndicationExtension.Load(IXPathNavigable)"/> method.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="ISyndicationExtension.Loaded"/> event <b>must</b> be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic).</exception>
        bool Load(XmlReader reader);
        #endregion

        #region WriteTo(XmlWriter writer)
        /// <summary>
        /// Writes the syndication extension to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to write the syndication extension.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        void WriteTo(XmlWriter writer);
        #endregion

        #region WriteXmlNamespaceDeclaration(XmlWriter writer)
        /// <summary>
        /// Writes the prefixed XML namespace for the current syndication extension to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to write the prefixed XML namespace declaration to.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        void WriteXmlNamespaceDeclaration(XmlWriter writer);
        #endregion
    }
}
