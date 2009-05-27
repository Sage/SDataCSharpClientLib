using System;
using System.Globalization;

namespace Sage.SData.Client.Common
{
    /// <summary>
    /// Provides methods for generating and parsing date-time information exposed by syndicated content. This class cannot be inherited.
    /// </summary>
    /// <remarks>
    ///     See <a href="http://www.ietf.org/rfc/rfc0822.txt">RFC #822: Standard for ARPA Internet Text Messages (Date and Time Specification)</a> 
    ///     and <a href="http://www.ietf.org/rfc/rfc3339.txt">RFC #3339: Date and Time on the Internet (Timestamps)</a> for further information about 
    ///     the date-time formats implemented in the <see cref="SyndicationDateTimeUtility"/> class.
    /// </remarks>
    public static class SyndicationDateTimeUtility
    {
        //============================================================
        //	RFC-3339 FORMAT METHODS
        //============================================================
        #region ParseRfc3339DateTime(string value)
        /// <summary>
        /// Converts the specified string representation of a RFC-3339 formatted date to its <see cref="DateTime"/> equivalent.
        /// </summary>
        /// <param name="value">A string containing a RFC-3339 formatted date to convert.</param>
        /// <returns>A <see cref="DateTime"/> equivalent to the RFC-3339 formatted date contained in <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        /// <exception cref="FormatException">The <paramref name="value"/> is not a recognized as a RFC-3339 formatted date.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rfc")]
        public static DateTime ParseRfc3339DateTime(string value)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            DateTime result = DateTime.MinValue;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNullOrEmptyString(value, "value");

            //------------------------------------------------------------
            //	Parse RFC-3339 formatted date
            //------------------------------------------------------------
            if (SyndicationDateTimeUtility.TryParseRfc3339DateTime(value, out result))
            {
                return result;
            }
            else
            {
                throw new FormatException(String.Format(null, "'{0}' is not a valid RFC-3339 formatted date-time value.", value));
            }
        }
        #endregion

        #region ToRfc3339DateTime(DateTime utcDateTime)
        /// <summary>
        /// Converts the value of the supplied <see cref="DateTime"/> object to its equivalent RFC-3339 date string representation.
        /// </summary>
        /// <param name="utcDateTime">The UTC <see cref="DateTime"/> object to convert.</param>
        /// <returns>A string that contains the RFC-3339 date string representation of the supplied <see cref="DateTime"/> object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rfc")]
        public static string ToRfc3339DateTime(DateTime utcDateTime)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            DateTimeFormatInfo dateTimeFormat   = CultureInfo.InvariantCulture.DateTimeFormat;

            //------------------------------------------------------------
            //	Return RFC-3339 formatted date-time string
            //------------------------------------------------------------
            if (utcDateTime.Kind == DateTimeKind.Local)
            {
                return utcDateTime.ToString("yyyy'-'MM'-'dd'T'HH:mm:ss.ffzzz", dateTimeFormat);
            }
            else
            {
                return utcDateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.ff'Z'", dateTimeFormat);
            }
        }
        #endregion

        #region TryParseRfc3339DateTime(string value, out DateTime result)
        /// <summary>
        /// Converts the specified string representation of a RFC-3339 formatted date to its <see cref="DateTime"/> equivalent.
        /// </summary>
        /// <param name="value">A string containing a RFC-3339 formatted date to convert.</param>
        /// <param name="result">
        ///     When this method returns, contains the <see cref="DateTime"/> value equivalent to the date and time contained in <paramref name="value"/>, if the conversion succeeded, or <see cref="DateTime.MinValue">MinValue</see> if the conversion failed. 
        ///     The conversion fails if the <paramref name="value"/> parameter is a <b>null</b> or empty string, or does not contain a valid string representation of a RFC-3339 formatted date. 
        ///     This parameter is passed uninitialized.
        /// </param>
        /// <returns><b>true</b> if the <paramref name="value"/> parameter was converted successfully; otherwise, <b>false</b>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rfc")]
        public static bool TryParseRfc3339DateTime(string value, out DateTime result)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            DateTimeFormatInfo dateTimeFormat   = CultureInfo.InvariantCulture.DateTimeFormat;
            string[] formats                    = new string[15];

            //------------------------------------------------------------
            //	Define valid RFC-3339 formats
            //------------------------------------------------------------
            formats[0]  = dateTimeFormat.SortableDateTimePattern;
            formats[1]  = dateTimeFormat.UniversalSortableDateTimePattern;
            formats[2]  = "yyyy'-'MM'-'dd'T'HH:mm:ss'Z'";
            formats[3]  = "yyyy'-'MM'-'dd'T'HH:mm:ss.f'Z'";
            formats[4]  = "yyyy'-'MM'-'dd'T'HH:mm:ss.ff'Z'";
            formats[5]  = "yyyy'-'MM'-'dd'T'HH:mm:ss.fff'Z'";
            formats[6]  = "yyyy'-'MM'-'dd'T'HH:mm:ss.ffff'Z'";
            formats[7]  = "yyyy'-'MM'-'dd'T'HH:mm:ss.fffff'Z'";
            formats[8]  = "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffff'Z'";
            formats[9]  = "yyyy'-'MM'-'dd'T'HH:mm:sszzz";
            formats[10] = "yyyy'-'MM'-'dd'T'HH:mm:ss.ffzzz";
            formats[11] = "yyyy'-'MM'-'dd'T'HH:mm:ss.fffzzz";
            formats[12] = "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffzzz";
            formats[13] = "yyyy'-'MM'-'dd'T'HH:mm:ss.fffffzzz";
            formats[14] = "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffffzzz";

            //------------------------------------------------------------
            //	Validate parameter  
            //------------------------------------------------------------
            if (String.IsNullOrEmpty(value))
            {
                result = DateTime.MinValue;
                return false;
            }

            //------------------------------------------------------------
            //	Perform conversion of RFC-3339 formatted date-time string
            //------------------------------------------------------------
            return DateTime.TryParseExact(value, formats, dateTimeFormat, DateTimeStyles.AssumeUniversal, out result);
        }
        #endregion

        //============================================================
        //	RFC-822 FORMAT METHODS
        //============================================================
        #region ReplaceRfc822TimeZoneWithOffset(string value)
        /// <summary>
        /// Replaces the RFC-822 time-zone component with its offset equivalent.
        /// </summary>
        /// <param name="value">A string containing a RFC-822 formatted date to convert.</param>
        /// <returns>A string containing a RFC-822 formatted date, with the <i>zone</i> component converted to its offset equivalent.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        /// <seealso cref="TryParseRfc822DateTime(string, out DateTime)"/>
        private static string ReplaceRfc822TimeZoneWithOffset(string value)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNullOrEmptyString(value, "value");

            //------------------------------------------------------------
            //	Perform conversion
            //------------------------------------------------------------
            value   = value.Trim();
            if (value.EndsWith("UT", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}GMT", value.TrimEnd("UT".ToCharArray()));
            }
            else if (value.EndsWith("EST", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}-05:00", value.TrimEnd("EST".ToCharArray()));
            }
            else if (value.EndsWith("EDT", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}-04:00", value.TrimEnd("EDT".ToCharArray()));
            }
            else if (value.EndsWith("CST", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}-06:00", value.TrimEnd("CST".ToCharArray()));
            }
            else if (value.EndsWith("CDT", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}-05:00", value.TrimEnd("CDT".ToCharArray()));
            }
            else if (value.EndsWith("MST", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}-07:00", value.TrimEnd("MST".ToCharArray()));
            }
            else if (value.EndsWith("MDT", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}-06:00", value.TrimEnd("MDT".ToCharArray()));
            }
            else if (value.EndsWith("PST", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}-08:00", value.TrimEnd("PST".ToCharArray()));
            }
            else if (value.EndsWith("PDT", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}-07:00", value.TrimEnd("PDT".ToCharArray()));
            }
            else if (value.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}GMT", value.TrimEnd("Z".ToCharArray()));
            }
            else if (value.EndsWith("A", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}-01:00", value.TrimEnd("A".ToCharArray()));
            }
            else if (value.EndsWith("M", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}-12:00", value.TrimEnd("M".ToCharArray()));
            }
            else if (value.EndsWith("N", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}+01:00", value.TrimEnd("N".ToCharArray()));
            }
            else if (value.EndsWith("Y", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format(null, "{0}+12:00", value.TrimEnd("Y".ToCharArray()));
            }
            else
            {
                return value;
            }
        }
        #endregion

        #region ParseRfc822DateTime(string value)
        /// <summary>
        /// Converts the specified string representation of a RFC-822 formatted date to its <see cref="DateTime"/> equivalent.
        /// </summary>
        /// <param name="value">A string containing a RFC-822 formatted date to convert.</param>
        /// <returns>A <see cref="DateTime"/> equivalent to the RFC-822 formatted date contained in <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        /// <exception cref="FormatException">The <paramref name="value"/> is not a recognized as a RFC-822 formatted date.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rfc")]
        public static DateTime ParseRfc822DateTime(string value)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            DateTime result = DateTime.MinValue;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNullOrEmptyString(value, "value");

            //------------------------------------------------------------
            //	Parse RFC-3339 formatted date
            //------------------------------------------------------------
            if (SyndicationDateTimeUtility.TryParseRfc822DateTime(value, out result))
            {
                return result;
            }
            else
            {
                throw new FormatException(String.Format(null, "'{0}' is not a valid RFC-822 formatted date-time value.", value));
            }
        }
        #endregion

        #region ToRfc822DateTime(DateTime dateTime)
        /// <summary>
        /// Converts the value of the supplied <see cref="DateTime"/> object to its equivalent RFC-822 date string representation.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> object to convert.</param>
        /// <returns>A string that contains the RFC-822 date string representation of the supplied <see cref="DateTime"/> object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rfc")]
        public static string ToRfc822DateTime(DateTime dateTime)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            DateTimeFormatInfo dateTimeFormat   = CultureInfo.InvariantCulture.DateTimeFormat;

            //------------------------------------------------------------
            //	Return RFC-822 formatted date-time string
            //------------------------------------------------------------
            return dateTime.ToString(dateTimeFormat.RFC1123Pattern, dateTimeFormat);
        }
        #endregion

        #region TryParseRfc822DateTime(string value, out DateTime result)
        /// <summary>
        /// Converts the specified string representation of a RFC-822 formatted date to its <see cref="DateTime"/> equivalent.
        /// </summary>
        /// <param name="value">A string containing a RFC-822 formatted date to convert.</param>
        /// <param name="result">
        ///     When this method returns, contains the <see cref="DateTime"/> value equivalent to the date and time contained in <paramref name="value"/>, if the conversion succeeded, or <see cref="DateTime.MinValue">MinValue</see> if the conversion failed. 
        ///     The conversion fails if the <paramref name="value"/> parameter is a <b>null</b> or empty string, or does not contain a valid string representation of a RFC-822 formatted date. 
        ///     This parameter is passed uninitialized.
        /// </param>
        /// <returns><b>true</b> if the <paramref name="value"/> parameter was converted successfully; otherwise, <b>false</b>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rfc")]
        public static bool TryParseRfc822DateTime(string value, out DateTime result)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            DateTimeFormatInfo dateTimeFormat   = CultureInfo.InvariantCulture.DateTimeFormat;
            string[] formats                    = new string[3];

            //------------------------------------------------------------
            //	Define valid RFC-822 formats
            //------------------------------------------------------------
            formats[0]  = dateTimeFormat.RFC1123Pattern;
            formats[1]  = "ddd',' d MMM yyyy HH:mm:ss zzz";
            formats[2]  = "ddd',' dd MMM yyyy HH:mm:ss zzz";

            //------------------------------------------------------------
            //	Validate parameter  
            //------------------------------------------------------------
            if (String.IsNullOrEmpty(value))
            {
                result = DateTime.MinValue;
                return false;
            }

            //------------------------------------------------------------
            //	Perform conversion of RFC-822 formatted date-time string
            //------------------------------------------------------------
            return DateTime.TryParseExact(SyndicationDateTimeUtility.ReplaceRfc822TimeZoneWithOffset(value), formats, dateTimeFormat, DateTimeStyles.None, out result);
        }
        #endregion
    }
}
