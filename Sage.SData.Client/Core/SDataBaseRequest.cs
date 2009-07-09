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
        public const string HTTP = "http";

        /// <summary>
        /// protocl value for https
        /// </summary>
        public const string HTTPS = "https";

        private string _protocol;

        /// <summary>
        /// Accessor method for protocol, 
        /// </summary>
        /// <remarks>HTTP is the default but can be HTTPS</remarks>
        public string Protocol
        {
            get { return _protocol; }
            set { _protocol = value; }
        }

        private string _serverName;

        /// <summary>
        /// Accessor method for Server
        /// </summary>
        /// <remarks>IP address is also allowed (192.168.1.1).
        /// Can be followed by port number. For example www.example.com:5493. 
        /// 5493 is the recommended port number for SData services that are not exposed on the Internet.
        /// </remarks>
        public string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }

        private string _virtualDirectory;

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

        private ISDataService _service;

        /// <summary>
        /// the ISDataService to use for this request
        /// </summary>
        public ISDataService Service
        {
            get { return _service; }
            set { _service = value; }
        }

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
        /// parameterless constructor
        /// </summary>
        protected SDataBaseRequest() {}

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