using System.Linq;
using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Open Search Extensions
    /// </summary>
    public static class OpenSearchExtensionHelper
    {
        /// <summary>
        /// Extension method to retrieve opensearch items per page
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public static int? GetOpenSearchItemsPerPage(this AtomFeed feed)
        {
            var context = GetContext(feed, false);
            return context != null ? context.ItemsPerPage : null;
        }

        /// <summary>
        /// Extension method to retrieve opensearch total results
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public static int? GetOpenSearchTotalResults(this AtomFeed feed)
        {
            var context = GetContext(feed, false);
            return context != null ? context.TotalResults : null;
        }

        /// <summary>
        /// Extension method to retrieve opensearch start index
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public static int? GetOpenSearchStartIndex(this AtomFeed feed)
        {
            var context = GetContext(feed, false);
            return context != null ? context.StartIndex : null;
        }

        /// <summary>
        /// Extension method to set opensearch items per page
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        public static void SetOpenSearchItemsPerPage(this AtomEntry entry, int value)
        {
            var context = GetContext(entry, true);
            context.ItemsPerPage = value;
        }

        /// <summary>
        /// Extension method to set opensearch total results
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        public static void SetOpenSearchTotalResults(this AtomEntry entry, int value)
        {
            var context = GetContext(entry, true);
            context.TotalResults = value;
        }

        /// <summary>
        /// Extension method to set opensearch start index
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="value"></param>
        public static void SetOpenSearchStartIndex(this AtomEntry entry, int value)
        {
            var context = GetContext(entry, true);
            context.StartIndex = value;
        }

        private static OpenSearchExtensionContext GetContext(IExtensibleSyndicationObject entry, bool createIfMissing)
        {
            var extension = entry.Extensions.OfType<OpenSearchExtension>().FirstOrDefault();

            if (extension == null)
            {
                if (!createIfMissing)
                {
                    return null;
                }

                extension = new OpenSearchExtension();
                entry.AddExtension(extension);
            }

            return extension.Context;
        }
    }
}