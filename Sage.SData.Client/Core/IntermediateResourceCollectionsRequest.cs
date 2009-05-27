using System;
using System.Collections.Generic;
using System.Text;
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
        private string _dataSet;
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
            get { return _dataSet; }
            set { _dataSet = value;}
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
         public IntermediateResourceCollectionsRequest(ISDataService service)
            : base(service)
        {
             ContractName = service.ContractName;
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
         public new AtomFeed Read()
         {
             return Service.ReadFeed(this);
         }

         /// <summary>
         /// Converts this request to a string
         /// </summary>
         /// <returns>url version of the request</returns>
        public override string ToString()
        {
            string retval =
                this.Protocol + "://" +
                this.ServerName + "/" +
                this.VirtualDirectory + "/" +
                this.Application + "/" +
                this.ContractName + "/" +
                this.DataSet;
            return retval;

        }
    }
}
