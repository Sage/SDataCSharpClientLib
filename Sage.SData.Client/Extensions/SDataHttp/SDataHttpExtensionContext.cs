using System;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Encapsulates specific information about an individual <see cref="SDataHttpExtension"/>.
    /// </summary>
    [Serializable]
    public class SDataHttpExtensionContext
    {
        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================

        /// <summary>
        /// Http Method
        /// </summary>
        public SDataHttpMethod? HttpMethod { get; set; }

        /// <summary>
        /// Http Status
        /// </summary>
        public int? HttpStatus { get; set; }

        /// <summary>
        /// Http Message
        /// </summary>
        public string HttpMessage { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public Uri Location { get; set; }

        /// <summary>
        /// ETag
        /// </summary>
        public string ETag { get; set; }

        /// <summary>
        /// If Match
        /// </summary>
        public string IfMatch { get; set; }

        //============================================================
        //	PUBLIC METHODS
        //============================================================

        #region Load(XPathNavigator source, XmlNamespaceManager manager)

        /// <summary>
        /// Initializes the syndication extension context using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <b>XPathNavigator</b> used to load this <see cref="SDataExtensionContext"/>.</param>
        /// <param name="manager">The <see cref="XmlNamespaceManager"/> object used to resolve prefixed syndication extension elements and attributes.</param>
        /// <returns><b>true</b> if the <see cref="SDataExtensionContext"/> was able to be initialized using the supplied <paramref name="source"/>; otherwise <b>false</b>.</returns>
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
                var httpMethodNavigator = source.SelectSingleNode("http:httpMethod", manager);
                if (httpMethodNavigator != null && !string.IsNullOrEmpty(httpMethodNavigator.Value))
                {
                    HttpMethod = (SDataHttpMethod) Enum.Parse(typeof (SDataHttpMethod), httpMethodNavigator.Value, true);
                    wasLoaded = true;
                }

                var httpStatusNavigator = source.SelectSingleNode("http:httpStatus", manager);
                if (httpStatusNavigator != null && !string.IsNullOrEmpty(httpStatusNavigator.Value))
                {
                    HttpStatus = (int) (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), httpStatusNavigator.Value, true);
                    wasLoaded = true;
                }

                var httpMessageNavigator = source.SelectSingleNode("http:httpMessage", manager);
                if (httpMessageNavigator != null)
                {
                    HttpMessage = httpMessageNavigator.Value;
                    wasLoaded = true;
                }

                var locationNavigator = source.SelectSingleNode("http:location", manager);
                if (locationNavigator != null && !string.IsNullOrEmpty(locationNavigator.Value))
                {
                    Location = new Uri(locationNavigator.Value);
                    wasLoaded = true;
                }

                var eTagNavigator = source.SelectSingleNode("http:etag", manager);
                if (eTagNavigator != null)
                {
                    ETag = eTagNavigator.Value;
                    wasLoaded = true;
                }

                var ifMatchNavigator = source.SelectSingleNode("http:ifMatch", manager);
                if (ifMatchNavigator != null)
                {
                    IfMatch = ifMatchNavigator.Value;
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

            if (HttpMethod != null)
            {
                writer.WriteElementString("httpMethod", xmlNamespace, HttpMethod.ToString().ToUpper());
            }

            if (HttpStatus != null)
            {
                writer.WriteElementString("httpStatus", xmlNamespace, HttpStatus.ToString());
            }

            if (!string.IsNullOrEmpty(HttpMessage))
            {
                writer.WriteElementString("httpMessage", xmlNamespace, HttpMessage);
            }

            if (Location != null)
            {
                writer.WriteElementString("location", xmlNamespace, Location.ToString());
            }

            if (!string.IsNullOrEmpty(ETag))
            {
                writer.WriteElementString("etag", xmlNamespace, ETag);
            }

            if (!string.IsNullOrEmpty(IfMatch))
            {
                writer.WriteElementString("ifMatch", xmlNamespace, IfMatch);
            }
        }

        #endregion
    }

    /// <summary>
    /// Enum of HTTP Methods
    /// </summary>
    public enum SDataHttpMethod
    {
        /// <summary>
        /// Get Request
        /// </summary>
        Get,
        /// <summary>
        /// Post Request
        /// </summary>
        Post,
        /// <summary>
        /// Put Request
        /// </summary>
        Put,
        /// <summary>
        /// Delete Requets
        /// </summary>
        Delete
    }
}