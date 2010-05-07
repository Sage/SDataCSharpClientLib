// Copyright (c) Sage (UK) Limited 2010. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use this
// code. Please contact Sage (UK) if you do not have such a licence. Sage will take
// appropriate legal action against those who make unauthorised use of this code.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Represents a W3C DateTime structure.
    /// </summary>
    /// <remarks>See http://www.w3.org/TR/NOTE-datetime for details on the W3C date time guidelines.</remarks>
    [Serializable]
    public class W3CDateTime
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================

        #region PRIVATE/PROTECTED/PUBLIC MEMBERS

        /// <summary>
        /// Private member to hold the datetime in a UTC format.
        /// </summary>
        private readonly DateTime _utcDateTime;

        /// <summary>
        /// Private member to hold the UTC format timspan offest.
        /// </summary>
        private readonly TimeSpan _utcOffset;

        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="W3CDateTime"/> class.
        /// </summary>
        /// <param name="dateTime">The datetime to represent in a W3C format.</param>
        public W3CDateTime(DateTime dateTime)
        {
            //------------------------------------------------------------
            //	Attempt to handle class initialization
            //------------------------------------------------------------
            //------------------------------------------------------------
            //	Set class members
            //------------------------------------------------------------
            _utcDateTime = dateTime;

            if (dateTime.Equals(DateTime.MinValue))
            {
                _utcOffset = TimeSpan.Zero;
            }
            else
            {
                _utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
                _utcDateTime -= _utcOffset;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="W3CDateTime"/> class.
        /// </summary>
        /// <param name="dateTime">The datetime to represent in a W3C format.</param>
        /// <param name="offset">The UTC offest for the datetime.</param>
        public W3CDateTime(DateTime dateTime, TimeSpan offset)
        {
            //------------------------------------------------------------
            //	Attempt to handle class initialization
            //------------------------------------------------------------
            //------------------------------------------------------------
            //	Set class members
            //------------------------------------------------------------
            _utcDateTime = dateTime;
            _utcOffset = offset;
        }

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================

        /// <summary>
        /// Gets the W3C datetime. 
        /// </summary>
        public DateTime DateTime
        {
            get { return _utcDateTime + _utcOffset; }
        }

        /// <summary>
        /// Gets the UTC offset.
        /// </summary>
        public TimeSpan UtcOffset
        {
            get { return _utcOffset; }
        }

        /// <summary>
        /// Gets the UTC datetime.
        /// </summary>
        public DateTime UtcTime
        {
            get { return _utcDateTime; }
        }

        //============================================================
        //	PUBLIC ROUTINES
        //============================================================

        private const string W3CDateFormat = @"^(?<year>\d\d\d\d)(-(?<month>\d\d)(-(?<day>\d\d)?)?)?(T(?<hour>\d\d)(:(?<min>\d\d)(:(?<sec>\d\d)(?<ms>\.\d+)?)?)?)?(?<ofs>(Z|[+\-]\d\d:\d\d))?$";

        private static readonly Regex Regex = new Regex(
            W3CDateFormat,
            RegexOptions.IgnoreCase |
            RegexOptions.CultureInvariant |
            RegexOptions.IgnorePatternWhitespace |
            RegexOptions.Compiled);

        /// <summary>
        /// Converts the specified string representation of a W3C date and time to its <see cref="W3CDateTime"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <returns>A W3CDateTime equivalent to the date and time contained in s.</returns>
        public static W3CDateTime Parse(string s)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------

            var offset = TimeSpan.Zero;
            W3CDateTime w3CDateTime;

            //------------------------------------------------------------
            //	Attempt to convert string representation
            //------------------------------------------------------------
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("s");
            }

            //------------------------------------------------------------
            //	Initialize regular expression
            //------------------------------------------------------------
            var regularExpression = Regex;

            //------------------------------------------------------------
            //	Extract results of regular expression match
            //------------------------------------------------------------
            var match = regularExpression.Match(s);

            //------------------------------------------------------------
            //	Determine if string represents a W3C DateTime
            //------------------------------------------------------------
            if (!match.Success)
            {
                //------------------------------------------------------------
                //	Raise exception
                //------------------------------------------------------------
                throw new FormatException("DateTime is not in a valid format");
            }

            //------------------------------------------------------------
            //	Attempt to parse string
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Extract year and handle 2/3 digit years
                //------------------------------------------------------------
                var year = int.Parse(match.Groups["year"].Value, CultureInfo.InvariantCulture);
                if (year < 1000 && match.Groups["year"].Length < 3)
                {
                    if (year < 50)
                    {
                        year = year + 2000;
                    }
                    else
                    {
                        year = year + 1999;
                    }
                }

                //------------------------------------------------------------
                //	Extract other date time parts
                //------------------------------------------------------------
                var month = (match.Groups["month"].Success) ? int.Parse(match.Groups["month"].Value, CultureInfo.InvariantCulture) : 1;
                var day = match.Groups["day"].Success ? int.Parse(match.Groups["day"].Value, CultureInfo.InvariantCulture) : 1;

                int millisecond;
                int second;
                int minute;
                int hour;
                if (match.Groups["hour"].Success)
                {
                    hour = match.Groups["hour"].Success ? int.Parse(match.Groups["hour"].Value, CultureInfo.InvariantCulture) : 0;
                    minute = match.Groups["min"].Success ? int.Parse(match.Groups["min"].Value, CultureInfo.InvariantCulture) : 0;
                    second = match.Groups["sec"].Success ? int.Parse(match.Groups["sec"].Value, CultureInfo.InvariantCulture) : 0;
                    millisecond = match.Groups["ms"].Success ? (int) Math.Round((1000*double.Parse(match.Groups["ms"].Value, CultureInfo.InvariantCulture))) : 0;
                }
                else
                {
                    hour = 0;
                    minute = 0;
                    second = 0;
                    millisecond = 0;
                }

                //------------------------------------------------------------
                //	Calculate offset
                //------------------------------------------------------------
                if (match.Groups["ofs"].Success)
                {
                    offset = ParseW3COffSet(match.Groups["ofs"].Value);
                }

                //------------------------------------------------------------
                //	Generate result
                //------------------------------------------------------------
                w3CDateTime = new W3CDateTime(new DateTime(year, month, day, hour, minute, second, millisecond) - offset, offset);
            }
            catch (Exception exception)
            {
                //------------------------------------------------------------
                //	Raise exception
                //------------------------------------------------------------
                throw new FormatException("DateTime is not in a valid format", exception);
            }

            //------------------------------------------------------------
            //	Return result
            //------------------------------------------------------------
            return w3CDateTime;
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of value of this instance.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            //------------------------------------------------------------
            //	Return string representation of W3C date time.
            //------------------------------------------------------------
            return (_utcDateTime + _utcOffset).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) + FormatOffset(_utcOffset, ":");
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of value of this instance.</returns>
        public string ToDateString()
        {
            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            //------------------------------------------------------------
            //	Return string representation of W3C date time.
            //------------------------------------------------------------

            // Don't include the offset if we are not including the time otherwise the offset may be interpreted as a time
            // e.g. a date string 2009-10-09-7:00 would be interpreted as 7AM on 9th October instead of being seen as a -7 hour offset
            return (_utcDateTime + _utcOffset).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a value indicating if sucessfully able to parse W3C formatted datetime string.
        /// </summary>
        /// <param name="date">The W3C datetime formatted string to parse.</param>
        /// <param name="result">The <see cref="W3CDateTime"/> represented by the datetime string.</param>
        /// <returns><b>true</b> if able to parse string representation, otherwise returns false.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="date"/> is an empty string or is a null reference (Nothing in Visual Basic).</exception>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool TryParse(string date, out W3CDateTime result)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool parseSucceeded;

            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            if (string.IsNullOrEmpty(date))
            {
                throw new ArgumentNullException("date");
            }

            //------------------------------------------------------------
            //	Attempt to parse
            //------------------------------------------------------------
            try
            {
                result = Parse(date);
                parseSucceeded = true;
            }
            catch (Exception)
            {
                result = null;
                parseSucceeded = false;
            }

            //------------------------------------------------------------
            //	Return result
            //------------------------------------------------------------
            return parseSucceeded;
        }

        //============================================================
        //	PRIVATE ROUTINES
        //============================================================

        /// <summary>
        /// Converts the value of the specified <see cref="TimeSpan"/> to its equivalent string representation.
        /// </summary>
        /// <param name="offset">The <see cref="TimeSpan"/> to convert.</param>
        /// <param name="separator">Separator used to deliminate hours and minutes.</param>
        /// <returns>A string representation of the TimeSpan.</returns>
        private static string FormatOffset(TimeSpan offset, string separator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            var formattedOffset = string.Empty;

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            //------------------------------------------------------------
            //	Generate formatted result
            //------------------------------------------------------------
            if (offset >= TimeSpan.Zero)
            {
                formattedOffset = "+";
            }

            formattedOffset = string.Concat(formattedOffset, offset.Hours.ToString("00", CultureInfo.InvariantCulture), separator, offset.Minutes.ToString("00", CultureInfo.InvariantCulture));

            //------------------------------------------------------------
            //	Return result
            //------------------------------------------------------------
            return formattedOffset;
        }

        /// <summary>
        /// Converts the specified string representation of an offset to its <see cref="TimeSpan"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a offset to convert.</param>
        /// <returns>A TimeSpan equivalent to the offset contained in s.</returns>
        private static TimeSpan ParseW3COffSet(string s)
        {
            //------------------------------------------------------------
            //	Attempt to parse offset string
            //------------------------------------------------------------
            //------------------------------------------------------------
            //	Return timespan offset
            //------------------------------------------------------------
            if (string.IsNullOrEmpty(s) || s == "Z")
            {
                return TimeSpan.Zero;
            }
            if (s[0] == '+')
            {
                return TimeSpan.Parse(s.Substring(1));
            }
            return TimeSpan.Parse(s);
        }

        /// <summary>
        /// Returns a value indicating whether the specified date represents a null date.
        /// </summary>
        /// <param name="dateTime">The <see cref="W3CDateTime"/> that needs to be checked.</param>
        /// <returns><b>true</b> if the value represents a null date, otherwise returns false.</returns>
        public static bool IsNull(W3CDateTime dateTime)
        {
            return dateTime == null || dateTime.DateTime == DateTime.MinValue;
        }
    }
}