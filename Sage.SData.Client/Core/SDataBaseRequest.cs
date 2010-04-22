using System.Collections.Generic;
using Sage.SData.Client.Common;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Base class for all SData Requests
    /// </summary>
    public abstract class SDataBaseRequest : ISDataRequestSettings
    {
        /// <summary>
        /// Protocol value for http
        /// </summary>
        /// <remarks>the default</remarks>
        public const string Http = "http";

        /// <summary>
        /// Protocol value for https
        /// </summary>
        public const string Https = "https";

        private readonly ISDataService _service;
        private readonly SDataUri _uri;

        protected SDataUri Uri
        {
            get { return _uri; }
        }

        /// <summary>
        /// Accessor method for protocol
        /// </summary>
        /// <remarks>HTTP is the default but can be HTTPS</remarks>
        public string Protocol
        {
            get { return _uri.Scheme; }
            set { _uri.Scheme = value; }
        }

        /// <summary>
        /// Accessor method for server
        /// </summary>
        /// <remarks>IP address is also allowed (192.168.1.1).
        /// Can be followed by port number. For example www.example.com:5493. 
        /// 5493 is the recommended port number for SData services that are not exposed on the Internet.
        /// </remarks>
        public string ServerName
        {
            get { return _uri.Host; }
            set
            {
                if (_uri.Host != value)
                {
                    var pos = value.IndexOf(':');
                    int port;

                    if (pos >= 0 && int.TryParse(value.Substring(pos + 1), out port))
                    {
                        _uri.Host = value.Substring(0, pos);
                        _uri.Port = port;
                    }
                    else
                    {
                        _uri.Host = value;
                    }
                }
            }
        }

        /// <summary>
        /// Accessor method for port
        /// </summary>
        public int? Port
        {
            get { return _uri.Port; }
            set { _uri.Port = value ?? 80; }
        }

        /// <summary>
        /// Accessor method for virtual directory
        /// </summary>
        /// <remarks>Must be sdata, unless the technical framework imposes something different.
        ///</remarks>
        public string VirtualDirectory
        {
            get { return _uri.Server; }
            set { _uri.Server = value; }
        }

        /// <summary>
        /// Dictionary of query name-value pairs
        /// </summary>
        /// <example>where, salesorderamount lt 15.00
        /// orderby, salesorderid
        /// </example>
        public IDictionary<string, string> QueryValues
        {
            get { return _uri.QueryArgs; }
        }

        /// <summary>
        /// the ISDataService to use for this request
        /// </summary>
        public ISDataService Service
        {
            get { return _service; }
        }

        protected virtual void BuildUrl(SDataUri uri)
        {
        }

        protected SDataBaseRequest(ISDataService service)
        {
            Guard.ArgumentNotNull(service, "service");

            _service = service;
            _uri = new SDataUri();
            Protocol = service.Protocol;
            ServerName = service.ServerName;
            Port = service.Port;
            VirtualDirectory = !string.IsNullOrEmpty(service.VirtualDirectory) ? service.VirtualDirectory : "sdata";
        }

        /// <summary>
        /// function to format url string for the request
        /// </summary>
        /// <returns>formatted string</returns>
        public override string ToString()
        {
            var uri = new SDataUri(Uri);
            BuildUrl(uri);
            return uri.ToString();
        }
    }
}