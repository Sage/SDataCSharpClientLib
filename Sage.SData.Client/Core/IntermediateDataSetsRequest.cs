using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Intermediate URL to retrieve enumeration of datasets
    /// from the specified application
    /// </summary>
    /// <example>http://sdata.acme.com/sdata/sageApp/test</example>
    /// <remarks>Feed level category = contract Entry level category = dataset</remarks>
    public class IntermediateDataSetsRequest : IntermediateContractsRequest
    {
        /// <summary>
        /// Accessor method for contractName
        /// </summary>
        /// <remarks>An SData service can support several “integration contracts” side-by-side. 
        /// For example, a typical Sage ERP service will support a crmErp contract which exposes 
        /// the resources required by CRM integration (with schemas imposed by the CRM/ERP contract) 
        /// and a native or default contract which exposes all the resources of the ERP in their native format.
        /// </remarks>
        public string ContractName
        {
            get { return Uri.Contract; }
            set { Uri.Contract = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public IntermediateDataSetsRequest(ISDataService service)
            : base(service)
        {
            ContractName = !string.IsNullOrEmpty(service.ContractName) ? service.ContractName : "-";
        }

        /// <summary>
        /// Reads the AtomFeed for enumeration of datasets
        /// </summary>
        /// <returns>AtomFeed</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the IntermediateDataSetsRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ Enumeration of DataSets" 
        ///         />
        ///     </code>
        /// </example>
        public override AtomFeed Read()
        {
            return Service.ReadFeed(this);
        }
    }
}