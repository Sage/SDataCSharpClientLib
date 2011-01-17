using System.Collections.ObjectModel;
using System.Linq;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;
using Sage.SData.Client.Framework;
using Sage.SData.Client.Metadata;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// SDataExtension Helpers Extension Methods
    /// </summary>
    public static class SDataExtensionHelper
    {
        /// <summary>
        /// Extension method to retrieve sdata payload
        /// </summary>
        /// <param name="entry">the entry</param>
        /// <returns></returns>
        public static SDataPayload GetSDataPayload(this AtomEntry entry)
        {
            var context = GetContext(entry, false);
            return context != null ? context.Payload : null;
        }

        /// <summary>
        /// Retrieves diagnosis from the feed
        /// </summary>
        /// <param name="feed">the AtomFeed</param>
        /// <returns></returns>
        public static Collection<Diagnosis> GetSDataDiagnoses(this AtomFeed feed)
        {
            var context = GetContext(feed, true);
            return context != null ? context.Diagnoses : null;
        }

        /// <summary>
        /// Retrieves the diagnosis from an entry
        /// </summary>
        /// <param name="entry">the AtomEntry</param>
        /// <returns></returns>
        public static Collection<Diagnosis> GetSDataDiagnoses(this AtomEntry entry)
        {
            var context = GetContext(entry, true);
            return context != null ? context.Diagnoses : null;
        }

        /// <summary>
        /// Retrieves the inline XML schema embedded in the feed
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public static SDataSchema GetSDataSchema(this AtomFeed feed)
        {
            var context = GetContext(feed, true);
            return context != null ? context.Schema : null;
        }

        /// <summary>
        /// Retrieves the inline XML schema embedded in the feed entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static SDataSchema GetSDataSchema(this AtomEntry entry)
        {
            var context = GetContext(entry, true);
            return context != null ? context.Schema : null;
        }

        /// <summary>
        /// Extension method to set sdata payload
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="payload"></param>
        public static void SetSDataPayload(this AtomEntry entry, SDataPayload payload)
        {
            var context = GetContext(entry, true);
            context.Payload = payload;
        }

        private static SDataExtensionContext GetContext(IExtensibleSyndicationObject entry, bool createIfMissing)
        {
            Guard.ArgumentNotNull(entry, "entry");
            var extension = entry.Extensions.OfType<SDataExtension>().FirstOrDefault();

            if (extension == null)
            {
                if (!createIfMissing)
                {
                    return null;
                }

                extension = new SDataExtension();
                entry.AddExtension(extension);
            }

            return extension.Context;
        }
    }
}