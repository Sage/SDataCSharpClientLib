using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Common;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Encapsulates specific information about an individual <see cref="SDataExtension"/>.
    /// </summary>
    [Serializable]
    public class SDataExtensionContext
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================

        #region PRIVATE/PROTECTED/PUBLIC MEMBERS

        /// <summary>
        /// Private member to hold information that allows the client to diagnose errors
        /// </summary>
        private Collection<Diagnosis> _diagnoses;

        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================

        /// <summary>
        /// SDataPayload that represent the sdata:payload element in an AtomEntry
        /// </summary>
        public SDataPayload Payload { get; set; }

        /// <summary>
        /// Gets information that allows the client to diagnose errors
        /// </summary>
        public Collection<Diagnosis> Diagnoses
        {
            get { return _diagnoses ?? (_diagnoses = new Collection<Diagnosis>()); }
        }

        /// <summary>
        /// Gets the inline XML schema that describes the feed or entry
        /// </summary>
        public string Schema { get; set; }

        //============================================================
        //	PUBLIC METHODS
        //============================================================

        #region Load(XPathNavigator source, XmlNamespaceManager manager)

        /// <summary>
        /// Initializes the syndication extension context using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <b>XPathNavigator</b> used to load this <see cref="SDataExtensionContext"/>.</param>
        /// <param name="manager">The <see cref="XmlNamespaceManager"/> object used to resolve prefixed syndication extension elements and attributes.</param>
        /// <returns><b>true</b> if the <see cref="SDataExtensionContext"/> was able to be initialized using the supplied <paramref name="source"/>; otherwise <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="manager"/> is a null reference (Nothing in Visual Basic).</exception>
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
            Guard.ArgumentNotNull(manager, "manager");

            //------------------------------------------------------------
            //	Attempt to extract syndication extension information
            //------------------------------------------------------------
            if (source.HasChildren)
            {
                var payloadNavigator = source.SelectSingleNode("sdata:payload/*", manager);
                if (payloadNavigator != null)
                {
                    var payload = new SDataPayload();
                    if (payload.Load(payloadNavigator, manager))
                    {
                        Payload = payload;
                        wasLoaded = true;
                    }
                }

                var diagnoses = source.Select("sdata:diagnoses", manager);
                foreach (XPathNavigator item in diagnoses)
                {
                    var diagnosis = new Diagnosis();
                    if (diagnosis.Load(item, manager))
                    {
                        Diagnoses.Add(diagnosis);
                        wasLoaded = true;
                    }
                }

                var schemaNavigator = source.SelectSingleNode("sdata:schema", manager);
                if (schemaNavigator != null && !string.IsNullOrEmpty(schemaNavigator.Value))
                {
                    Schema = schemaNavigator.Value;
                    wasLoaded = true;
                }
            }

            return wasLoaded;
        }

        #endregion

        #region WriteTo(XmlWriter writer, string xmlNamespace)

        /// <summary>
        /// Writes the current context to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to write the current context.</param>
        /// <param name="xmlNamespace">The XML namespace used to qualify prefixed syndication extension elements and attributes.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlNamespace"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="xmlNamespace"/> is an empty string.</exception>
        public void WriteTo(XmlWriter writer, string xmlNamespace)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");
            Guard.ArgumentNotNullOrEmptyString(xmlNamespace, "xmlNamespace");

            //------------------------------------------------------------
            //	Write current extension details to the writer
            //------------------------------------------------------------

            if (Payload != null)
            {
                Payload.WriteTo(writer, xmlNamespace);
            }

            if (Diagnoses.Count > 0)
            {
                writer.WriteStartElement("diagnoses", xmlNamespace);
                foreach (var diagnosis in Diagnoses)
                {
                    diagnosis.WriteTo(writer, xmlNamespace);
                }
                writer.WriteEndElement();
            }

            if (Schema != null)
            {
                writer.WriteStartElement("schema", xmlNamespace);
                writer.WriteCData(Schema);
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}