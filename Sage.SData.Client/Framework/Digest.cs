// Copyright (c) Sage (UK) Limited 2007. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use
// this code. Please contact [email@sage.com] if you do not have such a licence.
// Sage will take appropriate legal action against those who make unauthorised use of this
// code.

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Provides details of a Digest for sync.
    /// </summary>
    [XmlRoot(Namespace = Common.SData.Namespace)]
    [XmlType(TypeName = "digest", Namespace = Common.SData.Namespace)]
    public class Digest
    {
        #region Fields

        private string _strOrigin;
        private DigestEntry[] _oEntries;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="Digest"/> class.
        /// </summary>
        public Digest()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Digest"/> class with the
        /// specified attributes.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="entries"></param>
        public Digest(string origin, params DigestEntry[] entries)
        {
            _strOrigin = origin;
            _oEntries = entries;
        }

        #region Properties

        [XmlElement("origin")]
        public string Origin
        {
            get { return _strOrigin; }
            set { _strOrigin = value; }
        }

        [XmlElement("digestEntry")]
        public DigestEntry[] Entries
        {
            get { return _oEntries; }
            set { _oEntries = value; }
        }

        #endregion

        /// <summary>
        /// Loads this <see cref="Digest"/> using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <param name="manager">The <see cref="XmlNamespaceManager"/> object used to resolve prefixed syndication extension elements and attributes.</param>
        /// <returns><b>true</b> if the <see cref="Digest"/> was initialized using the supplied <paramref name="source"/>, otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     This method expects the supplied <paramref name="source"/> to be positioned on the XML element that represents a <see cref="Digest"/>.
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
                var originNavigator = source.SelectSingleNode("sync:origin", manager);
                if (originNavigator != null && !string.IsNullOrEmpty(originNavigator.Value))
                {
                    Origin = originNavigator.Value;
                    wasLoaded = true;
                }

                var entriesNavigator = source.Select("sync:digestEntry", manager);
                var entries = new List<DigestEntry>();
                foreach (XPathNavigator item in entriesNavigator)
                {
                    var entry = new DigestEntry();
                    if (entry.Load(item, manager))
                    {
                        entries.Add(entry);
                        wasLoaded = true;
                    }
                }
                Entries = entries.ToArray();
            }

            return wasLoaded;
        }

        /// <summary>
        /// Saves the current <see cref="Digest"/> to the specified <see cref="XmlWriter"/>.
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
            writer.WriteStartElement("digest", xmlNamespace);

            if (Origin != null)
            {
                writer.WriteElementString("origin", xmlNamespace, Origin);
            }

            if (Entries != null)
            {
                foreach (var entry in Entries)
                {
                    entry.WriteTo(writer, xmlNamespace);
                }
            }

            writer.WriteEndElement();
        }
    }
}