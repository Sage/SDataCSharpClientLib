using Sage.SData.Client.Atom;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// The template resource allows a service consumer to obtain the resource's default property values. 
    /// For example, the URL in the example could be used to fill the contents of a “Create Account” form.
    /// The $template segment is only valid after the resource kind segment (accounts in the example).
    /// Template resources are usually read only, but some applications could expose them in read/write mode, typically to admin accounts.
    /// </summary>
    public class SDataTemplateResourceRequest : SDataOperationalRequest
    {
        private const string TemplateTerm = "$template";

        /// <summary>
        /// gets the string version of this SData URL
        /// </summary>
        /// <returns>return the string </returns>
        public SDataTemplateResourceRequest(ISDataService service)
            : base(service)
        {
        }

        protected override void BuildUrl(SDataUri uri)
        {
            base.BuildUrl(uri);
            uri.AppendPath(TemplateTerm);
        }

        /// <summary>
        /// Reads the template 
        /// </summary>
        /// <returns>AtomEntry representing the template</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the AsyncRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ a Template Resource" 
        ///         />
        ///     </code>
        /// </example>
        public AtomEntry Read()
        {
            return Service.ReadEntry(this);
        }
    }
}