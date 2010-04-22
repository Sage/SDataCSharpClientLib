/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
12/07/2007	brian.kuhn	Created SyndicationResourceSaveSettings Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Sage.SData.Client.Common
{
    /// <summary>
    /// Specifies a set of features to support on a <see cref="ISyndicationResource"/> object persisted by the <see cref="ISyndicationResource.Save(Stream, SyndicationResourceSaveSettings)"/> method.
    /// </summary>
    [Serializable()]
    public sealed class SyndicationResourceSaveSettings : IComparable
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
        /// Private member to hold a value indicating if write/save operations should attempt to minimize the size of the resulting output.
        /// </summary>
        private bool minimizeOutputSize;
        /// <summary>
        /// Private member to hold a collection of types that represent the syndication extensions supported by the save operation.
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
        #region SyndicationResourceSaveSettings()
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationResourceSaveSettings"/> class.
        /// </summary>
        public SyndicationResourceSaveSettings()
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
        ///     <b>true</b> if the syndication extensions supported by the save operation are automatically determined based on the syndication extensions added to the syndication resource and its child entities; otherwise <b>false</b>. 
        ///     The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        ///     Automatic detection of supported syndication extensions will <b>not</b> remove any syndication extensions already added 
        ///     to the <see cref="SupportedExtensions"/> collection prior to the save operation execution.
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
        /// Gets or sets the character encoding to use when persisting a syndication resource.
        /// </summary>
        /// <value>A <see cref="Encoding"/> object that indicates the character encoding to use when persisting a syndication resource. The default value is <see cref="Encoding.UTF8"/>.</value>
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

        #region MinimizeOutputSize
        /// <summary>
        /// Gets or sets a value indicating if syndication resource persist operations should attempt to minimize the physical size of the resulting output.
        /// </summary>
        /// <value><b>true</b> if output size should be as small as possible; otherwise <b>false</b>. The default value is <b>false</b>.</value>
        public bool MinimizeOutputSize
        {
            get
            {
                return minimizeOutputSize;
            }

            set
            {
                minimizeOutputSize = value;
            }
        }
        #endregion

        #region SupportedExtensions
        /// <summary>
        /// Gets the syndication extensions that extend the syndication resource.
        /// </summary>
        /// <value>
        ///     A <see cref="Collection{T}"/> collection of <see cref="Type"/> objects that represent syndication extension instances used during the save operation. 
        ///     The default value is an <i>empty</i> collection.
        /// </value>
        /// <remarks>
        ///     During a save operation, each of these syndication extension types is instantiated and used to write the prefixed XML namespace declarations on the root syndication resource entity.
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

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="SyndicationResourceSaveSettings"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="SyndicationResourceSaveSettings"/>.</returns>
        /// <remarks>
        ///     This method returns a human-readable string for the current instance.
        /// </remarks>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Build the string representation
            //------------------------------------------------------------
            return String.Format(null, "[SyndicationResourceSaveSettings(CharacterEncoding = \"{0}\", MinimizeOutputSize = \"{1}\", Autodetect = \"{2}\", SupportedExtensions = \"{3}\")]", this.CharacterEncoding.WebName, this.MinimizeOutputSize, this.AutoDetectExtensions, this.SupportedExtensions.GetHashCode().ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
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
            SyndicationResourceSaveSettings value  = obj as SyndicationResourceSaveSettings;

            if (value != null)
            {
                int result  = String.Compare(this.CharacterEncoding.WebName, value.CharacterEncoding.WebName, StringComparison.OrdinalIgnoreCase);
                result      = result | this.MinimizeOutputSize.CompareTo(value.MinimizeOutputSize);
                result      = result | ComparisonUtility.CompareSequence(this.SupportedExtensions, value.SupportedExtensions);
                result      = result | this.AutoDetectExtensions.CompareTo(value.AutoDetectExtensions);

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
            if (!(obj is SyndicationResourceSaveSettings))
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
        public static bool operator ==(SyndicationResourceSaveSettings first, SyndicationResourceSaveSettings second)
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
        public static bool operator !=(SyndicationResourceSaveSettings first, SyndicationResourceSaveSettings second)
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
        public static bool operator <(SyndicationResourceSaveSettings first, SyndicationResourceSaveSettings second)
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
        public static bool operator >(SyndicationResourceSaveSettings first, SyndicationResourceSaveSettings second)
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
