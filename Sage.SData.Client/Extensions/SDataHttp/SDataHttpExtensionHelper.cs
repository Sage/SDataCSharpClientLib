using System;
using System.Linq;
using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Helper class for accessing AtomEntry SDataHttpExtensions
    /// </summary>
    public static class SDataHttpExtensionHelper
    {
        /// <summary>
        /// Extension method to retrieve sdata http method
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static SDataHttpMethod? GetSDataHttpMethod(this AtomEntry entry)
        {
            var context = GetContext(entry, false);
            return context != null ? context.HttpMethod : null;
        }

        /// <summary>
        /// Extension method to retrieve sdata http status
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static int? GetSDataHttpStatus(this AtomEntry entry)
        {
            var context = GetContext(entry, false);
            return context != null ? context.HttpStatus : null;
        }

        /// <summary>
        /// Extension method to retrieve sdata http message
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetSDataHttpMessage(this AtomEntry entry)
        {
            var context = GetContext(entry, false);
            return context != null ? context.HttpMessage : null;
        }

        /// <summary>
        /// Extension method to retrieve sdata http location
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static Uri GetSDataHttpLocation(this AtomEntry entry)
        {
            var context = GetContext(entry, false);
            return context != null ? context.Location : null;
        }

        /// <summary>
        /// Extension method to retrieve sdata http etag
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetSDataHttpETag(this AtomEntry entry)
        {
            var context = GetContext(entry, false);
            return context != null ? context.ETag : null;
        }

        /// <summary>
        /// Extension method to retrieve sdata http if match
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetSDataHttpIfMatch(this AtomEntry entry)
        {
            var context = GetContext(entry, false);
            return context != null ? context.IfMatch : null;
        }

        /// <summary>
        /// Extension method to set sdata http method
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        public static void SetSDataHttpMethod(this AtomEntry entry, SDataHttpMethod? value)
        {
            GetContext(entry, true).HttpMethod = value;
        }

        /// <summary>
        /// Extension method to set sdata http status
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        public static void SetSDataHttpStatus(this AtomEntry entry, int? value)
        {
            GetContext(entry, true).HttpStatus = value;
        }

        /// <summary>
        /// Extension method to set sdata http message
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        public static void SetSDataHttpMessage(this AtomEntry entry, string value)
        {
            GetContext(entry, true).HttpMessage = value;
        }

        /// <summary>
        /// Extension method to set sdata http location
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        public static void SetSDataHttpLocation(this AtomEntry entry, Uri value)
        {
            GetContext(entry, true).Location = value;
        }

        /// <summary>
        /// Extension method to set sdata http etag
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        public static void SetSDataHttpETag(this AtomEntry entry, string value)
        {
            GetContext(entry, true).ETag = value;
        }

        /// <summary>
        /// Extension method to set sdata http if method
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        public static void SetSDataHttpIfMatch(this AtomEntry entry, string value)
        {
            GetContext(entry, true).IfMatch = value;
        }

        private static SDataHttpExtensionContext GetContext(IExtensibleSyndicationObject entry, bool createIfMissing)
        {
            var extension = entry.Extensions.OfType<SDataHttpExtension>().FirstOrDefault();

            if (extension == null)
            {
                if (!createIfMissing)
                {
                    return null;
                }

                extension = new SDataHttpExtension();
                entry.AddExtension(extension);
            }

            return extension.Context;
        }
    }
}