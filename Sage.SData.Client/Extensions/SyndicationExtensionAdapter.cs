using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.XPath;

using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Represents a <see cref="XPathNavigator"/> and <see cref="SyndicationResourceLoadSettings"/> that are used to fill a <see cref="IExtensibleSyndicationObject"/>.
    /// </summary>
    public class SyndicationExtensionAdapter
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the XPathNavigator used to load a syndication extension.
        /// </summary>
        private XPathNavigator adapterNavigator;
        /// <summary>
        /// Private member to hold the XPathNavigator used to configure the load of a syndication extension.
        /// </summary>
        private SyndicationResourceLoadSettings adapterSettings  = new SyndicationResourceLoadSettings();
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SyndicationExtensionAdapter(XPathNavigator navigator, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationExtensionAdapter"/> class using the supplied <see cref="XPathNavigator"/> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="navigator">A read-only <see cref="XPathNavigator"/> object for navigating through the extended syndication resource information.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the load operation of the <see cref="IExtensibleSyndicationObject"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        public SyndicationExtensionAdapter(XPathNavigator navigator, SyndicationResourceLoadSettings settings)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(navigator, "navigator");
            Guard.ArgumentNotNull(settings, "settings");

            adapterNavigator    = navigator;
            adapterSettings     = settings;
        }
        #endregion

        //============================================================
        //	STATIC PROPERTIES
        //============================================================
        #region FrameworkExtensions
        /// <summary>
        /// Gets the collection of <see cref="Type"/> objects that represent <see cref="ISyndicationExtension"/> instances natively supported by the framework.
        /// </summary>
        /// <value>
        ///     <see cref="Collection{T}"/> collection of <see cref="Type"/> objects that represent <see cref="ISyndicationExtension"/> instances natively supported by the framework.
        /// </value>
        public static Collection<Type> FrameworkExtensions
        {
            get
            {
                Collection<Type> extensions = new Collection<Type>();

                extensions.Add(typeof(OpenSearchExtension));
                extensions.Add(typeof(SDataExtension));
                extensions.Add(typeof(SDataHttpExtension));
                extensions.Add(typeof(SimpleListSyndicationExtension));

                return extensions;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Navigator
        /// <summary>
        /// Gets the <see cref="XPathNavigator"/> used to fill an extensible syndication resource.
        /// </summary>
        /// <value>The <see cref="XPathNavigator"/> used to fill an extensible syndication resource.</value>
        public XPathNavigator Navigator
        {
            get
            {
                return adapterNavigator;
            }
        }
        #endregion

        #region Settings
        /// <summary>
        /// Gets the <see cref="SyndicationResourceLoadSettings"/> used to configure the fill of an extensible syndication resource.
        /// </summary>
        /// <value>The <see cref="SyndicationResourceLoadSettings"/> used to configure the fill of an extensible syndication resource.</value>
        public SyndicationResourceLoadSettings Settings
        {
            get
            {
                return adapterSettings;
            }
        }
        #endregion

        //============================================================
        //	STATIC METHODS
        //============================================================
        #region FillExtensionTypes(IExtensibleSyndicationObject entity, Collection<Type> types)
        /// <summary>
        /// Fills the specified collection of <see cref="Type"/> objects using the supplied <see cref="IExtensibleSyndicationObject"/>.
        /// </summary>
        /// <param name="entity">A <see cref="IExtensibleSyndicationObject"/> to extract syndication extensions from.</param>
        /// <param name="types">The <see cref="Collection{T}"/> collection of <see cref="Type"/> objects to be filled.</param>
        /// <remarks>
        ///    This method provides implementers of the <see cref="ISyndicationResource"/> interface with a simple way 
        ///    to fill a <see cref="SyndicationResourceSaveSettings.SupportedExtensions"/> collection when implementing the 
        ///    <see cref="ISyndicationResource.Save(XmlWriter, SyndicationResourceSaveSettings)"/> abstract method.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="entity"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="types"/> is a null reference (Nothing in Visual Basic).</exception>
        public static void FillExtensionTypes(IExtensibleSyndicationObject entity, Collection<Type> types)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(entity, "entity");
            Guard.ArgumentNotNull(types, "types");

            //------------------------------------------------------------
            //	Fill types collection using entity extensions
            //------------------------------------------------------------
            if (entity.HasExtensions)
            {
                foreach (ISyndicationExtension extension in entity.Extensions)
                {
                    if (extension != null)
                    {
                        Type type   = extension.GetType();
                        if (!types.Contains(type))
                        {
                            types.Add(type);
                        }
                    }
                }
            }
        }
        #endregion

        #region GetExtensions(Collection<Type> types)
        /// <summary>
        /// Creates a collection of <see cref="ISyndicationExtension"/> instances for the specified types.
        /// </summary>
        /// <param name="types">A <see cref="Collection{T}"/> collection of <see cref="Type"/> objects to be instantiated.</param>
        /// <returns>A <see cref="Collection{T}"/> collection of <see cref="ISyndicationExtension"/> objects instantiated using the supplied <paramref name="types"/>.</returns>
        /// <remarks>
        ///     <para>Each <see cref="ISyndicationExtension"/> instance in the <see cref="Collection{T}"/> collection will be instantiated using its default constructor. </para>
        ///     <para>Types that are a null reference or do not implement the <see cref="ISyndicationExtension"/> interface are ignored.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="types"/> is a null reference (Nothing in Visual Basic).</exception>
        public static Collection<ISyndicationExtension> GetExtensions(Collection<Type> types)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            Collection<ISyndicationExtension> extensions    = new Collection<ISyndicationExtension>();

            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(types, "types");

            //------------------------------------------------------------
            //	Activate instance for each valid type
            //------------------------------------------------------------
            foreach(Type type in types)
            {
                if (type != null)
                {
                    ISyndicationExtension extension = Activator.CreateInstance(type) as ISyndicationExtension;
                    if (extension != null)
                    {
                        extensions.Add(extension);
                    }
                }
            }

            return extensions;
        }
        #endregion

        #region GetExtensions(Collection<Type> types, Dictionary<string, string> namespaces)
        /// <summary>
        /// Creates a collection of <see cref="ISyndicationExtension"/> instances for the specified types.
        /// </summary>
        /// <param name="types">A <see cref="Collection{T}"/> collection of <see cref="Type"/> objects that represent user-defined syndication extensions to be instantiated.</param>
        /// <param name="namespaces">A collection of XML nameapces that are used to filter the available native framework syndication extensions.</param>
        /// <returns>
        ///     A <see cref="Collection{T}"/> collection of <see cref="ISyndicationExtension"/> objects instantiated using the supplied <paramref name="types"/> and <paramref name="namespaces"/>.
        /// </returns>
        /// <remarks>
        ///     This method instantiates all of the available native framework syndication extensions, and then filters them based on the XML namespaces and prefixes contained in the supplied <paramref name="namespaces"/>. 
        ///     The user defined syndication extensions are then instantiated, and are added to the return collection if they do not already exist.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="types"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="namespaces"/> is a null reference (Nothing in Visual Basic).</exception>
        public static Collection<ISyndicationExtension> GetExtensions(Collection<Type> types, Dictionary<string, string> namespaces)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            Collection<ISyndicationExtension> supportedExtensions   = new Collection<ISyndicationExtension>();

            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(types, "types");
            Guard.ArgumentNotNull(namespaces, "namespaces");

            //------------------------------------------------------------
            //	Add native framework extensions based on XML namespaces
            //------------------------------------------------------------
            Collection<ISyndicationExtension> nativeExtensions  = SyndicationExtensionAdapter.GetExtensions(SyndicationExtensionAdapter.FrameworkExtensions);

            foreach (ISyndicationExtension extension in nativeExtensions)
            {
                if (namespaces.ContainsValue(extension.XmlNamespace) || namespaces.ContainsKey(extension.XmlPrefix))
                {
                    if (!supportedExtensions.Contains(extension))
                    {
                        supportedExtensions.Add(extension);
                    }
                }
            }

            //------------------------------------------------------------
            //	Add user defined extensions if not already in collection
            //------------------------------------------------------------
            Collection<ISyndicationExtension> userExtensions    = SyndicationExtensionAdapter.GetExtensions(types);
            foreach (ISyndicationExtension extension in userExtensions)
            {
                if (!supportedExtensions.Contains(extension))
                {
                    supportedExtensions.Add(extension);
                }
            }

            return supportedExtensions;
        }
        #endregion

        #region WriteExtensionsTo(IEnumerable<ISyndicationExtension> extensions, XmlWriter writer)
        /// <summary>
        /// Saves the supplied <see cref="IEnumerable{T}"/> collection of <see cref="ISyndicationExtension"/> objects to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="extensions">A <see cref="IEnumerable{T}"/> collection of <see cref="ISyndicationExtension"/> objects that represent the syndication extensions to be written.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> to which you want to save.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="extensions"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public static void WriteExtensionsTo(IEnumerable<ISyndicationExtension> extensions, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(extensions, "extensions");
            Guard.ArgumentNotNull(writer, "writer");

            //------------------------------------------------------------
            //	Write each syndication extension to the supplied writer
            //------------------------------------------------------------
            foreach (ISyndicationExtension extension in extensions)
            {
                extension.WriteTo(writer);
            }
        }
        #endregion

        #region WriteXmlNamespaceDeclarations(Collection<Type> types, XmlWriter writer)
        /// <summary>
        /// Writes the prefixed XML namespace declarations for the supplied <see cref="Collection{T}"/> collection of syndication extension <see cref="Type"/> objects to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="types">A <see cref="Collection{T}"/> collection of <see cref="Type"/> objects that represent the syndication extensions to write prefixed XML namespace declarations for.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> to which you want to save.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="types"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public static void WriteXmlNamespaceDeclarations(Collection<Type> types, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(types, "types");
            Guard.ArgumentNotNull(writer, "writer");

            //------------------------------------------------------------
            //	Write each syndication extension prefixed XML namespace 
            //  to the supplied writer
            //------------------------------------------------------------
            foreach (Type type in types)
            {
                if (type != null)
                {
                    ISyndicationExtension extension = Activator.CreateInstance(type) as ISyndicationExtension;
                    if (extension != null)
                    {
                        extension.WriteXmlNamespaceDeclaration(writer);
                    }
                }
            }
        }
        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================
        #region Fill(IExtensibleSyndicationObject entity)
        /// <summary>
        /// Modifies the <see cref="IExtensibleSyndicationObject"/> to match the data source.
        /// </summary>
        /// <remarks>
        ///     A default <see cref="XmlNamespaceManager"/> is created against this adapter's <see cref="Navigator"/> property 
        ///     when resolving prefixed syndication elements and attributes.
        /// </remarks>
        /// <param name="entity">The <see cref="IExtensibleSyndicationObject"/> to be filled.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="entity"/> is a null reference (Nothing in Visual Basic).</exception>
        public void Fill(IExtensibleSyndicationObject entity)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(entity, "entity");

            //------------------------------------------------------------
            //	Create namespace resolver
            //------------------------------------------------------------
            XmlNamespaceManager manager = new XmlNamespaceManager(this.Navigator.NameTable);

            //------------------------------------------------------------
            //	Fill syndication extensions
            //------------------------------------------------------------
            this.Fill(entity, manager);
        }
        #endregion

        #region Fill(IExtensibleSyndicationObject entity, XmlNamespaceManager manager)
        /// <summary>
        /// Modifies the <see cref="IExtensibleSyndicationObject"/> to match the data source.
        /// </summary>
        /// <param name="entity">The <see cref="IExtensibleSyndicationObject"/> to be filled.</param>
        /// <param name="manager">The <see cref="XmlNamespaceManager"/> used to resolve prefixed syndication elements and attributes.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="entity"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="manager"/> is a null reference (Nothing in Visual Basic).</exception>
        public void Fill(IExtensibleSyndicationObject entity, XmlNamespaceManager manager)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            Collection<ISyndicationExtension> extensions    = new Collection<ISyndicationExtension>();

            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(entity, "entity");
            Guard.ArgumentNotNull(manager, "manager");

            //------------------------------------------------------------
            //	Get syndication extensions supported by the load operation
            //------------------------------------------------------------
            if (this.Settings.AutoDetectExtensions)
            {
                extensions  = SyndicationExtensionAdapter.GetExtensions(this.Settings.SupportedExtensions, (Dictionary<string, string>)this.Navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml));
            }
            else
            {
                extensions  = SyndicationExtensionAdapter.GetExtensions(this.Settings.SupportedExtensions);
            }

            //------------------------------------------------------------
            //	Add syndication extensions to entity if they exist in source
            //------------------------------------------------------------
            foreach (ISyndicationExtension extension in extensions)
            {
                if (extension.ExistsInSource(this.Navigator))
                {
                    ISyndicationExtension instance  = (ISyndicationExtension)Activator.CreateInstance(extension.GetType());

                    if (instance.Load(this.Navigator))
                    {
                        ((Collection<ISyndicationExtension>)entity.Extensions).Add(instance);
                    }
                }
            }
        }
        #endregion
    }
}
