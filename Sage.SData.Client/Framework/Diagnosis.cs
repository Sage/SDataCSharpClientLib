// Copyright (c) Sage (UK) Limited 2007. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use
// this code. Please contact [email@sage.com] if you do not have such a licence.
// Sage will take appropriate legal action against those who make unauthorised use of this
// code.

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Specifies the types of diagnosis code.
    /// </summary>
    public enum DiagnosisCode
    {
        /// <summary>
        /// Invalid URL syntax.
        /// </summary>
        BadUrlSyntax,

        /// <summary>
        /// Invalid Query Parameter.
        /// </summary>
        BadQueryParameter,

        /// <summary>
        /// Application does not exist.
        /// </summary>
        ApplicationNotFound,

        /// <summary>
        /// Application exists but is not available.
        /// </summary>
        ApplicationUnavailable,

        /// <summary>
        /// Dataset does not exist.
        /// </summary>
        DatasetNotFound,

        /// <summary>
        /// Dataset exists but is not available.
        /// </summary>
        DatasetUnavailable,

        /// <summary>
        /// Contract does not exist.
        /// </summary>
        ContractNotFound,

        /// <summary>
        /// Resource kind does not exist.
        /// </summary>
        ResourceKindNotFound,

        /// <summary>
        /// Invalid syntax for a where condition.
        /// </summary>
        BadWhereSyntax,

        /// <summary>
        /// Application specific diagnosis, detail is in the applicationCode element.
        /// </summary>
        ApplicationDiagnosis
    }

    /// <summary>
    /// Provides details of an error that has occurred.
    /// </summary>
    [XmlSchemaProvider("_sf_GetSchema")]
    public class Diagnosis
    {
        #region Fields

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="Diagnosis"/> class.
        /// </summary>
        public Diagnosis()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Diagnosis"/> class.
        /// </summary>
        /// <param name="severity">One of the <see cref="Severity"/> values.</param>
        /// <param name="message">A friendly message for the diagnosis.</param>
        /// <param name="code">One of the <see cref="DiagnosisCode"/> values.</param>
        public Diagnosis(Severity severity, string message, DiagnosisCode code) :
            this(severity, message, code, String.Empty)
        {
        }

        public Diagnosis(Severity severity, string message, DiagnosisCode code, string applicationCode) :
            this(severity, message, code, applicationCode, string.Empty)
        {
        }

        public Diagnosis(Severity severity, string message, DiagnosisCode code, string applicationCode, string stackTrace) :
            this(severity, message, code, applicationCode, stackTrace, string.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Diagnosis"/> class.
        /// </summary>
        /// <param name="severity">One of the <see cref="Severity"/> values.</param>
        /// <param name="message">A friendly message for the diagnosis.</param>
        /// <param name="code">The application specific code for the error.</param>
        public Diagnosis(Severity severity, string message, DiagnosisCode code, string applicationCode, string stackTrace, string payloadPath)
        {
            Severity = severity;
            SDataCode = code;
            ApplicationCode = applicationCode;
            Message = message;
            StackTrace = stackTrace;
            PayloadPath = payloadPath;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="Severity"/> of the error.
        /// </summary>
        /// <value>One of the <see cref="Severity"/> values.</value>
        [XmlElement("severity")]
        public Severity? Severity { get; set; }

        /// <summary>
        /// Gets or sets the SData diagnosis code for the error.
        /// </summary>
        /// <value>An SData diagnosis code for the error.</value>
        [XmlElement("sdataCode")]
        public DiagnosisCode? SDataCode { get; set; }

        /// <summary>
        /// Gets or sets the application specific diagnosis code for the error.
        /// </summary>
        /// <value>An application specific diagnosis code for the error.</value>
        [XmlElement("applicationCode")]
        public string ApplicationCode { get; set; }

        /// <summary>
        /// Gets or sets a friendly message for the diagnosis.
        /// </summary>
        /// <value>A user friendly description of the error.</value>
        [XmlElement("message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the stack trace for the error.
        /// </summary>
        /// <value>A stack trace of the error.</value>
        [XmlElement("stackTrace")]
        public string StackTrace { get; set; }

        /// <summary>
        /// XPath expression that designates the payload element which is responsible for the error
        /// </summary>
        /// <value>An XPath expression</value>
        [XmlElement("payloadPath")]
        public string PayloadPath { get; set; }

        #endregion

        #region Statics

        /// <summary>
        /// Adds the schema for this class to the schema collection.
        /// </summary>
        /// <param name="schemas">Schema collection to add this classes schema to.</param>
        /// <returns>Full qualified name to the schema.</returns>
        public static XmlQualifiedName _sf_GetSchema(XmlSchemaSet schemas)
        {
            return new XmlQualifiedName("diagnosis", Common.SData.Namespace);
        }

        #endregion

        /// <summary>
        /// Loads this <see cref="Diagnosis"/> using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <param name="manager">The <see cref="XmlNamespaceManager"/> object used to resolve prefixed syndication extension elements and attributes.</param>
        /// <returns><b>true</b> if the <see cref="Diagnosis"/> was initialized using the supplied <paramref name="source"/>, otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     This method expects the supplied <paramref name="source"/> to be positioned on the XML element that represents a <see cref="Diagnosis"/>.
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
                    Severity = (Severity) Enum.Parse(typeof (Severity), severityNavigator.Value, true);
                    wasLoaded = true;
                }

                var sDataCodeNavigator = source.SelectSingleNode("sdata:sdataCode", manager);
                if (sDataCodeNavigator != null && !string.IsNullOrEmpty(sDataCodeNavigator.Value))
                {
                    SDataCode = (DiagnosisCode) Enum.Parse(typeof (DiagnosisCode), sDataCodeNavigator.Value, true);
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
        /// Saves the current <see cref="Diagnosis"/> to the specified <see cref="XmlWriter"/>.
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