using System.Collections.Generic;
using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// SData URL for a resource collection
    /// </summary>
    public class SDataResourceCollectionRequest : SDataApplicationRequest
    {
        /// <summary>
        /// Accessor method for query can be null
        /// </summary>
        /// <remarks>
        /// string used to filter data
        /// </remarks>
        /// <example>startIndex=21&amp;count=10
        ///</example>
        public string Query { get; set; }

        /// <summary>
        ///  Dictionary of query name value pairs
        /// </summary>
        /// <example>where, salesorderamount lt 15.00
        /// orderby, salesorderid
        /// </example>
        public IDictionary<string, string> QueryValues { get; set; }

        /// <summary>
        /// Indicates the index of the first resource returned by the query. This index is 1-based (not 0-based).
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// Indicates the number of resources requested by the service consumer.
        /// The service may choose to return a different number of resources and it
        /// indicates this by setting the itemsPerPage element in the returned feed.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// AtomFeed Reader for the request
        /// </summary>
        public AtomFeedReader Reader { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public SDataResourceCollectionRequest(ISDataService service)
            : base(service)
        {
            Count = -1;
            StartIndex = -1;
            QueryValues = new Dictionary<string, string>();
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
        /// <param name="feed">the feed</param>
        /// <returns></returns>
        public bool Read(AtomFeed feed)
        {
            if (Reader == null)
            {
                Reader = new AtomFeedReader(Service, feed, this);
            }
            return Reader.Read();
        }

        protected override void BuildUrl(UrlBuilder builder)
        {
            base.BuildUrl(builder);

            if (Query != null)
            {
                builder.Query = Query;
            }

            foreach (var pair in QueryValues)
            {
                builder.QueryParameters[pair.Key] = pair.Value;
            }

            if (Count > -1)
            {
                builder.QueryParameters["count"] = Count.ToString();
            }

            if (StartIndex > -1)
            {
                builder.QueryParameters["startIndex"] = StartIndex.ToString();
            }
        }
    }
}