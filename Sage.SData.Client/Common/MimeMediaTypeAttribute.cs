using System;

namespace Sage.SData.Client.Common
{
    /// <summary>
    /// Associates IANA MIME media type information with a target element. This class cannot be inherited.
    /// </summary>
    /// <remarks>
    ///     See <a href="http://www.iana.org/assignments/media-types">http://www.iana.org/assignments/media-types</a> for a listing of the registered IANA MIME media types and sub-types.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    [Serializable()]
    public sealed class MimeMediaTypeAttribute : Attribute, IComparable
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the MIME media type name.
        /// </summary>
        private string mimeMediaTypeName    = String.Empty;
        /// <summary>
        /// Private member to hold the MIME media sub-type name.
        /// </summary>
        private string mimeMediaSubTypeName = String.Empty;
        /// <summary>
        /// Private member to hold a URI that points to the documentation the describes the MIME media type.
        /// </summary>
        private Uri mimeMediaDocumentation;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region MimeMediaTypeAttribute()
        /// <summary>
        /// Initializes a new instance of the <see cref="MimeMediaTypeAttribute"/> class.
        /// </summary>
        public MimeMediaTypeAttribute() : base()
        {
            //------------------------------------------------------------
            //	
            //------------------------------------------------------------
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Documentation
        /// <summary>
        /// Gets or sets a URI that points to the documentation the describes the MIME media type for the attributed field.
        /// </summary>
        /// <value>A <see cref="Uri"/> that points to the documentation the describes the MIME media type for the attributed field.</value>
        public string Documentation
        {
            get
            {
                return mimeMediaDocumentation != null ? mimeMediaDocumentation.ToString() : String.Empty;
            }

            set
            {
                if (value == null)
                {
                    mimeMediaDocumentation = null;
                }
                else
                {
                    Uri url;
                    if (Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out url))
                    {
                        mimeMediaDocumentation = url;
                    }
                    else
                    {
                        mimeMediaDocumentation = null;
                    }
                }
            }
        }
        #endregion

        #region Name
        /// <summary>
        /// Gets or sets the MIME media type name for the attributed field.
        /// </summary>
        /// <value>The MIME media type name for the attributed field.</value>
        public string Name
        {
            get
            {
                return mimeMediaTypeName;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    mimeMediaTypeName = String.Empty;
                }
                else
                {
                    mimeMediaTypeName = value.Trim();
                }
            }
        }
        #endregion

        #region SubName
        /// <summary>
        /// Gets or sets the MIME media sub-type name for the attributed field.
        /// </summary>
        /// <value>The MIME media sub-type name for the attributed field.</value>
        public string SubName
        {
            get
            {
                return mimeMediaSubTypeName;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    mimeMediaSubTypeName = String.Empty;
                }
                else
                {
                    mimeMediaSubTypeName = value.Trim();
                }
            }
        }
        #endregion

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="MimeMediaTypeAttribute"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="MimeMediaTypeAttribute"/>.</returns>
        /// <remarks>
        ///     This method returns a human-readable string for the current instance.
        /// </remarks>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Build the string representation
            //------------------------------------------------------------
            return String.Format(null, "[MimeMediaType(Name = \"{0}\", SubName = \"{1}\", Documentation = \"{2}\")]", this.Name, this.SubName, this.Documentation != null ? this.Documentation.ToString() : String.Empty);
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
            MimeMediaTypeAttribute value  = obj as MimeMediaTypeAttribute;

            if (value != null)
            {
                int result  = String.Compare(this.Documentation, value.Documentation, StringComparison.OrdinalIgnoreCase);
                result      = result | String.Compare(this.Name, value.Name, StringComparison.OrdinalIgnoreCase);
                result      = result | String.Compare(this.SubName, value.SubName, StringComparison.OrdinalIgnoreCase);

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
            if (!(obj is MimeMediaTypeAttribute))
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
        public static bool operator ==(MimeMediaTypeAttribute first, MimeMediaTypeAttribute second)
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
        public static bool operator !=(MimeMediaTypeAttribute first, MimeMediaTypeAttribute second)
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
        public static bool operator <(MimeMediaTypeAttribute first, MimeMediaTypeAttribute second)
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
        public static bool operator >(MimeMediaTypeAttribute first, MimeMediaTypeAttribute second)
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