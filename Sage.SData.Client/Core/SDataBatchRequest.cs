using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// SData Batch URL class
    /// </summary>
    /// <example>http://sdata/sageApp/test/-/salesOrders/$batch </example>
    public class SDataBatchRequest : SDataApplicationRequest, IDisposable
    {
        private static string _batch = "$batch";
        private Queue _requests;

        /// <summary>
        /// Queue of batch requests
        /// </summary>
        public Queue Requests
        {
            get { return _requests; }
            set { _requests = value; }
        }

        private AtomFeed _feed;

        /// <summary>
        /// Feed for the batch request
        /// </summary>
        public AtomFeed Feed
        {
            get { return _feed; }
            set { _feed = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public SDataBatchRequest(ISDataService service)
            : base(service)
        {
            _requests = new Queue();
            BatchProcess.Instance.CurrentStack.Push(this);
        }

        /// <summary>
        /// Processes the request asynchronously
        /// </summary>
        /// <param name="uuid">unique identifier for the asynch transaction</param>
        /// <returns>AsyncRequest object to manage the transaction</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataBatchRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="CREATE a BATCH Operation (Asynchronous)" 
        ///         />
        ///     </code>
        /// </example>
        public AsyncRequest CreateAsync(string uuid)
        {
            throw new NotImplementedException();
        }

        protected override void BuildUrl(UrlBuilder builder)
        {
            base.BuildUrl(builder);
            builder.PathSegments.Add(_batch);
        }

        /// <summary>
        /// Processes the batch
        /// </summary>
        public void Dispose()
        {
            AtomFeed feed = new AtomFeed();
            feed.Title = new AtomTextConstruct("Batch");
            feed.Id = new AtomId(new Uri(ToString()));
            feed.UpdatedOn = DateTime.Now;

            XPathNavigator feedNav = feed.CreateNavigator();
            XmlDocument feedDoc = new XmlDocument();
            feedDoc.LoadXml(feedNav.OuterXml);
            XmlAttribute attr = feedDoc.CreateAttribute("xmlns:http");
            attr.Value = "http://schemas.sage.com/sdata/2008/1";
            feedDoc.DocumentElement.Attributes.Append(attr);
            feedNav = feedDoc.CreateNavigator();

            foreach (string[] request in Requests)
            {
                AtomEntry entry = new AtomEntry();
                entry.Id = new AtomId(new Uri(request[0]));
                string verb = request[1];

                if (verb == "PUT" || verb == "POST")
                {
                    entry.Load(new MemoryStream(Encoding.UTF8.GetBytes(request[2])));
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(entry.CreateNavigator().InnerXml);

                XmlElement method = doc.CreateElement("http", "httpMethod", "http://schemas.sage.com/sdata/http/2008/1");
                method.AppendChild(doc.CreateTextNode(verb));
                doc.DocumentElement.AppendChild(method);
                XPathNavigator navigator = doc.CreateNavigator();

                feedNav.MoveToRoot();
                feedNav.MoveToFirstChild();
                feedNav.AppendChild(navigator.OuterXml);
            }

            _feed = Service.CreateFeed(this, feedNav) as AtomFeed;
        }
    }
}