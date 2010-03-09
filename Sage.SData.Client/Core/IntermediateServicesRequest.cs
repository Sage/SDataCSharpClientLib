using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Intermediate URL to retrieve enumeration of services
    /// from the specified contract or contract and resource kind
    /// </summary>
    /// <example>
    /// http://sdata.acme.com/sdata/sageApp/test/prod/$service
    /// http://sdata.acme.com/sdata/sageApp/test/prod/accounts/$service
    /// </example>
    /// <remarks>Feed level category = service field level category = operation</remarks>
    public class IntermediateServicesRequest : IntermediateResourceCollectionsRequest
    {
        private const string ServiceTerm = "$service";

        /// <summary>
        /// Accessor method for resourceKind
        /// </summary>
        /// <remarks>
        /// This URL segment identifies the kind of resource that is queried (account, contact, salesOrder, etc.)
        /// This URL returns the collection of all account resources, as an Atom feed. 
        /// If the contract exposes a large number of resources kinds, a functional group can be inserted before the 
        /// resource kind segment (eventually a hierarchy of functional groups), which act as a folder (a hierarchy of folders) to organize the resources.
        /// Typical functional groups would be financials, commercials, HR, etc.
        /// NOTE Can be empty
        /// </remarks>
        public string ResourceKind { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="service"></param>
        public IntermediateServicesRequest(ISDataService service)
            : base(service)
        {
        }

        protected override void BuildUrl(SDataUri uri)
        {
            base.BuildUrl(uri);

            if (!string.IsNullOrEmpty(ResourceKind))
            {
                uri.AppendPath(ResourceKind);
            }

            uri.AppendPath(ServiceTerm);
        }
    }
}