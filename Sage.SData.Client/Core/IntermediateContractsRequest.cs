using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Intermediate request to retrieve enumeration of contracts
    /// from the specified application and dataset
    /// </summary>
    /// <example>http://sdata.acme.com/sdata/sageApp</example>
    /// <remarks>Feed level category = application Entry level category = contract</remarks>
    public class IntermediateContractsRequest : IntermediateApplicationsRequest
    {
        private string _application;

        /// <summary>
        /// Accessor method for application
        /// </summary>
        /// <remarks>the application name</remarks>
        public string Application
        {
            get { return _application; }
            set { _application = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public IntermediateContractsRequest(ISDataService service)
            : base(service) {}

        /// <summary>
        /// Reads the AtomFeed for enumeration of contracts
        /// </summary>
        /// <returns>AtomFeed</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the IntermediateContractsRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ Enumeration of Contracts" 
        ///         />
        ///     </code>
        /// </example>
        public new AtomFeed Read()
        {
            return Service.ReadFeed(this);
        }

        protected override void BuildUrl(UrlBuilder builder)
        {
            base.BuildUrl(builder);

            if (string.IsNullOrEmpty(Application))
            {
                Application = Service.ApplicationName;
            }

            builder.PathSegments.Add(Application);
        }
    }
}