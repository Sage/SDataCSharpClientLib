using System;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Common;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Encapsulates specific information about an individual <see cref="SDataSyncExtension"/>.
    /// </summary>
    [Serializable]
    public class SDataSyncExtensionContext
    {
        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================

        /// <summary>
        /// Sync Mode
        /// </summary>
        public SyncMode? SyncMode { get; set; }

        /// <summary>
        /// Digest
        /// </summary>
        public Digest Digest { get; set; }

        /// <summary>
        /// Sync State
        /// </summary>
        public SyncState SyncState { get; set; }

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
                var syncModeNavigator = source.SelectSingleNode("sync:syncMode", manager);
                if (syncModeNavigator != null && !string.IsNullOrEmpty(syncModeNavigator.Value))
                {
                    SyncMode = (SyncMode) Enum.Parse(typeof (SyncMode), syncModeNavigator.Value, true);
                    wasLoaded = true;
                }

                var digestNavigator = source.SelectSingleNode("sync:digest", manager);
                if (digestNavigator != null)
                {
                    var digest = new Digest();
                    if (digest.Load(digestNavigator, manager))
                    {
                        Digest = digest;
                        wasLoaded = true;
                    }
                }

                var syncStateNavigator = source.SelectSingleNode("sync:syncState", manager);
                if (syncStateNavigator != null)
                {
                    var syncState = new SyncState();
                    if (syncState.Load(syncStateNavigator, manager))
                    {
                        SyncState = syncState;
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

            if (SyncMode != null)
            {
                writer.WriteElementString("syncMode", xmlNamespace, SyncMode.ToString().ToUpper());
            }

            if (Digest != null)
            {
                Digest.WriteTo(writer, xmlNamespace);
            }

            if (SyncState != null)
            {
                SyncState.WriteTo(writer, xmlNamespace);
            }
        }

        #endregion
    }
}