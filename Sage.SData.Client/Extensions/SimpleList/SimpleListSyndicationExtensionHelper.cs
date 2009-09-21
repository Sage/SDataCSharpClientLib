using System.Collections.ObjectModel;
using System.Linq;
using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// SLE extension class
    /// </summary>
    public static class SimpleListSyndicationExtensionHelper
    {
        /// <summary>
        /// Gets information that allows the client to group or filter on the values of feed properties.
        /// </summary>
        /// <value>
        ///     A <see cref="Collection{T}"/> collection of <see cref="SimpleListGroup"/> objects that represent information that allows the client to group or filter on the values of feed properties. 
        ///     The default value is an <i>empty</i> collection.
        /// </value>
        public static Collection<SimpleListGroup> GetSimpleListGrouping(this AtomFeed feed)
        {
            var context = GetContext(feed, true);
            return context.Grouping;
        }

        /// <summary>
        /// Gets information that allows the client to sort on the values of feed properties.
        /// </summary>
        /// <value>
        ///     A <see cref="Collection{T}"/> collection of <see cref="SimpleListSort"/> objects that represent information that allows the client to sort on the values of feed properties. 
        ///     The default value is an <i>empty</i> collection.
        /// </value>
        public static Collection<SimpleListSort> GetSimpleListSorting(this AtomFeed feed)
        {
            var context = GetContext(feed, true);
            return context.Sorting;
        }

        /// <summary>
        /// Gets a value indicating if this feed is intended to be consumed as a list.
        /// </summary>
        /// <value><b>true</b> if the syndication feed is intended to be consumed as a list; otherwise false.</value>
        /// <remarks>
        ///     This property allows the publisher of a feed document to indicate to the consumers of the feed that the feed is intended to be consumed as a list, 
        ///     and as such is the primary means for feed consumers to identify lists.
        /// </remarks>
        public static bool? GetSimpleListTreatAsList(this AtomFeed feed)
        {
            var context = GetContext(feed, false);
            return context != null ? context.TreatAsList : (bool?) null;
        }

        /// <summary>
        /// Sets a value indicating if this feed is intended to be consumed as a list.
        /// </summary>
        /// <value><b>true</b> if the syndication feed is intended to be consumed as a list; otherwise false.</value>
        /// <remarks>
        ///     This property allows the publisher of a feed document to indicate to the consumers of the feed that the feed is intended to be consumed as a list, 
        ///     and as such is the primary means for feed consumers to identify lists.
        /// </remarks>
        public static void SetSimpleListTreatAsList(this AtomFeed feed, bool value)
        {
            var context = GetContext(feed, true);
            context.TreatAsList = value;
        }

        private static SimpleListSyndicationExtensionContext GetContext(IExtensibleSyndicationObject entry, bool createIfMissing)
        {
            var extension = entry.Extensions.OfType<SimpleListSyndicationExtension>().FirstOrDefault();

            if (extension == null)
            {
                if (!createIfMissing)
                {
                    return null;
                }

                extension = new SimpleListSyndicationExtension();
                entry.AddExtension(extension);
            }

            return extension.Context;
        }
    }
}