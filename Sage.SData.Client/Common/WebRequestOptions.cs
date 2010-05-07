using System;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Sage.SData.Client.Common
{
    /// <summary>
    /// Holds options that should be applied to web requests.
    /// </summary>
    [Serializable()]
    public class WebRequestOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequestOptions"/> class.
        /// </summary>
        public WebRequestOptions() : this(null, null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequestOptions"/> class using the specified <see cref="ICredentials">credentials</see>.
        /// </summary>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the source resource when required. This value can be <b>null</b>.
        /// </param>
        public WebRequestOptions(ICredentials credentials) : this(credentials, null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequestOptions"/> class using the specified <see cref="ICredentials">credentials</see> and  <see cref="IWebProxy">proxy</see>.
        /// </summary>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the source resource when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="proxy">
        ///     A <see cref="IWebProxy"/> that provides proxy access to the source resource when required. This value can be <b>null</b>.
        /// </param>
        public WebRequestOptions(ICredentials credentials, IWebProxy proxy)
        {
            _allowAutoRedirect  = true;
            _keepAlive          = true;
            _credentials        = credentials;
            _proxy = proxy;

            if (credentials == null)
            {
                _useDefaultCredentials  = true;
            }
        }

        #region WebRequest Options

        private AuthenticationLevel? _authenticationLevel;
        [NonSerialized()] private RequestCachePolicy _cachePolicy;
        private string _connectionGroupName;
        private ICredentials _credentials;
        private WebHeaderCollection _headers;
        private TokenImpersonationLevel? _impersonationLevel;
        private bool? _preAuthenticate;
        private IWebProxy _proxy;
        private int? _timeout;
        private bool? _useDefaultCredentials;

        /// <summary>Gets or sets values indicating the level of authentication and impersonation used for this request.</summary>
        public AuthenticationLevel? AuthenticationLevel
        {
            get { return _authenticationLevel; }
            set { _authenticationLevel = value; }
        }

        /// <summary>Gets or sets the cache policy for this request.</summary>
        public RequestCachePolicy CachePolicy
        {
            get { return _cachePolicy; }
            set { _cachePolicy = value; }
        }

        /// <summary>Gets or sets the name of the connection group for the request.</summary>
        public string ConnectionGroupName
        {
            get { return _connectionGroupName; }
            set { _connectionGroupName = value; }
        }

        /// <summary>Gets or sets the network credentials used for authenticating the request with the Internet resource.</summary>
        public ICredentials Credentials
        {
            get { return _credentials; }
            set { _credentials = value; }
        }

        /// <summary>Gets or sets the collection of header name/value pairs associated with the request.</summary>
        public WebHeaderCollection Headers
        {
            get { return _headers; }
            set { _headers = value; }
        }

        /// <summary>Gets or sets the impersonation level for the current request.</summary>
        public TokenImpersonationLevel? ImpersonationLevel
        {
            get { return _impersonationLevel; }
            set { _impersonationLevel = value; }
        }

        /// <summary>Indicates whether to pre-authenticate the request.</summary>
        public bool? PreAuthenticate
        {
            get { return _preAuthenticate; }
            set { _preAuthenticate = value; }
        }

        /// <summary>Gets or sets the network proxy to use to access this Internet resource.</summary>
        public IWebProxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        /// <summary>Gets or sets the length of time before the request times out.</summary>
        public int? Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        /// <summary>Gets or sets a <see cref="T:System.Boolean"/> value that controls whether <see cref="P:System.Net.CredentialCache.DefaultCredentials"></see> are sent with requests.</summary>
        public bool? UseDefaultCredentials
        {
            get { return _useDefaultCredentials; }
            set { _useDefaultCredentials = value; }
        }

        #endregion

        #region FtpWebRequest Options

        private long? _contentOffset;
        private bool? _enableSsl;
        private string _renameTo;
        private bool? _useBinary;
        private bool? _usePassive;

        /// <summary>Gets or sets a byte offset into the file being downloaded by this request.</summary>
        public long? ContentOffset
        {
            get { return _contentOffset; }
            set { _contentOffset = value; }
        }

        /// <summary>Gets or sets a <see cref="T:System.Boolean"/> that specifies that an SSL connection should be used.</summary>
        public bool? EnableSsl
        {
            get { return _enableSsl; }
            set { _enableSsl = value; }
        }

        /// <summary>Gets or sets the new name of a file being renamed.</summary>
        public string RenameTo
        {
            get { return _renameTo; }
            set { _renameTo = value; }
        }

        /// <summary>Gets or sets a <see cref="T:System.Boolean"/> value that specifies the data type for file transfers.</summary>
        public bool? UseBinary
        {
            get { return _useBinary; }
            set { _useBinary = value; }
        }

        /// <summary>Gets or sets the behavior of a client application's data transfer process.</summary>
        public bool? UsePassive
        {
            get { return _usePassive; }
            set { _usePassive = value; }
        }

        #endregion

        #region HttpWebRequest Options

        private string _accept;
        private bool? _allowAutoRedirect;
        private bool? _allowWriteStreamBuffering;
        private DecompressionMethods? _automaticDecompression;
        private string _connection;
        private HttpContinueDelegate _continueDelegate;
        private CookieContainer _cookieContainer;
        private string _expect;
        private int? _maximumAutomaticRedirections;
        private int? _maximumResponseHeadersLength;
        private string _mediaType;
        private bool? _pipelined;
        private Version _protocolVersion;
        private string _referer;
        private bool? _sendChunked;
        private string _transferEncoding;
        private bool? _unsafeAuthenticatedConnectionSharing;
        private string _userAgent;

        /// <summary>Gets or sets the value of the Accept HTTP header.</summary>
        public string Accept
        {
            get { return _accept; }
            set { _accept = value; }
        }

        /// <summary>Gets or sets a value that indicates whether the request should follow redirection responses.</summary>
        public bool? AllowAutoRedirect
        {
            get { return _allowAutoRedirect; }
            set { _allowAutoRedirect = value; }
        }

        /// <summary>Gets or sets a value that indicates whether to buffer the data sent to the Internet resource.</summary>
        public bool? AllowWriteStreamBuffering
        {
            get { return _allowWriteStreamBuffering; }
            set { _allowWriteStreamBuffering = value; }
        }

        /// <summary>Gets or sets the type of decompression that is used.</summary>
        public DecompressionMethods? AutomaticDecompression
        {
            get { return _automaticDecompression; }
            set { _automaticDecompression = value; }
        }

        /// <summary>Gets or sets the value of the Connection HTTP header.</summary>
        public string Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>Gets or sets the delegate method called when an HTTP 100-continue response is received from the Internet resource.</summary>
        public HttpContinueDelegate ContinueDelegate
        {
            get { return _continueDelegate; }
            set { _continueDelegate = value; }
        }

        /// <summary>Gets or sets the cookies associated with the request.</summary>
        public CookieContainer CookieContainer
        {
            get { return _cookieContainer; }
            set { _cookieContainer = value; }
        }

        /// <summary>Gets or sets the value of the Expect HTTP header.</summary>
        public string Expect
        {
            get { return _expect; }
            set { _expect = value; }
        }

        /// <summary>Gets or sets the maximum number of redirects that the request follows.</summary>
        public int? MaximumAutomaticRedirections
        {
            get { return _maximumAutomaticRedirections; }
            set { _maximumAutomaticRedirections = value; }
        }

        /// <summary>Gets or sets the maximum allowed length of the response headers.</summary>
        public int? MaximumResponseHeadersLength
        {
            get { return _maximumResponseHeadersLength; }
            set { _maximumResponseHeadersLength = value; }
        }

        /// <summary>Gets or sets the media type of the request.</summary>
        public string MediaType
        {
            get { return _mediaType; }
            set { _mediaType = value; }
        }

        /// <summary>Gets or sets a value that indicates whether to pipeline the request to the Internet resource.</summary>
        public bool? Pipelined
        {
            get { return _pipelined; }
            set { _pipelined = value; }
        }

        /// <summary>Gets or sets the version of HTTP to use for the request.</summary>
        public Version ProtocolVersion
        {
            get { return _protocolVersion; }
            set { _protocolVersion = value; }
        }

        /// <summary>Gets or sets the value of the Referer HTTP header.</summary>
        public string Referer
        {
            get { return _referer; }
            set { _referer = value; }
        }

        /// <summary>Gets or sets a value that indicates whether to send data in segments to the Internet resource.</summary>
        public bool? SendChunked
        {
            get { return _sendChunked; }
            set { _sendChunked = value; }
        }

        /// <summary>Gets or sets the value of the Transfer-encoding HTTP header.</summary>
        public string TransferEncoding
        {
            get { return _transferEncoding; }
            set { _transferEncoding = value; }
        }

        /// <summary>Gets or sets a value that indicates whether to allow high-speed NTLM-authenticated connection sharing.</summary>
        public bool? UnsafeAuthenticatedConnectionSharing
        {
            get { return _unsafeAuthenticatedConnectionSharing; }
            set { _unsafeAuthenticatedConnectionSharing = value; }
        }

        /// <summary>Gets or sets the value of the User-agent HTTP header.</summary>
        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        #endregion

        #region Common FtpWebRequest & HttpWebRequest Options

        private X509CertificateCollection _clientCertificates;
        private bool? _keepAlive;
        private int? _readWriteTimeout;

        /// <summary>Gets or sets the collection of security certificates that are associated with this request.</summary>
        public X509CertificateCollection ClientCertificates
        {
            get { return _clientCertificates; }
            set { _clientCertificates = value; }
        }

        /// <summary>Gets or sets a value that indicates whether to make a persistent connection to the Internet resource.</summary>
        public bool? KeepAlive
        {
            get { return _keepAlive; }
            set { _keepAlive = value; }
        }

        /// <summary>Gets or sets a time-out when writing to or reading from a stream.</summary>
        public int? ReadWriteTimeout
        {
            get { return _readWriteTimeout; }
            set { _readWriteTimeout = value; }
        }

        #endregion

        /// <summary>
        /// Applies all options on the current instance to the supplied <see cref="WebRequest"/>.
        /// </summary>
        /// <param name="request">A <see cref="WebRequest"/> that should be configured.</param>
        public void ApplyOptions(WebRequest request)
        {
            if (AuthenticationLevel != null)    request.AuthenticationLevel     = AuthenticationLevel.Value;
            if (CachePolicy != null)            request.CachePolicy             = CachePolicy;
            if (ConnectionGroupName != null)    request.ConnectionGroupName     = ConnectionGroupName;
            if (Credentials != null)            request.Credentials             = Credentials;
            if (Headers != null)                request.Headers                 = Headers;
            if (ImpersonationLevel != null)     request.ImpersonationLevel      = ImpersonationLevel.Value;
            if (PreAuthenticate != null)        request.PreAuthenticate         = PreAuthenticate.Value;
            if (Proxy != null)                  request.Proxy                   = Proxy;
            if (Timeout != null)                request.Timeout                 = Timeout.Value;
            if (UseDefaultCredentials != null)  request.UseDefaultCredentials   = UseDefaultCredentials.Value;

            FtpWebRequest ftpRequest = request as FtpWebRequest;
            if (ftpRequest != null)
            {
                ApplyFtpOptions(ftpRequest);
            }

            HttpWebRequest httpRequest = request as HttpWebRequest;
            if (httpRequest != null)
            {
                ApplyHttpOptions(httpRequest);
            }
        }
        
        private void ApplyFtpOptions(FtpWebRequest ftpRequest)
        {
            if (ContentOffset != null)  ftpRequest.ContentOffset    = ContentOffset.Value;
            if (EnableSsl != null)      ftpRequest.EnableSsl        = EnableSsl.Value;
            if (RenameTo != null)       ftpRequest.RenameTo         = RenameTo;
            if (UseBinary != null)      ftpRequest.UseBinary        = UseBinary.Value;
            if (UsePassive != null)     ftpRequest.UsePassive       = UsePassive.Value;

            if (ClientCertificates != null) ftpRequest.ClientCertificates   = ClientCertificates;
            if (KeepAlive != null)          ftpRequest.KeepAlive            = KeepAlive.Value;
            if (ReadWriteTimeout != null)   ftpRequest.ReadWriteTimeout     = ReadWriteTimeout.Value;
        }
        
        private void ApplyHttpOptions(HttpWebRequest httpRequest)
        {
            if (Accept != null)                                 httpRequest.Accept                                  = Accept;
            if (AllowAutoRedirect != null)                      httpRequest.AllowAutoRedirect                       = AllowAutoRedirect.Value;
            if (AllowWriteStreamBuffering != null)              httpRequest.AllowWriteStreamBuffering               = AllowWriteStreamBuffering.Value;
            if (AutomaticDecompression != null)                 httpRequest.AutomaticDecompression                  = AutomaticDecompression.Value;
            if (Connection != null)                             httpRequest.Connection                              = Connection;
            if (ContinueDelegate != null)                       httpRequest.ContinueDelegate                        = ContinueDelegate;
            if (CookieContainer != null)                        httpRequest.CookieContainer                         = CookieContainer;
            if (Expect != null)                                 httpRequest.Expect                                  = Expect;
            if (MaximumAutomaticRedirections != null)           httpRequest.MaximumAutomaticRedirections            = MaximumAutomaticRedirections.Value;
            if (MaximumResponseHeadersLength != null)           httpRequest.MaximumResponseHeadersLength            = MaximumResponseHeadersLength.Value;
            if (MediaType != null)                              httpRequest.MediaType                               = MediaType;
            if (Pipelined != null)                              httpRequest.Pipelined                               = Pipelined.Value;
            if (ProtocolVersion != null)                        httpRequest.ProtocolVersion                         = ProtocolVersion;
            if (Referer != null)                                httpRequest.Referer                                 = Referer;
            if (SendChunked != null)                            httpRequest.SendChunked                             = SendChunked.Value;
            if (TransferEncoding != null)                       httpRequest.TransferEncoding                        = TransferEncoding;
            if (UnsafeAuthenticatedConnectionSharing != null)   httpRequest.UnsafeAuthenticatedConnectionSharing    = UnsafeAuthenticatedConnectionSharing.Value;
            if (UserAgent != null)                              httpRequest.UserAgent                               = UserAgent;

            if (ClientCertificates != null) httpRequest.ClientCertificates  = ClientCertificates;
            if (KeepAlive != null)          httpRequest.KeepAlive           = KeepAlive.Value;
            if (ReadWriteTimeout != null)   httpRequest.ReadWriteTimeout    = ReadWriteTimeout.Value;
        }
    }
}