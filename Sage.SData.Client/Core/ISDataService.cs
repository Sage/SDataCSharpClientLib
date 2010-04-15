using System;
using System.Net;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;
using Sage.SData.Client.Metadata;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Interface for SDataServices
    /// </summary>
    public interface ISDataService
    {
        /// <summary>
        /// Flag set when service has been initialized
        /// </summary>
        [Obsolete("Explicit initialization is no longer required.")]
        bool Initialized { get; }

        /// <remarks>
        /// Creates the service with predefined values for the url
        /// </remarks>
        string Url { get; set; }

        /// <summary>
        /// Accessor method for protocol, 
        /// </summary>
        /// <remarks>HTTP is the default but can be HTTPS</remarks>
        string Protocol { get; set; }

        /// <remarks>IP address is also allowed (192.168.1.1).
        /// Can be followed by port number. For example www.example.com:5493. 
        /// 5493 is the recommended port number for SData services that are not exposed on the Internet.
        /// </remarks>
        string ServerName { get; set; }

        /// <summary>
        /// Accessor method for port
        /// </summary>
        int? Port { get; set; }

        /// <summary>
        /// Accessor method for virtual directory
        /// </summary>
        /// <remarks>Must be sdata, unless the technical framework imposes something different.
        ///</remarks>
        string VirtualDirectory { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        /// <remarks>
        ///     The <see cref="ApplicationName"/> is used to identify users specific to an application. That is, the same syndication resource can exist in the data store 
        ///     for multiple applications that specify a different <see cref="ApplicationName"/>. This enables multiple applications to use the same data store to store resource 
        ///     information without running into duplicate syndication resource conflicts. Alternatively, multiple applications can use the same syndication resource data store 
        ///     by specifying the same <see cref="ApplicationName"/>. The <see cref="ApplicationName"/> can be set programmatically or declaratively in the configuration for the application.
        /// </remarks>
        string ApplicationName { get; set; }

        /// <summary>
        /// Accessor method for contractName
        /// </summary>
        /// <remarks>An SData service can support several “integration contracts” side-by-side. 
        /// For example, a typical Sage ERP service will support a crmErp contract which exposes 
        /// the resources required by CRM integration (with schemas imposed by the CRM/ERP contract) 
        /// and a native or default contract which exposes all the resources of the ERP in their native format.
        /// </remarks>
        string ContractName { get; set; }

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
        string DataSet { get; set; }

        /// <summary>
        /// Get set for the user name to authenticate with
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Get/set for the password to authenticate with
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the timeout in milliseconds used during requests.
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the cookies passed during requests.
        /// </summary>
        CookieContainer Cookies { get; set; }

        /// <summary>
        /// Gets or sets the user agent passed during requests.
        /// </summary>
        string UserAgent { get; set; }

        /// <summary>
        /// Adds a new syndication resource to the data source.
        /// </summary>
        /// <param name="request">The request that identifies the resource within the syndication data source.</param>
        /// <param name="entry">The <see cref="ISyndicationResource"/> to be created within the data source.</param>
        AtomEntry CreateEntry(SDataBaseRequest request, AtomEntry entry);

        /// <summary>
        /// Adds new sydication resource to the data source returning an AtomFeed
        /// </summary>
        /// <param name="request">The request that identifies the resource within the syndication data source.</param>
        /// <param name="feed">The <see cref="ISyndicationResource"/> to be created within the data source.</param>
        /// <returns></returns>
        AtomFeed CreateFeed(SDataBaseRequest request, AtomFeed feed);

        /// <summary>
        /// Adds new sydication resource to the data source returning an AtomFeed
        /// </summary>
        /// <param name="request"></param>
        /// <param name="feed"></param>
        /// <param name="eTag"></param>
        /// <returns></returns>
        AtomFeed CreateFeed(SDataBaseRequest request, AtomFeed feed, out string eTag);

        /// <summary>
        /// Asynchronous PUT to the server
        /// </summary>
        /// <param name="request">The request that identifies the resource within the syndication data source.</param>
        /// <param name="resource">The resource that should be created asynchronously.</param>
        AsyncRequest CreateAsync(SDataBaseRequest request, ISyndicationResource resource);

        /// <summary>
        /// Generic delete from server
        /// </summary>
        /// <param name="url">the url for the operation</param>
        /// <returns><b>true</b> returns true if the operation was successful</returns>
        bool Delete(string url);

        /// <summary>
        /// Removes a resource from the syndication data source.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool DeleteEntry(SDataBaseRequest request);

        /// <summary>
        /// Removes a resource from the syndication data source.
        /// </summary>
        /// <param name="request">The request from the syndication data source for the resource to be removed.</param>
        /// <param name="entry">The resourc that is being deleted</param>
        /// <returns><b>true</b> if the syndication resource was successfully deleted; otherwise, <b>false</b>.</returns>
        bool DeleteEntry(SDataBaseRequest request, AtomEntry entry);

        /// <summary>
        /// generic read from the specified url
        /// </summary>
        /// <param name="url">url to read from </param>
        /// <returns>string response from server</returns>
        object Read(string url);

        /// <summary>
        /// Reads resource information from the data source based on the URL.
        /// </summary>
        /// <param name="request">request for the syndication resource to get information for.</param>
        /// <returns>AtomFeed <see cref="AtomFeed"/> populated with the specified resources's information from the data source.</returns>
        AtomFeed ReadFeed(SDataBaseRequest request);

        /// <summary>
        /// Reads resource information from the data source based on the URL and the specified ETag.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="eTag"></param>
        /// <returns></returns>
        AtomFeed ReadFeed(SDataBaseRequest request, ref string eTag);

        /// <summary>
        /// Reads resource information from the data source based on the URL.
        /// </summary>
        /// <param name="request">request for the syndication resource to get information for.</param>
        /// <returns>An AtomEntry <see cref="AtomEntry"/> populated with the specified resources's information from the data source.</returns>
        AtomEntry ReadEntry(SDataBaseRequest request);

        /// <summary>
        /// Reads resource information from the data source based on the URL and the ETag of the specified entry.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        AtomEntry ReadEntry(SDataBaseRequest request, AtomEntry entry);

        /// <summary>
        /// Reads xsd from a $schema request
        /// </summary>
        /// <param name="request">url for the syndication resource to get information for.</param>
        /// <returns>XmlSchema </returns>
        SDataSchema ReadSchema(SDataResourceSchemaRequest request);

        /// <summary>
        /// Updates information about a syndication resource in the data source.
        /// </summary>
        /// <param name="request">The url from the syndication data source for the resource to be updated.</param>
        /// <param name="entry">
        ///     An object that implements the <see cref="ISyndicationResource"/> interface that represents the updated information for the resource.
        /// </param>
        AtomEntry UpdateEntry(SDataBaseRequest request, AtomEntry entry);

        /// <summary>
        /// Initializes the <see cref="SDataService"/> 
        /// </summary>
        /// <remarks>Set the User Name and Password to authenticate with and build the url</remarks>
        [Obsolete("Explicit initialization is no longer required.")]
        void Initialize();
    }
}