using System.Collections.ObjectModel;
#if NET3_5
using System.Linq;
#endif
using System.Xml.XPath;
using Sage.SData.Client.Atom;

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
        public static XPathNavigator GetSDataPayload(this AtomEntry entry)
        {
            var context = GetContext(entry, false);
            return context != null ? context.Payload : null;
        }

        /// <summary>
        /// Retrieves diagnosis from the feed
        /// </summary>
        /// <param name="feed">the AtomFeed</param>
        /// <returns></returns>
        public static Collection<SDataDiagnosis> GetSDataDiagnoses(this AtomFeed feed)
        {
            var context = GetContext(feed, true);
            return context != null ? context.Diagnoses : null;
        }

        /// <summary>
        /// Retrieves the diagnosis from an entry
        /// </summary>
        /// <param name="entry">the AtomEntry</param>
        /// <returns></returns>
        public static Collection<SDataDiagnosis> GetSDataDiagnoses(this AtomEntry entry)
        {
            var context = GetContext(entry, true);
            return context != null ? context.Diagnoses : null;
        }

        /// <summary>
        /// Extension method to set sdata payload
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="payload"></param>
        public static void SetSDataPayload(this AtomEntry entry, XPathNavigator payload)
        {
            var context = GetContext(entry, true);
            context.Payload = payload;
        }

        private static SDataExtensionContext GetContext(IExtensibleSyndicationObject entry, bool createIfMissing)
        {
            SDataExtension extension = null;
#if NET3_5
            extension = entry.Extensions.OfType<SDataExtension>().FirstOrDefault();
#else
            foreach (ISyndicationExtension item in entry.Extensions)
            {
                if(item is SDataExtension)
                {
                    extension = item as SDataExtension;
                    break;
                }
            }
#endif
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