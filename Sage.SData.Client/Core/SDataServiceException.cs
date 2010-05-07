using System.Net;
using System.Xml;
using System.Xml.Serialization;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Exception thrown by SDataService class
    /// </summary>
    public class SDataServiceException : WebException
    {
        private readonly Diagnosis _diagnosis;

        /// <summary>
        /// An object containing additional diagnostic information about the error.
        /// </summary>
        public Diagnosis Diagnosis
        {
            get { return _diagnosis; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="innerException"></param>
        public SDataServiceException(WebException innerException)
            : base(innerException.Message, innerException)
        {
            if (innerException.Response == null)
            {
                return;
            }

            var serializer = new XmlSerializer(typeof (Diagnosis));

            using (var stream = innerException.Response.GetResponseStream())
            {
                try
                {
                    _diagnosis = (Diagnosis) serializer.Deserialize(stream);
                }
                catch (XmlException)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// gets the exception message
        /// </summary>
        public override string Message
        {
            get { return _diagnosis != null ? _diagnosis.Message : base.Message; }
        }
    }
}