// Copyright (c) Sage (UK) Limited 2010. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use this
// code. Please contact Sage (UK) if you do not have such a licence. Sage will take
// appropriate legal action against those who make unauthorised use of this code.

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// TODO
    /// </summary>
    public class SDataRequest
    {
        private readonly IList<RequestOperation> _operations;

        /// <summary>
        /// TODO
        /// </summary>
        public SDataRequest(string uri)
            : this(uri, new RequestOperation())
        {
        }

        /// <summary>
        /// TODO
        /// </summary>
        public SDataRequest(string uri, HttpMethod method)
            : this(uri, new RequestOperation(method, null))
        {
        }

        /// <summary>
        /// TODO
        /// </summary>
        public SDataRequest(string uri, HttpMethod method, ISyndicationResource resource)
            : this(uri, new RequestOperation(method, resource))
        {
        }

        /// <summary>
        /// TODO
        /// </summary>
        public SDataRequest(string uri, params RequestOperation[] operations)
        {
            Uri = uri;
            UserAgent = "Sage";
            Timeout = 120000;
            _operations = new List<RequestOperation>(operations);
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public MediaType? Accept { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public CookieContainer Cookies { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public IList<RequestOperation> Operations
        {
            get { return _operations; }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public SDataResponse GetResponse()
        {
            var uri = Uri;
            var operation = _operations.Count == 1
                           ? _operations[0]
                           : CreateBatchOperation();
            string location = null;

            while (true)
            {
                var request = CreateRequest(uri, operation);
                var response = request.GetResponse();
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
            }

            uri.AppendPath("$batch");
            return new RequestOperation(HttpMethod.Post, feed);
        }

        private WebRequest CreateRequest(string uri, RequestOperation op)
        {
            var request = WebRequest.Create(uri);
            request.Method = op.Method.ToString().ToUpper();
            request.Timeout = Timeout;
            request.Proxy = Proxy;
            request.PreAuthenticate = false;

            var httpRequest = request as HttpWebRequest;
            if (httpRequest != null)
            {
                httpRequest.AllowAutoRedirect = false;
                httpRequest.ReadWriteTimeout = Timeout;
                httpRequest.KeepAlive = false;
                httpRequest.ProtocolVersion = HttpVersion.Version10;

                if (Accept != null)
                {
                    httpRequest.Accept = MediaTypeNames.GetMediaType(Accept.Value);
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
                request.ContentType = op.Resource is AtomFeed
                                          ? MediaTypeNames.AtomFeedMediaType
                                          : MediaTypeNames.AtomEntryMediaType;

                using (var stream = request.GetRequestStream())
                {
                    op.Resource.Save(stream);
                }
            }

            return request;
        }

        private class AsyncResult : IAsyncResult
        {
            private readonly WebRequest _request;
            private readonly IAsyncResult _inner;

            public AsyncResult(WebRequest request, IAsyncResult inner)
            {
                _request = request;
                _inner = inner;
            }

            public WebRequest Request
            {
                get { return _request; }
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
    }
}