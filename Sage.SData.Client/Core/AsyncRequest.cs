using System;
using System.IO;
using System.Text;
using System.Xml;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Represents the return value from a an asychronous call for a service operation or batch operation
    /// </summary>
    /// <example>
    ///     <code lang="cs" title="The following code example demonstrates the usage of the AsyncRequest class.">
    ///         <code 
    ///             source=".\Example.cs" 
    ///             region="CREATE a Service Operation (Asynchronous)" 
    ///         />
    ///     </code>
    /// </example>
    public class AsyncRequest
    {
        /// <summary>
        /// the service 
        /// </summary>
        public ISDataService Service { get; set; }

        /// <summary>
        /// Current phase of the process
        /// </summary>
        public string Phase { get; set; }

        /// <summary>
        /// Description of the phase
        /// </summary>
        public string PhaseDetail { get; set; }

        /// <summary>
        /// the amount of the process completed
        /// </summary>
        public decimal Progress { get; set; }

        /// <summary>
        /// The amount of time in seconds that the process
        /// has been executing
        /// </summary>
        public int ElapsedSeconds { get; set; }

        /// <summary>
        /// the amountof time in seconds remaining for the 
        /// process to complete
        /// </summary>
        public int RemainingSeconds { get; set; }

        /// <summary>
        /// the amount of time in milli secs to poll the server
        /// for a response
        /// </summary>
        public int PollingMilliseconds { get; set; }

        /// <summary>
        /// The response from the asynchronous operatoin
        /// </summary>
        public ISyndicationResource Response { get; set; }

        /// <summary>
        /// string representing the url for the location header
        /// </summary>
        public string TrackingUrl { get; set; }

        /// <summary>
        /// refreshes the object, if the repsponse has been received
        /// <see cref="Response"/> it will be non null
        /// </summary>
        public void Refresh()
        {
            var xml = Service.Read(TrackingUrl);
            var doc = new XmlDocument();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                doc.Load(stream);
            }

            var node = doc.SelectSingleNode("sdata:tracking");

            // we are still processing the request
            if (node != null)
            {
                Phase = doc.SelectSingleNode("sdata:phase").InnerXml;
                PhaseDetail = doc.SelectSingleNode("sdata:phaseDetail").InnerXml;
                Progress = Convert.ToDecimal(doc.SelectSingleNode("sdata:progress").InnerXml);
                ElapsedSeconds = Convert.ToInt32(doc.SelectSingleNode("sdata:elapsedSeconds").InnerXml);
                RemainingSeconds = Convert.ToInt32(doc.SelectSingleNode("sdata:remainingSeconds").InnerXml);
            }
            else
            {
                node = doc.SelectSingleNode("feed");
                if (node != null)
                {
                    // its a feed
                    var feed = new AtomFeed();
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                    {
                        feed.Load(stream);
                    }
                    Response = feed;
                }
                else
                {
                    // its an entry, we should be done
                    var entry = new AtomEntry();
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                    {
                        entry.Load(stream);
                    }
                    Response = entry;
                }

                // now we have to delete this thing, the clean up from the SData Spec
                Service.Delete(TrackingUrl);
            }
        }
    }
}