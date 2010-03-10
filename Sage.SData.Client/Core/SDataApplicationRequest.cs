using System;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Base class for urls containing Application and Contract
    /// </summary>
    public abstract class SDataApplicationRequest : SDataBaseRequest
    {
        /// <summary>
        /// Accessor method for application
        /// </summary>
        /// <remarks>the application name</remarks>
        public string ApplicationName
        {
            get { return Uri.Product; }
            set { Uri.Product = value; }
        }

        /// <summary>
        /// Accessor method for application
        /// </summary>
        /// <remarks>the application name</remarks>
        [Obsolete("Use the ApplicationName property instead.")]
        public string Application
        {
            get { return ApplicationName; }
            set { ApplicationName = value; }
        }

        /// <summary>
        /// Accessor method for contractName
        /// </summary>
        /// <remarks>An SData service can support several “integration contracts” side-by-side. 
        /// For example, a typical Sage ERP service will support a crmErp contract which exposes 
        /// the resources required by CRM integration (with schemas imposed by the CRM/ERP contract) 
        /// and a native or default contract which exposes all the resources of the ERP in their native format.
        /// </remarks>
        public string ContractName
        {
            get { return Uri.Contract; }
            set { Uri.Contract = value; }
        }

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
            get { return Uri.CompanyDataset; }
            set { Uri.CompanyDataset = value; }
        }

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
        public string ResourceKind { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        protected SDataApplicationRequest(ISDataService service)
            : base(service)
        {
            ApplicationName = !string.IsNullOrEmpty(service.ApplicationName) ? service.ApplicationName : "-";
            ContractName = !string.IsNullOrEmpty(service.ContractName) ? service.ContractName : "-";
            DataSet = !string.IsNullOrEmpty(service.DataSet) ? service.DataSet : "-";
        }

        protected override void BuildUrl(SDataUri uri)
        {
            base.BuildUrl(uri);

            if (!string.IsNullOrEmpty(ResourceKind))
            {
                uri.AppendPath(ResourceKind);
            }
        }
    }
}