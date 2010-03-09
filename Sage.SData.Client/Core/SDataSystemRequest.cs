using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// SData reserves the following URL for “system” resources and services:
    /// At this stage, SData only reserves the $system segment name, it does not standardize the URLs below this level.
    /// </summary>
    /// <remarks>http://sdata.acme.com/sdata/$system</remarks>
    public class SDataSystemRequest : SDataBaseRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public SDataSystemRequest(ISDataService service)
            : base(service)
        {
        }

        /// <summary>
        /// Sends a PUT request to the server based on the URL for the request
        /// </summary>
        /// <returns>AtomFeed returned from the server</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataSystemRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ System Resources or Services" 
        ///         />
        ///     </code>
        /// </example>
        public AtomFeed Read()
        {
            return Service.ReadFeed(this);
        }
    }
}