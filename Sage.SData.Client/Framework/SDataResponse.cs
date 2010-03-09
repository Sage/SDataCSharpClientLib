// Copyright (c) Sage (UK) Limited 2010. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use this
// code. Please contact Sage (UK) if you do not have such a licence. Sage will take
// appropriate legal action against those who make unauthorised use of this code.

using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// TODO
    /// </summary>
    public class SDataResponse
    {
        private readonly HttpStatusCode _statusCode;
        private readonly MediaType _contentType;
        private readonly string _eTag;
        private readonly string _location;
        private readonly object _content;

        internal SDataResponse(WebResponse response)
        {
            var httpResponse = response as HttpWebResponse;
            _statusCode = httpResponse != null ? httpResponse.StatusCode : 0;
            _contentType = MediaTypeNames.GetMediaType(response.ContentType);
            _eTag = response.Headers[HttpResponseHeader.ETag];
            _location = response.Headers[HttpResponseHeader.Location];

            if (_statusCode != HttpStatusCode.NoContent)
            {
                using (var stream = response.GetResponseStream())
                {
                    switch (_contentType)
                    {
                        case MediaType.Atom:
                        {
                            var feed = new AtomFeed();
                            feed.Load(stream);
                            _content = feed;
                            break;
                        }
                        case MediaType.AtomEntry:
                        {
                            var entry = new AtomEntry();
                            entry.Load(stream);
                            _content = entry;
                            break;
                        }
                        default:
                        {
                            if (_contentType == MediaType.Xml)
                            {
                                var serializer = new XmlSerializer(typeof (SDataTracking));

                                try
                                {
                                    _content = serializer.Deserialize(stream);
                                    break;
                                }
                                catch (XmlException)
                                {
                                }
                            }

                            using (var reader = new StreamReader(stream))
                            {
                                _content = reader.ReadToEnd();
                            }

                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return _statusCode; }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public MediaType ContentType
        {
            get { return _contentType; }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string ETag
        {
            get { return _eTag; }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string Location
        {
            get { return _location; }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public object Content
        {
            get { return _content; }
        }
    }
}