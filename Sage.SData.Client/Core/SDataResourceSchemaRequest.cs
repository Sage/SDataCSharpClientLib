using System.Xml.Schema;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Request to to get resource schema
    /// </summary>
    public class SDataResourceSchemaRequest : SDataOperationalRequest
    {
        private static string _keyWord = "$schema";

        private string _version;

        /// <summary>
        /// Accessor method for version
        /// </summary>
        /// <remarks>
        /// SData recommends that schema links include a version parameter. 
        /// This enables consumers to cache them more efficiently (see Resource Versioning section for details).
        /// NOTE Not Required
        /// </remarks>
        /// <example>
        /// without resource kind http://sdata.acme.com/sdata/sageApp/test/$schema 
        /// with resource kind and version http://sdata.acme.com/sdata/sageApp/test/accounts/$schema?version=5
        /// </example>
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }


        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public SDataResourceSchemaRequest(ISDataService service)
            : base(service) {}


        /// <summary>
        /// Reads the Xml Schema for a resource
        /// </summary>
        /// <returns>XmlSchema</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataResourceSchemaRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ a Resource Schema" 
        ///         />
        ///     </code>
        /// </example>
        public XmlSchema Read()
        {
            return Service.Read(this);
        }


        /// <summary>
        /// gets the string version of this SData URL
        /// </summary>
        /// <returns>return the string </returns>
        public override string ToString()
        {
            string retval = string.Empty;
            if (ResourceKind != string.Empty && ResourceKind != null)
            {
                retval =
                    Protocol + "://" +
                    ServerName + "/" +
                    VirtualDirectory + "/" +
                    Application + "/" +
                    ContractName + "/" +
                    DataSet + "/" +
                    ResourceKind + "/" + _keyWord;
            }
            else
            {
                retval =
                    Protocol + "://" +
                    ServerName + "/" +
                    VirtualDirectory + "/" +
                    Application + "/" +
                    ContractName + "/" +
                    DataSet + "/" + _keyWord;
            }

            return retval;
        }
    }
}