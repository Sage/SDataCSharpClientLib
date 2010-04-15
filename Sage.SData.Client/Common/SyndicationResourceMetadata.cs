/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
12/10/2007	brian.kuhn	Created SyndicationResourceMetadata Class
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace Sage.SData.Client.Common
{
    /// <summary>
    /// Represents metadata associated with a <see cref="ISyndicationResource">syndication resource</see>.
    /// </summary>
    [Serializable()]
    public class SyndicationResourceMetadata : IComparable
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the syndication content format that the syndication resource conforms to.
        /// </summary>
        private SyndicationContentFormat resourceFormat         = SyndicationContentFormat.None;
        /// <summary>
        /// Private member to hold the XML namespaces declared in the syndication resource's root element.
        /// </summary>
        private Dictionary<string, string> resourceNamespaces   = new Dictionary<string,string>();
        /// <summary>
        /// Private member to hold the version of the syndication specification that the resource conforms to.
        /// </summary>
        private Version resourceVersion;
        /// <summary>
        /// Private member to hold a XPath navigator that can be used to navigate the root element of the syndication resource.
        /// </summary>
        [NonSerialized()]
        private XPathNavigator resourceRootNode;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SyndicationResourceMetadata(XPathNavigator navigator)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationResourceMetadata"/> class using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract the syndication resource meta-data from.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        public SyndicationResourceMetadata(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(navigator, "navigator");

            //------------------------------------------------------------
            //	Extract syndication resource meta-data from navigator
            //------------------------------------------------------------
            this.Load(navigator);
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Format
        /// <summary>
        /// Gets the <see cref="SyndicationContentFormat"/> that the syndication resource conforms to.
        /// </summary>
        /// <value>
        ///     A <see cref="SyndicationContentFormat"/> enumeration value that indicates the syndication specification the resource conforms to. 
        ///     If the syndication content format is unable to be determined, returns <see cref="SyndicationContentFormat.None"/>.
        /// </value>
        public SyndicationContentFormat Format
        {
            get
            {
                return resourceFormat;
            }

            protected set
            {
                resourceFormat = value;
            }
        }
        #endregion

        #region Namespaces
        /// <summary>
        /// Gets a dictionary of the XML namespaces declared in the syndication resource.
        /// </summary>
        /// <value>A dictionary of the resource's XML namespaces, keyed off of the namespace prefix. If no XML namespaces are declared on the root element of the resource, returns an empty dictionary.</value>
        public Dictionary<string, string> Namespaces
        {
            get
            {
                return resourceNamespaces;
            }
        }
        #endregion

        #region Resource
        /// <summary>
        /// Gets a read-only <see cref="XPathNavigator"/> object that can be used to navigate the root element of the syndication resource.
        /// </summary>
        /// <value>A read-only <see cref="XPathNavigator"/> object that can be used to navigate the root element of the syndication resource.</value>
        public XPathNavigator Resource
        {
            get
            {
                return resourceRootNode;
            }
        }
        #endregion

        #region Version
        /// <summary>
        /// Gets the <see cref="Version"/> of the syndication specification that the resource conforms to.
        /// </summary>
        /// <value>The version number of the syndication specification that the resource conforms to. If format version is unable to be determined, returns <b>null</b>.</value>
        public Version Version
        {
            get
            {
                return resourceVersion;
            }
        }
        #endregion

        //============================================================
        //	STATIC METHODS
        //============================================================
        #region GetVersionFromAttribute(XPathNavigator navigator, string name)
        /// <summary>
        /// Returns a <see cref="Version"/> object for the value of the XML attribute in <paramref name="navigator"/> with a local name specified by <paramref name="name"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract the XML attribute value from.</param>
        /// <param name="name">The name of the attribute to parse in the <paramref name="navigator"/>.</param>
        /// <returns>The <see cref="Version"/> represented by the value of the specified XML attribute. If unable to determine version, returns <b>null</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="name"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="name"/> is an empty string.</exception>
        protected static Version GetVersionFromAttribute(XPathNavigator navigator, string name)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            Version version = null;

            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(navigator, "navigator");
            Guard.ArgumentNotNullOrEmptyString(name, "name");

            //------------------------------------------------------------
            //	Extract the version information
            //------------------------------------------------------------
            string value    = navigator.GetAttribute(name, String.Empty);

            //------------------------------------------------------------
            //	Extract version for specified attribute
            //------------------------------------------------------------
            if (!String.IsNullOrEmpty(value))
            {
                try
                {
                    version = new Version(value);
                }
                catch (ArgumentOutOfRangeException)
                {
                    return null;
                }
                catch (ArgumentException)
                {
                    return null;
                }
                catch(FormatException)
                {
                    return null;
                }
                catch (OverflowException)
                {
                    return null;
                }
            }
            
            return version;
        }
        #endregion

        #region TryParseApmlResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        /// <summary>
        /// Determines if the specified <see cref="XPathNavigator"/> represents a Attention Profiling Markup Language (APML) formatted syndication resource.
        /// </summary>
        /// <param name="resource">A <see cref="XPathNavigator"/> that represents the syndication resource to attempt to parse.</param>
        /// <param name="navigator">A <see cref="XPathNavigator"/> that can be used to navigate the root element of the syndication resource. This parameter is passed uninitialized.</param>
        /// <param name="version">The version of the syndication specification that the resource conforms to. This parameter is passed uninitialized.</param>
        /// <returns><b>true</b> if <paramref name="resource"/> represents a Attention Profiling Markup Language (APML) formatted syndication resource; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="resource"/> is a null reference (Nothing in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Apml")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        protected static bool TryParseApmlResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool resourceConformsToFormat   = false;
            XmlNamespaceManager manager     = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(resource, "resource");

            //------------------------------------------------------------
            //	Initialize namespace resolver
            //------------------------------------------------------------
            manager = new XmlNamespaceManager(resource.NameTable);
            manager.AddNamespace("apml", "http://www.apml.org/apml-0.6");

            //------------------------------------------------------------
            //	Determine if resource conforms to syndication format
            //------------------------------------------------------------
            version = null;
            if ((navigator = resource.SelectSingleNode("APML", manager)) != null || (navigator = resource.SelectSingleNode("apml:APML", manager)) != null)
            {
                //------------------------------------------------------------
                //	Extract version and namespaces of resource
                //------------------------------------------------------------
                version                                 = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");
                Dictionary<string, string> namespaces   = (Dictionary<string, string>)navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);

                if (namespaces.ContainsValue("http://www.apml.org/apml-0.6"))
                {
                    resourceConformsToFormat    = true;
                    if (version == null)
                    {
                        version = new Version(0, 6);
                    }
                }
            }

            return resourceConformsToFormat;
        }
        #endregion

        #region TryParseAtomResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        /// <summary>
        /// Determines if the specified <see cref="XPathNavigator"/> represents a Atom formatted syndication resource.
        /// </summary>
        /// <param name="resource">A <see cref="XPathNavigator"/> that represents the syndication resource to attempt to parse.</param>
        /// <param name="navigator">A <see cref="XPathNavigator"/> that can be used to navigate the root element of the syndication resource. This parameter is passed uninitialized.</param>
        /// <param name="version">The version of the syndication specification that the resource conforms to. This parameter is passed uninitialized.</param>
        /// <returns><b>true</b> if <paramref name="resource"/> represents a Atom formatted syndication resource; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="resource"/> is a null reference (Nothing in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        protected static bool TryParseAtomResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool resourceConformsToFormat   = false;
            XmlNamespaceManager manager     = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(resource, "resource");

            //------------------------------------------------------------
            //	Initialize namespace resolver
            //------------------------------------------------------------
            manager = new XmlNamespaceManager(resource.NameTable);
            manager.AddNamespace("atom", "http://www.w3.org/2005/Atom");
            manager.AddNamespace("atom03", "http://purl.org/atom/ns#");

            //------------------------------------------------------------
            //	Determine if resource conforms to syndication format
            //------------------------------------------------------------
            version = null;
            if ((navigator = resource.SelectSingleNode("feed", manager)) != null || (navigator = resource.SelectSingleNode("atom:feed", manager)) != null || (navigator = resource.SelectSingleNode("atom03:feed", manager)) != null)
            {
                //------------------------------------------------------------
                //	Extract version and namespaces of feed
                //------------------------------------------------------------
                version                                 = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");
                Dictionary<string, string> namespaces   = (Dictionary<string, string>)navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);

                if (namespaces.ContainsValue("http://www.w3.org/2005/Atom"))
                {
                    resourceConformsToFormat    = true;
                    if (version == null)
                    {
                        version = new Version(1, 0);
                    }
                }
                else if (namespaces.ContainsValue("http://purl.org/atom/ns#"))
                {
                    resourceConformsToFormat    = true;
                    if (version == null)
                    {
                        version = new Version(0, 3);
                    }
                }
            }
            else if ((navigator = resource.SelectSingleNode("entry", manager)) != null || (navigator = resource.SelectSingleNode("atom:entry", manager)) != null || (navigator = resource.SelectSingleNode("atom03:entry", manager)) != null)
            {
                //------------------------------------------------------------
                //	Extract version and namespaces of entry
                //------------------------------------------------------------
                version                                 = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");
                Dictionary<string, string> namespaces   = (Dictionary<string, string>)navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);

                if (namespaces.ContainsValue("http://www.w3.org/2005/Atom"))
                {
                    resourceConformsToFormat    = true;
                    if (version == null)
                    {
                        version = new Version(1, 0);
                    }
                }
                else if (namespaces.ContainsValue("http://purl.org/atom/ns#"))
                {
                    resourceConformsToFormat    = true;
                    if (version == null)
                    {
                        version = new Version(0, 3);
                    }
                }
            }

            return resourceConformsToFormat;
        }
        #endregion

        #region TryParseAtomPublishingCategoriesResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        /// <summary>
        /// Determines if the specified <see cref="XPathNavigator"/> represents a Atom Publishing Protocol category document formatted syndication resource.
        /// </summary>
        /// <param name="resource">A <see cref="XPathNavigator"/> that represents the syndication resource to attempt to parse.</param>
        /// <param name="navigator">A <see cref="XPathNavigator"/> that can be used to navigate the root element of the syndication resource. This parameter is passed uninitialized.</param>
        /// <param name="version">The version of the syndication specification that the resource conforms to. This parameter is passed uninitialized.</param>
        /// <returns><b>true</b> if <paramref name="resource"/> represents a Atom formatted syndication resource; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="resource"/> is a null reference (Nothing in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        protected static bool TryParseAtomPublishingCategoriesResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool resourceConformsToFormat   = false;
            XmlNamespaceManager manager     = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(resource, "resource");

            //------------------------------------------------------------
            //	Initialize namespace resolver
            //------------------------------------------------------------
            manager = new XmlNamespaceManager(resource.NameTable);
            manager.AddNamespace("atom", "http://www.w3.org/2005/Atom");
            manager.AddNamespace("atom03", "http://purl.org/atom/ns#");
            manager.AddNamespace("app", "http://www.w3.org/2007/app");

            //------------------------------------------------------------
            //	Determine if resource conforms to syndication format
            //------------------------------------------------------------
            version = null;
            if ((navigator = resource.SelectSingleNode("categories", manager)) != null || (navigator = resource.SelectSingleNode("app:categories", manager)) != null)
            {
                //------------------------------------------------------------
                //	Extract version and namespaces of entry
                //------------------------------------------------------------
                version                                 = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");
                Dictionary<string, string> namespaces   = (Dictionary<string, string>)navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);

                if (namespaces.ContainsValue("http://www.w3.org/2007/app"))
                {
                    resourceConformsToFormat    = true;
                    if (version == null)
                    {
                        version = new Version(1, 0);
                    }
                }
            }

            return resourceConformsToFormat;
        }
        #endregion

        #region TryParseAtomPublishingServiceResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        /// <summary>
        /// Determines if the specified <see cref="XPathNavigator"/> represents a Atom Publishing Protocol service document formatted syndication resource.
        /// </summary>
        /// <param name="resource">A <see cref="XPathNavigator"/> that represents the syndication resource to attempt to parse.</param>
        /// <param name="navigator">A <see cref="XPathNavigator"/> that can be used to navigate the root element of the syndication resource. This parameter is passed uninitialized.</param>
        /// <param name="version">The version of the syndication specification that the resource conforms to. This parameter is passed uninitialized.</param>
        /// <returns><b>true</b> if <paramref name="resource"/> represents a Atom formatted syndication resource; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="resource"/> is a null reference (Nothing in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        protected static bool TryParseAtomPublishingServiceResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool resourceConformsToFormat   = false;
            XmlNamespaceManager manager     = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(resource, "resource");

            //------------------------------------------------------------
            //	Initialize namespace resolver
            //------------------------------------------------------------
            manager = new XmlNamespaceManager(resource.NameTable);
            manager.AddNamespace("atom", "http://www.w3.org/2005/Atom");
            manager.AddNamespace("atom03", "http://purl.org/atom/ns#");
            manager.AddNamespace("app", "http://www.w3.org/2007/app");

            //------------------------------------------------------------
            //	Determine if resource conforms to syndication format
            //------------------------------------------------------------
            version = null;
            if ((navigator = resource.SelectSingleNode("service", manager)) != null || (navigator = resource.SelectSingleNode("app:service", manager)) != null)
            {
                //------------------------------------------------------------
                //	Extract version and namespaces of entry
                //------------------------------------------------------------
                version                                 = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");
                Dictionary<string, string> namespaces   = (Dictionary<string, string>)navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);

                if (namespaces.ContainsValue("http://www.w3.org/2007/app"))
                {
                    resourceConformsToFormat    = true;
                    if (version == null)
                    {
                        version = new Version(1, 0);
                    }
                }
            }

            return resourceConformsToFormat;
        }
        #endregion

        #region TryParseBlogMLResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        /// <summary>
        /// Determines if the specified <see cref="XPathNavigator"/> represents a  Web Log Markup Language (BlogML) formatted syndication resource.
        /// </summary>
        /// <param name="resource">A <see cref="XPathNavigator"/> that represents the syndication resource to attempt to parse.</param>
        /// <param name="navigator">A <see cref="XPathNavigator"/> that can be used to navigate the root element of the syndication resource. This parameter is passed uninitialized.</param>
        /// <param name="version">The version of the syndication specification that the resource conforms to. This parameter is passed uninitialized.</param>
        /// <returns><b>true</b> if <paramref name="resource"/> represents a  Web Log Markup Language (BlogML) formatted syndication resource; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="resource"/> is a null reference (Nothing in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        protected static bool TryParseBlogMLResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool resourceConformsToFormat   = false;
            XmlNamespaceManager manager     = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(resource, "resource");

            //------------------------------------------------------------
            //	Initialize namespace resolver
            //------------------------------------------------------------
            manager = new XmlNamespaceManager(resource.NameTable);
            manager.AddNamespace("blogML", "http://www.blogml.com/2006/09/BlogML");

            //------------------------------------------------------------
            //	Determine if resource conforms to syndication format
            //------------------------------------------------------------
            version = null;
            if ((navigator = resource.SelectSingleNode("blog", manager)) != null || (navigator = resource.SelectSingleNode("blogML:blog", manager)) != null)
            {
                //------------------------------------------------------------
                //	Extract version and namespaces of resource
                //------------------------------------------------------------
                version                                 = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");
                Dictionary<string, string> namespaces   = (Dictionary<string, string>)navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);

                if (namespaces.ContainsValue("http://www.blogml.com/2006/09/BlogML"))
                {
                    resourceConformsToFormat = true;
                    if (version == null)
                    {
                        version = new Version(2, 0);
                    }
                }
            }

            return resourceConformsToFormat;
        }
        #endregion

        #region TryParseMicroSummaryGeneratorResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        /// <summary>
        /// Determines if the specified <see cref="XPathNavigator"/> represents a Microsummary Generator formatted syndication resource.
        /// </summary>
        /// <param name="resource">A <see cref="XPathNavigator"/> that represents the syndication resource to attempt to parse.</param>
        /// <param name="navigator">A <see cref="XPathNavigator"/> that can be used to navigate the root element of the syndication resource. This parameter is passed uninitialized.</param>
        /// <param name="version">The version of the syndication specification that the resource conforms to. This parameter is passed uninitialized.</param>
        /// <returns><b>true</b> if <paramref name="resource"/> represents a Microsummary Generator formatted syndication resource; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="resource"/> is a null reference (Nothing in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        protected static bool TryParseMicroSummaryGeneratorResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool resourceConformsToFormat   = false;
            XmlNamespaceManager manager     = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(resource, "resource");

            //------------------------------------------------------------
            //	Initialize namespace resolver
            //------------------------------------------------------------
            manager = new XmlNamespaceManager(resource.NameTable);
            manager.AddNamespace("micro", "http://www.mozilla.org/microsummaries/0.1");

            //------------------------------------------------------------
            //	Determine if resource conforms to syndication format
            //------------------------------------------------------------
            version = null;
            if ((navigator = resource.SelectSingleNode("generator", manager)) != null || (navigator = resource.SelectSingleNode("micro:generator", manager)) != null)
            {
                //------------------------------------------------------------
                //	Extract version and namespaces of resource
                //------------------------------------------------------------
                version                                 = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");
                Dictionary<string, string> namespaces   = (Dictionary<string, string>)navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);

                if (namespaces.ContainsValue("http://www.mozilla.org/microsummaries/0.1"))
                {
                    resourceConformsToFormat = true;
                    if (version == null)
                    {
                        version = new Version(0, 1);
                    }
                }
            }

            return resourceConformsToFormat;
        }
        #endregion

        #region TryParseNewsMLResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        /// <summary>
        /// Determines if the specified <see cref="XPathNavigator"/> represents a News Markup Language (NewsML) formatted syndication resource.
        /// </summary>
        /// <param name="resource">A <see cref="XPathNavigator"/> that represents the syndication resource to attempt to parse.</param>
        /// <param name="navigator">A <see cref="XPathNavigator"/> that can be used to navigate the root element of the syndication resource. This parameter is passed uninitialized.</param>
        /// <param name="version">The version of the syndication specification that the resource conforms to. This parameter is passed uninitialized.</param>
        /// <returns><b>true</b> if <paramref name="resource"/> represents a News Markup Language (NewsML) formatted syndication resource; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="resource"/> is a null reference (Nothing in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        protected static bool TryParseNewsMLResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool resourceConformsToFormat   = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(resource, "resource");

            //------------------------------------------------------------
            //	Determine if resource conforms to syndication format
            //------------------------------------------------------------
            version = null;
            if ((navigator = resource.SelectSingleNode("NewsML")) != null)
            {
                //------------------------------------------------------------
                //	Extract version of resource
                //------------------------------------------------------------
                version     = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");

                resourceConformsToFormat    = true;
                if (version == null)
                {
                    version = new Version(2, 0);
                }
            }

            return resourceConformsToFormat;
        }
        #endregion

        #region TryParseOpenSearchDescriptionResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        /// <summary>
        /// Determines if the specified <see cref="XPathNavigator"/> represents a OpenSearch Description formatted syndication resource.
        /// </summary>
        /// <param name="resource">A <see cref="XPathNavigator"/> that represents the syndication resource to attempt to parse.</param>
        /// <param name="navigator">A <see cref="XPathNavigator"/> that can be used to navigate the root element of the syndication resource. This parameter is passed uninitialized.</param>
        /// <param name="version">The version of the syndication specification that the resource conforms to. This parameter is passed uninitialized.</param>
        /// <returns><b>true</b> if <paramref name="resource"/> represents a OpenSearch Description formatted syndication resource; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="resource"/> is a null reference (Nothing in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        protected static bool TryParseOpenSearchDescriptionResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool resourceConformsToFormat   = false;
            XmlNamespaceManager manager     = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(resource, "resource");

            //------------------------------------------------------------
            //	Initialize namespace resolver
            //------------------------------------------------------------
            manager = new XmlNamespaceManager(resource.NameTable);
            manager.AddNamespace("search", "http://a9.com/-/spec/opensearch/1.1/");

            //------------------------------------------------------------
            //	Determine if resource conforms to syndication format
            //------------------------------------------------------------
            version = null;
            if ((navigator = resource.SelectSingleNode("OpenSearchDescription", manager)) != null || (navigator = resource.SelectSingleNode("search:OpenSearchDescription", manager)) != null)
            {
                //------------------------------------------------------------
                //	Extract version and namespaces of resource
                //------------------------------------------------------------
                version                                 = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");
                Dictionary<string, string> namespaces   = (Dictionary<string, string>)navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);

                if (namespaces.ContainsValue("http://a9.com/-/spec/opensearch/1.1/"))
                {
                    resourceConformsToFormat = true;
                    if (version == null)
                    {
                        version = new Version(1, 1);
                    }
                }
            }

            return resourceConformsToFormat;
        }
        #endregion

        #region TryParseOpmlResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        /// <summary>
        /// Determines if the specified <see cref="XPathNavigator"/> represents a  Outline Processor Markup Language (OPML) formatted syndication resource.
        /// </summary>
        /// <param name="resource">A <see cref="XPathNavigator"/> that represents the syndication resource to attempt to parse.</param>
        /// <param name="navigator">A <see cref="XPathNavigator"/> that can be used to navigate the root element of the syndication resource. This parameter is passed uninitialized.</param>
        /// <param name="version">The version of the syndication specification that the resource conforms to. This parameter is passed uninitialized.</param>
        /// <returns><b>true</b> if <paramref name="resource"/> represents a  Outline Processor Markup Language (OPML) formatted syndication resource; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="resource"/> is a null reference (Nothing in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Opml")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        protected static bool TryParseOpmlResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool resourceConformsToFormat   = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(resource, "resource");

            //------------------------------------------------------------
            //	Determine if resource conforms to syndication format
            //------------------------------------------------------------
            version = null;
            if ((navigator = resource.SelectSingleNode("opml")) != null)
            {
                //------------------------------------------------------------
                //	Extract version of resource
                //------------------------------------------------------------
                version = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");

                resourceConformsToFormat    = true;
                if (version == null)
                {
                    version = new Version(2, 0);
                }
            }

            return resourceConformsToFormat;
        }
        #endregion

        #region TryParseRsdResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        /// <summary>
        /// Determines if the specified <see cref="XPathNavigator"/> represents a Really Simple Discovery (RSD) formatted syndication resource.
        /// </summary>
        /// <param name="resource">A <see cref="XPathNavigator"/> that represents the syndication resource to attempt to parse.</param>
        /// <param name="navigator">A <see cref="XPathNavigator"/> that can be used to navigate the root element of the syndication resource. This parameter is passed uninitialized.</param>
        /// <param name="version">The version of the syndication specification that the resource conforms to. This parameter is passed uninitialized.</param>
        /// <returns><b>true</b> if <paramref name="resource"/> represents a Really Simple Discovery (RSD) formatted syndication resource; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="resource"/> is a null reference (Nothing in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rsd")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        protected static bool TryParseRsdResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool resourceConformsToFormat   = false;
            XmlNamespaceManager manager     = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(resource, "resource");

            //------------------------------------------------------------
            //	Initialize namespace resolver
            //------------------------------------------------------------
            manager = new XmlNamespaceManager(resource.NameTable);
            manager.AddNamespace("rsd", "http://archipelago.phrasewise.com/rsd");

            //------------------------------------------------------------
            //	Determine if resource conforms to syndication format
            //------------------------------------------------------------
            version = null;
            if ((navigator = resource.SelectSingleNode("rsd", manager)) != null || (navigator = resource.SelectSingleNode("rsd:rsd", manager)) != null)
            {
                //------------------------------------------------------------
                //	Extract version and namespaces of resource
                //------------------------------------------------------------
                version                                 = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");
                Dictionary<string, string> namespaces   = (Dictionary<string, string>)navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);

                if (namespaces.ContainsValue("http://archipelago.phrasewise.com/rsd"))
                {
                    resourceConformsToFormat = true;
                    if (version == null)
                    {
                        version = new Version(1, 0);
                    }
                }
                else if (String.Compare(navigator.Name, "rsd", StringComparison.OrdinalIgnoreCase) == 0 && version != null)
                {
                    //  Most web log software actually fails to provide the default XML namespace per RSD spec, so this is a hack/compromise
                    resourceConformsToFormat    = true;
                }
            }

            return resourceConformsToFormat;
        }
        #endregion

        #region TryParseRssResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        /// <summary>
        /// Determines if the specified <see cref="XPathNavigator"/> represents a Really Simple Syndication (RSS) formatted syndication resource.
        /// </summary>
        /// <param name="resource">A <see cref="XPathNavigator"/> that represents the syndication resource to attempt to parse.</param>
        /// <param name="navigator">A <see cref="XPathNavigator"/> that can be used to navigate the root element of the syndication resource. This parameter is passed uninitialized.</param>
        /// <param name="version">The version of the syndication specification that the resource conforms to. This parameter is passed uninitialized.</param>
        /// <returns><b>true</b> if <paramref name="resource"/> represents a Really Simple Syndication (RSS) formatted syndication resource; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="resource"/> is a null reference (Nothing in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        protected static bool TryParseRssResource(XPathNavigator resource, out XPathNavigator navigator, out Version version)
        {
             //	Local members
            //------------------------------------------------------------
            bool resourceConformsToFormat   = false;
            XmlNamespaceManager manager     = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(resource, "resource");

            //------------------------------------------------------------
            //	Initialize namespace resolver
            //------------------------------------------------------------
            manager = new XmlNamespaceManager(resource.NameTable);
            manager.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            manager.AddNamespace("rss09", "http://my.netscape.com/rdf/simple/0.9/");
            manager.AddNamespace("rss10", "http://purl.org/rss/1.0/");

            //------------------------------------------------------------
            //	Determine if resource conforms to syndication format
            //------------------------------------------------------------
            version = null;
            if ((navigator = resource.SelectSingleNode("rss", manager)) != null)
            {
                //------------------------------------------------------------
                //	Extract version and namespaces of resource
                //------------------------------------------------------------
                version                                 = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");
                Dictionary<string, string> namespaces   = (Dictionary<string, string>)navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);

                resourceConformsToFormat    = true;
                if (version == null)
                {
                    version = new Version(2, 0);
                }
            }
            else if ((navigator = resource.SelectSingleNode("rdf:RDF", manager)) != null)
            {
                //------------------------------------------------------------
                //	Extract version and namespaces of resource
                //------------------------------------------------------------
                version                                 = SyndicationResourceMetadata.GetVersionFromAttribute(navigator, "version");
                Dictionary<string, string> namespaces   = (Dictionary<string, string>)navigator.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);

                if (namespaces.ContainsValue("http://purl.org/rss/1.0/"))
                {
                    resourceConformsToFormat    = true;
                    version                     = new Version(1, 0);
                }
                else if (namespaces.ContainsValue("http://my.netscape.com/rdf/simple/0.9/"))
                {
                    resourceConformsToFormat    = true;
                    version                     = new Version(0, 9);
                }
            }

            return resourceConformsToFormat;
        }
        #endregion

        //============================================================
        //	PRIVATE METHODS
        //============================================================
        #region Load(XPathNavigator resource)
        /// <summary>
        /// Extracts the content format, version, and XML namespaces for a syndication resource from the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="resource">The <see cref="XPathNavigator"/> to extract the syndication resource meta-data from.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="resource"/> is a null reference (Nothing in Visual Basic).</exception>
        private void Load(XPathNavigator resource)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            XPathNavigator navigator    = null;
            Version version             = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(resource, "resource");

            //------------------------------------------------------------
            //	Get XML namespaces declared on syndication resource
            //------------------------------------------------------------
            Dictionary<string, string> namespaces   = (Dictionary<string, string>)resource.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);
            foreach (string prefix in namespaces.Keys)
            {
                resourceNamespaces.Add(prefix, namespaces[prefix]);
            }

            //------------------------------------------------------------
            //	Get syndication resource format version using 
            //  version XML attribute
            //------------------------------------------------------------
            resourceVersion     = SyndicationResourceMetadata.GetVersionFromAttribute(resource, "version");

            //------------------------------------------------------------
            //	Determine content format based on root element qualified name
            //  and expected XML namespace(s)
            //------------------------------------------------------------
            if (SyndicationResourceMetadata.TryParseApmlResource(resource, out navigator, out version))
            {
                resourceFormat      = SyndicationContentFormat.Apml;
                resourceRootNode    = navigator;
                resourceVersion     = version;
            }
            else if (SyndicationResourceMetadata.TryParseAtomResource(resource, out navigator, out version))
            {
                resourceFormat      = SyndicationContentFormat.Atom;
                resourceRootNode    = navigator;
                resourceVersion     = version;
            }
            else if (SyndicationResourceMetadata.TryParseAtomPublishingCategoriesResource(resource, out navigator, out version))
            {
                resourceFormat      = SyndicationContentFormat.AtomCategoryDocument;
                resourceRootNode    = navigator;
                resourceVersion     = version;
            }
            else if (SyndicationResourceMetadata.TryParseAtomPublishingServiceResource(resource, out navigator, out version))
            {
                resourceFormat      = SyndicationContentFormat.AtomServiceDocument;
                resourceRootNode    = navigator;
                resourceVersion     = version;
            }
            else if (SyndicationResourceMetadata.TryParseBlogMLResource(resource, out navigator, out version))
            {
                resourceFormat      = SyndicationContentFormat.BlogML;
                resourceRootNode    = navigator;
                resourceVersion     = version;
            }
            else if (SyndicationResourceMetadata.TryParseMicroSummaryGeneratorResource(resource, out navigator, out version))
            {
                resourceFormat      = SyndicationContentFormat.MicroSummaryGenerator;
                resourceRootNode    = navigator;
                resourceVersion     = version;
            }
            else if (SyndicationResourceMetadata.TryParseNewsMLResource(resource, out navigator, out version))
            {
                resourceFormat      = SyndicationContentFormat.NewsML;
                resourceRootNode    = navigator;
                resourceVersion     = version;
            }
            else if (SyndicationResourceMetadata.TryParseOpenSearchDescriptionResource(resource, out navigator, out version))
            {
                resourceFormat      = SyndicationContentFormat.OpenSearchDescription;
                resourceRootNode    = navigator;
                resourceVersion     = version;
            }
            else if (SyndicationResourceMetadata.TryParseOpmlResource(resource, out navigator, out version))
            {
                resourceFormat      = SyndicationContentFormat.Opml;
                resourceRootNode    = navigator;
                resourceVersion     = version;
            }
            else if (SyndicationResourceMetadata.TryParseRsdResource(resource, out navigator, out version))
            {
                resourceFormat      = SyndicationContentFormat.Rsd;
                resourceRootNode    = navigator;
                resourceVersion     = version;
            }
            else if (SyndicationResourceMetadata.TryParseRssResource(resource, out navigator, out version))
            {
                resourceFormat      = SyndicationContentFormat.Rss;
                resourceRootNode    = navigator;
                resourceVersion     = version;
            }
            else
            {
                resourceFormat      = SyndicationContentFormat.None;
                resourceRootNode    = null;
                resourceVersion     = null;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="SyndicationResourceMetadata"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="SyndicationResourceMetadata"/>.</returns>
        /// <remarks>
        ///     This method returns a human-readable string for the current instance. Hash code values are displayed for applicable properties.
        /// </remarks>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Build the string representation
            //------------------------------------------------------------
            string format       = this.Format.ToString();
            string version      = this.Version != null ? this.Version.ToString() : String.Empty;
            string namespaces   = this.Namespaces != null ? this.Namespaces.GetHashCode().ToString(System.Globalization.NumberFormatInfo.InvariantInfo) : String.Empty;
            string resource     = this.Resource != null ? this.Resource.GetHashCode().ToString(System.Globalization.NumberFormatInfo.InvariantInfo) : String.Empty;

            return String.Format(null, "[SyndicationResourceMetadata(Format = \"{0}\", Version = \"{1}\", Namespaces = \"{2}\", Resource = \"{3}\")]", format, version, namespaces, resource);
        }
        #endregion

        //============================================================
        //	ICOMPARABLE IMPLEMENTATION
        //============================================================
        #region CompareTo(object obj)
        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared.</returns>
        /// <exception cref="ArgumentException">The <paramref name="obj"/> is not the expected <see cref="Type"/>.</exception>
        public int CompareTo(object obj)
        {
            //------------------------------------------------------------
            //	If target is a null reference, instance is greater
            //------------------------------------------------------------
            if (obj == null)
            {
                return 1;
            }

            //------------------------------------------------------------
            //	Determine comparison result using property state of objects
            //------------------------------------------------------------
            SyndicationResourceMetadata value  = obj as SyndicationResourceMetadata;

            if (value != null)
            {
                int result  = this.Format.CompareTo(value.Format);

                if (this.Version != null)
                {
                    result  = result | this.Version.CompareTo(value.Version);
                }
                else if (value.Version != null)
                {
                    result  = result | -1;
                }

                if (this.Namespaces != null && value.Namespaces != null)
                {
                    result  = result | ComparisonUtility.CompareSequence(this.Namespaces, value.Namespaces, StringComparison.Ordinal);
                }
                else if (this.Namespaces != null && value.Namespaces == null)
                {
                    result  = result | 1;
                }
                else if (this.Namespaces == null && value.Namespaces != null)
                {
                    result  = result | -1;
                }

                if (this.Resource != null && value.Resource != null)
                {
                    result  = result | String.Compare(this.Resource.OuterXml, value.Resource.OuterXml, StringComparison.OrdinalIgnoreCase);
                }
                else if (this.Resource != null && value.Resource == null)
                {
                    result  = result | 1;
                }
                else if (this.Resource == null && value.Resource != null)
                {
                    result  = result | -1;
                }

                return result;
            }
            else
            {
                throw new ArgumentException(String.Format(null, "obj is not of type {0}, type was found to be '{1}'.", this.GetType().FullName, obj.GetType().FullName), "obj");
            }
        }
        #endregion

        #region Equals(Object obj)
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current instance.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current instance.</param>
        /// <returns><b>true</b> if the specified <see cref="Object"/> is equal to the current instance; otherwise, <b>false</b>.</returns>
        public override bool Equals(Object obj)
        {
            //------------------------------------------------------------
            //	Determine equality via type then by comparision
            //------------------------------------------------------------
            if (!(obj is SyndicationResourceMetadata))
            {
                return false;
            }

            return (this.CompareTo(obj) == 0);
        }
        #endregion

        #region GetHashCode()
        /// <summary>
        /// Returns a hash code for the current instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            //------------------------------------------------------------
            //	Generate has code using unique value of ToString() method
            //------------------------------------------------------------
            char[] charArray    = this.ToString().ToCharArray();

            return charArray.GetHashCode();
        }
        #endregion

        #region == operator
        /// <summary>
        /// Determines if operands are equal.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>true</b> if the values of its operands are equal, otherwise; <b>false</b>.</returns>
        public static bool operator ==(SyndicationResourceMetadata first, SyndicationResourceMetadata second)
        {
            //------------------------------------------------------------
            //	Handle null reference comparison
            //------------------------------------------------------------
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return true;
            }
            else if (object.Equals(first, null) && !object.Equals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }
        #endregion

        #region != operator
        /// <summary>
        /// Determines if operands are not equal.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>false</b> if its operands are equal, otherwise; <b>true</b>.</returns>
        public static bool operator !=(SyndicationResourceMetadata first, SyndicationResourceMetadata second)
        {
            return !(first == second);
        }
        #endregion

        #region < operator
        /// <summary>
        /// Determines if first operand is less than second operand.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>true</b> if the first operand is less than the second, otherwise; <b>false</b>.</returns>
        public static bool operator <(SyndicationResourceMetadata first, SyndicationResourceMetadata second)
        {
            //------------------------------------------------------------
            //	Handle null reference comparison
            //------------------------------------------------------------
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return false;
            }
            else if (object.Equals(first, null) && !object.Equals(second, null))
            {
                return true;
            }

            return (first.CompareTo(second) < 0);
        }
        #endregion

        #region > operator
        /// <summary>
        /// Determines if first operand is greater than second operand.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>true</b> if the first operand is greater than the second, otherwise; <b>false</b>.</returns>
        public static bool operator >(SyndicationResourceMetadata first, SyndicationResourceMetadata second)
        {
            //------------------------------------------------------------
            //	Handle null reference comparison
            //------------------------------------------------------------
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return false;
            }
            else if (object.Equals(first, null) && !object.Equals(second, null))
            {
                return false;
            }

            return (first.CompareTo(second) > 0);
        }
        #endregion
    }
}
