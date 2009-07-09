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
        private const string _keyword = "$service";

        private string _resourceKind;

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
        public string ResourceKind
        {
            get { return _resourceKind; }
            set { _resourceKind = value; }
        }

        /// <summary>
        /// Reads the AtomFeed for enumeration services
        /// </summary>
        /// <returns>AtomFeed</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the IntermediateServicesRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ Enumeration of Services" 
        ///         />
        ///     </code>
        /// </example>
        //public AtomFeed Read()
        //{
        //    try
        //    {
        //        return Service.ReadFeed(this);
        //    }
        //    catch (Exception e)
        //    {
        //        
        //        throw e;
        //    }
        //}
        public IntermediateServicesRequest(ISDataService service)
            : base(service) {}

        protected override void BuildUrl(UrlBuilder builder)
        {
            base.BuildUrl(builder);

            if (!string.IsNullOrEmpty(ResourceKind))
            {
                builder.PathSegments.Add(ResourceKind);
            }

            builder.PathSegments.Add(_keyword);
        }
    }
}