using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// SData request for an endpoint collection
    /// </summary>
    public class SDataEndPointCollectionRequest : SDataBaseRequest
    {
        private AtomFeedReader _reader;

        /// <summary>
        /// AtomFeed Reader for the request
        /// </summary>
        public AtomFeedReader Reader
        {
            get { return _reader; }
            set { _reader = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public SDataEndPointCollectionRequest(ISDataService service)
            : base(service) {}

        /// <summary>
        /// Reads the AtomFeed for a resource collection
        /// </summary>
        /// <returns>AtomFeed</returns>
        public AtomFeed Read()
        {
            return Service.ReadFeed(this);
        }

        protected override void BuildUrl(UrlBuilder builder)
        {
            base.BuildUrl(builder);

            builder.PathSegments.Add("$system");
            builder.PathSegments.Add("registry");
            builder.PathSegments.Add("endpoints");
        }
    }
}