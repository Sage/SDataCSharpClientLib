/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
11/27/2007	brian.kuhn	Created TrackbackDiscoveryMetadata Class
****************************************************************************/
using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Sage.SData.Client.Common
{
    /// <summary>
    /// Represents metadata about a web log entry that allows clients to auto-discover the TrackBack ping URL for that entry.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trackback")]
    [Serializable()]
    public class TrackbackDiscoveryMetadata : IComparable
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the XML namespace for Resource Description Framework (RDF) entities.
        /// </summary>
        private const string RDF_NAMESPACE          = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        /// <summary>
        /// Private member to hold the XML namespace for Dublin Core entities.
        /// </summary>
        private const string DUBLIN_CORE_NAMESPACE  = "http://purl.org/dc/elements/1.1/";
        /// <summary>
        /// Private member to hold the XML namespace for Trackback entities.
        /// </summary>
        private const string TRACKBACK_NAMESPACE    = "http://madskills.com/public/xml/rss/module/trackback/";
        /// <summary>
        /// Private member to hold the title of the discoverable web log entry.
        /// </summary>
        private string trackbackDiscoveryTitle  = String.Empty;
        /// <summary>
        /// Private member to hold Resource Description Framework entity reference.
        /// </summary>
        private Uri trackbackDiscoveryAbout;
        /// <summary>
        /// Private member to hold the unique identifier of the discoverable web log entry.
        /// </summary>
        private Uri trackbackDiscoveryIdentifier;
        /// <summary>
        /// Private member to hold Trackback ping endpoint of the discoverable web log entry.
        /// </summary>
        private Uri trackbackDiscoveryPingEndpoint;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region TrackbackDiscoveryMetadata()
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackbackDiscoveryMetadata"/> class.
        /// </summary>
        public TrackbackDiscoveryMetadata()
        {
            //------------------------------------------------------------
            //	
            //------------------------------------------------------------
        }
        #endregion

        #region TrackbackDiscoveryMetadata(XPathNavigator navigator)
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackbackDiscoveryMetadata"/> class using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract the Trackback auto-discovery meta-data from.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        public TrackbackDiscoveryMetadata(XPathNavigator navigator) : this()
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(navigator, "navigator");

            //------------------------------------------------------------
            //	Extract auto-discovery meta-data from navigator
            //------------------------------------------------------------
            this.Load(navigator);
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region About
        /// <summary>
        /// Gets or sets the Resource Description Framework (RDF) entity reference.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents the Resource Description Framework (RDF) entity reference.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri About
        {
            get
            {
                return trackbackDiscoveryAbout;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                trackbackDiscoveryAbout = value;
            }
        }
        #endregion

        #region Identifier
        /// <summary>
        /// Gets or sets the unique identifier for the discoverable web log entry.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents the unique identifier for the discoverable web log entry.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri Identifier
        {
            get
            {
                return trackbackDiscoveryIdentifier;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                trackbackDiscoveryIdentifier = value;
            }
        }
        #endregion

        #region PingUrl
        /// <summary>
        /// Gets or sets the Trackback ping notification endpoint for the discoverable web log entry.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents the Trackback ping URL for the discoverable web log entry.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri PingUrl
        {
            get
            {
                return trackbackDiscoveryPingEndpoint;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                trackbackDiscoveryPingEndpoint = value;
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets or sets the title of the discoverable web log entry.
        /// </summary>
        /// <value>The title of the discoverable web log entry.</value>
        public string Title
        {
            get
            {
                return trackbackDiscoveryTitle;
            }

            set
            {
                if(String.IsNullOrEmpty(value))
                {
                    trackbackDiscoveryTitle = String.Empty;
                }
                else
                {
                    trackbackDiscoveryTitle = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================
        #region Load(XPathNavigator navigator)
        /// <summary>
        /// Loads this <see cref="TrackbackDiscoveryMetadata"/> using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="navigator">The <see cref="XPathNavigator"/> to extract the Trackback auto-discovery meta-data from.</param>
        /// <returns><b>true</b> if Trackback auto-discovery meta-data was extracted from the <paramref name="navigator"/>, otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     Will return <b>false</b> if the <i>trackback:ping</i> attribute is not found on the <b>rdf:Description</b> element.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        public bool Load(XPathNavigator navigator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool wasLoaded              = false;
            XmlNamespaceManager manager = null;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(navigator, "navigator");

            //------------------------------------------------------------
            //	Initialize XML namespace manager with expected namespaces
            //------------------------------------------------------------
            manager = new XmlNamespaceManager(navigator.NameTable);
            manager.AddNamespace("rdf", RDF_NAMESPACE);
            manager.AddNamespace("dc", DUBLIN_CORE_NAMESPACE);
            manager.AddNamespace("trackback", TRACKBACK_NAMESPACE);

            //------------------------------------------------------------
            //	Attempt to extract Trackback meta-data information
            //------------------------------------------------------------
            XPathNavigator descriptionNavigator = navigator.SelectSingleNode("rdf:RDF\rdf:Description", manager);

            if (descriptionNavigator != null && descriptionNavigator.HasAttributes)
            {
                string aboutAttribute       = descriptionNavigator.GetAttribute("about", RDF_NAMESPACE);
                string identifierAttribute  = descriptionNavigator.GetAttribute("identifier", DUBLIN_CORE_NAMESPACE);
                string titleAttribute       = descriptionNavigator.GetAttribute("title", DUBLIN_CORE_NAMESPACE);
                string pingAttribute        = descriptionNavigator.GetAttribute("ping", TRACKBACK_NAMESPACE);

                if (String.IsNullOrEmpty(pingAttribute))
                {
                    //------------------------------------------------------------
                    //	rdf:Description elment does not have a trackback:ping 
                    //  attribute, so return false.
                    //------------------------------------------------------------
                    return false;
                }

                if (!String.IsNullOrEmpty(aboutAttribute))
                {
                    Uri about;
                    if (Uri.TryCreate(aboutAttribute, UriKind.RelativeOrAbsolute, out about))
                    {
                        this.About  = about;
                        wasLoaded   = true;
                    }
                }

                if (!String.IsNullOrEmpty(identifierAttribute))
                {
                    Uri identifier;
                    if (Uri.TryCreate(identifierAttribute, UriKind.RelativeOrAbsolute, out identifier))
                    {
                        this.Identifier = identifier;
                        wasLoaded       = true;
                    }
                }

                if (!String.IsNullOrEmpty(titleAttribute))
                {
                    this.Title  = titleAttribute;
                    wasLoaded   = true;
                }

                if (!String.IsNullOrEmpty(pingAttribute))
                {
                    Uri ping;
                    if (Uri.TryCreate(pingAttribute, UriKind.RelativeOrAbsolute, out ping))
                    {
                        this.PingUrl    = ping;
                        wasLoaded       = true;
                    }
                }
            }

            return wasLoaded;
        }
        #endregion

        #region WriteTo(XmlWriter writer)
        /// <summary>
        /// Saves the current <see cref="TrackbackDiscoveryMetadata"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to which you want to save.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public void WriteTo(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");

            //------------------------------------------------------------
            //	Write embedded RDF that rpresents this discovery meta-data
            //------------------------------------------------------------
            writer.WriteStartElement("rdf", "RDF", RDF_NAMESPACE);
            writer.WriteAttributeString("xmlns", "dc", null, DUBLIN_CORE_NAMESPACE);
            writer.WriteAttributeString("xmlns", "trackback", null, TRACKBACK_NAMESPACE);

            writer.WriteStartElement("rdf", "Description", RDF_NAMESPACE);
            writer.WriteAttributeString("rdf", "about", RDF_NAMESPACE, this.About != null ? this.About.ToString() : String.Empty);
            writer.WriteAttributeString("dc", "identifier", DUBLIN_CORE_NAMESPACE, this.Identifier != null ? this.Identifier.ToString() : String.Empty);
            writer.WriteAttributeString("dc", "title", DUBLIN_CORE_NAMESPACE, !String.IsNullOrEmpty(this.Title) ? this.Title : String.Empty);
            writer.WriteAttributeString("trackback", "ping", TRACKBACK_NAMESPACE, this.PingUrl != null ? this.PingUrl.ToString() : String.Empty);
            writer.WriteEndElement();

            writer.WriteFullEndElement();
        }
        #endregion

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="TrackbackDiscoveryMetadata"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="TrackbackDiscoveryMetadata"/>.</returns>
        /// <remarks>
        ///     This method returns the XML representation for the current instance.
        /// </remarks>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Build the string representation
            //------------------------------------------------------------
            using (MemoryStream stream = new MemoryStream())
            {
                XmlWriterSettings settings  = new XmlWriterSettings();
                settings.Indent             = true;
                settings.OmitXmlDeclaration = true;
                settings.ConformanceLevel   = ConformanceLevel.Fragment;

                using(XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    this.WriteTo(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using(StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
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
            TrackbackDiscoveryMetadata value  = obj as TrackbackDiscoveryMetadata;

            if (value != null)
            {
                int result  = Uri.Compare(this.About, value.About, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);
                result      = result | Uri.Compare(this.Identifier, value.Identifier, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);
                result      = result | Uri.Compare(this.PingUrl, value.PingUrl, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);
                result      = result | String.Compare(this.Title, value.Title, StringComparison.OrdinalIgnoreCase);

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
            if (!(obj is TrackbackDiscoveryMetadata))
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
        public static bool operator ==(TrackbackDiscoveryMetadata first, TrackbackDiscoveryMetadata second)
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
        public static bool operator !=(TrackbackDiscoveryMetadata first, TrackbackDiscoveryMetadata second)
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
        public static bool operator <(TrackbackDiscoveryMetadata first, TrackbackDiscoveryMetadata second)
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
        public static bool operator >(TrackbackDiscoveryMetadata first, TrackbackDiscoveryMetadata second)
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
