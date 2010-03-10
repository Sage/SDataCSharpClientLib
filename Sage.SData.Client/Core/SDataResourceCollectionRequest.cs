using System;
using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// SData URL for a resource collection
    /// </summary>
    public class SDataResourceCollectionRequest : SDataApplicationRequest
    {
        /// <summary>
        /// Indicates the index of the first resource returned by the query. This index is 1-based (not 0-based).
        /// </summary>
        public long? StartIndex
        {
            get { return Uri.StartIndex; }
            set { Uri.StartIndex = value; }
        }

        /// <summary>
        /// Indicates the number of resources requested by the service consumer.
        /// The service may choose to return a different number of resources and it
        /// indicates this by setting the itemsPerPage element in the returned feed.
        /// </summary>
        public long? Count
        {
            get { return Uri.Count; }
            set { Uri.Count = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public SDataResourceCollectionRequest(ISDataService service)
            : base(service)
        {
        }

        /// <summary>
        /// Reads the AtomFeed for a resource collection
        /// </summary>
        /// <returns>AtomFeed</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataResourceCollectionRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ a Resource Collection Feed" 
        ///         />
        ///     </code>
        /// </example>
        public AtomFeed Read()
        {
            return Service.ReadFeed(this);
        }

        /// <summary>
        /// Performs initial read from AtomFeedReader
        /// </summary>
        /// <returns></returns>
        public AtomFeedReader ExecuteReader()
        {
            var reader = new AtomFeedReader(Service, this);
            return reader.Read() ? reader : null;
        }

        /// <summary>
        /// Performs initial read from AtomFeedReader
        /// </summary>
        /// <param name="feed">the feed</param>
        /// <returns></returns>
        [Obsolete("Use the ExecuteReader method instead, which automatically fetches the feed's first page.")]
        public bool Read(AtomFeed feed)
        {
            return ExecuteReader() != null;
        }
    }
}