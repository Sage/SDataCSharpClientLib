using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// The exception that is thrown when an error occurs on an SData server.
    /// </summary>
    public class SDataException : WebException
    {
        private readonly Collection<Diagnosis> _diagnoses;
        private readonly HttpStatusCode? _statusCode;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="innerException"></param>
        public SDataException(WebException innerException)
            : base(innerException.Message, innerException, innerException.Status, innerException.Response)
        {
            if (Response == null)
            {
                return;
            }

            var httpResponse = Response as HttpWebResponse;
            _statusCode = httpResponse != null ? httpResponse.StatusCode : (HttpStatusCode?) null;
            MediaType mediaType;

            if (MediaTypeNames.TryGetMediaType(Response.ContentType, out mediaType) && mediaType == MediaType.Xml)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var responseStream = Response.GetResponseStream())
                    {
                        CopyStream(responseStream, memoryStream);
                    }

                    _diagnoses = Deserialize<Diagnoses>(memoryStream);

                    if (_diagnoses == null)
                    {
                        var diagnosis = Deserialize<Diagnosis>(memoryStream);

                        if (diagnosis != null)
                        {
                            _diagnoses = new Collection<Diagnosis> {diagnosis};
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the high level diagnostic information returned from the server.
        /// </summary>
        [Obsolete("Use the Diagnoses property instead.")]
        public Diagnosis Diagnosis
        {
            get { return _diagnoses != null && _diagnoses.Count > 0 ? _diagnoses[0] : null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<Diagnosis> Diagnoses
        {
            get { return _diagnoses; }
        }

        /// <summary>
        /// Gets the HTTP status code associated with the exception.
        /// </summary>
        public HttpStatusCode? StatusCode
        {
            get { return _statusCode; }
        }

        /// <summary>
        /// Gets a message that describes the exception.
        /// </summary>
        public override string Message
        {
            get
            {
                return _diagnoses != null
                           ? string.Join(Environment.NewLine, _diagnoses.Select(diagnosis => diagnosis.Message).ToArray())
                           : base.Message;
            }
        }

        private static void CopyStream(Stream source, Stream destination)
        {
            var buffer = new byte[0x1000];
            int num;
            while ((num = source.Read(buffer, 0, buffer.Length)) != 0)
            {
                destination.Write(buffer, 0, num);
            }
        }

        private static T Deserialize<T>(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var serializer = new XmlSerializer(typeof (T));

            try
            {
                return (T) serializer.Deserialize(stream);
            }
            catch (XmlException)
            {
            }
            catch (InvalidOperationException)
            {
            }

            return default(T);
        }
    }
}