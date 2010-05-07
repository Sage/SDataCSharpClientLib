// Copyright (c) Sage (UK) Limited 2010. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use this
// code. Please contact Sage (UK) if you do not have such a licence. Sage will take
// appropriate legal action against those who make unauthorised use of this code.

using System;
using System.Collections.Generic;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Helper class for parsing the query part of a <see cref="Uri"/>
    /// </summary>
    public static class UriQueryParser
    {
        #region Methods

        /// <summary>
        /// Parses the arguments from the specified query string.
        /// </summary>
        /// <param name="query">The query to parse.</param>
        public static IDictionary<string, string> Parse(string query)
        {
            var args = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            Parse(args, query);
            return args;
        }

        /// <summary>
        /// Parses the arguments from the specified query string.
        /// </summary>
        /// <param name="args">On exit contains the query arguments.</param>
        /// <param name="query">The query to parse.</param>
        public static void Parse(IDictionary<string, string> args, string query)
        {
            if (string.IsNullOrEmpty(query))
                return;

            ParserWorker(args, query, null, UriFormatter.QueryArgPrefix);
        }

        /// <summary>
        /// Parses the arguments from the specified query string.
        /// </summary>
        /// <param name="query">The query to parse.</param>
        /// <param name="separator">The character separating the arguments.</param>
        public static IDictionary<string, string> Parse(string query, string separator)
        {
            var args = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            Parse(args, query, null, separator);
            return args;
        }

        /// <summary>
        /// Parses the arguments from the specified query string.
        /// </summary>
        /// <param name="args">On exit contains the query arguments.</param>
        /// <param name="query">The query to parse.</param>
        /// <param name="separator">The character separating the arguments.</param>
        public static void Parse(IDictionary<string, string> args, string query, string separator)
        {
            if (string.IsNullOrEmpty(query))
                return;

            ParserWorker(args, query, null, separator);
        }

        /// <summary>
        /// Parses the arguments from the specified query string.
        /// </summary>
        /// <param name="query">The query to parse.</param>
        /// <param name="complexArgs">Array of arguments that can be quoted.</param>
        public static IDictionary<string, string> Parse(string query, IEnumerable<string> complexArgs)
        {
            var args = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            Parse(args, query, complexArgs);
            return args;
        }

        /// <summary>
        /// Parses the arguments from the specified query string.
        /// </summary>
        /// <param name="args">On exit contains the query arguments.</param>
        /// <param name="query">The query to parse.</param>
        /// <param name="complexArgs">Array of arguments that can be quoted.</param>
        public static void Parse(IDictionary<string, string> args, string query, IEnumerable<string> complexArgs)
        {
            if (string.IsNullOrEmpty(query))
                return;

            ParserWorker(args, query, complexArgs, UriFormatter.QueryArgPrefix);
        }

        /// <summary>
        /// Parses the arguments from the specified query string.
        /// </summary>
        /// <param name="query">The query to parse.</param>
        /// <param name="complexArgs">Array of arguments that can be quoted.</param>
        /// <param name="separator">The character separating the arguments.</param>
        public static IDictionary<string, string> Parse(string query, IEnumerable<string> complexArgs, string separator)
        {
            var args = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            Parse(args, query, complexArgs, separator);
            return args;
        }

        /// <summary>
        /// Parses the arguments from the specified query string.
        /// </summary>
        /// <param name="args">On exit contains the query arguments.</param>
        /// <param name="query">The query to parse.</param>
        /// <param name="complexArgs">Array of arguments that can be quoted.</param>
        /// <param name="separator">The character separating the arguments.</param>
        public static void Parse(IDictionary<string, string> args, string query, IEnumerable<string> complexArgs, string separator)
        {
            if (string.IsNullOrEmpty(query))
                return;

            ParserWorker(args, query, complexArgs, separator);
        }

        #endregion

        #region Local Methods

        private static void ParserWorker(IDictionary<string, string> args, string query, IEnumerable<string> complexArgs, string separator)
        {
            var start = -1;
            string complexArgName = null;

            if (complexArgs != null)
            {
                foreach (var complexArg in complexArgs)
                {
                    var index = query.IndexOf(complexArg + UriFormatter.QueryArgValuePrefix, StringComparison.InvariantCultureIgnoreCase);

                    if (index < 0)
                        continue;

                    if (start < 0 || index < start)
                    {
                        start = index;
                        complexArgName = complexArg;
                    }
                }
            }

            if (complexArgName != null)
            {
                if (start > 0)
                    ParserWorker(args, query.Substring(0, start - 1), complexArgs, separator);

                // Move past the complex arg part part
                start += complexArgName.Length + 1;

                var end = start;
                var length = query.Length;
                bool isComplex;

                if (query[start] != '"')
                {
                    isComplex = false;

                    // Simple expression q=<expression>
                    end = query.IndexOf(separator, end);

                    if (end < 0)
                        end = length;
                }
                else
                {
                    // Double quoted expression q="<expression"
                    isComplex = true;

                    // Move past the double quote
                    start++;
                    end++;

                    // Find the end quote of the expression

                    while (end < length)
                    {
                        // Find the next double quote
                        var next = query.IndexOf('"', end);

                        if (next < 0)
                        {
                            // Probably badly formed, but we just ignore it
                            end = length;
                            break;
                        }

                        // Found a double quote
                        end = next;

                        // If its not escape, its the last
                        if (query[end - 1] != '\\')
                            break;

                        end++;
                    }
                }

                // Parse the bit after the query expression
                var delta = isComplex ? 1 : 0;

                if (end < length - delta)
                    ParserWorker(args, query.Substring(end + (delta + 1)), complexArgs, separator);

                // Finally parse the query string
                var expression = query.Substring(start, end - start);
                expression = expression.Replace("\\\"", "\"");

                args[complexArgName] = expression;
            }
            else
            {
                foreach (var arg in query.Split(separator[0]))
                {
                    var parts = arg.Split(UriFormatter.QueryArgValuePrefix[0]);

                    if (parts[0].Length == 0)
                        continue;

                    var key = parts[0].Trim();

                    if (parts.Length == 1)
                        args[key] = string.Empty;
                    else
                        args[key] = Uri.UnescapeDataString(parts[1]);
                }
            }
        }

        #endregion
    }
}