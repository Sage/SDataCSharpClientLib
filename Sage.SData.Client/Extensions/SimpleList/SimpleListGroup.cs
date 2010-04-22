/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
02/05/2008	brian.kuhn	Created SimpleListGroup Class
****************************************************************************/
using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Represents information that informs the client that the property to which it refers is one that is <i>groupable</i>, 
    /// meaning that the client should provide a user interface that allows the user to group or filter on the values of that property.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This informational entity makes reference to XML elements that are child-elements within the items of the same feed, using the supported extension mechanism of the feed format. 
    ///         Groupable properties <i>should</i> contain a small set of discrete values.
    ///     </para>
    ///     <para>
    ///         The value which is to be grouped <b>must be</b> the text content of the element itself (i.e. the character data contained in the element). 
    ///         Values of attributes or nested elements <b>cannot</b> be used for grouping. The property referred to must have no child-elements. 
    ///         In general, only one instance of a property should appear in each item. Clients are free to ignore repeated instances of properties.
    ///     </para>
    /// </remarks>
    /// <seealso cref="SimpleListSyndicationExtensionContext.Grouping"/>
    [Serializable()]
    public class SimpleListGroup : IComparable
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the full namespace used in the groupable property.
        /// </summary>
        private Uri groupNamespace;
        /// <summary>
        /// Private member to hold the name of the groupable property.
        /// </summary>
        private string groupElement = String.Empty;
        /// <summary>
        /// Private member to hold a human-readable name for the groupable property.
        /// </summary>
        private string groupLabel   = String.Empty;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SimpleListGroup()
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleListGroup"/> class.
        /// </summary>
        public SimpleListGroup()
        {
            //------------------------------------------------------------
            //	
            //------------------------------------------------------------
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Element
        /// <summary>
        /// Get or sets the name of this groupable property.
        /// </summary>
        /// <value>The name of this groupable property. The default value is <see cref="String.Empty"/>.</value>
        /// <remarks>
        ///     If this property is equal to <see cref="String.Empty"/>, it is assumed that the <see cref="Label"/> property is included 
        ///     and that this <see cref="SimpleListGroup"/> refers to the default sort order.
        /// </remarks>
        public string Element
        {
            get
            {
                return groupElement;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    groupElement = String.Empty;
                }
                else
                {
                    groupElement = value.Trim();
                }
            }
        }
        #endregion

        #region Label
        /// <summary>
        /// Get or sets a human-readable name for this groupable property.
        /// </summary>
        /// <value>A human-readable name for this groupable property. The default value is <see cref="String.Empty"/>.</value>
        /// <remarks>
        ///     <para>
        ///         If this property is <see cref="String.Empty"/>, the client should use the value of the <see cref="Element"/> property as the human-readable name.
        ///     </para>
        ///     <para>The <see cref="Label"/> property is <b>required</b> if the <see cref="Element"/> property is an <i>empty string</i>.</para>
        /// </remarks>
        public string Label
        {
            get
            {
                return groupLabel;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    groupLabel = String.Empty;
                }
                else
                {
                    groupLabel = value.Trim();
                }
            }
        }
        #endregion

        #region Namespace
        /// <summary>
        /// Gets or sets the full namespace identifier used to qualify this <see cref="Element"/>.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents the full namespace identifier used to qualify this <see cref="Element"/> property. The default value is <b>null</b>.</value>
        /// <remarks>
        ///     If the value of this property is <b>null</b>, it is assumed that the <see cref="Element"/> does not live in a namespace.
        /// </remarks>
        public Uri Namespace
        {
            get
            {
                return groupNamespace;
            }

            set
            {
                groupNamespace = value;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================
        #region Load(XPathNavigator source)
        /// <summary>
        /// Loads this <see cref="SimpleListGroup"/> using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns><b>true</b> if the <see cref="SimpleListGroup"/> was initialized using the supplied <paramref name="source"/>, otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     This method expects the supplied <paramref name="source"/> to be positioned on the XML element that represents a <see cref="SimpleListGroup"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        public bool Load(XPathNavigator source)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool wasLoaded              = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            //------------------------------------------------------------
            //	Attempt to extract syndication information
            //------------------------------------------------------------
            if (source.HasAttributes)
            {
                string namespaceAttribute   = source.GetAttribute("ns", String.Empty);
                string elementAttribute     = source.GetAttribute("element", String.Empty);
                string labelAttribute       = source.GetAttribute("label", String.Empty);

                if (!String.IsNullOrEmpty(namespaceAttribute))
                {
                    Uri elementNamespace;
                    if (Uri.TryCreate(namespaceAttribute, UriKind.RelativeOrAbsolute, out elementNamespace))
                    {
                        this.Namespace  = elementNamespace;
                        wasLoaded       = true;
                    }
                }

                if (!String.IsNullOrEmpty(elementAttribute))
                {
                    this.Element    = elementAttribute;
                    wasLoaded       = true;
                }

                if (!String.IsNullOrEmpty(labelAttribute))
                {
                    this.Label  = labelAttribute;
                    wasLoaded   = true;
                }
            }

            return wasLoaded;
        }
        #endregion

        #region WriteTo(XmlWriter writer)
        /// <summary>
        /// Saves the current <see cref="SimpleListGroup"/> to the specified <see cref="XmlWriter"/>.
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
            //	Create extension instance to retrieve XML namespace
            //------------------------------------------------------------
            SimpleListSyndicationExtension extension    = new SimpleListSyndicationExtension();

            //------------------------------------------------------------
            //	Write XML representation of the current instance
            //------------------------------------------------------------
            writer.WriteStartElement("group", extension.XmlNamespace);

            if (this.Namespace != null)
            {
                writer.WriteAttributeString("ns", this.Namespace.ToString());
            }

            if (!String.IsNullOrEmpty(this.Element))
            {
                writer.WriteAttributeString("element", this.Element);
            }

            if (!String.IsNullOrEmpty(this.Label))
            {
                writer.WriteAttributeString("label", this.Label);
            }

            writer.WriteEndElement();
        }
        #endregion

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="SimpleListGroup"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="SimpleListGroup"/>.</returns>
        /// <remarks>
        ///     This method returns the XML representation for the current instance.
        /// </remarks>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Build the string representation
            //------------------------------------------------------------
            using(MemoryStream stream = new MemoryStream())
            {
                XmlWriterSettings settings  = new XmlWriterSettings();
                settings.ConformanceLevel   = ConformanceLevel.Fragment;
                settings.Indent             = true;
                settings.OmitXmlDeclaration = true;

                using(XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    this.WriteTo(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (StreamReader reader = new StreamReader(stream))
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
            SimpleListGroup value  = obj as SimpleListGroup;

            if (value != null)
            {
                int result  = String.Compare(this.Element, value.Element, StringComparison.OrdinalIgnoreCase);
                result      = result | String.Compare(this.Label, value.Label, StringComparison.OrdinalIgnoreCase);
                result      = result | Uri.Compare(this.Namespace, value.Namespace, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.Ordinal);

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
            if (!(obj is SimpleListGroup))
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
        public static bool operator ==(SimpleListGroup first, SimpleListGroup second)
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
        public static bool operator !=(SimpleListGroup first, SimpleListGroup second)
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
        public static bool operator <(SimpleListGroup first, SimpleListGroup second)
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
        public static bool operator >(SimpleListGroup first, SimpleListGroup second)
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
