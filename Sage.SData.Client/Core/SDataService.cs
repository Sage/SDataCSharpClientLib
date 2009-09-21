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
        private string _applicationName;
        private string _serverName;
        private string _protocol;
        private string _virtualDirectory;
        private string _dataSet;
        private string _contractName;
        private string _url;

        private WebClient _client;
        private readonly CookieContainer _cookies = new CookieContainer();

        private string _userName;
        private string _passWord;
        private bool _initialized;

        /// <summary>
        /// Flag set when Service is initialzed
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
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
        public string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        /// <summary>
        /// Accessor method for protocol, 
        /// </summary>
        /// <remarks>HTTP is the default but can be HTTPS</remarks>
        public string Protocol
        {
            get { return _protocol; }
            set { _protocol = value; }
        }

        /// <remarks>
        /// Creates the service with predefined values for the url
        /// </remarks>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <remarks>
        /// IP address is also allowed (192.168.1.1).
        /// Can be followed by port number. For example www.example.com:5493. 
        /// 5493 is the recommended port number for SData services that are not exposed on the Internet.
        /// </remarks>
        public string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }

        /// <summary>
        /// Accessor method for virtual directory
        /// </summary>
        /// <remarks>Must be sdata, unless the technical framework imposes something different.
        ///</remarks>
        public string VirtualDirectory
        {
            get { return _virtualDirectory; }
            set { _virtualDirectory = value; }
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
            get { return _dataSet; }
            set { _dataSet = value; }
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
            get { return _contractName; }
            set { _contractName = value; }
        }

        /// <summary>
        /// Get set for the user name to authenticate with
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// Get/set for the password to authenticate with
        /// </summary>
        public string Password
        {
            get { return _passWord; }
            set { _passWord = value; }
        }

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
                string strxml = resource.OuterXml;

                _client = CreateWebClient(requestUrl);

                _client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml;type=feed");

                string result = _client.UploadString(requestUrl, "POST", strxml);
                AtomFeed feed = new AtomFeed();
                feed.Load(new MemoryStream(Encoding.UTF8.GetBytes(result)));

                return feed;
            }
            catch (WebException e)
            {
                StreamReader sr = new StreamReader(e.Response.GetResponseStream());
                string data = sr.ReadToEnd();
                SDataServiceException ex = new SDataServiceException();
                ex.Data = data;
                throw ex;
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
                string strxml = resource.CreateNavigator().OuterXml;
                if (BatchProcess.Instance.CurrentStack.Count > 0)
                {
                    string[] batchrequest = new string[3];
                    batchrequest[0] = requestUrl;
                    batchrequest[1] = "POST";
                    batchrequest[2] = resource.CreateNavigator().OuterXml;
                    BatchProcess.Instance.AddToBatch(batchrequest);
                    return null;
                }

                _client = CreateWebClient(requestUrl);

                _client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml;type=entry");
                _client.Headers.Add(HttpRequestHeader.IfMatch, ((AtomEntry) resource).GetSDataHttpETag());

                string result = _client.UploadString(requestUrl, "POST", strxml);
                AtomEntry entry = new AtomEntry();
                entry.Load(new MemoryStream(Encoding.UTF8.GetBytes(result)));

                return entry;
            }
            catch (WebException e)
            {
                StreamReader sr = new StreamReader(e.Response.GetResponseStream());
                string data = sr.ReadToEnd();
                SDataServiceException ex = new SDataServiceException();
                ex.Data = data;
                throw ex;
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
                _client = CreateWebClient(requestUrl);

                _client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml");

                string result = _client.UploadString(requestUrl, "POST", requestUrl);
                XmlDocument document = new XmlDocument();

                document.Load(new MemoryStream(Encoding.UTF8.GetBytes(result)));
                AsyncRequest asyncRequest = new AsyncRequest();

                asyncRequest.Phase = document.SelectSingleNode("phase").InnerXml;
                asyncRequest.PhaseDetail = document.SelectSingleNode("phaseDetail").InnerXml;
                asyncRequest.Progress = Convert.ToDecimal(document.SelectSingleNode("progress").InnerXml);
                asyncRequest.ElapsedSeconds = Convert.ToInt32(document.SelectSingleNode("elapsedSeconds").InnerXml);
                asyncRequest.RemainingSeconds = Convert.ToInt32(document.SelectSingleNode("remainingSeconds").InnerXml);
                asyncRequest.PollingMilliseconds = Convert.ToInt32(document.SelectSingleNode("pollingMillis").InnerXml);
                //asyncRequest.XmlDoc = document;
                asyncRequest.TrackingUrl = _client.Headers.Get("Location");

                return asyncRequest;
            }
            catch (WebException e)
            {
                StreamReader sr = new StreamReader(e.Response.GetResponseStream());
                string data = sr.ReadToEnd();
                SDataServiceException ex = new SDataServiceException();
                ex.Data = data;
                throw ex;
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
                _client = CreateWebClient(_url);

                _client.UploadString(url, "DELETE");

                return true;
            }
            catch (WebException e)
            {
                StreamReader sr = new StreamReader(e.Response.GetResponseStream());
                string data = sr.ReadToEnd();
                SDataServiceException ex = new SDataServiceException();
                ex.Data = data;
                throw ex;
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

                if (BatchProcess.Instance.CurrentStack.Count > 0)
                {
                    string[] batchrequest = new string[3];
                    batchrequest[0] = requestUrl;
                    batchrequest[1] = "DELETE";
                    BatchProcess.Instance.AddToBatch(batchrequest);
                    return true;
                }

                _client = CreateWebClient(requestUrl);

                _client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml;type=entry");
                _client.Headers.Add(HttpRequestHeader.IfMatch, ((AtomEntry) resource).GetSDataHttpETag());
                _client.UploadString(requestUrl, "DELETE", resource.CreateNavigator().OuterXml);
                return true;
            }
            catch (WebException e)
            {
                StreamReader sr = new StreamReader(e.Response.GetResponseStream());
                string data = sr.ReadToEnd();
                SDataServiceException ex = new SDataServiceException();
                ex.Data = data;
                throw ex;
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
            CredentialCache cache = new CredentialCache();
            _client.Encoding = Encoding.UTF8;
            _client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml");
            cache.Add(new Uri(_url), "Digest", new NetworkCredential(_userName, _passWord));
            cache.Add(new Uri(_url), "Basic", new NetworkCredential(_userName, _passWord));

            _client.Credentials = cache; // digest authentication supported 
            return _client.DownloadString(url);
        }

        /// <summary>
        /// Reads resource information from the data source based on the URL.
        /// </summary>
        /// <param name="request">request for the syndication resource to get information for.</param>
        /// <returns>AtomFeed <see cref="AtomFeed"/> populated with the specified resources's information from the data source.</returns>
        public AtomFeed ReadFeed(SDataBaseRequest request)
        {
            return ReadFeed(request, null);
        }

        /// <summary>
        /// Reads resource information from the data source based on the URL. Allows override 
        /// of load settings
        /// </summary>
        /// <param name="request">request for the syndication resource to get information for.</param>
        /// <param name="settings"> if not null, used to override web timeouts</param>
        /// <returns>AtomFeed <see cref="AtomFeed"/> populated with the specified resources's information from the data source.</returns>
        public AtomFeed ReadFeed(SDataBaseRequest request, SyndicationResourceLoadSettings settings)
        {
            try
            {
                CredentialCache cache = new CredentialCache();
                cache.Add(new Uri(request.ToString()), "Digest", new NetworkCredential(_userName, _passWord));
                cache.Add(new Uri(request.ToString()), "Basic", new NetworkCredential(_userName, _passWord));

                return AtomFeed.Create(new Uri(request.ToString()), cache, null, settings);
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    SDataServiceException ex = new SDataServiceException();
                    StreamReader sr = new StreamReader(e.Response.GetResponseStream());
                    ex.Data = sr.ReadToEnd();
                    throw ex;
                }

                throw;
            }
            catch (Exception ex)
            {
                throw new SDataClientException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Reads resource information from the data source based on the URL.
        /// </summary>
        /// <param name="request">request for the syndication resource to get information for.</param>
        /// <returns>An AtomEntry <see cref="AtomEntry"/> populated with the specified resources's information from the data source.</returns>
        public AtomEntry ReadEntry(SDataBaseRequest request)
        {
            try
            {
                if (BatchProcess.Instance.CurrentStack.Count > 0)
                {
                    string[] batchrequest = new string[3];
                    batchrequest[0] = request.ToString();
                    batchrequest[1] = "GET";
                    BatchProcess.Instance.AddToBatch(batchrequest);
                    return null;
                }

                CredentialCache cache = new CredentialCache();
                cache.Add(new Uri(request.ToString()), "Digest", new NetworkCredential(_userName, _passWord));
                cache.Add(new Uri(request.ToString()), "Basic", new NetworkCredential(_userName, _passWord));
                return AtomEntry.Read(new Uri(request.ToString()), cache, null);
            }
            catch (WebException e)
            {
                StreamReader sr = new StreamReader(e.Response.GetResponseStream());
                string data = sr.ReadToEnd();
                SDataServiceException ex = new SDataServiceException();
                ex.Data = data;
                throw ex;
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
                _client = CreateWebClient(requestUrl);

                _client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml;type=entry");

                string result = _client.DownloadString(requestUrl);

                XmlTextReader reader = new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(result)));

                return XmlSchema.Read(reader, null);
            }
            catch (WebException e)
            {
                StreamReader sr = new StreamReader(e.Response.GetResponseStream());
                string data = sr.ReadToEnd();
                SDataServiceException ex = new SDataServiceException();
                ex.Data = data;
                throw ex;
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

                if (BatchProcess.Instance.CurrentStack.Count > 0)
                {
                    string[] batchrequest = new string[3];
                    batchrequest[0] = requestUrl;
                    batchrequest[1] = "PUT";
                    batchrequest[2] = resource.CreateNavigator().OuterXml;
                    BatchProcess.Instance.AddToBatch(batchrequest);
                    return null;
                }

                _client = CreateWebClient(requestUrl);

                _client.Headers.Add(HttpRequestHeader.ContentType, "application/atom+xml;type=entry");
                _client.Headers.Add(HttpRequestHeader.IfMatch, ((AtomEntry) resource).GetSDataHttpETag());
                string result = _client.UploadString(requestUrl, "PUT", resource.CreateNavigator().OuterXml);
                AtomEntry entry = new AtomEntry();
                entry.Load(new MemoryStream(Encoding.UTF8.GetBytes(result)));
                return entry;
            }
            catch (WebException e)
            {
                SDataServiceException ex = new SDataServiceException(e.Message, e);

                if (e.Response != null)
                {
                    using (StreamReader sr = new StreamReader(e.Response.GetResponseStream()))
                    {
                        ex.Data = sr.ReadToEnd();
                    }
                }

                throw ex;
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
            _url = url;
            _userName = userName;
            _passWord = password;

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
        /// <remarks>sett the User Name and Password to authenticate with and build the url</remarks>
        public void Initialize()
        {
            if (_url == null)
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

                _url = builder.ToString();
            }

            _client = CreateWebClient(_url);
            Initialized = true;
        }

        protected virtual WebClient CreateWebClient(string url)
        {
            var uri = new Uri(url);
            var cred = new NetworkCredential(_userName, _passWord);
            var cache = new CredentialCache
                            {
                                {uri, "Digest", cred},
                                {uri, "Basic", cred}
                            };
            return new CookieAwareWebClient(_cookies)
                       {
                           Encoding = Encoding.UTF8,
                           Credentials = cache
                       };
        }

        protected void AppendCookie(Cookie cookie)
        {
            _cookies.Add(cookie);
        }

        protected void AppendCookies(CookieCollection cookies)
        {
            _cookies.Add(cookies);
        }

        private class CookieAwareWebClient : WebClient
        {
            private readonly CookieContainer _container;

            public CookieAwareWebClient(CookieContainer container)
            {
                _container = container;
            }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address);
                var httpWebRequest = request as HttpWebRequest;

                if (httpWebRequest != null)
                {
                    httpWebRequest.CookieContainer = _container;
                }

                return request;
            }
        }
    }
}