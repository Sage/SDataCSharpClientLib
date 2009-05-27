using System;
using System.Xml.XPath;

using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Provides data for the <see cref="ISyndicationExtension.Loaded"/> event.
    /// </summary>
    /// <remarks>
    ///     A <see cref="ISyndicationExtension.Loaded"/> event occurs whenever the <see cref="ISyndicationExtension.Load(System.Xml.XmlReader)"/> 
    ///     or <see cref="ISyndicationExtension.Load(System.Xml.XPath.IXPathNavigable)"/> methods are called.
    /// </remarks>
    /// <seealso cref="ISyndicationExtension"/>
    /// <seealso cref="ISyndicationExtension.Load(System.Xml.XPath.IXPathNavigable)"/>
    /// <seealso cref="ISyndicationExtension.Load(System.Xml.XmlReader)"/>
    [Serializable()]
    public class SyndicationExtensionLoadedEventArgs : EventArgs, IComparable
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold instance of event with no event data.
        /// </summary>
        private static readonly SyndicationExtensionLoadedEventArgs emptyEventArguments  = new SyndicationExtensionLoadedEventArgs();
        /// <summary>
        /// Private member to hold read-only XPathNavigator object for navigating the XML data used to load the syndication extension.
        /// </summary>
        [NonSerialized()]
        private XPathNavigator eventNavigator;
        /// <summary>
        /// Private member to hold the syndication extension that resulted from the load operation.
        /// </summary>
        private ISyndicationExtension eventExtension;
        #endregion
        
        //============================================================
		//	CONSTRUCTORS
        //============================================================
        #region SyndicationExtensionLoadedEventArgs()
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationExtensionLoadedEventArgs"/> class.
        /// </summary>
        public SyndicationExtensionLoadedEventArgs()
		{
			//------------------------------------------------------------
			//	
			//------------------------------------------------------------
		}
		#endregion

        #region SyndicationExtensionLoadedEventArgs(IXPathNavigable navigator)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationExtensionLoadedEventArgs"/> class using the supplied <see cref="IXPathNavigable"/>.
        /// </summary>
        /// <param name="data">A <see cref="IXPathNavigable"/> object that represents the XML data that was used to load the syndication extension.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="data"/> is a null reference (Nothing in Visual Basic).</exception>
        public SyndicationExtensionLoadedEventArgs(IXPathNavigable data) : this()
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(data, "data");

            eventNavigator  = data.CreateNavigator();
        }
        #endregion

        #region SyndicationExtensionLoadedEventArgs(IXPathNavigable data, ISyndicationExtension extension)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationExtensionLoadedEventArgs"/> class using the supplied <see cref="IXPathNavigable"/> and <see cref="ISyndicationExtension"/>.
        /// </summary>
        /// <param name="data">A <see cref="IXPathNavigable"/> object that represents the XML data that was used to load the syndication extension.</param>
        /// <param name="extension">
        ///     A <see cref="ISyndicationExtension"/> that represents the syndication extension after the load operation completed.
        /// </param>
        /// <exception cref="ArgumentNullException">The <paramref name="data"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="extension"/> is a null reference (Nothing in Visual Basic).</exception>
        public SyndicationExtensionLoadedEventArgs(IXPathNavigable data, ISyndicationExtension extension) : this(data)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(extension, "extension");

            eventExtension  = extension;
        }
        #endregion

        //============================================================
        //	STATIC PROPERTIES
        //============================================================
        #region Empty
        /// <summary>
        /// Represents an syndication extension loaded event with no event data.
        /// </summary>
        /// <value>An uninitialized instance of the <see cref="SyndicationExtensionLoadedEventArgs"/> class.</value>
        /// <remarks>The value of Empty is a read-only instance of <see cref="SyndicationExtensionLoadedEventArgs"/> equivalent to the result of calling the <see cref="SyndicationExtensionLoadedEventArgs()"/> constructor.</remarks>
        public static new SyndicationExtensionLoadedEventArgs Empty
        {
            get
            {
                return emptyEventArguments;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Data
        /// <summary>
        /// Gets a read-only <see cref="XPathNavigator"/> object for navigating the XML data that was used to load the syndication extension.
        /// </summary>
        /// <value>
        ///     A read-only <see cref="XPathNavigator"/> object for navigating the XML data that was used to load the syndication extension.
        /// </value>
        public XPathNavigator Data
        {
            get
            {
                return eventNavigator;
            }
        }
        #endregion

        #region Extension
        /// <summary>
        /// Gets the <see cref="ISyndicationExtension"/> that resulted from the load operation.
        /// </summary>
        /// <value>
        ///     The <see cref="ISyndicationExtension"/> that resulted from the load operation. 
        /// </value>
        public ISyndicationExtension Extension
        {
            get
            {
                return eventExtension;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="SyndicationExtensionLoadedEventArgs"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="SyndicationExtensionLoadedEventArgs"/>.</returns>
        /// <remarks>
        ///     This method returns a human-readable string for the current instance. Hash code values are displayed for applicable properties.
        /// </remarks>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Build the string representation
            //------------------------------------------------------------
            string name         = this.Extension != null ? this.Extension.Name : String.Empty;
            string prefix       = this.Extension != null ? this.Extension.XmlPrefix : String.Empty;
            string xmlNamespace = this.Extension != null ? this.Extension.XmlNamespace : String.Empty;
            string extension    = this.Extension != null ? this.Extension.GetHashCode().ToString(System.Globalization.NumberFormatInfo.InvariantInfo) : String.Empty;
            string data         = this.Data != null ? this.Data.GetHashCode().ToString(System.Globalization.NumberFormatInfo.InvariantInfo) : String.Empty;

            return String.Format(null, "[SyndicationExtensionLoadedEventArgs(Name = \"{0}\", Prefix = \"{1}\", Namespace = \"{2}\", Extension = \"{3}\", Data = \"{4}\")]", name, prefix, xmlNamespace, extension, data);
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
            SyndicationExtensionLoadedEventArgs value  = obj as SyndicationExtensionLoadedEventArgs;

            if (value != null)
            {
                int result  = 0;

                if (this.Data != null)
                {
                    if(value.Data != null)
                    {
                        result  = result | String.Compare(this.Data.OuterXml, value.Data.OuterXml, StringComparison.Ordinal);
                    }
                    else
                    {
                        result  = result | 1;
                    }
                }
                else if (value.Data != null)
                {
                    result  = result | -1;
                }

                if (this.Extension != null)
                {
                    if (value.Extension != null)
                    {
                        result  = result | String.Compare(this.Extension.ToString(), value.Extension.ToString(), StringComparison.Ordinal);
                    }
                    else
                    {
                        result  = result | 1;
                    }
                }
                else if (value.Extension != null)
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
            if (!(obj is SyndicationExtensionLoadedEventArgs))
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
        public static bool operator ==(SyndicationExtensionLoadedEventArgs first, SyndicationExtensionLoadedEventArgs second)
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
        public static bool operator !=(SyndicationExtensionLoadedEventArgs first, SyndicationExtensionLoadedEventArgs second)
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
        public static bool operator <(SyndicationExtensionLoadedEventArgs first, SyndicationExtensionLoadedEventArgs second)
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
        public static bool operator >(SyndicationExtensionLoadedEventArgs first, SyndicationExtensionLoadedEventArgs second)
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
