using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Exception thrown by SDataService class
    /// </summary>
    public class SDataServiceException : WebException
    {

        private string _data;
        /// <summary>
        /// data from underlying webexception stream
        /// </summary>
        public new string Data
        {
            get{ return _data;}
            set{ _data = value;}
        }

        /// <summary>
        /// constructor
        /// </summary>
        public SDataServiceException()
            :base() {}
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message"></param>
        public SDataServiceException(String message)
            :base(message) {}
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SDataServiceException(String message, WebException innerException)
            :base(message, innerException) {}

        /// <summary>
        /// gets the exception message
        /// </summary>
        public override string Message
        {
            get
            {
               XmlDocument doc = new XmlDocument();
                doc.LoadXml(Data);

                XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
                manager.AddNamespace("sdata", "http://schemas.sage.com/sdata/2008/1");
                XPathNavigator nav = doc.DocumentElement.CreateNavigator();
                XPathNavigator message_nav = nav.SelectSingleNode(".//sdata:message", manager);
                string message = message_nav.InnerXml;

                int length = message.IndexOf("Stack");
                
                string retval = message.Substring(0, length);
                return retval;
    
            }
        }
    }
}
