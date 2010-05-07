using System;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Service Operation URL
    /// </summary>
    public class SDataServiceOperationRequest : SDataApplicationRequest
    {
        private const string ServiceTerm = "$service";

        /// <summary>
        /// Accessor method for operationName
        /// </summary>
        /// <remarks>
        /// This element identifies a service operation. The operation must be invoked by posting an Atom entry containing the parameters to this URL. 
        /// The SData service will return the computed prices as an Atom entry containing the result (see the description of Synchronous Service Operations for details).
        /// In the example above, the input and output are Atom entries, but SData also supports Atom feeds as input and/or output to service operations 
        /// (see Service Operations section for details).
        /// The $service component may be placed after the contract segment (after test in the example above), 
        /// if it applies globally to the entire contract or it may be placed after the resource kind segment (as in the example above), if it applies to resources of a specific kind.
        /// </remarks>
        public string OperationName { get; set; }

        /// <summary>
        /// The input entry containing parameter values to be submitted to the service.
        /// </summary>
        public AtomEntry Entry { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public SDataServiceOperationRequest(ISDataService service)
            : base(service)
        {
        }

        /// <summary>
        /// Creates POST to the server
        /// </summary>
        /// <returns>AtomFeed returned from the server</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataServiceOperationRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="CREATE a Service Operation (Synchronous)" 
        ///         />
        ///     </code>
        /// </example>
        public AtomEntry Create()
        {
            if (Entry == null)
            {
                throw new InvalidOperationException("Request entry must be specified");
            }

            return Service.CreateEntry(this, Entry);
        }

        /// <summary>
        /// Execute the service operation asynchronously.
        /// </summary>
        /// <returns></returns>
        public AsyncRequest CreateAsync()
        {
            return Service.CreateAsync(this, Entry);
        }

        protected override void BuildUrl(SDataUri uri)
        {
            base.BuildUrl(uri);
            uri.AppendPath(ServiceTerm);

            if (!string.IsNullOrEmpty(OperationName))
            {
                uri.AppendPath(OperationName);
            }
        }
    }
}