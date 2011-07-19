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
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
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
        /// Gets of sets the credentials associated with this request.
        /// </summary>
        public ICredentials Credentials { get; set; }

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

        public IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
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

            var request = CreateRequest(uri, operation);
            return new AsyncResultWrapper<WebResponse>(request.BeginGetResponse, request.EndGetResponse, callback, state);
        }

        public SDataResponse EndGetResponse(IAsyncResult asyncResult)
        {
            var result = asyncResult as AsyncResultWrapper<WebResponse>;
            if (result == null)
            {
                throw new ArgumentException();
            }

            WebResponse response;
            try
            {
                response = result.GetResult();
            }
            catch (WebException ex)
            {
                throw new SDataException(ex);
            }

            return new SDataResponse(response, null);
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

            if (Credentials != null)
            {
                request.Credentials = Credentials;
            }
            else if (!string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(Password))
            {
                var uriPrefix = new Uri(uri);
                var cred = new NetworkCredential(UserName, Password);
                request.Credentials = new CredentialCache
                                      {
                                          {uriPrefix, "Basic", cred},
                                          {uriPrefix, "Digest", cred},
                                          {uriPrefix, "NTLM", cred},
                                          {uriPrefix, "Kerberos", cred},
                                          {uriPrefix, "Negotiate", cred}
                                      };
            }
            else
            {
                request.Credentials = CredentialCache.DefaultCredentials;
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
                    var requestStream = op.Files.Count > 0 ? new MemoryStream() : stream;
                    MediaType mediaType;

                    if (op.Resource is ISyndicationResource)
                    {
                        mediaType = op.Resource is AtomFeed ? MediaType.Atom : MediaType.AtomEntry;
                        ((ISyndicationResource) op.Resource).Save(requestStream);
                    }
                    else if (op.Resource is IXmlSerializable)
                    {
                        mediaType = MediaType.Xml;

                        using (var xmlWriter = XmlWriter.Create(requestStream))
                        {
                            ((IXmlSerializable) op.Resource).WriteXml(xmlWriter);
                        }
                    }
                    else if (op.Resource is string)
                    {
                        mediaType = MediaType.Text;

                        using (var writer = new StreamWriter(requestStream))
                        {
                            writer.Write((string) op.Resource);
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }

                    if (op.ContentType != null)
                    {
                        mediaType = op.ContentType.Value;
                    }

                    var contentType = MediaTypeNames.GetMediaType(mediaType);

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

        #region Nested type: AsyncResultWrapper

        private class AsyncResultWrapper<T> : IAsyncResult
        {
            private readonly IAsyncResult _inner;
            private readonly Func<IAsyncResult, T> _end;

            public AsyncResultWrapper(Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, T> end, AsyncCallback callback, object state)
            {
                _inner = begin(callback != null ? asyncResult => callback(this) : (AsyncCallback) null, state);
                _end = end;
            }

            public T GetResult()
            {
                return _end(_inner);
            }

            #region IAsyncResult Members

            public bool IsCompleted
            {
                get { return _inner.IsCompleted; }
            }

            public WaitHandle AsyncWaitHandle
            {
                get { return _inner.AsyncWaitHandle; }
            }

            public object AsyncState
            {
                get { return _inner.AsyncState; }
            }

            public bool CompletedSynchronously
            {
                get { return _inner.CompletedSynchronously; }
            }

            #endregion
        }

        #endregion
    }
}