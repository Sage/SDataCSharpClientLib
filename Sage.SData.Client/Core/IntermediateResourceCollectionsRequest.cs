using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Intermediate request to retrieve enumeration of resource collections
    /// from the specified by a given contract
    /// </summary>
    /// <example>http://sdata.acme.com/sdata/sageApp/test/prod</example>
    /// <remarks>
    /// Feed level category = dataset Entry level category = collection
    /// Enumeration of resource collections exposed by the test contract
    /// </remarks>
    public class IntermediateResourceCollectionsRequest : IntermediateDataSetsRequest
    {
        /// <summary>
        /// Accessor method for dataSet
        /// </summary>
        /// <remarks>Identifies the dataset when the application gives access to several datasets, such as several companies and production/test datasets.
        /// If the application can only handle a single dataset, or if it can be configured with a default dataset, 
        /// a hyphen can be used as a placeholder for the default dataset. 
        /// For example, if prod is the default dataset in the example above, the URL could be shortened as:
        /// http://www.example.com/sdata/sageApp/test/-/accounts?startIndex=21&amp;count=10 
        /// If several parameters are required to specify the dataset (for example database name and company id), 
        /// they should be formatted as a single segment in the URL. For example, sageApp/test/demodb;acme/accounts -- the semicolon separator is application specific, not imposed by SData.
        ///</remarks>
        public string DataSet
        {
            get { return Uri.CompanyDataset; }
            set { Uri.CompanyDataset = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public IntermediateResourceCollectionsRequest(ISDataService service)
            : base(service)
        {
            DataSet = !string.IsNullOrEmpty(service.DataSet) ? service.DataSet : "-";
        }

        /// <summary>
        /// Reads the AtomFeed for enumeration of resource collections
        /// </summary>
        /// <returns>AtomFeed</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the IntermediateResourceCollectionsRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ Enumeration of Resource Collections" 
        ///         />
        ///     </code>
        /// </example>
        public override AtomFeed Read()
        {
            return Service.ReadFeed(this);
        }
    }
}