// Copyright (c) Sage (UK) Limited 2010. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use this
// code. Please contact Sage (UK) if you do not have such a licence. Sage will take
// appropriate legal action against those who make unauthorised use of this code.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Parses a Uri path into segments
    /// </summary>
    public static class UriPathParser
    {
        #region Constants

        private const string Pattern = @"(?<segment>
                                           [^/(]*                       # anything other than slash or open paren
                                         )
                                         (
                                           \(
                                             (?<predicate>
                                               (
                                                 ('([^']|(''))*')       # single quoted literal string
                                                 |
                                                 (""([^""]|(""""))*"")  # double quoted literal string
                                                 |
                                                 ([^'"")]*)             # anything other than quote or close paren
                                               )*
                                             )
                                           \)
                                         )?";

        private static readonly Regex Regex = new Regex(Pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        private static readonly UriPathSegment[] EmptyPath = new UriPathSegment[] {};

        #endregion

        /// <summary>
        /// Parses the path of the specified <see cref="Uri"/> into segments.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> containing the path to parse into segments.</param>
        /// <returns>An array of segments that form the path for the specified <see cref="Uri"/>.</returns>
        public static UriPathSegment[] Parse(Uri uri)
        {
            return Parse(uri.AbsolutePath);
        }

        /// <summary>
        /// Parses the specified path into segments.
        /// </summary>
        /// <param name="path">The path to parse into segments.</param>
        /// <returns>An array of segments that form the specified path.</returns>
        public static UriPathSegment[] Parse(string path)
        {
            if (string.IsNullOrEmpty(path))
                return EmptyPath;

            var segments = new List<UriPathSegment>();
            var match = Regex.Match(path);

            while (match.Success)
            {
                var segment = match.Groups["segment"].Value;

                if (segment.Length != 0)
                    segments.Add(new UriPathSegment(segment, match.Groups["predicate"].Value));

                match = match.NextMatch();
            }

            return segments.ToArray();
        }
    }
}