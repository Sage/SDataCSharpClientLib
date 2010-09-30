using System;
using System.Collections.Generic;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Extensions;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// SData Batch URL class
    /// </summary>
    /// <example>http://sdata/sageApp/test/-/salesOrders/$batch </example>
    public class SDataBatchRequest : SDataApplicationRequest, IDisposable
    {
        private const string BatchTerm = "$batch";

        private readonly IList<SDataBatchRequestItem> _items;

        /// <summary>
        /// Collection of batch request items
        /// </summary>
        public IList<SDataBatchRequestItem> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Collection of batch request items
        /// </summary>
        [Obsolete("Use the Items property instead.")]
        public IList<SDataBatchRequestItem> Requests
        {
            get { return _items; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public SDataBatchRequest(ISDataService service)
            : base(service)
        {
            _items = new List<SDataBatchRequestItem>();
            BatchProcess.Instance.Requests.Add(this);
        }

        /// <summary>
        /// Processes the request asynchronously
        /// </summary>
        /// <returns>AsyncRequest object to manage the transaction</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataBatchRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="CREATE a BATCH Operation (Asynchronous)" 
        ///         />
        ///     </code>
        /// </example>
        public AsyncRequest CreateAsync()
        {
            var feed = GetFeed();
            return Service.CreateAsync(this, feed);
        }

        /// <summary>
        /// Processes the request asynchronously
        /// </summary>
        /// <param name="trackingId">unique identifier for the async transaction</param>
        /// <returns>AsyncRequest object to manage the transaction</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataBatchRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="CREATE a BATCH Operation (Asynchronous)" 
        ///         />
        ///     </code>
        /// </example>
        [Obsolete("Use the parameterless CreateAsync method instead, which automatically handles the tracking ID internally.")]
        public AsyncRequest CreateAsync(string trackingId)
        {
            return CreateAsync();
        }

        protected override void BuildUrl(SDataUri uri)
        {
            base.BuildUrl(uri);
            uri.AppendPath(BatchTerm);
        }

        /// <summary>
        /// Processes the batch
        /// </summary>
        public AtomFeed Commit()
        {
            var feed = GetFeed();
            feed = Service.CreateFeed(this, feed);
            Items.Clear();
            return feed;
        }

        public void Dispose()
        {
            Items.Clear();
            BatchProcess.Instance.Requests.Remove(this);
        }

        private AtomFeed GetFeed()
        {
            var feed = new AtomFeed
                       {
                           Title = new AtomTextConstruct("Batch"),
                           Id = new AtomId(new Uri(ToString())),
                           UpdatedOn = DateTime.Now
                       };

            foreach (var item in Items)
            {
                var entry = item.Entry ?? new AtomEntry {Id = new AtomId(new Uri(item.Url))};
                entry.SetSDataHttpMethod(item.Method);
                entry.SetSDataHttpIfMatch(item.ETag);
                feed.AddEntry(entry);
            }

            return feed;
        }
    }

    /// <summary>
    /// SData batch request item class.
    /// </summary>
    public class SDataBatchRequestItem
    {
        /// <summary>
        /// The url of the request item.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The HTTP method of the request item.
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// The input entry of the request item.
        /// </summary>
        public AtomEntry Entry { get; set; }

        /// <summary>
        /// The ETag of the request item.
        /// </summary>
        public string ETag { get; set; }
    }
}