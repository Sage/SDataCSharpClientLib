using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
#if NET3_5
using System.Linq;
#endif
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

        private string _phase;
        private string _phaseDetail;
        private decimal _progress;
        private int _elapsedSeconds;
        private int _remainingSeconds;
        private int _pollingMillSeconds;
        private string _trackingID;
        private string _trackingUrl;
        private ISDataService _service;

        private ISyndicationResource _response;
        //private XDocument _xmldoc;

        /// <summary>
        /// the service 
        /// </summary>
        public ISDataService Service
        {
            get { return _service; }
            set { _service = value; }
        }


        /// <summary>
        /// Current phase of the process
        /// </summary>
        public string Phase
        {
            get{ return _phase;}
            set { _phase = value; }
        }

        /// <summary>
        /// Description of the phase
        /// </summary>
        public string PhaseDetail
        {
            get { return _phaseDetail; }
            set { _phaseDetail = value;}
        }
        /// <summary>
        /// the amount of the process completed
        /// </summary>
        public decimal Progress
        {
            get { return _progress; }
            set { _progress = value; }
        }

        /// <summary>
        /// The amount of time in seconds that the process
        /// has been executing
        /// </summary>
        public int ElapsedSeconds
        {
            get { return _elapsedSeconds; }
            set{ _elapsedSeconds = value;}
        }

        /// <summary>
        /// the amountof time in seconds remaining for the 
        /// process to complete
        /// </summary>
        public int RemainingSeconds
        {
            get { return _remainingSeconds; }
            set{ _remainingSeconds = value;}
        }

        /// <summary>
        /// the amount of time in milli secs to poll the server
        /// for a response
        /// </summary>
        public int PollingMilliseconds
        {
            get { return _pollingMillSeconds; }
            set { _pollingMillSeconds = value; }
        }


        /// <summary>
        /// string version of the UUID for this ansych request
        /// </summary>
        public string TrackingID
        {
            get { return _trackingID; }
            set{ _trackingID = value;}
        }

        /// <summary>
        /// The response from the web request
        /// </summary>
       
        /*
        public XDocument XmlDoc
        {
            get { return _xmldoc; }
            set { _xmldoc = value; }
        }
        */
        /// <summary>
        /// The response from the asynchronous operatoin
        /// </summary>
        public ISyndicationResource Response
        {
            get { return _response; }
            set{ _response = value;}
        }

        /// <summary>
        /// string representing the url for the location header
        /// </summary>
        public string TrackingUrl
        {
            get { return _trackingUrl; }
            set{ _trackingUrl = value;}
        }

        /// <summary>
        /// refreshes the object, if the repsponse has been received
        /// <see cref="_response"/> it will be non null
        /// </summary>
        public void Refresh()
        {
            string xml = _service.Read(TrackingUrl);
            XmlDocument doc = new XmlDocument();
            doc.Load(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml)));

            XmlNode node = doc.SelectSingleNode("sdata:tracking");


            // we are still processing the request
            if(node != null)
            {
                this.Phase = doc.SelectSingleNode("sdata:phase").InnerXml;
                this.PhaseDetail = doc.SelectSingleNode("sdata:phaseDetail").InnerXml;
                this.Progress = Convert.ToDecimal(doc.SelectSingleNode("sdata:progress").InnerXml);
                this.ElapsedSeconds = Convert.ToInt32(doc.SelectSingleNode("sdata:elapsedSeconds").InnerXml);
                this.RemainingSeconds = Convert.ToInt32(doc.SelectSingleNode("sdata:remainingSeconds").InnerXml);
            }
            else
            {
                node = doc.SelectSingleNode("feed");
                if(node != null)
                {
                    // its a feed
                    AtomFeed feed = new AtomFeed();
                    feed.Load(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml)));
                    Response = feed;
                }
                else
                {
                    // its an entry
                    // we should be done
                    AtomEntry entry = new AtomEntry();
                    entry.Load(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml)));
                    Response = entry;
                }


                // now we have to delete this thing, the clean up from the SData Spec
                _service.Delete(this.TrackingUrl);
        
            }
        }
    }

}
