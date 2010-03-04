namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Base class for all SData Requests
    /// </summary>
    public abstract class SDataBaseRequest : ISDataRequestSettings
    {
        /// <summary>
        /// protocol value for http
        /// </summary>
        /// <remarks>the default</remarks>
        public const string Http = "http";

        /// <summary>
        /// protocl value for https
        /// </summary>
        public const string Https = "https";

        /// <summary>
        /// Accessor method for protocol, 
        /// </summary>
        /// <remarks>HTTP is the default but can be HTTPS</remarks>
        public string Protocol { get; set; }

        /// <summary>
        /// Accessor method for Server
        /// </summary>
        /// <remarks>IP address is also allowed (192.168.1.1).
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
        /// the ISDataService to use for this request
        /// </summary>
        public ISDataService Service { get; set; }

        /// <summary>
        /// gets or sets the user agent
        /// </summary>
        public string UserAgent { get; set; }

        protected virtual void BuildUrl(UrlBuilder builder)
        {
            var server = ServerName;
            var pos = server.IndexOf(':');

            if (pos >= 0)
            {
                var port = int.Parse(server.Substring(pos + 1));
                server = server.Substring(0, pos);
                builder.Port = port;
            }

            builder.Scheme = Protocol;
            builder.Host = server;
            builder.PathSegments.Add(VirtualDirectory);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">an ISDataService to use for thie request</param>
        protected SDataBaseRequest(ISDataService service)
        {
            if (service == null)
            {
                throw new SDataClientException("SDataService is null");
            }
            if (!service.Initialized)
            {
                throw new SDataClientException("Service Not Initialized");
            }

            Service = service;
            Protocol = service.Protocol;
            ServerName = service.ServerName;
            VirtualDirectory = service.VirtualDirectory;
        }

        /// <summary>
        /// function to format url string for the request
        /// </summary>
        /// <returns>formatted string</returns>
        public override string ToString()
        {
            var builder = new UrlBuilder();
            BuildUrl(builder);
            return builder.ToString();
        }
    }
}