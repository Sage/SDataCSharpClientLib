using System;

using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Represents the data-type of a simple list property.
    /// </summary>
    /// <seealso cref="SimpleListSort.DataType"/>
    /// <seealso cref="SimpleListSort.DataTypeAsString(SimpleListDataType)"/>
    /// <seealso cref="SimpleListSort.DataTypeByName(string)"/>
    [Serializable()]
    public enum SimpleListDataType
    {
        /// <summary>
        /// No data-type specified.
        /// </summary>
        [EnumerationMetadata(DisplayName = "", AlternateValue = "")]
        None    = 0,

        /// <summary>
        /// The data type of the simple list property represents a date-time value.
        /// </summary>
        [EnumerationMetadata(DisplayName = "Date", AlternateValue = "date")]
        Date    = 1,

        /// <summary>
        /// The data type of the simple list property represents a numeric value.
        /// </summary>
        [EnumerationMetadata(DisplayName = "Number", AlternateValue = "number")]
        Number  = 2,

        /// <summary>
        /// The data type of the simple list property represents a textual value.
        /// </summary>
        [EnumerationMetadata(DisplayName = "Text", AlternateValue = "text")]
        Text    = 3
    }
}