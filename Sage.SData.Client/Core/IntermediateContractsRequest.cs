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
        /// <summary>
        /// Accessor method for application
        /// </summary>
        /// <remarks>the application name</remarks>
        public string ApplicationName
        {
            get { return Uri.Product; }
            set { Uri.Product = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public IntermediateContractsRequest(ISDataService service)
            : base(service)
        {
            ApplicationName = !string.IsNullOrEmpty(service.ApplicationName) ? service.ApplicationName : "-";
        }

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
        public override AtomFeed Read()
        {
            return Service.ReadFeed(this);
        }
    }
}