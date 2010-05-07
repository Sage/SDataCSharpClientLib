/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
02/05/2008	brian.kuhn	Created SimpleListSort Class
****************************************************************************/
using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Represents information that enables the publisher to indicate to the client that the property to which it refers is one that is <i>sortable</i>, 
    /// meaning that the client should provide a user interface that allows the user to sort on that property.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This informational entity makes reference to XML elements that are child-elements within the items of the same feed, using the supported extension mechanism of the feed format. 
    ///         This entity can also be used to provide a label for the default sort that appears in the list.
    ///     </para>
    ///     <para>
    ///         The value which is to be sorted <b>must be</b> the text content of the element itself (i.e. the character data contained in the element). 
    ///         Values of attributes or nested elements <b>cannot</b> be used for sorting. The property referred to must have no child-elements. 
    ///         In general, only one instance of a property should appear in each item. Clients are free to ignore repeated instances of properties.
    ///     </para>
    /// </remarks>
    /// <seealso cref="SimpleListSyndicationExtensionContext.Sorting"/>
    [Serializable()]
    public class SimpleListSort : IComparable
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the full namespace used in the sortable property.
        /// </summary>
        private Uri sortNamespace;
        /// <summary>
        /// Private member to hold the name of the sortable property.
        /// </summary>
        private string sortElement              = String.Empty;
        /// <summary>
        /// Private member to hold a human-readable name for the sortable property.
        /// </summary>
        private string sortLabel                = String.Empty;
        /// <summary>
        /// Private member to hold the data-type of the sortable property.
        /// </summary>
        private SimpleListDataType sortDataType = SimpleListDataType.None;
        /// <summary>
        /// Private member to hold a value indicating if the sortable property is the default sort order in the list.
        /// </summary>
        private bool sortIsDefault;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SimpleListSort()
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleListSort"/> class.
        /// </summary>
        public SimpleListSort()
        {
            //------------------------------------------------------------
            //	
            //------------------------------------------------------------
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region DataType
        /// <summary>
        /// Gets or sets the data-type of this sortable property.
        /// </summary>
        /// <value>A <see cref="SimpleListDataType"/> enumeration value that represents the data-type of this sortable property. The default value is <see cref="SimpleListDataType.None"/>.</value>
        /// <remarks>
        ///     If the value of this property is <see cref="SimpleListDataType.None"/>, it <i>should</i> be assumed default data-type of this sortable property is <see cref="SimpleListDataType.Text"/>.
        /// </remarks>
        /// <seealso cref="DataTypeAsString(SimpleListDataType)"/>
        /// <seealso cref="DataTypeByName(string)"/>
        public SimpleListDataType DataType
        {
            get
            {
                return sortDataType;
            }

            set
            {
                sortDataType = value;
            }
        }
        #endregion

        #region Element
        /// <summary>
        /// Get or sets the name of this sortable property.
        /// </summary>
        /// <value>The name of this sortable property. The default value is <see cref="String.Empty"/>.</value>
        /// <remarks>
        ///     If this property is equal to <see cref="String.Empty"/>, it is assumed that the <see cref="Label"/> property is included 
        ///     and that this <see cref="SimpleListSort"/> refers to the default sort order.
        /// </remarks>
        public string Element
        {
            get
            {
                return sortElement;
            }

            set
            {
                if(String.IsNullOrEmpty(value))
                {
                    sortElement = String.Empty;
                }
                else
                {
                    sortElement = value.Trim();
                }
            }
        }
        #endregion

        #region IsDefault
        /// <summary>
        /// Gets or sets a value indicating if this sortable property is the default sort order in the list.
        /// </summary>
        /// <value><b>true</b> if this sortable property is the default sort order in the list; otherwise <b>false</b>. The default value is <b>false</b>.</value>
        /// <remarks>
        ///     The items in the list <b>must</b> be already be sorted by the element, meaning the client <b>should not</b> expect to have to resort by this field if it displaying content directly from the list. 
        ///     The client <i>should</i> respect only the first <see cref="SimpleListSort"/> that has a <see cref="IsDefault"/> property with a value of <b>true</b> that it encounters.
        /// </remarks>
        public bool IsDefault
        {
            get
            {
                return sortIsDefault;
            }

            set
            {
                sortIsDefault = value;
            }
        }
        #endregion

        #region Label
        /// <summary>
        /// Get or sets a human-readable name for this sortable property.
        /// </summary>
        /// <value>A human-readable name for this sortable property. The default value is <see cref="String.Empty"/>.</value>
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
                return sortLabel;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    sortLabel = String.Empty;
                }
                else
                {
                    sortLabel = value.Trim();
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
                return sortNamespace;
            }

            set
            {
                sortNamespace = value;
            }
        }
        #endregion

        //============================================================
        //	STATIC METHODS
        //============================================================
        #region DataTypeAsString(SimpleListDataType type)
        /// <summary>
        /// Returns the data type identifier for the supplied <see cref="SimpleListDataType"/>.
        /// </summary>
        /// <param name="type">The <see cref="SimpleListDataType"/> to get the data type identifier for.</param>
        /// <returns>The data type identifier for the supplied <paramref name="type"/>, otherwise returns an empty string.</returns>
        public static string DataTypeAsString(SimpleListDataType type)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string name = String.Empty;

            //------------------------------------------------------------
            //	Return alternate value based on supplied protocol
            //------------------------------------------------------------
            foreach (System.Reflection.FieldInfo fieldInfo in typeof(SimpleListDataType).GetFields())
            {
                if (fieldInfo.FieldType == typeof(SimpleListDataType))
                {
                    SimpleListDataType dataType = (SimpleListDataType)Enum.Parse(fieldInfo.FieldType, fieldInfo.Name);

                    if (dataType == type)
                    {
                        object[] customAttributes   = fieldInfo.GetCustomAttributes(typeof(EnumerationMetadataAttribute), false);

                        if (customAttributes != null && customAttributes.Length > 0)
                        {
                            EnumerationMetadataAttribute enumerationMetadata = customAttributes[0] as EnumerationMetadataAttribute;

                            name    = enumerationMetadata.AlternateValue;
                            break;
                        }
                    }
                }
            }

            return name;
        }
        #endregion

        #region DataTypeByName(string name)
        /// <summary>
        /// Returns the <see cref="SimpleListDataType"/> enumeration value that corresponds to the specified data type name.
        /// </summary>
        /// <param name="name">The name of the data type.</param>
        /// <returns>A <see cref="SimpleListDataType"/> enumeration value that corresponds to the specified string, otherwise returns <b>SimpleListDataType.None</b>.</returns>
        /// <remarks>This method disregards case of specified data type name.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="name"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="name"/> is an empty string.</exception>
        public static SimpleListDataType DataTypeByName(string name)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            SimpleListDataType dataType = SimpleListDataType.None;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNullOrEmptyString(name, "name");

            //------------------------------------------------------------
            //	Determine syndication content format based on supplied name
            //------------------------------------------------------------
            foreach (System.Reflection.FieldInfo fieldInfo in typeof(SimpleListDataType).GetFields())
            {
                if (fieldInfo.FieldType == typeof(SimpleListDataType))
                {
                    SimpleListDataType type     = (SimpleListDataType)Enum.Parse(fieldInfo.FieldType, fieldInfo.Name);
                    object[] customAttributes   = fieldInfo.GetCustomAttributes(typeof(EnumerationMetadataAttribute), false);

                    if (customAttributes != null && customAttributes.Length > 0)
                    {
                        EnumerationMetadataAttribute enumerationMetadata = customAttributes[0] as EnumerationMetadataAttribute;

                        if (String.Compare(name, enumerationMetadata.AlternateValue, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            dataType = type;
                            break;
                        }
                    }
                }
            }

            return dataType;
        }
        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================
        #region Load(XPathNavigator source)
        /// <summary>
        /// Loads this <see cref="SimpleListSort"/> using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns><b>true</b> if the <see cref="SimpleListSort"/> was initialized using the supplied <paramref name="source"/>, otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     This method expects the supplied <paramref name="source"/> to be positioned on the XML element that represents a <see cref="SimpleListSort"/>.
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
                string dataTypeAttribute    = source.GetAttribute("data-type", String.Empty);
                string defaultAttribute     = source.GetAttribute("default", String.Empty);

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

                if (!String.IsNullOrEmpty(dataTypeAttribute))
                {
                    SimpleListDataType dataType = SimpleListSort.DataTypeByName(dataTypeAttribute);
                    if (dataType != SimpleListDataType.None)
                    {
                        this.DataType   = dataType;
                        wasLoaded       = true;
                    }
                }

                if (!String.IsNullOrEmpty(defaultAttribute))
                {
                    if (String.Compare(defaultAttribute, "true", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        this.IsDefault  = true;
                        wasLoaded       = true;
                    }
                    else if (String.Compare(defaultAttribute, "false", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        this.IsDefault  = false;
                        wasLoaded       = true;
                    }
                }
            }

            return wasLoaded;
        }
        #endregion

        #region WriteTo(XmlWriter writer)
        /// <summary>
        /// Saves the current <see cref="SimpleListSort"/> to the specified <see cref="XmlWriter"/>.
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
            writer.WriteStartElement("sort", extension.XmlNamespace);

            if(this.Namespace != null)
            {
                writer.WriteAttributeString("ns", this.Namespace.ToString());
            }

            if(!String.IsNullOrEmpty(this.Element))
            {
                writer.WriteAttributeString("element", this.Element);
            }

            if (!String.IsNullOrEmpty(this.Label))
            {
                writer.WriteAttributeString("label", this.Label);
            }

            if (this.DataType != SimpleListDataType.None)
            {
                writer.WriteAttributeString("data-type", SimpleListSort.DataTypeAsString(this.DataType));
            }

            if(this.IsDefault)
            {
                writer.WriteAttributeString("default", "true");
            }

            writer.WriteEndElement();
        }
        #endregion

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="SimpleListSort"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="SimpleListSort"/>.</returns>
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
            SimpleListSort value  = obj as SimpleListSort;

            if (value != null)
            {
                int result  = this.DataType.CompareTo(value.DataType);
                result      = result | String.Compare(this.Element, value.Element, StringComparison.OrdinalIgnoreCase);
                result      = result | this.IsDefault.CompareTo(value.IsDefault);
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
            if (!(obj is SimpleListSort))
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
        public static bool operator ==(SimpleListSort first, SimpleListSort second)
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
        public static bool operator !=(SimpleListSort first, SimpleListSort second)
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
        public static bool operator <(SimpleListSort first, SimpleListSort second)
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
        public static bool operator >(SimpleListSort first, SimpleListSort second)
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
