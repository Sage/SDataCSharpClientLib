using System;
using System.Collections.Generic;
using System.Text;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Base class for all SData Requests
    /// </summary>
    public class SDataBaseRequest : ISDataRequestSettings
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
        /// Acessor method for protocol, 
        /// </summary>
        /// <remarks>HTTP is the default but can be HTTPS</remarks>
        public string Protocol
        {
            get { return _protocol; }
            set { _protocol = value;}
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
            set{ _virtualDirectory = value;}

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


        /// <summary>
        /// function to format url string for the request
        /// </summary>
        /// <returns>formatted string</returns>
        public override string ToString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">an ISDataService to use for thie request</param>
        public SDataBaseRequest(ISDataService service)
        {
            if(service == null)
            {
                throw new SDataClientException("SDataService is null");
            }
            if(!service.Initialized)
            {
                throw new SDataClientException("Service Not Initialized");
            }
            Service = service;

            this.Protocol = service.Protocol;
            this.ServerName = service.ServerName;
            this.VirtualDirectory = service.VirtualDirectory;
            
        }

        /// <summary>
        /// parameterless contstructor
        /// </summary>
        protected SDataBaseRequest()
        {
            
        }


        /// <summary>
        /// Sends a PUT request to the server based on the the request configuration
        /// </summary>
        /// <param name="service">service configuration</param>
        /// <returns>ISydicationResource either a AtomEntry or AtomFeed depending on the the class implementing the interface</returns>
        public ISyndicationResource Create(ISDataService service)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates an aysnchronous transaction to the server
        /// </summary>
        /// <param name="uuid">unquiqe identifier for the transaction</param>
        /// <returns>AsynRequest used to handle the transaction </returns>
        protected AsyncRequest CreateAsync(string uuid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends a GET request to the server based on the request
        /// Reads an AtomFeed or AtomEntry from the server
        /// <param name="service">server service configuration</param>
        /// <returns>AtomFeed or AtomEntry depending on the class implenting the interface</returns>
        /// </summary>
        protected  ISyndicationResource Read(ISDataService service)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends a POST request to the server based on the class implenting the interace.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        protected  ISyndicationResource Update(ISDataService service)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends a POST request to the server based on the requesst
        /// Deletes and atom feed from the server
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        protected  bool Delete(ISDataService service)
        {
            throw new NotImplementedException();
        }

    }
}
