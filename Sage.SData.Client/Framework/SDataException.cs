using System;
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
        private readonly Diagnosis _diagnosis;
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

            var serializer = new XmlSerializer(typeof (Diagnosis));

            using (var stream = Response.GetResponseStream())
            {
                try
                {
                    _diagnosis = (Diagnosis) serializer.Deserialize(stream);
                }
                catch (XmlException)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        /// <summary>
        /// Gets the high level diagnostic information returned from the server.
        /// </summary>
        public Diagnosis Diagnosis
        {
            get { return _diagnosis; }
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
            get { return _diagnosis != null ? _diagnosis.Message : base.Message; }
        }
    }
}