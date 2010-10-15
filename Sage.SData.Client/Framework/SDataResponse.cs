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
using System.Xml;
using System.Xml.Serialization;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Mime;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// An interfact which encapsulates interesting information returned
    /// from a request.
    /// </summary>
    public interface ISDataResponse
    {
        /// <summary>
        /// The response status code.
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// The response content type.
        /// </summary>
        MediaType? ContentType { get; }

        /// <summary>
        /// The response ETag.
        /// </summary>
        string ETag { get; }

        /// <summary>
        /// The response location.
        /// </summary>
        string Location { get; }

        /// <summary>
        /// The response content.
        /// </summary>
        object Content { get; }
    }

    /// <summary>
    /// The response class which encapsulates interesting information returned
    /// from a request.
    /// </summary>
    public class SDataResponse : ISDataResponse
    {
        private readonly HttpStatusCode _statusCode;
        private readonly MediaType? _contentType;
        private readonly string _eTag;
        private readonly string _location;
        private readonly object _content;
        private readonly IList<AttachedFile> _files;

        internal SDataResponse(WebResponse response, string redirectLocation)
        {
            var httpResponse = response as HttpWebResponse;
            _statusCode = httpResponse != null ? httpResponse.StatusCode : 0;

            MediaType contentType;
            if (MediaTypeNames.TryGetMediaType(response.ContentType, out contentType))
            {
                _contentType = contentType;
            }

            _eTag = response.Headers[HttpResponseHeader.ETag];
            _location = response.Headers[HttpResponseHeader.Location] ?? redirectLocation;
            _files = new List<AttachedFile>();

            if (_statusCode != HttpStatusCode.NoContent && _contentType != null)
            {
                using (var responseStream = response.GetResponseStream())
                {
                    string boundary;

                    if (_contentType == MediaType.Multipart && TryGetMultipartBoundary(response.ContentType, out boundary))
                    {
                        var multipart = MimeMessage.Parse(responseStream, boundary);
                        var isFirst = true;

                        foreach (var part in multipart)
                        {
                            if (isFirst)
                            {
                                _contentType = MediaTypeNames.GetMediaType(part.ContentType);
                                _content = LoadContent(part.Content, _contentType.Value);
                                isFirst = false;
                            }
                            else
                            {
                                var fileName = part.ContentDisposition != null ? part.ContentDisposition.FileName : null;
                                _files.Add(new AttachedFile(part.ContentType, fileName, part.Content));
                            }
                        }
                    }
                    else
                    {
                        _content = LoadContent(responseStream, _contentType.Value);
                    }
                }
            }
        }

        private static bool TryGetMultipartBoundary(string contentType, out string boundary)
        {
            ContentType type;

            try
            {
                type = new ContentType(contentType);
                boundary = type.Boundary;
                return !string.IsNullOrEmpty(boundary);
            }
            catch (FormatException)
            {
                boundary = null;
                return false;
            }
        }

        /// <summary>
        /// The response status code.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return _statusCode; }
        }

        /// <summary>
        /// The response content type.
        /// </summary>
        public MediaType? ContentType
        {
            get { return _contentType; }
        }

        /// <summary>
        /// The response ETag.
        /// </summary>
        public string ETag
        {
            get { return _eTag; }
        }

        /// <summary>
        /// The response location.
        /// </summary>
        public string Location
        {
            get { return _location; }
        }

        /// <summary>
        /// The response content.
        /// </summary>
        public object Content
        {
            get { return _content; }
        }

        /// <summary>
        /// Gets the files attached to the response.
        /// </summary>
        public IList<AttachedFile> Files
        {
            get { return _files; }
        }

        private static object LoadContent(Stream stream, MediaType contentType)
        {
            switch (contentType)
            {
                case MediaType.Atom:
                    return LoadFeedContent(stream);
                case MediaType.AtomEntry:
                    return LoadEntryContent(stream);
                default:
                    return LoadOtherContent(stream, contentType);
            }
        }

        private static AtomFeed LoadFeedContent(Stream stream)
        {
            var feed = new AtomFeed();
            feed.Load(stream);
            return feed;
        }

        private static AtomEntry LoadEntryContent(Stream stream)
        {
            var entry = new AtomEntry();
            entry.Load(stream);
            return entry;
        }

        private static object LoadOtherContent(Stream stream, MediaType contentType)
        {
            if (contentType == MediaType.Xml)
            {
                using (var memory = new MemoryStream())
                {
                    int num;
                    var buffer = new byte[0x1000];

                    while ((num = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        memory.Write(buffer, 0, num);
                    }

                    memory.Seek(0, SeekOrigin.Begin);
                    var content = LoadTrackingContent(memory);

                    if (content != null)
                    {
                        return content;
                    }

                    memory.Seek(0, SeekOrigin.Begin);
                    return LoadStringContent(memory);
                }
            }

            return LoadStringContent(stream);
        }

        private static Tracking LoadTrackingContent(Stream stream)
        {
            var serializer = new XmlSerializer(typeof (Tracking));

            try
            {
                return (Tracking) serializer.Deserialize(stream);
            }
            catch (XmlException)
            {
            }
            catch (InvalidOperationException)
            {
            }

            return null;
        }

        private static string LoadStringContent(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}