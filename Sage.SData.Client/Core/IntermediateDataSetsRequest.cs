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
        private string _contractName;

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
            get { return _contractName; }
            set { _contractName = value; }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public IntermediateDataSetsRequest(ISDataService service)
            : base(service)
        {
            Application = service.ApplicationName;
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
        public new AtomFeed Read()
        {
            return Service.ReadFeed(this);
        }


        /// <summary>
        /// gets the string version of this SData URL
        /// </summary>
        /// <returns>return the string </returns>
        public override string ToString()
        {
            string retval =
                Protocol + "://" +
                ServerName + "/" +
                VirtualDirectory + "/" +
                Application + "/" +
                ContractName;
            return retval;
        }
    }
}