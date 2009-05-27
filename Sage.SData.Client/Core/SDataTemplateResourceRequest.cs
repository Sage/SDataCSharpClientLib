using System;
using System.Collections.Generic;
using System.Text;
using Sage.SData.Client.Atom;

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
        private static string _keyWord = "$template?format=atomentry";

        /// <summary>
        /// gets the string version of this SData URL
        /// </summary>
        /// <returns>return the string </returns>
        
       
        public SDataTemplateResourceRequest(ISDataService service) : base(service)
        {

        }

        /// <summary>
        /// Converts this request to a string
        /// </summary>
        /// <returns>url version of the request</returns>
        public override string ToString()
        {
            string retval = string.Empty;


            if (Application == string.Empty || Application == null)
            {
                Application = Service.ApplicationName;
            }
            if (ContractName == string.Empty || ContractName == null)
            {
                ContractName = Service.ContractName;
            }
            if (DataSet == string.Empty || DataSet == null)
            {
                DataSet = Service.DataSet;
            }


            retval = this.Protocol + "://" +
                     this.ServerName + "/" +
                     this.VirtualDirectory + "/" +
                     this.Application + "/" +
                     this.ContractName + "/" +
                     this.DataSet + "/" +
                     this.ResourceKind +"/" + _keyWord;

            return retval;
        }



        /// <summary>
        /// Reads the templatte 
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
