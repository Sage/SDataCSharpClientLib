using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Common;

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
        private Collection<SDataDiagnosis> _diagnoses;

        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================

        /// <summary>
        /// XPathNavigator that represent the sdata:payload element in a an AtomEntry
        /// </summary>
        public XPathNavigator Payload { get; set; }

        /// <summary>
        /// Gets information that allows the client to diagnose errors
        /// </summary>
        public Collection<SDataDiagnosis> Diagnoses
        {
            get { return _diagnoses ?? (_diagnoses = new Collection<SDataDiagnosis>()); }
        }

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
                    Payload = payloadNavigator;
                    wasLoaded = true;
                }

                var diagnoses = source.Select("sdata:diagnoses", manager);
                foreach (XPathNavigator item in diagnoses)
                {
                    var diagnosis = new SDataDiagnosis();
                    if (diagnosis.Load(item, manager))
                    {
                        Diagnoses.Add(diagnosis);
                        wasLoaded = true;
                    }
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
                writer.WriteStartElement("payload", xmlNamespace);
                writer.WriteNode(Payload, true);
                writer.WriteEndElement();
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
        }

        #endregion
    }
}