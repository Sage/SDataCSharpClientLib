using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Intermediate request to retrieve enumeration of applications
    /// </summary>
    /// <example>http://sdata.acme.com/sdata</example>
    /// <remarks>Feed level category = provider Entry level category = application</remarks>
    public class IntermediateApplicationsRequest : SDataBaseRequest
    {
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public IntermediateApplicationsRequest(ISDataService service)
            : base(service) {}

        /// <summary>
        /// Reads the AtomFeed for enumeration of applications
        /// </summary>
        /// <returns>AtomFeed</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the IntermediateApplicatonsRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ Enumeration of Applications" 
        ///         />
        ///     </code>
        /// </example>
        public virtual AtomFeed Read()
        {
            return Service.ReadFeed(this);
        }
    }
}