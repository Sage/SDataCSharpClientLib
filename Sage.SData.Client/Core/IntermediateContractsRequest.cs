using System;
using System.Collections.Generic;
using System.Text;
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
            set { _application = value;}
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public IntermediateContractsRequest(ISDataService service) : base(service)
        {

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
                this.Application;

            return retval;
        }
    }
}
