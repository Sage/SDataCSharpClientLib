using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Service class for processing SData Request
    /// </summary>
    /// <example>
    ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataService class.">
    ///         <code 
    ///             source=".\Example.cs" 
    ///             region="SDataService Configuration" 
    ///         />
    ///     </code>
    /// </example>
    public class SDataService : ISDataService, ISDataRequestSettings
    {
        private CookieContainer _cookies;
        private bool _initialized;

        /// <summary>
        /// Flag set when Service is initialzed
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
        }

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
        public string ApplicationName { get; set; }

        /// <summary>
        /// Accessor method for protocol, 
        /// </summary>
        /// <remarks>HTTP is the default but can be HTTPS</remarks>
        public string Protocol { get; set; }

        /// <remarks>
        /// Creates the service with predefined values for the url
        /// </remarks>
        public string Url { get; set; }

        /// <remarks>
        /// IP address is also allowed (192.168.1.1).
        /// Can be followed by port number. For example www.example.com:5493. 
        /// 5493 is the recommended port number for SData services that are not exposed on the Internet.
        /// </remarks>
        public string ServerName { get; set; }

        /// <summary>
        /// Accessor method for virtual directory
        /// </summary>
        /// <remarks>Must be sdata, unless the technical framework imposes something different.
        ///</remarks>
        public string VirtualDirectory { get; set; }

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
        public string DataSet { get; set; }

        /// <summary>
        /// Accessor method for contractName
        /// </summary>
        /// <remarks>An SData service can support several “integration contracts” side-by-side. 
        /// For example, a typical Sage ERP service will support a crmErp contract which exposes 
        /// the resources required by CRM integration (with schemas imposed by the CRM/ERP contract) 
        /// and a native or default contract which exposes all the resources of the ERP in their native format.
        /// </remarks>
        public string ContractName { get; set; }

        /// <summary>
        /// Get set for the user name to authenticate with
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Get/set for the password to authenticate with
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Adds a new syndication resource to the data source.
        /// </summary>
        /// <param name="request">The request that identifies the resource within the syndication data source.</param>
        /// <param name="resource">The <see cref="ISyndicationResource"/> to be created within the data source.</param>
        public ISyndicationResource CreateFeed(SDataBaseRequest request, XPathNavigator resource)
        {
            try
            {
                var requestUrl = request.ToString();
                var strxml = resource.OuterXml;

                var client = CreateWebClient(requestUrl);

                client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml;type=feed");

                var result = client.UploadString(requestUrl, "POST", strxml);
                var feed = new AtomFeed();
                feed.Load(new MemoryStream(Encoding.UTF8.GetBytes(result)));

                return feed;
            }
            catch (WebException ex)
            {
                throw new SDataServiceException(ex);
            }
            catch (Exception ex)
            {
                throw new SDataClientException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new syndication resource to the data source.
        /// </summary>
        /// <param name="request">The request that identifies the resource within the syndication data source.</param>
        /// <param name="resource">The <see cref="ISyndicationResource"/> to be created within the data source.</param>
        public ISyndicationResource Create(SDataBaseRequest request, ISyndicationResource resource)
        {
            try
            {
                var requestUrl = request.ToString();
                var strxml = resource.CreateNavigator().OuterXml;
                if (BatchProcess.Instance.CurrentStack.Count > 0)
                {
                    var batchrequest = new SDataBatchRequestItem
                                       {
                                           Uri = requestUrl,
                                           Verb = "POST",
                                           Body = resource.CreateNavigator().OuterXml
                                       };
                    BatchProcess.Instance.AddToBatch(batchrequest);
                    return null;
                }

                var client = CreateWebClient(requestUrl);

                client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml;type=entry");
                client.Headers.Add(HttpRequestHeader.IfMatch, ((AtomEntry) resource).GetSDataHttpETag());

                var result = client.UploadString(requestUrl, "POST", strxml);
                var entry = new AtomEntry();
                entry.Load(new MemoryStream(Encoding.UTF8.GetBytes(result)));

                return entry;
            }
            catch (WebException ex)
            {
                throw new SDataServiceException(ex);
            }
            catch (Exception ex)
            {
                throw new SDataClientException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Asynchronous PUT to the server
        /// </summary>
        /// <param name="request">The request that identifies the resource within the syndication data source.</param>
        public AsyncRequest CreateAsync(SDataBaseRequest request)
        {
            try
            {
                var requestUrl = request.ToString();
                var client = CreateWebClient(requestUrl);

                client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml");

                var result = client.UploadString(requestUrl, "POST", requestUrl);
                var document = new XmlDocument();

                document.Load(new MemoryStream(Encoding.UTF8.GetBytes(result)));

                return new AsyncRequest
                       {
                           Phase = document.SelectSingleNode("phase").InnerXml,
                           PhaseDetail = document.SelectSingleNode("phaseDetail").InnerXml,
                           Progress = Convert.ToDecimal(document.SelectSingleNode("progress").InnerXml),
                           ElapsedSeconds = Convert.ToInt32(document.SelectSingleNode("elapsedSeconds").InnerXml),
                           RemainingSeconds = Convert.ToInt32(document.SelectSingleNode("remainingSeconds").InnerXml),
                           PollingMilliseconds = Convert.ToInt32(document.SelectSingleNode("pollingMillis").InnerXml),
                           TrackingUrl = client.Headers.Get("Location")
                       };
            }
            catch (WebException ex)
            {
                throw new SDataServiceException(ex);
            }
            catch (Exception ex)
            {
                throw new SDataClientException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generic delete from server
        /// </summary>
        /// <param name="url">the url for the operation</param>
        /// <returns><b>true</b> returns true if the operation was successful</returns>
        public bool Delete(string url)
        {
            try
            {
                var client = CreateWebClient(Url);

                client.UploadString(url, "DELETE");

                return true;
            }
            catch (WebException ex)
            {
                throw new SDataServiceException(ex);
            }
            catch (Exception ex)
            {
                throw new SDataClientException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Removes a resource from the syndication data source.
        /// </summary>
        /// <param name="request">The request from the syndication data source for the resource to be removed.</param>
        /// <param name="resource">the resource that is being deleted</param>
        /// <returns><b>true</b> if the syndication resource was successfully deleted; otherwise, <b>false</b>.</returns>
        public bool Delete(SDataBaseRequest request, ISyndicationResource resource)
        {
            try
            {
                var requestUrl = request.ToString();
                var ifMatch = resource != null ? ((AtomEntry) resource).GetSDataHttpETag() : null;

                if (BatchProcess.Instance.CurrentStack.Count > 0)
                {
                    var batchrequest = new SDataBatchRequestItem
                                       {
                                           Uri = requestUrl,
                                           Verb = "DELETE",
                                           IfMatch = ifMatch
                                       };
                    BatchProcess.Instance.AddToBatch(batchrequest);
                    return true;
                }

                var client = CreateWebClient(requestUrl);

                client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml;type=entry");
                client.Headers.Add(HttpRequestHeader.IfMatch, ifMatch);
                client.UploadString(requestUrl, "DELETE", resource != null ? resource.CreateNavigator().OuterXml : string.Empty);
                return true;
            }
            catch (WebException ex)
            {
                throw new SDataServiceException(ex);
            }
            catch (Exception ex)
            {
                throw new SDataClientException(ex.Message, ex);
            }
        }

        /// <summary>
        /// generic read from the specified url
        /// </summary>
        /// <param name="url">url to read from </param>
        /// <returns>string response from server</returns>
        public string Read(string url)
        {
            var client = CreateWebClient(Url);

            var cache = new CredentialCache();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml");
            cache.Add(new Uri(Url), "Digest", new NetworkCredential(UserName, Password));
            cache.Add(new Uri(Url), "Basic", new NetworkCredential(UserName, Password));

            client.Credentials = cache; // digest authentication supported 
            return client.DownloadString(url);
        }

        /// <summary>
        /// Reads resource information from the data source based on the URL.
        /// </summary>
        /// <param name="request">request for the syndication resource to get information for.</param>
        /// <returns>AtomFeed <see cref="AtomFeed"/> populated with the specified resources's information from the data source.</returns>
        public AtomFeed ReadFeed(SDataBaseRequest request)
        {
            var settings = new SyndicationResourceLoadSettings {Timeout = new TimeSpan(0, 0, 120)};
            return ReadFeed(request, settings);
        }

        /// <summary>
        /// Reads resource information from the data source based on the URL.
        /// Allows override of load settings.
        /// </summary>
        /// <param name="request">Request for the syndication resource to get information for.</param>
        /// <param name="settings">If not null, used to override web timeouts.</param>
        /// <returns>An <see cref="AtomFeed"/> populated with the specified resources' information from the data source.</returns>
        public AtomFeed ReadFeed(SDataBaseRequest request, SyndicationResourceLoadSettings settings)
        {
            try
            {
                var cache = new CredentialCache
                            {
                                {new Uri(request.ToString()), "Digest", new NetworkCredential(UserName, Password)},
                                {new Uri(request.ToString()), "Basic", new NetworkCredential(UserName, Password)}
                            };
                var options = new WebRequestOptions(cache)
                              {
                                  CookieContainer = Cookies,
                                  UserAgent = request.UserAgent
                              };
                return AtomFeed.Create(new Uri(request.ToString()), options, settings);
            }
            catch (WebException ex)
            {
                throw new SDataServiceException(ex);
            }
            catch (Exception ex)
            {
                throw new SDataClientException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Reads resource information from the data source based on the URL.
        /// </summary>
        /// <param name="request">Request for the syndication resource to get information for.</param>
        /// <returns>An <see cref="AtomEntry"/> populated with the specified resources' information from the data source.</returns>
        public AtomEntry ReadEntry(SDataBaseRequest request)
        {
            try
            {
                if (BatchProcess.Instance.CurrentStack.Count > 0)
                {
                    var batchrequest = new SDataBatchRequestItem
                                       {
                                           Uri = request.ToString(),
                                           Verb = "GET"
                                       };
                    BatchProcess.Instance.AddToBatch(batchrequest);
                    return null;
                }

                var cache = new CredentialCache
                            {
                                {new Uri(request.ToString()), "Digest", new NetworkCredential(UserName, Password)},
                                {new Uri(request.ToString()), "Basic", new NetworkCredential(UserName, Password)}
                            };
                var options = new WebRequestOptions(cache)
                              {
                                  CookieContainer = Cookies,
                                  UserAgent = request.UserAgent
                              };
                var settings = new SyndicationResourceLoadSettings {Timeout = new TimeSpan(0, 0, 120)};
                return AtomEntry.Create(new Uri(request.ToString()), options, settings);
            }
            catch (WebException ex)
            {
                throw new SDataServiceException(ex);
            }
            catch (Exception ex)
            {
                throw new SDataClientException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Reads xsd from a $schema request
        /// </summary>
        /// <param name="request">url for the syndication resource to get information for.</param>
        /// <returns>XmlSchema </returns>
        public XmlSchema Read(SDataResourceSchemaRequest request)
        {
            try
            {
                var requestUrl = request.ToString();
                var client = CreateWebClient(requestUrl);

                client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml;type=entry");

                var result = client.DownloadString(requestUrl);

                var reader = new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(result)));

                return XmlSchema.Read(reader, null);
            }
            catch (WebException ex)
            {
                throw new SDataServiceException(ex);
            }
            catch (Exception ex)
            {
                throw new SDataClientException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Updates information about a syndication resource in the data source.
        /// </summary>
        /// <param name="request">The url from the syndication data source for the resource to be updated.</param>
        /// <param name="resource">
        ///     An object that implements the <see cref="ISyndicationResource"/> interface that represents the updated information for the resource.
        /// </param>
        public ISyndicationResource Update(SDataBaseRequest request, ISyndicationResource resource)
        {
            try
            {
                var requestUrl = request.ToString();
                var ifMatch = ((AtomEntry) resource).GetSDataHttpETag();

                if (BatchProcess.Instance.CurrentStack.Count > 0)
                {
                    var batchrequest = new SDataBatchRequestItem
                                       {
                                           Uri = requestUrl,
                                           Verb = "PUT",
                                           Body = resource.CreateNavigator().OuterXml,
                                           IfMatch = ifMatch
                                       };
                    BatchProcess.Instance.AddToBatch(batchrequest);
                    return null;
                }

                var client = CreateWebClient(requestUrl);

                client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml;type=entry");
                client.Headers.Add(HttpRequestHeader.IfMatch, ifMatch);
                var result = client.UploadString(requestUrl, "PUT", resource.CreateNavigator().OuterXml);
                var entry = new AtomEntry();
                entry.Load(new MemoryStream(Encoding.UTF8.GetBytes(result)));
                return entry;
            }
            catch (WebException ex)
            {
                throw new SDataServiceException(ex);
            }
            catch (Exception ex)
            {
                throw new SDataClientException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SDataService() {}

        /// <summary>
        /// Constructor with pre-set url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userName">user name used for credentials</param>
        /// <param name="password">password for user</param>
        public SDataService(string url, string userName, string password)
        {
            Url = url;
            UserName = userName;
            Password = password;

            var builder = new UrlBuilder(url);
            Protocol = builder.Scheme;
            ServerName = string.Format("{0}:{1}", builder.Host, builder.Port);

            var segmentCount = builder.PathSegments.Count;

            if (segmentCount > 0)
            {
                VirtualDirectory = builder.PathSegments[0];
            }
            if (segmentCount > 1)
            {
                ApplicationName = builder.PathSegments[1];
            }
            if (segmentCount > 2)
            {
                ContractName = builder.PathSegments[2];
            }
            if (segmentCount > 3)
            {
                DataSet = builder.PathSegments[3];
            }
        }

        /// <summary>
        /// Initializes the <see cref="SDataService"/> 
        /// </summary>
        /// <remarks>Set the User Name and Password to authenticate with and build the url</remarks>
        public void Initialize()
        {
            if (Url == null)
            {
                var server = ServerName;
                var pos = server.IndexOf(':');
                UrlBuilder builder;

                if (pos >= 0)
                {
                    var port = int.Parse(server.Substring(pos + 1));
                    server = server.Substring(0, pos);
                    builder = new UrlBuilder(Protocol, server, port);
                }
                else
                {
                    builder = new UrlBuilder(Protocol, server);
                }

                builder.PathSegments.Add(VirtualDirectory);
                builder.PathSegments.Add(ApplicationName);
                builder.PathSegments.Add(ContractName);
                builder.PathSegments.Add(DataSet);

                Url = builder.ToString();
            }

            _initialized = true;
        }

        protected virtual WebClient CreateWebClient(string url)
        {
            var uri = new Uri(url);
            var cred = new NetworkCredential(UserName, Password);
            var cache = new CredentialCache
                        {
                            {uri, "Digest", cred},
                            {uri, "Basic", cred}
                        };
            return new CookieAwareWebClient(Cookies)
                   {
                       Encoding = Encoding.UTF8,
                       Credentials = cache
                   };
        }

        /// <summary>
        /// Gets the cookie collection associated with all requests to the server.
        /// </summary>
        public CookieContainer Cookies
        {
            get { return _cookies ?? (_cookies = new CookieContainer()); }
            set { _cookies = value; }
        }

        private class CookieAwareWebClient : WebClient
        {
            private readonly CookieContainer _cookies;

            public CookieAwareWebClient(CookieContainer cookies)
            {
                _cookies = cookies;
            }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address);
                var httpWebRequest = request as HttpWebRequest;

                if (httpWebRequest != null)
                {
                    httpWebRequest.CookieContainer = _cookies;
                }

                return request;
            }
        }
    }
}