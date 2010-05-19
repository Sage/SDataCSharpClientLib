// Copyright (c) Sage (UK) Limited 2007. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use
// this code. Please contact [email@sage.com] if you do not have such a licence.
// Sage will take appropriate legal action against those who make unauthorised use of this
// code.

using System;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Provides details of a DigestEntry for sync.
    /// </summary>
    [XmlRoot(Namespace = Common.SData.Namespace)]
    [XmlType(TypeName = "digestEntry", Namespace = Common.SData.Namespace)]
    public class DigestEntry
    {
        #region Fields

        private string _strEndPoint;
        private long _lTick;
        private DateTime _oStamp;
        private int _iConflictPriority;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="DigestEntry"/> class.
        /// </summary>
        public DigestEntry()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DigestEntry"/> class with the
        /// specified attributes.
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="tick"></param>
        /// <param name="stamp"></param>
        /// <param name="conflictPriority"></param>
        public DigestEntry(string endPoint, long tick, DateTime stamp, int conflictPriority)
        {
            _strEndPoint = endPoint;
            _lTick = tick;
            _oStamp = stamp;
            _iConflictPriority = conflictPriority;
        }

        #region Properties

        [XmlElement("endpoint")]
        public string EndPoint
        {
            get { return _strEndPoint; }
            set { _strEndPoint = value; }
        }

        [XmlElement("tick")]
        public long Tick
        {
            get { return _lTick; }
            set { _lTick = value; }
        }

        [XmlElement("stamp")]
        public DateTime Stamp
        {
            get { return _oStamp; }
            set { _oStamp = value; }
        }

        [XmlElement("conflictPriority")]
        public int ConflictPriority
        {
            get { return _iConflictPriority; }
            set { _iConflictPriority = value; }
        }

        #endregion

        /// <summary>
        /// Loads this <see cref="DigestEntry"/> using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <param name="manager">The <see cref="XmlNamespaceManager"/> object used to resolve prefixed syndication extension elements and attributes.</param>
        /// <returns><b>true</b> if the <see cref="DigestEntry"/> was initialized using the supplied <paramref name="source"/>, otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     This method expects the supplied <paramref name="source"/> to be positioned on the XML element that represents a <see cref="DigestEntry"/>.
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
                var endPointNavigator = source.SelectSingleNode("sync:endpoint", manager);
                if (endPointNavigator != null && !string.IsNullOrEmpty(endPointNavigator.Value))
                {
                    EndPoint = endPointNavigator.Value;
                    wasLoaded = true;
                }

                var tickNavigator = source.SelectSingleNode("sync:tick", manager);
                if (tickNavigator != null && !string.IsNullOrEmpty(tickNavigator.Value))
                {
                    Tick = Convert.ToInt64(tickNavigator.Value);
                    wasLoaded = true;
                }

                var stampNavigator = source.SelectSingleNode("sync:stamp", manager);
                if (stampNavigator != null)
                {
                    Stamp = W3CDateTime.Parse(stampNavigator.Value).DateTime;
                    wasLoaded = true;
                }

                var conflictPriorityNavigator = source.SelectSingleNode("sync:conflictPriority", manager);
                if (conflictPriorityNavigator != null)
                {
                    ConflictPriority = Convert.ToInt32(conflictPriorityNavigator.Value);
                    wasLoaded = true;
                }
            }

            return wasLoaded;
        }

        /// <summary>
        /// Saves the current <see cref="DigestEntry"/> to the specified <see cref="XmlWriter"/>.
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
            writer.WriteStartElement("digestEntry", xmlNamespace);

            if (EndPoint != null)
            {
                writer.WriteElementString("endpoint", xmlNamespace, EndPoint);
            }

            writer.WriteElementString("tick", xmlNamespace, Tick.ToString());
            writer.WriteElementString("stamp", xmlNamespace, new W3CDateTime(Stamp).ToString());
            writer.WriteElementString("conflictPriority", xmlNamespace, ConflictPriority.ToString());

            writer.WriteEndElement();
        }
    }
}