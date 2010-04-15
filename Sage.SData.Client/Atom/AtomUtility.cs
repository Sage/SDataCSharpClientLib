/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
12/06/2007	brian.kuhn	Created AtomUtility Class
****************************************************************************/
using System;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

using Sage.SData.Client.Common;

namespace Sage.SData.Client.Atom
{
    /// <summary>
    /// Provides methods that comprise common utility features shared across the Atom syndication entities. This class cannot be inherited.
    /// </summary>
    /// <remarks>This utility class is not intended for use outside the Atom syndication entities within the framework.</remarks>
    internal static class AtomUtility
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the Atom 1.0 namespace identifier.
        /// </summary>
        private const string ATOM_NAMESPACE     = "http://www.w3.org/2005/Atom";
        /// <summary>
        /// Private member to hold the Atom Publishing Protocol 1.0 namespace identifier.
        /// </summary>
        private const string ATOMPUB_NAMESPACE  = "http://www.w3.org/2007/app";
        /// <summary>
        /// Private member to hold the XHTML namespace identifier.
        /// </summary>
        private const string XHTML_NAMESPACE    = "http://www.w3.org/1999/xhtml";
        /// <summary>
        /// Private member to hold the XML 1.1 namespace identifier.
        /// </summary>
        private const string XML_NAMESPACE      = "http://www.w3.org/XML/1998/namespace";
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region AtomNamespace
        /// <summary>
        /// Gets the XML namespace URI for the Atom 1.0 specification.
        /// </summary>
        /// <value>The XML namespace URI for the Atom 1.0 specification.</value>
        public static string AtomNamespace
        {
            get
            {
                return ATOM_NAMESPACE;
            }
        }
        #endregion

        #region AtomPublishingNamespace
        /// <summary>
        /// Gets the XML namespace URI for the Atom Publishing Protocol 1.0 specification.
        /// </summary>
        /// <value>The XML namespace URI for the Atom Publishing Protocol 1.0 specification.</value>
        public static string AtomPublishingNamespace
        {
            get
            {
                return ATOMPUB_NAMESPACE;
            }
        }
        #endregion

        #region XhtmlNamespace
        /// <summary>
        /// Gets the XML namespace URI for the XHTML specification.
        /// </summary>
        /// <value>The XML namespace URI for the Extensible Hyper-Text Markup Lanaguage (XHTML) specification.</value>
        public static string XhtmlNamespace
        {
            get
            {
                return XHTML_NAMESPACE;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================
        #region CreateNamespaceManager(XmlNameTable nameTable)
        /// <summary>
        /// Initializes a <see cref="XmlNamespaceManager"/> object for resolving prefixed XML namespaces within Atom syndication entities.
        /// </summary>
        /// <param name="nameTable">The table of atomized string objects.</param>
        /// <returns>A <see cref="XmlNamespaceManager"/> that resolves prefixed XML namespaces and provides scope management for these namespaces.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="nameTable"/> is a null reference (Nothing in Visual Basic).</exception>
        public static XmlNamespaceManager CreateNamespaceManager(XmlNameTable nameTable)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            XmlNamespaceManager manager = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(nameTable, "nameTable");

            //------------------------------------------------------------
            //	Initialize XML namespace resolver
            //------------------------------------------------------------
            manager = new XmlNamespaceManager(nameTable);
            manager.AddNamespace("atom", !String.IsNullOrEmpty(manager.DefaultNamespace) ? manager.DefaultNamespace : ATOM_NAMESPACE);
            manager.AddNamespace("app", ATOMPUB_NAMESPACE);
            manager.AddNamespace("xhtml", XHTML_NAMESPACE);

            return manager;
        }
        #endregion

        #region CompareCommonObjectAttributes(IAtomCommonObjectAttributes source, IAtomCommonObjectAttributes target)
        /// <summary>
        /// Compares objects that implement the <see cref="IAtomCommonObjectAttributes"/> interface.
        /// </summary>
        /// <param name="source">A object that implements the <see cref="IAtomCommonObjectAttributes"/> interface to be compared.</param>
        /// <param name="target">A object that implements the <see cref="IAtomCommonObjectAttributes"/> to compare with the <paramref name="source"/>.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared.</returns>
        public static int CompareCommonObjectAttributes(IAtomCommonObjectAttributes source, IAtomCommonObjectAttributes target)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            int result  = 0;

            //------------------------------------------------------------
            //	Handle parameter null reference cases
            //------------------------------------------------------------
            if (source == null && target == null)
            {
                return 0;
            }
            else if (source != null && target == null)
            {
                return 1;
            }
            else if (source == null && target != null)
            {
                return -1;
            }

            //------------------------------------------------------------
            //	Attempt to perform comparison
            //------------------------------------------------------------
            result  = result | Uri.Compare(source.BaseUri, target.BaseUri, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);

            string sourceLanguageName   = source.Language != null ? source.Language.Name : String.Empty;
            string targetLanguageName   = target.Language != null ? target.Language.Name : String.Empty;
            result                      = result | String.Compare(sourceLanguageName, targetLanguageName, StringComparison.OrdinalIgnoreCase);

            return result;
        }
        #endregion

        #region FillCommonObjectAttributes(IAtomCommonObjectAttributes target, XPathNavigator source)
        /// <summary>
        /// Modifies the <see cref="IAtomCommonObjectAttributes"/> to match the data source.
        /// </summary>
        /// <param name="target">The object that implements the <see cref="IAtomCommonObjectAttributes"/> interface to be filled.</param>
        /// <param name="source">The <see cref="XPathNavigator"/> to extract Atom common attribute information from.</param>
        /// <returns><b>true</b> if the <paramref name="target"/> was initialized using the supplied <paramref name="source"/>, otherwise <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="target"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        public static bool FillCommonObjectAttributes(IAtomCommonObjectAttributes target, XPathNavigator source)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool wasLoaded  = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(target, "target");
            Guard.ArgumentNotNull(source, "source");

            //------------------------------------------------------------
            //	Initialize XML namespace resolver
            //------------------------------------------------------------
            XmlNamespaceManager manager = AtomUtility.CreateNamespaceManager(source.NameTable);

            //------------------------------------------------------------
            //	Attempt to extract xml:base attribute information
            //------------------------------------------------------------
            string xmlBaseAttribute = source.GetAttribute("base", manager.LookupNamespace("xml"));
            if (!String.IsNullOrEmpty(xmlBaseAttribute))
            {
                Uri baseUri;
                if (Uri.TryCreate(xmlBaseAttribute, UriKind.RelativeOrAbsolute, out baseUri))
                {
                    target.BaseUri  = baseUri;
                    wasLoaded       = true;
                }
            }

            //------------------------------------------------------------
            //	Attempt to extract xml:lang attribute information
            //------------------------------------------------------------
            string xmlLangAttribute = source.GetAttribute("lang", manager.LookupNamespace("xml"));
            if (!String.IsNullOrEmpty(xmlLangAttribute))
            {
                try
                {
                    CultureInfo language    = new CultureInfo(source.XmlLang);
                    target.Language         = language;
                    wasLoaded               = true;
                }
                catch (ArgumentException)
                {
                    System.Diagnostics.Trace.TraceWarning("Unable to determine CultureInfo with a name of {0}.", source.XmlLang);
                }
            }

            return wasLoaded;
        }
        #endregion

        #region WriteCommonObjectAttributes(IAtomCommonObjectAttributes target, XmlWriter writer)
        /// <summary>
        /// Saves the current <see cref="IAtomCommonObjectAttributes"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="source">A object that implements the <see cref="IAtomCommonObjectAttributes"/> interface to extract Atom common attribute information from.</param>
        /// <param name="writer">The <see cref="XmlWriter"/> to which the <paramref name="source"/> information will be written.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public static void WriteCommonObjectAttributes(IAtomCommonObjectAttributes source, XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(writer, "writer");

            //------------------------------------------------------------
            //	Write xml:base attribute if necessary
            //------------------------------------------------------------
            if (source.BaseUri != null)
            {
                writer.WriteAttributeString("xml", "base", XML_NAMESPACE, source.BaseUri.ToString());
            }

            //------------------------------------------------------------
            //	Write xml:lang attribute if necessary
            //------------------------------------------------------------
            if (source.Language != null)
            {
                writer.WriteAttributeString("xml", "lang", XML_NAMESPACE, source.Language.Name);
            }
        }
        #endregion
    }
}
