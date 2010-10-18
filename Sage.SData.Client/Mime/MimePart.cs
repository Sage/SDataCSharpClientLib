using System;
using System.IO;
using System.Net;
using System.Net.Mime;

namespace Sage.SData.Client.Mime
{
    /// <summary>
    /// Represents a MIME protocol message part.
    /// </summary>
    public class MimePart : IDisposable
    {
        private readonly Stream _content;
        private readonly WebHeaderCollection _headers;

        /// <summary>
        /// Initializes a new instance of the <see cref="MimePart"/> class using the supplied content.
        /// </summary>
        /// <param name="content">The content of the MIME part.</param>
        public MimePart(Stream content)
            : this(content, new WebHeaderCollection())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MimePart"/> class using the supplied content and headers.
        /// </summary>
        /// <param name="content">The content of the MIME part.</param>
        /// <param name="headers">The headers of the MIME part.</param>
        public MimePart(Stream content, WebHeaderCollection headers)
        {
            _headers = headers;
            _content = content;
        }

        /// <summary>
        /// Gets the headers of the MIME part.
        /// </summary>
        public WebHeaderCollection Headers
        {
            get { return _headers; }
        }

        /// <summary>
        /// Gets the content type of the MIME part.
        /// </summary>
        public string ContentType
        {
            get { return _headers[HttpRequestHeader.ContentType]; }
            set { SetHeaderValue(HttpRequestHeader.ContentType, value); }
        }

        /// <summary>
        /// Gets the content length of the MIME part.
        /// </summary>
        public int? ContentLength
        {
            get
            {
                int length;
                return int.TryParse(_headers[HttpRequestHeader.ContentLength], out length) ? length : (int?) null;
            }
            set { SetHeaderValue(HttpRequestHeader.ContentLength, value != null ? value.ToString() : null); }
        }

        /// <summary>
        /// Gets the content transfer encoding of the MIME part.
        /// </summary>
        public string ContentTransferEncoding
        {
            get { return _headers["Content-Transfer-Encoding"]; }
            set { SetHeaderValue("Content-Transfer-Encoding", value); }
        }

        /// <summary>
        /// Gets the content disposition of the MIME part.
        /// </summary>
        public ContentDisposition ContentDisposition
        {
            get
            {
                var value = _headers["Content-Disposition"];
                try
                {
                    return value != null ? new ContentDisposition(value) : null;
                }
                catch (FormatException)
                {
                    return null;
                }
            }
            set { SetHeaderValue("Content-Disposition", value != null ? value.ToString() : null); }
        }

        /// <summary>
        /// Gets the content stream of the MIME part.
        /// </summary>
        public Stream Content
        {
            get { return _content; }
        }

        private void SetHeaderValue(HttpRequestHeader name, string value)
        {
            if (value != null)
            {
                _headers[name] = value;
            }
            else
            {
                _headers.Remove(name);
            }
        }

        private void SetHeaderValue(string name, string value)
        {
            if (value != null)
            {
                _headers[name] = value;
            }
            else
            {
                _headers.Remove(name);
            }
        }

        /// <summary>
        /// Writes the MIME part to the specified stream writer.
        /// </summary>
        /// <param name="writer">The destination writer to write to.</param>
        /// <param name="boundary">The unique string used to designated the beginning of the part.</param>
        public void WriteTo(StreamWriter writer, string boundary)
        {
            writer.WriteLine("--{0}", boundary);

            if (_content != null && _headers[HttpRequestHeader.ContentLength] == null)
            {
                _headers[HttpRequestHeader.ContentLength] = _content.Length.ToString();
            }

            writer.Write(_headers);

            if (_content != null)
            {
                writer.Flush();
                CopyStream(_content, writer.BaseStream);
            }

            writer.WriteLine();
        }

        private static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[0x1000];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_content != null)
            {
                _content.Dispose();
            }
        }

        #endregion
    }
}