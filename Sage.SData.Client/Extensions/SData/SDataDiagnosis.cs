using System;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Provides details of an error that has occurred.
    /// </summary>
    public class SDataDiagnosis
    {
        /// <summary>
        /// Gets or sets the <see cref="Severity"/> of the error.
        /// </summary>
        /// <value>One of the <see cref="Severity"/> values.</value>
        public SDataDiagnosisSeverity? Severity { get; set; }

        /// <summary>
        /// Gets or sets the SData diagnosis code for the error.
        /// </summary>
        /// <value>An SData diagnosis code for the error.</value>
        public SDataDiagnosisCode? SDataCode { get; set; }

        /// <summary>
        /// Gets or sets the application specific diagnosis code for the error.
        /// </summary>
        /// <value>An application specific diagnosis code for the error.</value>
        public string ApplicationCode { get; set; }

        /// <summary>
        /// Gets or sets the user friendly description of the error.
        /// </summary>
        /// <value>A user friendly description of the error.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the stack trace for the error.
        /// </summary>
        /// <value>A stack trace of the error.</value>
        public string StackTrace { get; set; }

        /// <summary>
        /// XPath expression that designates the payload element which is responsible for the error
        /// </summary>
        /// <value>An XPath expression</value>
        public string PayloadPath { get; set; }

        /// <summary>
        /// Loads this <see cref="SDataDiagnosis"/> using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <param name="manager">The <see cref="XmlNamespaceManager"/> object used to resolve prefixed syndication extension elements and attributes.</param>
        /// <returns><b>true</b> if the <see cref="SDataDiagnosis"/> was initialized using the supplied <paramref name="source"/>, otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     This method expects the supplied <paramref name="source"/> to be positioned on the XML element that represents a <see cref="SDataDiagnosis"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        public bool Load(XPathNavigator source, XmlNamespaceManager manager)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            var wasLoaded = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            //------------------------------------------------------------
            //	Attempt to extract syndication information
            //------------------------------------------------------------
            if (source.HasChildren)
            {
                var severityNavigator = source.SelectSingleNode("sdata:severity", manager);
                if (severityNavigator != null && !string.IsNullOrEmpty(severityNavigator.Value))
                {
                    Severity = (SDataDiagnosisSeverity) Enum.Parse(typeof (SDataDiagnosisSeverity), severityNavigator.Value, true);
                    wasLoaded = true;
                }

                var sDataCodeNavigator = source.SelectSingleNode("sdata:sdataCode", manager);
                if (sDataCodeNavigator != null && !string.IsNullOrEmpty(sDataCodeNavigator.Value))
                {
                    SDataCode = (SDataDiagnosisCode) Enum.Parse(typeof (SDataDiagnosisCode), sDataCodeNavigator.Value, true);
                    wasLoaded = true;
                }

                var applicationCodeNavigator = source.SelectSingleNode("sdata:applicationCode", manager);
                if (applicationCodeNavigator != null)
                {
                    ApplicationCode = applicationCodeNavigator.Value;
                    wasLoaded = true;
                }

                var messageNavigator = source.SelectSingleNode("sdata:message", manager);
                if (messageNavigator != null)
                {
                    Message = messageNavigator.Value;
                    wasLoaded = true;
                }

                var stackTraceNavigator = source.SelectSingleNode("sdata:stackTrace", manager);
                if (stackTraceNavigator != null)
                {
                    StackTrace = stackTraceNavigator.Value;
                    wasLoaded = true;
                }

                var payloadPathNavigator = source.SelectSingleNode("sdata:payloadPath", manager);
                if (payloadPathNavigator != null)
                {
                    PayloadPath = payloadPathNavigator.Value;
                    wasLoaded = true;
                }
            }

            return wasLoaded;
        }

        /// <summary>
        /// Saves the current <see cref="SDataDiagnosis"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to write the current context.</param>
        /// <param name="xmlNamespace">The XML namespace used to qualify prefixed syndication extension elements and attributes.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public void WriteTo(XmlWriter writer, string xmlNamespace)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");

            //------------------------------------------------------------
            //	Write XML representation of the current instance
            //------------------------------------------------------------
            writer.WriteStartElement("diagnosis", xmlNamespace);

            if (Severity != null)
            {
                writer.WriteElementString("severity", xmlNamespace, Severity.ToString());
            }

            if (SDataCode != null)
            {
                writer.WriteElementString("sdataCode", xmlNamespace, SDataCode.ToString());
            }

            if (ApplicationCode != null)
            {
                writer.WriteElementString("applicationCode", xmlNamespace, ApplicationCode);
            }

            if (Message != null)
            {
                writer.WriteElementString("message", xmlNamespace, Message);
            }

            if (StackTrace != null)
            {
                writer.WriteElementString("stackTrace", xmlNamespace, StackTrace);
            }

            if (PayloadPath != null)
            {
                writer.WriteElementString("payloadPath", xmlNamespace, PayloadPath);
            }

            writer.WriteEndElement();
        }
    }
}