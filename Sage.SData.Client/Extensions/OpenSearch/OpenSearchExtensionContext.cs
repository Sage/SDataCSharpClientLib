using System;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Encapsulates specific information about an individual <see cref="OpenSearchExtension"/>.
    /// </summary>
    [Serializable]
    public class OpenSearchExtensionContext
    {
        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================

        /// <summary>
        /// Items Per Page
        /// </summary>
        public int? ItemsPerPage { get; set; }

        /// <summary>
        /// Total Results
        /// </summary>
        public int? TotalResults { get; set; }

        /// <summary>
        /// Start Index
        /// </summary>
        public int? StartIndex { get; set; }

        //============================================================
        //	PUBLIC METHODS
        //============================================================

        #region Load(XPathNavigator source, XmlNamespaceManager manager)

        /// <summary>
        /// Initializes the syndication extension context using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <b>XPathNavigator</b> used to load this <see cref="OpenSearchExtensionContext"/>.</param>
        /// <param name="manager">The <see cref="XmlNamespaceManager"/> object used to resolve prefixed syndication extension elements and attributes.</param>
        /// <returns><b>true</b> if the <see cref="OpenSearchExtensionContext"/> was able to be initialized using the supplied <paramref name="source"/>; otherwise <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="manager"/> is a null reference (Nothing in Visual Basic).</exception>
        public bool Load(XPathNavigator source, XmlNamespaceManager manager)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            var wasLoaded = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(manager, "manager");

            //------------------------------------------------------------
            //	Attempt to extract syndication extension information
            //------------------------------------------------------------
            if (source.HasChildren)
            {
                var totalResultsNavigator = source.SelectSingleNode("opensearch:totalResults", manager);
                if (totalResultsNavigator != null && !string.IsNullOrEmpty(totalResultsNavigator.Value))
                {
                    TotalResults = Convert.ToInt32(totalResultsNavigator.Value);
                    wasLoaded = true;
                }

                var startIndexNavigator = source.SelectSingleNode("opensearch:startIndex", manager);
                if (startIndexNavigator != null && !string.IsNullOrEmpty(startIndexNavigator.Value))
                {
                    StartIndex = Convert.ToInt32(startIndexNavigator.Value);
                    wasLoaded = true;
                }

                var itemsPerPageNavigator = source.SelectSingleNode("opensearch:itemsPerPage", manager);
                if (itemsPerPageNavigator != null && !string.IsNullOrEmpty(itemsPerPageNavigator.Value))
                {
                    ItemsPerPage = Convert.ToInt32(itemsPerPageNavigator.Value);
                    wasLoaded = true;
                }
            }

            return wasLoaded;
        }

        #endregion

        #region WriteTo(XmlWriter writer, string xmlNamespace)

        /// <summary>
        /// Writes the current context to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to write the current context.</param>
        /// <param name="xmlNamespace">The XML namespace used to qualify prefixed syndication extension elements and attributes.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlNamespace"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlNamespace"/> is an empty string.</exception>
        public void WriteTo(XmlWriter writer, string xmlNamespace)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");
            Guard.ArgumentNotNullOrEmptyString(xmlNamespace, "xmlNamespace");

            //------------------------------------------------------------
            //	Write current extension details to the writer
            //------------------------------------------------------------

            if (ItemsPerPage != null)
            {
                writer.WriteElementString("itemsPerPage", xmlNamespace, ItemsPerPage.ToString());
            }

            if (TotalResults != null)
            {
                writer.WriteElementString("totalResults", xmlNamespace, TotalResults.ToString());
            }

            if (StartIndex != null)
            {
                writer.WriteElementString("startIndex", xmlNamespace, StartIndex.ToString());
            }
        }

        #endregion
    }
}