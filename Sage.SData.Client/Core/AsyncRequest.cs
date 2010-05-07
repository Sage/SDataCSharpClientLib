using Sage.SData.Client.Common;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Encapsulates the return value of an asychronous call to a service operation or a batch operation.
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
        private Tracking _tracking;

        /// <summary>
        /// Initialises a new instance of the <see cref="AsyncRequest"/> class.
        /// </summary>
        /// <param name="service">The service that performs requests.</param>
        /// <param name="trackingUrl">The url used to make progress requests.</param>
        /// <param name="tracking">The tracking information from the initial request.</param>
        public AsyncRequest(ISDataService service, string trackingUrl, Tracking tracking)
        {
            Guard.ArgumentNotNull(service, "service");
            Guard.ArgumentNotNull(trackingUrl, "trackingUrl");
            Guard.ArgumentNotNull(tracking, "tracking");

            _service = service;
            _trackingUrl = trackingUrl;
            _tracking = tracking;
        }

        /// <summary>
        /// The service that performs requests.
        /// </summary>
        public ISDataService Service
        {
            get { return _service; }
        }

        /// <summary>
        /// The current phase of the process.
        /// </summary>
        public string Phase
        {
            get { return _tracking.Phase; }
        }

        /// <summary>
        /// The description of the current phase.
        /// </summary>
        public string PhaseDetail
        {
            get { return _tracking.PhaseDetail; }
        }

        /// <summary>
        /// The percentage of the process that has been completed so far.
        /// </summary>
        public decimal Progress
        {
            get { return _tracking.Progress; }
        }

        /// <summary>
        /// The amount of time in seconds that the process has been running.
        /// </summary>
        public int ElapsedSeconds
        {
            get { return _tracking.ElapsedSeconds; }
        }

        /// <summary>
        /// The amount of time in seconds that the process has remaining.
        /// </summary>
        public int RemainingSeconds
        {
            get { return _tracking.RemainingSeconds; }
        }

        /// <summary>
        /// The amount of time in milliseconds that clients should wait between progress requests.
        /// </summary>
        public int PollingMilliseconds
        {
            get { return _tracking.PollingMillis; }
        }

        /// <summary>
        /// A url representing the location where progress requests can be made.
        /// </summary>
        public string TrackingUrl
        {
            get { return _trackingUrl; }
        }

        /// <summary>
        /// Makes a progress update request and refreshes the various progress properties.
        /// If the process has completed on the server then a <see cref="ISyndicationResource"/>
        /// result will be returned. Otherwise a null reference is returned.
        /// </summary>
        public ISyndicationResource Refresh()
        {
            var content = Service.Read(_trackingUrl);
            var tracking = content as Tracking;

            if (tracking != null)
            {
                _tracking = tracking;
                return null;
            }

            Service.Delete(_trackingUrl);

            return (ISyndicationResource) content;
        }
    }
}