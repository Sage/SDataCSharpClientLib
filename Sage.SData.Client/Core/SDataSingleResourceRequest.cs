using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Single Resource URL
    /// </summary>
    public class SDataSingleResourceRequest : SDataApplicationRequest
    {
        /// <summary>
        /// Accessor method for entry
        /// </summary>
        /// <remarks>
        /// this atom entry will be filled with the return from a resource template request and should be used for creating 
        /// a new SingleResource Request;
        /// </remarks>
        public AtomEntry Entry { get; set; }

        /// <summary>
        /// Accessor method for resourceSelector can be null
        /// </summary>
        /// <remarks>
        /// http://sdata.acme.com/sdata/sageApp/test/accounts('A001') Example:('A001')
        ///  This element identifies a single resource. The value between quotes is the primary key of the resource.
        /// The resource selector may also contain a predicate (a Boolean expression). This makes it possible to identify a 
        /// resource by a criteria other than its primary key, which may be helpful in mashup scenarios. For example, the following URL identifies an account by its taxID:
        /// http://sdata.acme.com/sdata/sageApp/test/accounts(taxID eq '1234')
        /// </remarks>
        public string ResourceSelector { get; set; }

        public string Include { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public SDataSingleResourceRequest(ISDataService service)
            : base(service)
        {
            Include = string.Empty;
        }

        /// <summary>
        /// initializes the single resource request and loads the atom entry
        /// </summary>
        /// <remarks>this should be used with the atom entry retured from the 
        /// SDataTemplateResourceRequest</remarks>
        /// <param name="service"></param>
        /// <param name="entry"></param>
        public SDataSingleResourceRequest(ISDataService service, AtomEntry entry)
            : base(service)
        {
            Entry = entry;
        }

        /// <summary>
        /// Reads the AtomEntry for the single resource request
        /// </summary>
        /// <returns>AtomEntry</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataSingleResourceRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ a Single Resource Entry" 
        ///         />
        ///     </code>
        /// </example>
        public virtual AtomEntry Read()
        {
            return Service.ReadEntry(this);
        }

        /// <summary>
        /// Creates the AtomEntry for the single resource request
        /// </summary>
        /// <returns>AtomEntry</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataSingleResourceRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="CREATE Single Resource Entry" 
        ///         />
        ///     </code>
        /// </example>
        public AtomEntry Create()
        {
            var result = Service.Create(this, Entry);
            return result as AtomEntry;
        }

        /// <summary>
        /// Delete the AtomEntry for the single resource request
        /// </summary>
        /// <returns>AtomEntry</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataSingleResourceRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="UPDATE Single Resource Entry" 
        ///         />
        ///     </code>
        /// </example>
        public AtomEntry Update()
        {
            var result = Service.Update(this, Entry);
            return result as AtomEntry;
        }

        /// <summary>
        /// Delete the AtomEntry for the single resource request
        /// </summary>
        /// <returns>AtomEntry</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataSingleResourceRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="DELETE a Single Resource Entry" 
        ///         />
        ///     </code>
        /// </example>
        public bool Delete()
        {
            return Service.Delete(this, Entry);
        }

        protected override void BuildUrl(UrlBuilder builder)
        {
            base.BuildUrl(builder);

            if (!string.IsNullOrEmpty(ResourceSelector))
            {
                var index = builder.PathSegments.Count - 1;
                builder.PathSegments[index] = builder.PathSegments[index] + ResourceSelector;
            }

            if (!string.IsNullOrEmpty(Include))
            {
                builder.QueryParameters["include"] = Include;
            }
        }
    }
}