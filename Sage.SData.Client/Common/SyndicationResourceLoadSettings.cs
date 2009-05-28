using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.XPath;

namespace Sage.SData.Client.Common
{
    /// <summary>
    /// Specifies a set of features to support on a <see cref="ISyndicationResource"/> object loaded by the <see cref="ISyndicationResource.Load(IXPathNavigable, SyndicationResourceLoadSettings)"/> method.
    /// </summary>
    [Serializable()]
    public sealed class SyndicationResourceLoadSettings : IComparable
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the character encoding to use when reading the syndication resource.
        /// </summary>
        private Encoding characterEncoding                      = Encoding.UTF8;
        /// <summary>
        /// Private member to hold a value indicating the maximum number of resource entities to retrieve from a syndication resource.
        /// </summary>
        private int maximumEntitiesToRetrieve;
        /// <summary>
        /// Private member to hold a value that specifies the amount of time after which a asynchronous load operation call times out.
        /// </summary>
        private TimeSpan requestTimeout                         = TimeSpan.FromSeconds(120);
        /// <summary>
        /// Private member to hold a collection of types that represent the syndication extensions supported by the load operation.
        /// </summary>
        private Collection<Type> supportedSyndicationExtensions;
        /// <summary>
        /// Private member to hold a value indicating if auto-detection of supported syndication extensions is enabled.
        /// </summary>
        private bool syndicationExtensionAutodetectionEnabled   = true;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SyndicationResourceLoadSettings()
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationResourceLoadSettings"/> class.
        /// </summary>
        public SyndicationResourceLoadSettings()
        {
            //------------------------------------------------------------
            //	
            //------------------------------------------------------------
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region AutoDetectExtensions
        /// <summary>
        /// Gets or sets a value indicating if auto-detection of supported syndication extensions is enabled.
        /// </summary>
        /// <value>
        ///     <b>true</b> if the syndication extensions supported by the load operation are automatically determined based on the XML namespaces declared on a syndication resource; otherwise <b>false</b>. 
        ///     The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        ///     Automatic detection of supported syndication extensions will <b>not</b> remove any syndication extensions already added 
        ///     to the <see cref="SupportedExtensions"/> collection prior to the load operation execution.
        /// </remarks>
        public bool AutoDetectExtensions
        {
            get
            {
                return syndicationExtensionAutodetectionEnabled;
            }

            set
            {
                syndicationExtensionAutodetectionEnabled = value;
            }
        }
        #endregion

        #region CharacterEncoding
        /// <summary>
        /// Gets or sets the character encoding to use when parsing a syndication resource.
        /// </summary>
        /// <value>A <see cref="Encoding"/> object that indicates the character encoding to use when parsing a syndication resource. The default value is <see cref="Encoding.UTF8"/>.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Encoding CharacterEncoding
        {
            get
            {
                return characterEncoding;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                characterEncoding = value;
            }
        }
        #endregion

        #region RetrievalLimit
        /// <summary>
        /// Gets or sets the maximum number of resource entities to retrieve from a syndication resource.
        /// </summary>
        /// <value>The maximum number of entities to retrieve from a syndication resource. The default value is 0, which indicates there is <b>no limit</b>.</value>
        /// <remarks>
        ///     This setting is typically used to optimize processing by reducing the number of resource entities that must be parsed. 
        ///     Some syndication resources may not utilize this setting if they do not represent a list of retrievable entities.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is less than zero.</exception>
        public int RetrievalLimit
        {
            get
            {
                return maximumEntitiesToRetrieve;
            }

            set
            {
                Guard.ArgumentNotLessThan(value, "value", 0);
                maximumEntitiesToRetrieve = value;
            }
        }
        #endregion

        #region SupportedExtensions
        /// <summary>
        /// Gets the syndication extensions to attempt to load from a syndication resource.
        /// </summary>
        /// <value>
        ///     A <see cref="Collection{T}"/> collection of <see cref="Type"/> objects that represent syndication extension instances to attempt to instantiate during the load operation. 
        ///     The default value is an <i>empty</i> collection.
        /// </value>
        /// <remarks>
        ///     If <see cref="AutoDetectExtensions"/> is <b>true</b>, this collection will be automatically filled during the load operation based on the XML namespaces declared on the syndication resource. 
        ///     Automatic detection will <b>not</b> remove any syndication extensions already added to this collection prior to the load operation execution.
        /// </remarks>
        public Collection<Type> SupportedExtensions
        {
            get
            {
                if (supportedSyndicationExtensions == null)
                {
                    supportedSyndicationExtensions = new Collection<Type>();
                }
                return supportedSyndicationExtensions;
            }
        }
        #endregion

        #region Timeout
        /// <summary>
        /// Gets or sets a value that specifies the amount of time after which asynchronous load operations will time out.
        /// </summary>
        /// <value>An <see cref="TimeSpan"/> that specifies the time-out period. The default value is 15 seconds.</value>
        /// <exception cref="ArgumentOutOfRangeException">The time-out period is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The time-out period is greater than a year.</exception>
        public TimeSpan Timeout
        {
            get
            {
                return requestTimeout;
            }

            set
            {
                if (value.TotalMilliseconds < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                else if (value > TimeSpan.FromDays(365))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                else
                {
                    requestTimeout = value;
                }
            }
        }
        #endregion

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="SyndicationResourceLoadSettings"/>.</returns>
        /// <remarks>
        ///     This method returns a human-readable string for the current instance.
        /// </remarks>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Build the string representation
            //------------------------------------------------------------
            return String.Format(null, "[SyndicationResourceLoadSettings(CharacterEncoding = \"{0}\", RetrievalLimit = \"{1}\", Timeout = \"{2}\", Autodetect = \"{3}\", SupportedExtensions = \"{4}\")]", this.CharacterEncoding.WebName, this.RetrievalLimit, this.Timeout.TotalMilliseconds, this.AutoDetectExtensions, this.SupportedExtensions.GetHashCode().ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
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
            SyndicationResourceLoadSettings value  = obj as SyndicationResourceLoadSettings;

            if (value != null)
            {
                int result  = String.Compare(this.CharacterEncoding.WebName, value.CharacterEncoding.WebName, StringComparison.OrdinalIgnoreCase);
                result      = result | this.RetrievalLimit.CompareTo(value.RetrievalLimit);
                result      = result | this.Timeout.CompareTo(value.Timeout);
                result      = result | this.AutoDetectExtensions.CompareTo(value.AutoDetectExtensions);
                result      = result | ComparisonUtility.CompareSequence(this.SupportedExtensions, value.SupportedExtensions);

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
            if (!(obj is SyndicationResourceLoadSettings))
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
        public static bool operator ==(SyndicationResourceLoadSettings first, SyndicationResourceLoadSettings second)
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
        public static bool operator !=(SyndicationResourceLoadSettings first, SyndicationResourceLoadSettings second)
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
        public static bool operator <(SyndicationResourceLoadSettings first, SyndicationResourceLoadSettings second)
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
        public static bool operator >(SyndicationResourceLoadSettings first, SyndicationResourceLoadSettings second)
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
