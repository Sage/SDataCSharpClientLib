using System.Net;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Exception thrown by SDataService class
    /// </summary>
    public class SDataServiceException : WebException
    {
        private readonly SDataDiagnosis _diagnosis;

        public SDataDiagnosis Diagnosis
        {
            get { return _diagnosis; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SDataServiceException(string message, WebException innerException)
            : base(message, innerException)
        {
            if (innerException.Response == null)
            {
                return;
            }

            XPathNavigator nav;

            using (var stream = innerException.Response.GetResponseStream())
            {
                nav = new XPathDocument(stream).CreateNavigator();
            }

            var mgr = new XmlNamespaceManager(nav.NameTable);
            mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            mgr.AddNamespace("sdata", "http://schemas.sage.com/sdata/2008/1");
            nav.MoveToChild(XPathNodeType.Element);
            var diagnosis = new SDataDiagnosis();

            if (diagnosis.Load(nav, mgr))
            {
                _diagnosis = diagnosis;
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