using System.Collections.Generic;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// SData Resource Property URL
    /// </summary>
    public class SDataResourcePropertyRequest : SDataSingleResourceRequest
    {
        private readonly IList<string> _properties;

        /// <summary>
        /// Dictionary containing the resource properties
        /// </summary>
        /// <remarks>
        /// SData only allows “relationship properties” in this context, 
        /// i.e., properties that point to another resource or to a collection of related resources. 
        /// So, properties like postalAddress or contacts are valid in the example above 
        /// (assuming that address and contact are treated as resource kinds by the service) 
        /// but a property like accountName would not be allowed (at least in a first version of the SData standard, this restriction may be lifted later).
        /// If the relationship designates a single resource, as in the example above (an account has a single postal address), the URL returns an Atom entry. 
        /// If the relationship designates a collection of resources (if we had used a property like addresses in the example above), the URL returns an Atom feed. 
        /// The sme:isCollection schema attribute can be used to determine whether the relationships is a collection or not (see Relationship Definition section for details).
        /// SData only allows property segments following a URL that identifies a single resource. So, URLs like accounts('A001')/postalAddress or accounts('A001')/contacts 
        /// are valid but a URL like accounts/postalAddress (all postal addresses of all accounts) is invalid.
        /// SData allows property segments to be chained, as long as the previous rule is satisfied. So, the following URLs are also valid:
        /// http://sdata.acme.com/sdata/sageApp/test/accounts('A001')/postalAddress/country
        /// http://sdata.acme.com/sdata/sageApp/test/accounts('A001')/addresses(type eq 'postal')/country
        /// </remarks>
        public IList<string> ResourceProperties
        {
            get { return _properties; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public SDataResourcePropertyRequest(ISDataService service)
            : base(service)
        {
            _properties = new List<string>();
        }

        /// <summary>
        /// Reads the AtomFeed for the resource property
        /// </summary>
        /// <returns>AtomFeed</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataResourceProperty class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ a Resource Property Request (AtomFeed)" 
        ///         />
        ///     </code>
        /// </example>
        public AtomFeed ReadFeed()
        {
            return Service.ReadFeed(this);
        }

        /// <summary>
        /// Reads the AtomEntry for the resource property
        /// </summary>
        /// <returns>AtomEntry</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataResourcePropertyRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ a Resource Property (AtomEntry)" 
        ///         />
        ///     </code>
        /// </example>
        public override AtomEntry Read()
        {
            return Service.ReadEntry(this);
        }

        protected override void BuildUrl(SDataUri uri)
        {
            base.BuildUrl(uri);

            foreach (var value in ResourceProperties)
            {
                uri.AppendPath(value);
            }
        }
    }
}