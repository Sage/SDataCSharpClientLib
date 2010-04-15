/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
01/18/2008	brian.kuhn	Created SyndicationExtension Class
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;

using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Provides the set of methods, properties and events that web content syndication extensions should inherit from.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The <see cref="SyndicationExtension"/> abstract class is provided to reduce the difficulty of implementing custom syndication extensions. 
    ///         While implementers are free to implement their custom syndication extensions by implementing the <see cref="ISyndicationExtension"/> interface, 
    ///         it is <i>recommended</i> that custom syndication extensions inherit from the <see cref="SyndicationExtension"/> base class.
    ///     </para>
    ///     <para>
    ///         If you choose to not inherit from the <see cref="SyndicationExtension"/> abstract base class, please be aware that the <see cref="SyndicationExtensionAdapter"/> class 
    ///         internally calls the <see cref="Activator.CreateInstance(Type)"/> method, and so any custom syndication extension will need to have the 
    ///         appropriate <see cref="SecurityPermissionAttribute"/> and <see cref="ReflectionPermissionAttribute"/> attributes applied as necessary.
    ///     </para>
    /// </remarks>
    [Serializable()]
    public abstract class SyndicationExtension : ISyndicationExtension, IXmlSerializable
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the human-readable description of the syndication extension.
        /// </summary>
        private string extensionDescription  = String.Empty;
        /// <summary>
        /// Private member to hold a URL that points to documentation for the syndication extension.
        /// </summary>
        private Uri extensionDocumentation;
        /// <summary>
        /// Private member to hold the human-readable name of the syndication extension.
        /// </summary>
        private string extensionName = String.Empty;
        /// <summary>
        /// Private member to hold the version of the specification that the syndication extension conforms to.
        /// </summary>
        private Version extensionVersion;
        /// <summary>
        /// Private member to hold the XML namespace that is used when qualifying the syndication extension's element and attribute names.
        /// </summary>
        private string extensionXmlNamespace = String.Empty;
        /// <summary>
        /// Private member to hold the prefix used to associate the syndication extension's element and attribute names with the syndication extension's XML namespace.
        /// </summary>
        private string extensionXmlPrefix    = String.Empty;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SyndicationExtension()
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationExtension"/> class.
        /// </summary>
        protected SyndicationExtension()
        {
            //------------------------------------------------------------
            //	
            //------------------------------------------------------------
        }
        #endregion

        #region SyndicationExtension(string xmlPrefix, string xmlNamespace, Version version)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationExtension"/> class using the supplied parameters.
        /// </summary>
        /// <param name="xmlPrefix">The prefix used to associate this syndication extension's element and attribute names with this syndication extension's XML namespace.</param>
        /// <param name="xmlNamespace">The XML namespace that is used when qualifying this syndication extension's element and attribute names.</param>
        /// <param name="version">The <see cref="Version"/> of the specification that this syndication extension conforms to.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlPrefix"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlPrefix"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlNamespace"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlNamespace"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="version"/> is a null reference (Nothing in Visual Basic).</exception>
        protected SyndicationExtension(string xmlPrefix, string xmlNamespace, Version version)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNullOrEmptyString(xmlPrefix, "xmlPrefix");
            Guard.ArgumentNotNullOrEmptyString(xmlNamespace, "xmlNamespace");
            Guard.ArgumentNotNull(version, "version");
            
            //------------------------------------------------------------
            //	Initialize static class members
            //------------------------------------------------------------
            extensionXmlPrefix      = xmlPrefix.Trim();
            extensionXmlNamespace   = xmlNamespace.Trim();
            extensionVersion        = version;
        }
        #endregion

        #region SyndicationExtension(string xmlPrefix, string xmlNamespace, Version version, Uri documentation, string name, string description)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationExtension"/> class using the supplied parameters.
        /// </summary>
        /// <param name="xmlPrefix">The prefix used to associate this syndication extension's element and attribute names with this syndication extension's XML namespace.</param>
        /// <param name="xmlNamespace">The XML namespace that is used when qualifying this syndication extension's element and attribute names.</param>
        /// <param name="version">The <see cref="Version"/> of the specification that this syndication extension conforms to.</param>
        /// <param name="documentation">A <see cref="Uri"/> that points to the documentation for this syndication extension.</param>
        /// <param name="name">A human-readable name for this syndication extension.</param>
        /// <param name="description">A human-readable description for this syndication extension.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlPrefix"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlPrefix"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlNamespace"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlNamespace"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="version"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="documentation"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="name"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="name"/> is an empty string.</exception>
        protected SyndicationExtension(string xmlPrefix, string xmlNamespace, Version version, Uri documentation, string name, string description) : this(xmlPrefix, xmlNamespace, version)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(documentation, "documentation");
            Guard.ArgumentNotNullOrEmptyString(name, "name");

            //------------------------------------------------------------
            //	Initialize static class members
            //------------------------------------------------------------
            extensionDocumentation      = documentation;
            extensionName               = name.Trim();
            if(!String.IsNullOrEmpty(description))
            {
                extensionDescription    = description.Trim();
            }
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Description
        /// <summary>
        /// Gets a human-readable description of this syndication extension.
        /// </summary>
        /// <value>A human-readable description for this syndication extension.</value>
        public string Description
        {
            get
            {
                return extensionDescription;
            }
        }
        #endregion

        #region Documentation
        /// <summary>
        /// Gets a <see cref="Uri"/> that points to documentation for this syndication extension.
        /// </summary>
        /// <value>A <see cref="Uri"/> that points to the documentation or implementation details for this syndication extension.</value>
        public Uri Documentation
        {
            get
            {
                return extensionDocumentation;
            }
        }
        #endregion

        #region Name
        /// <summary>
        /// Gets a human-readable name of this syndication extension.
        /// </summary>
        /// <value>A human-readable name for this syndication extension.</value>
        public string Name
        {
            get
            {
                return extensionName;
            }
        }
        #endregion

        #region Version
        /// <summary>
        /// Gets the <see cref="Version"/> of the specification that this syndication extension conforms to.
        /// </summary>
        /// <value>The <see cref="Version"/> of the specification that this syndication extension conforms to.</value>
        public Version Version
        {
            get
            {
                return extensionVersion;
            }
        }
        #endregion

        #region XmlNamespace
        /// <summary>
        /// Gets the XML namespace that is used when qualifying this syndication extension's element and attribute names.
        /// </summary>
        /// <value>The XML namespace that is used when qualifying this syndication extension's element and attribute names.</value>
        public string XmlNamespace
        {
            get
            {
                return extensionXmlNamespace;
            }
        }
        #endregion

        #region XmlPrefix
        /// <summary>
        /// Gets the prefix used to associate this syndication extension's element and attribute names with this syndication extension's XML namespace.
        /// </summary>
        /// <value>The prefix used to associate this syndication extension's element and attribute names with this syndication extension's <see cref="XmlNamespace">XML namespace</see>.</value>
        public string XmlPrefix
        {
            get
            {
                return extensionXmlPrefix;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC EVENTS
        //============================================================
        #region Loaded
        /// <summary>
        /// Occurs when the <see cref="SyndicationExtension"/> state has been changed by a load operation.
        /// </summary>
        /// <seealso cref="SyndicationExtension.Load(IXPathNavigable)"/>
        /// <seealso cref="SyndicationExtension.Load(XmlReader)"/>
        public event EventHandler<SyndicationExtensionLoadedEventArgs> Loaded;
        #endregion

        //============================================================
        //	EVENT HANDLER DELEGATE METHODS
        //============================================================
        #region OnExtensionLoaded(SyndicationExtensionLoadedEventArgs e)
        /// <summary>
        /// Raises the <see cref="SyndicationExtension.Loaded"/> event.
        /// </summary>
        /// <param name="e">A <see cref="SyndicationExtensionLoadedEventArgs"/> that contains the event data.</param>
        protected virtual void OnExtensionLoaded(SyndicationExtensionLoadedEventArgs e)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            EventHandler<SyndicationExtensionLoadedEventArgs> handler   = null;

            //------------------------------------------------------------
            //	Raise event on registered handler(s)
            //------------------------------------------------------------
            handler = this.Loaded;

            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================
        #region CreateNamespaceManager(XPathNavigator navigator)
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
        public XmlNamespaceManager CreateNamespaceManager(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            XmlNamespaceManager manager = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(navigator, "navigator");

            //------------------------------------------------------------
            //	Initialize XML namespace resolver
            //------------------------------------------------------------
            manager = new XmlNamespaceManager(navigator.NameTable);

            Dictionary<string, string> namespaces   = (Dictionary<string, string>)navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);
            string existingXmlNamespace             = String.Empty;
            if (namespaces.ContainsKey(this.XmlPrefix))
            {
                existingXmlNamespace    = namespaces[this.XmlPrefix];
            }

            manager.AddNamespace(this.XmlPrefix, !String.IsNullOrEmpty(existingXmlNamespace) ? existingXmlNamespace : this.XmlNamespace);

            return manager;
        }
        #endregion

        #region ExistsInSource(XPathNavigator source)
        /// <summary>
        /// Determines if the <see cref="SyndicationExtension"/> exists in the XML data in the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <see cref="XPathNavigator"/> to parse.</param>
        /// <returns><b>true</b> if the <see cref="SyndicationExtension"/> elements or attributes are present in the <paramref name="source"/>; otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     <para>
        ///         This method should be as lightweight as possible when determining if the <see cref="SyndicationExtension"/> or its related entities are present in the <paramref name="source"/>. 
        ///         The default implementation utilizes the <see cref="XPathNavigator.GetNamespacesInScope(XmlNamespaceScope)"/> method to determine if the <paramref name="source"/> contains 
        ///         the expected namespace(s) for this <see cref="SyndicationExtension"/>.
        ///     </para>
        ///     <para>It is recommended that you call this method prior to executing a possibly costly load operation using the <see cref="SyndicationExtension.Load(IXPathNavigable)"/> method.</para>
        /// </remarks>
        public virtual bool ExistsInSource(XPathNavigator source)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool extensionExists = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            //------------------------------------------------------------
            //	Determine if extension exists
            //------------------------------------------------------------
            Dictionary<string, string> namespaces   = (Dictionary<string, string>)source.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);
            
            if (namespaces.ContainsValue(this.XmlNamespace))
            {
                extensionExists = true;
            }
            else if (namespaces.ContainsKey(this.XmlPrefix))
            {
                extensionExists = true;
            }

            return extensionExists;
        }
        #endregion

        #region WriteXmlNamespaceDeclaration(XmlWriter writer)
        /// <summary>
        /// Writes the prefixed XML namespace for the current syndication extension to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to write the prefixed XML namespace declaration to.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public void WriteXmlNamespaceDeclaration(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");

            //------------------------------------------------------------
            //	Write xmlns attribute for current extension
            //------------------------------------------------------------
            writer.WriteAttributeString("xmlns", this.XmlPrefix, null, this.XmlNamespace);
        }
        #endregion

        //============================================================
        //	IXMLSERIALIZABLE IMPLEMENTATION
        //============================================================
        #region GetSchema()
        /// <summary>
        /// This method is reserved and <u>should not be used</u>. When implementing the <see cref="IXmlSerializable"/> interface, it is recommended 
        /// that a <b>null</b> reference (Nothing in Visual Basic) is returned from this method, and instead, if 
        /// specifying a custom schema is required, to apply the <see cref="XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        ///     A <see cref="XmlSchema"/> object that represents an in-memory representation of an XML Schema as specified 
        ///     in the <b>World Wide Web Consortium (W3C)</b> XML Schema <i>Structures</i> and <i>Datatypes</i> specifications. 
        ///     The default return value for this method is a <b>null</b> reference (Nothing in Visual Basic).
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         When serializing or deserializing an object, the <see cref="XmlSerializer"/> class does not perform XML validation. 
        ///         For this reason, it is often safe to omit schema information by providing a trivial implementation of this method, 
        ///         for example by returning a <b>null</b> reference (Nothing in Visual Basic).
        ///     </para>
        ///     <para>
        ///         Some .NET Framework types as well as legacy custom types implementing the <see cref="IXmlSerializable"/> interface may be using <see cref="IXmlSerializable.GetSchema()"/> 
        ///         instead of <see cref="XmlSchemaProviderAttribute"/>. In this case, the method returns an accurate XML schema that describes the XML representation 
        ///         of the object generated by the <see cref="WriteXml(XmlWriter)"/> method.
        ///     </para>
        /// </remarks>
        public XmlSchema GetSchema()
        {
            return null;
        }
        #endregion

        #region ReadXml(XmlReader reader)
        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> stream from which the object is deserialized.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic).</exception>
        public void ReadXml(XmlReader reader)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(reader, "reader");

            this.Load(reader);
        }
        #endregion

        #region WriteXml(XmlWriter writer)
        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the object is serialized.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public void WriteXml(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");

            this.WriteTo(writer);
        }
        #endregion

        //============================================================
        //	ABSTRACT METHODS
        //============================================================
        #region Load(IXPathNavigable source)
        /// <summary>
        /// Initializes the syndication extension using the supplied <see cref="IXPathNavigable"/>.
        /// </summary>
        /// <param name="source">The <b>IXPathNavigable</b> used to load the syndication extension.</param>
        /// <returns><b>true</b> if the <see cref="SyndicationExtension"/> was able to be initialized using the supplied <paramref name="source"/>; otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     <para>
        ///         <b>Notes to Implementers:</b>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="SyndicationExtension.Loaded"/> event <b>must</b> be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        public abstract bool Load(IXPathNavigable source);
        #endregion

        #region Load(XmlReader reader)
        /// <summary>
        /// Initializes the syndication extension using the supplied <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to load the syndication extension.</param>
        /// <returns><b>true</b> if the <see cref="SyndicationExtension"/> was able to be initialized using the supplied <paramref name="reader"/>; otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     <para>
        ///         <b>Notes to Implementers:</b>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                     When implementing this method, the <see cref="XmlReader"/> should be used to create a <see cref="IXPathNavigable"/> 
        ///                     that is then passed to the <see cref="SyndicationExtension.Load(IXPathNavigable)"/> method.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="SyndicationExtension.Loaded"/> event <b>must</b> be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic).</exception>
        public abstract bool Load(XmlReader reader);
        #endregion

        #region WriteTo(XmlWriter writer)
        /// <summary>
        /// Writes the syndication extension to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to write the syndication extension.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public abstract void WriteTo(XmlWriter writer);
        #endregion
    }
}
