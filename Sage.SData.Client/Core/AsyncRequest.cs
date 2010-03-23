using Sage.SData.Client.Common;
using Sage.SData.Client.Framework;

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
        private readonly ISDataService _service;
        private readonly string _trackingUrl;
        private SDataTracking _tracking;

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="service"></param>
        /// <param name="trackingUrl"></param>
        /// <param name="tracking"></param>
        public AsyncRequest(ISDataService service, string trackingUrl, SDataTracking tracking)
        {
            Guard.ArgumentNotNull(service, "service");
            Guard.ArgumentNotNull(trackingUrl, "trackingUrl");
            Guard.ArgumentNotNull(tracking, "tracking");

            _service = service;
            _trackingUrl = trackingUrl;
            _tracking = tracking;
        }

        /// <summary>
        /// the service 
        /// </summary>
        public ISDataService Service
        {
            get { return _service; }
        }

        /// <summary>
        /// Current phase of the process
        /// </summary>
        public string Phase
        {
            get { return _tracking.Phase; }
        }

        /// <summary>
        /// Description of the phase
        /// </summary>
        public string PhaseDetail
        {
            get { return _tracking.PhaseDetail; }
        }

        /// <summary>
        /// the amount of the process completed
        /// </summary>
        public decimal Progress
        {
            get { return _tracking.Progress; }
        }

        /// <summary>
        /// The amount of time in seconds that the process
        /// has been executing
        /// </summary>
        public int ElapsedSeconds
        {
            get { return _tracking.ElapsedSeconds; }
        }

        /// <summary>
        /// the amountof time in seconds remaining for the 
        /// process to complete
        /// </summary>
        public int RemainingSeconds
        {
            get { return _tracking.RemainingSeconds; }
        }

        /// <summary>
        /// the amount of time in milli secs to poll the server
        /// for a response
        /// </summary>
        public int PollingMilliseconds
        {
            get { return _tracking.PollingMillis; }
        }

        /// <summary>
        /// string representing the url for the location header
        /// </summary>
        public string TrackingUrl
        {
            get { return _trackingUrl; }
        }

        /// <summary>
        /// refreshes the object, if the repsponse has been received
        /// <see cref="ISyndicationResource"/> it will be non null
        /// </summary>
        public ISyndicationResource Refresh()
        {
            var content = Service.Read(_trackingUrl);
            var tracking = content as SDataTracking;

            if (tracking != null)
            {
                _tracking = tracking;
                return null;
            }

            // now we have to delete this thing, the clean up from the SData Spec
            Service.Delete(_trackingUrl);

            return (ISyndicationResource) content;
        }
    }
}