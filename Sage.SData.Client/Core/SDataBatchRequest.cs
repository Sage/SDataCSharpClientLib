using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// SData Batch URL class
    /// </summary>
    /// <example>http://sdata/sageApp/test/-/salesOrders/$batch </example>
    public class SDataBatchRequest : SDataApplicationRequest, IDisposable
    {
        private const string BatchTerm = "$batch";

        public string TrackingId { get; set; }

        /// <summary>
        /// Queue of batch requests
        /// </summary>
        public Queue<SDataBatchRequestItem> Requests { get; set; }

        /// <summary>
        /// Feed for the batch request
        /// </summary>
        public AtomFeed Feed { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public SDataBatchRequest(ISDataService service)
            : base(service)
        {
            Requests = new Queue<SDataBatchRequestItem>();
            BatchProcess.Instance.CurrentStack.Push(this);
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
        public AsyncRequest CreateAsync(string trackingId)
        {
            TrackingId = trackingId;
            return Service.CreateAsync(this);
        }

        protected override void BuildUrl(UrlBuilder builder)
        {
            base.BuildUrl(builder);
            builder.PathSegments.Add(BatchTerm);

            if (!string.IsNullOrEmpty(TrackingId))
            {
                builder.QueryParameters["trackingId"] = TrackingId;
            }
        }

        /// <summary>
        /// Processes the batch
        /// </summary>
        public void Commit()
        {
            var feed = new AtomFeed
                       {
                           Title = new AtomTextConstruct("Batch"),
                           Id = new AtomId(new Uri(ToString())),
                           UpdatedOn = DateTime.Now
                       };

            var feedNav = feed.CreateNavigator();
            var feedDoc = new XmlDocument();
            feedDoc.LoadXml(feedNav.OuterXml);
            var attr = feedDoc.CreateAttribute("xmlns:http");
            attr.Value = "http://schemas.sage.com/sdata/2008/1";
            feedDoc.DocumentElement.Attributes.Append(attr);
            feedNav = feedDoc.CreateNavigator();

            foreach (var request in Requests)
            {
                var entry = new AtomEntry {Id = new AtomId(new Uri(request.Uri))};
                var verb = request.Verb;

                if (verb == "PUT" || verb == "POST")
                {
                    entry.Load(new MemoryStream(Encoding.UTF8.GetBytes(request.Body)));
                }

                var doc = new XmlDocument();
                doc.LoadXml(entry.CreateNavigator().InnerXml);

                var method = doc.CreateElement("http", "httpMethod", "http://schemas.sage.com/sdata/http/2008/1");
                method.AppendChild(doc.CreateTextNode(verb));
                doc.DocumentElement.AppendChild(method);

                if (!string.IsNullOrEmpty(request.IfMatch))
                {
                    var ifMatch = doc.CreateElement("http", "ifMatch", "http://schemas.sage.com/sdata/http/2008/1");
                    ifMatch.AppendChild(doc.CreateTextNode(request.IfMatch));
                    doc.DocumentElement.AppendChild(ifMatch);
                }

                feedNav.MoveToRoot();
                feedNav.MoveToFirstChild();
                feedNav.AppendChild(doc.CreateNavigator().OuterXml);
            }

            Feed = Service.CreateFeed(this, feedNav) as AtomFeed;
            Requests.Clear();
        }

        public void Dispose()
        {
            Requests.Clear();
        }
    }

    public class SDataBatchRequestItem
    {
        public string Uri { get; set; }
        public string Verb { get; set; }
        public string Body { get; set; }
        public string IfMatch { get; set; }
    }
}