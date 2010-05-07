/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
11/26/2007	brian.kuhn	Created EnumerationMetadataAttribute Class
****************************************************************************/
using System;

namespace Sage.SData.Client.Common
{
    /// <summary>
    /// Associates enumeration field description information with a target element. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    [Serializable()]
    public sealed class EnumerationMetadataAttribute : Attribute, IComparable
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        ///  Private member to hold the display name for the attributed field.
        /// </summary>
        private string enumMetadataDisplayName      = String.Empty;
        /// <summary>
        /// Private member to hold the alterate textual value for the attributed field.
        /// </summary>
        private string enumMetadataAlternateValue   = String.Empty;
        #endregion
        
        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region EnumerationMetadataAttribute()
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationMetadataAttribute"/> class.
        /// </summary>
        public EnumerationMetadataAttribute() : base()
        {
            //------------------------------------------------------------
            //	
            //------------------------------------------------------------
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region AlternateValue
        /// <summary>
        /// Gets or sets the alternate textual value for the attributed field.
        /// </summary>
        /// <value>The alternate textual value for the attributed field.</value>
        public string AlternateValue
        {
            get
            {
                return enumMetadataAlternateValue;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    enumMetadataAlternateValue = String.Empty;
                }
                else
                {
                    enumMetadataAlternateValue = value.Trim();
                }
            }
        }
        #endregion

        #region DisplayName
        /// <summary>
        /// Gets or sets the display name for the attributed field.
        /// </summary>
        /// <value>The display name for the attributed field.</value>
        public string DisplayName
        {
            get
            {
                return enumMetadataDisplayName;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    enumMetadataDisplayName = String.Empty;
                }
                else
                {
                    enumMetadataDisplayName = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="EnumerationMetadataAttribute"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="EnumerationMetadataAttribute"/>.</returns>
        /// <remarks>
        ///     This method returns a human-readable string for the current instance.
        /// </remarks>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Build the string representation
            //------------------------------------------------------------
            return String.Format(null, "[EnumerationMetadata(DisplayName = \"{0}\", AlternateValue=\"{1}\")]", this.DisplayName, this.AlternateValue);
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
            EnumerationMetadataAttribute value  = obj as EnumerationMetadataAttribute;

            if (value != null)
            {
                int result  = String.Compare(this.AlternateValue, value.AlternateValue, StringComparison.Ordinal);
                result      = result | String.Compare(this.DisplayName, value.DisplayName, StringComparison.OrdinalIgnoreCase);

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
            if (!(obj is EnumerationMetadataAttribute))
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
        public static bool operator ==(EnumerationMetadataAttribute first, EnumerationMetadataAttribute second)
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
        public static bool operator !=(EnumerationMetadataAttribute first, EnumerationMetadataAttribute second)
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
        public static bool operator <(EnumerationMetadataAttribute first, EnumerationMetadataAttribute second)
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
        public static bool operator >(EnumerationMetadataAttribute first, EnumerationMetadataAttribute second)
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