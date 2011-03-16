// Copyright (c) Sage (UK) Limited 2010. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use this
// code. Please contact Sage (UK) if you do not have such a licence. Sage will take
// appropriate legal action against those who make unauthorised use of this code.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;
using Sage.SData.Client.Extensions;
using Sage.SData.Client.Mime;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// The request class which is responsible for sending and
    /// receiving data over HTTP with the server.
    /// </summary>
    public class SDataRequest
    {
        private readonly IList<RequestOperation> _operations;
        private bool _proxySet;
        private IWebProxy _proxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SDataRequest"/> class.
        /// </summary>
        public SDataRequest(string uri)
            : this(uri, new RequestOperation())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDataRequest"/> class.
        /// </summary>
        public SDataRequest(string uri, HttpMethod method)
            : this(uri, new RequestOperation(method, null))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDataRequest"/> class.
        /// </summary>
        public SDataRequest(string uri, HttpMethod method, ISyndicationResource resource)
            : this(uri, new RequestOperation(method, resource))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDataRequest"/> class.
        /// </summary>
        public SDataRequest(string uri, params RequestOperation[] operations)
        {
            Uri = uri;
            UserAgent = "Sage";
            Timeout = 120000;
            _operations = new List<RequestOperation>(operations);
        }

        /// <summary>
        /// Gets or sets the target uri used by requests.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the user name used by requests.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password used by requests.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user agent passed during requests.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the timeout in milliseconds used during requests.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the proxy used by requests.
        /// </summary>
        public IWebProxy Proxy
        {
            get { return _proxy; }
            set
            {
                _proxySet = true;
                _proxy = value;
            }
        }

        /// <summary>
        /// Gets or sets the accept media types accepted by requests.
        /// </summary>
        public MediaType[] Accept { get; set; }

        /// <summary>
        /// Gets or sets the cookies associated with this request.
        /// </summary>
        public CookieContainer Cookies { get; set; }

        /// <summary>
        /// Lists the operations associated with this request.
        /// </summary>
        public IList<RequestOperation> Operations
        {
            get { return _operations; }
        }

        /// <summary>
        /// Execute the request and return a response object.
        /// </summary>
        public SDataResponse GetResponse()
        {
            var uri = Uri;
            RequestOperation operation;

            if (_operations.Count == 1)
            {
                operation = _operations[0];
            }
            else
            {
                operation = CreateBatchOperation();
                uri = new SDataUri(uri).AppendPath("$batch").ToString();
            }

            string location = null;

            while (true)
            {
                var request = CreateRequest(uri, operation);

                WebResponse response;
                try
                {
                    response = request.GetResponse();
                }
                catch (WebException ex)
                {
                    throw new SDataException(ex);
                }

                var httpResponse = response as HttpWebResponse;
                var statusCode = httpResponse != null ? httpResponse.StatusCode : 0;

                if (statusCode != HttpStatusCode.Found)
                    return new SDataResponse(response, location);

                uri = location = response.Headers[HttpResponseHeader.Location];
            }
        }

        private RequestOperation CreateBatchOperation()
        {
            var uri = new SDataUri(Uri);

            if (uri.PathSegments.Length != 4)
            {
                throw new InvalidOperationException("Batch requests can only be made on collection end points");
            }

            var feed = new AtomFeed();
            var batchOp = new RequestOperation(HttpMethod.Post, feed);

            foreach (var op in _operations)
            {
                AtomEntry entry;

                if (op.Resource == null)
                {
                    if (op.Method != HttpMethod.Post)
                    {
                        throw new InvalidOperationException("A predicate must be specified for GET, PUT and DELETE batch requests");
                    }

                    var entryUri = new SDataUri(uri) {CollectionPredicate = op.Predicate};
                    entry = new AtomEntry {Id = new AtomId(entryUri.Uri)};
                }
                else
                {
                    entry = op.Resource as AtomEntry;

                    if (entry == null)
                    {
                        throw new InvalidOperationException("Only atom entry resources can be submitted in batch requests");
                    }
                }

                entry.SetSDataHttpMethod(op.Method);

                if (!string.IsNullOrEmpty(op.ETag))
                {
                    entry.SetSDataHttpIfMatch(op.ETag);
                }

                feed.AddEntry(entry);

                foreach (var file in op.Files)
                {
                    batchOp.Files.Add(file);
                }
            }

            return batchOp;
        }

        private WebRequest CreateRequest(string uri, RequestOperation op)
        {
            var request = WebRequest.Create(uri);
            request.Method = op.Method.ToString().ToUpper();
            request.Timeout = Timeout;
            request.PreAuthenticate = true;

            if (_proxySet)
            {
                request.Proxy = _proxy;
            }

            var httpRequest = request as HttpWebRequest;
            if (httpRequest != null)
            {
                httpRequest.AllowAutoRedirect = false;
                httpRequest.ReadWriteTimeout = Timeout;
                httpRequest.KeepAlive = false;
                httpRequest.ProtocolVersion = HttpVersion.Version10;

                if (Accept != null)
                {
                    httpRequest.Accept = string.Join(",", Array.ConvertAll(Accept, type => MediaTypeNames.GetMediaType(type)));
                }

                if (Cookies != null)
                {
                    httpRequest.CookieContainer = Cookies;
                }

                if (!string.IsNullOrEmpty(UserAgent))
                {
                    httpRequest.UserAgent = UserAgent;
                }
            }

            if (!string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(Password))
            {
                var cred = new NetworkCredential(UserName, Password);
                request.Credentials = new CredentialCache
                                      {
                                          {new Uri(uri), "Digest", cred},
                                          {new Uri(uri), "Basic", cred}
                                      };
            }

            if (!string.IsNullOrEmpty(op.ETag))
            {
                var header = op.Method == HttpMethod.Get
                                 ? HttpRequestHeader.IfNoneMatch
                                 : HttpRequestHeader.IfMatch;
                request.Headers[header] = op.ETag;
            }

            if (op.Resource != null)
            {
                using (var stream = request.GetRequestStream())
                {
                    var contentType = op.Resource is AtomFeed
                                          ? MediaTypeNames.AtomFeedMediaType
                                          : MediaTypeNames.AtomEntryMediaType;

                    var requestStream = op.Files.Count > 0 ? new MemoryStream() : stream;
                    op.Resource.Save(requestStream);

                    if (op.Files.Count > 0)
                    {
                        requestStream.Seek(0, SeekOrigin.Begin);
                        var part = new MimePart(requestStream) {ContentType = contentType};

                        using (var multipart = new MimeMessage(part))
                        {
                            contentType = new ContentType("multipart/related") {Boundary = multipart.Boundary}.ToString();

                            foreach (var file in op.Files)
                            {
                                var type = !string.IsNullOrEmpty(file.ContentType) ? file.ContentType : "application/octet-stream";
                                var disposition = new ContentDisposition(DispositionTypeNames.Attachment) {FileName = file.FileName};
                                part = new MimePart(file.Stream)
                                       {
                                           ContentType = type,
                                           ContentTransferEncoding = "binary",
                                           ContentDisposition = disposition
                                       };
                                multipart.Add(part);
                            }

                            multipart.WriteTo(stream);
                        }
                    }

                    request.ContentType = contentType;
                }
            }

            return request;
        }
    }
}