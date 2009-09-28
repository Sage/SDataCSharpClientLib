using System.Net;

namespace Sage.SData.Client.Common
{
    ///<summary>
    /// Holds shared contextual information about the web request.
    ///</summary>
    public struct WebRequestContext
    {
        private readonly ICredentials credentials;
        private readonly IWebProxy proxy;
        private readonly CookieContainer cookies;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequestContext"/> class using the specified <see cref="ICredentials">credentials</see>.
        /// </summary>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the source resource when required. This value can be <b>null</b>.
        /// </param>
        public WebRequestContext(ICredentials credentials)
            : this(credentials, null, null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequestContext"/> class using the specified <see cref="ICredentials">credentials</see> and <see cref="IWebProxy">proxy</see>.
        /// </summary>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the source resource when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="proxy">
        ///     A <see cref="IWebProxy"/> that provides proxy access to the source resource when required. This value can be <b>null</b>.
        /// </param>
        public WebRequestContext(ICredentials credentials, IWebProxy proxy)
            : this(credentials, proxy, null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequestContext"/> class using the specified <see cref="ICredentials">credentials</see>, <see cref="ICredentials">credentials</see> and <see cref="CookieContainer">cookies</see>.
        /// </summary>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the source resource when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="proxy">
        ///     A <see cref="IWebProxy"/> that provides proxy access to the source resource when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="cookies">
        ///     A <see cref="CookieCollection"/> that provides somewhere shared state can be stored between requests. This value can be <b>null</b>.
        /// </param>
        public WebRequestContext(ICredentials credentials, IWebProxy proxy, CookieContainer cookies)
        {
            this.credentials = credentials;
            this.proxy = proxy;
            this.cookies = cookies;
        }

        #region Credentials
        /// <summary>
        /// Gets the authentication credentials utilized by the client when making the remote procedure call.
        /// </summary>
        /// <value>
        ///     A <see cref="ICredentials"/> that represents the authentication credentials utilized by the client when making the remote procedure call. 
        ///     If no credentials were provided, returns <b>null</b>.
        /// </value>
        public ICredentials Credentials
        {
            get { return credentials; }
        }
        #endregion

        #region Proxy
        /// <summary>
        /// Gets the web proxy utilized by the client to proxy the remote procedure call.
        /// </summary>
        /// <value>
        ///     A <see cref="IWebProxy"/> that represents the web proxy utilized by the client to proxy the remote procedure call. 
        ///     If no proxy was used, returns <b>null</b>.
        /// </value>
        public IWebProxy Proxy
        {
            get { return proxy; }
        }
        #endregion

        #region Cookies
        /// <summary>
        /// Gets or sets the cookie container used by the client to share state between requests.
        /// </summary>
        /// <value>
        ///     A <see cref="CookieContainer"/> object that represents the cookie container used by the client to share state between requests.
        ///     The default is a null reference (Nothing in Visual Basic), which indicates no cookies will be shared between requests.
        /// </value>
        public CookieContainer Cookies
        {
            get { return cookies; }
        }
        #endregion
    }
}