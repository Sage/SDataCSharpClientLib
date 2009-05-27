using System;
using System.Collections.Generic;
using System.Text;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Single Resource URL
    /// </summary>
    public class SDataSingleResourceRequest : SDataApplicationRequest
    {
        //private string _resourceKind;

        /// <summary>
        /// Accessor method for resourceKind
        /// </summary>
        /// <remarks>
        /// This URL segment identifies the kind of resource that is queried (account, contact, salesOrder, etc.)
        /// This URL returns the collection of all account resources, as an Atom feed. 
        /// If the contract exposes a large number of resources kinds, a functional group can be inserted before the 
        /// resource kind segment (eventually a hierarchy of functional groups), which act as a folder (a hierarchy of folders) to organize the resources.
        /// Typical functional groups would be financials, commercials, HR, etc.
        /// </remarks>



        private AtomEntry _entry;
        /// <summary>
        /// accessor method for entry
        /// </summary>
        /// <remarks>
        /// this atom entry will be filled with the return from a resource template request and should be used for creating 
        /// a new SingleResource Request;
        /// </remarks>
        public AtomEntry Entry
        {
            get{ return _entry;}
            set { _entry = value; }
        }
        
        private string _resourceSelector;
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
        public string ResourceSelector
        {
            get { return _resourceSelector; }
            set { _resourceSelector = value; }
        }



        private string _include;
        /// <summary>
        /// 
        /// </summary>
        public string Include
        {
            get { return _include; }   
            set { _include = value;}
        }
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
        {

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
        public AtomEntry Read()
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
            ISyndicationResource result;
            result = Service.Create(this, this.Entry);
            AtomEntry entry = result as AtomEntry;
            return entry;
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
            ISyndicationResource result;
            result = Service.Update(this, this.Entry);
            AtomEntry entry = result as AtomEntry;
            return entry;
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

        /// <summary>
        /// gets the string version of this SData URL
        /// </summary>
        /// <returns>return the string </returns>
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
                     this.ResourceKind +
                     this.ResourceSelector;

            if(Include != string.Empty)
            {
                retval += "?include=" + Include;
            }

            return retval;
        }

    }
}
