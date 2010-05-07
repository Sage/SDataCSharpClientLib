/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
01/23/2008	brian.kuhn	Created SimpleListSyndicationExtensionContext Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.XPath;

using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Encapsulates specific information about an individual <see cref="SimpleListSyndicationExtension"/>.
    /// </summary>
    [Serializable()]
    public class SimpleListSyndicationExtensionContext
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold a value indicating if the feed is intended to be consumed as a list.
        /// </summary>
        private bool extensionTreatAsList;
        /// <summary>
        /// Private member to hold information that allows the client to group or filter on the values of feed properties.
        /// </summary>
        private Collection<SimpleListGroup> extensionGroups;
        /// <summary>
        /// Private member to hold information that allows the client to sort on the values of feed properties.
        /// </summary>
        private Collection<SimpleListSort> extensionSorts;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SimpleListSyndicationExtensionContext()
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleListSyndicationExtensionContext"/> class.
        /// </summary>
        public SimpleListSyndicationExtensionContext()
        {
            //------------------------------------------------------------
            //	
            //------------------------------------------------------------
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Grouping
        /// <summary>
        /// Gets information that allows the client to group or filter on the values of feed properties.
        /// </summary>
        /// <value>
        ///     A <see cref="Collection{T}"/> collection of <see cref="SimpleListGroup"/> objects that represent information that allows the client to group or filter on the values of feed properties. 
        ///     The default value is an <i>empty</i> collection.
        /// </value>
        public Collection<SimpleListGroup> Grouping
        {
            get
            {
                if (extensionGroups == null)
                {
                    extensionGroups = new Collection<SimpleListGroup>();
                }
                return extensionGroups;
            }
        }
        #endregion

        #region Sorting
        /// <summary>
        /// Gets information that allows the client to sort on the values of feed properties.
        /// </summary>
        /// <value>
        ///     A <see cref="Collection{T}"/> collection of <see cref="SimpleListSort"/> objects that represent information that allows the client to sort on the values of feed properties. 
        ///     The default value is an <i>empty</i> collection.
        /// </value>
        public Collection<SimpleListSort> Sorting
        {
            get
            {
                if (extensionSorts == null)
                {
                    extensionSorts = new Collection<SimpleListSort>();
                }
                return extensionSorts;
            }
        }
        #endregion

        #region TreatAsList
        /// <summary>
        /// Gets or sets a value indicating if this feed is intended to be consumed as a list.
        /// </summary>
        /// <value><b>true</b> if the syndication feed is intended to be consumed as a list; otherwise false.</value>
        /// <remarks>
        ///     This property allows the publisher of a feed document to indicate to the consumers of the feed that the feed is intended to be consumed as a list, 
        ///     and as such is the primary means for feed consumers to identify lists.
        /// </remarks>
        public bool TreatAsList
        {
            get
            {
                return extensionTreatAsList;
            }

            set
            {
                extensionTreatAsList = value;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================
        #region Load(XPathNavigator source, XmlNamespaceManager manager)
        /// <summary>
        /// Initializes the syndication extension context using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <b>XPathNavigator</b> used to load this <see cref="SimpleListSyndicationExtensionContext"/>.</param>
        /// <param name="manager">The <see cref="XmlNamespaceManager"/> object used to resolve prefixed syndication extension elements and attributes.</param>
        /// <returns><b>true</b> if the <see cref="SimpleListSyndicationExtensionContext"/> was able to be initialized using the supplied <paramref name="source"/>; otherwise <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="manager"/> is a null reference (Nothing in Visual Basic).</exception>
        public bool Load(XPathNavigator source, XmlNamespaceManager manager)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool wasLoaded  = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(manager, "manager");

            //------------------------------------------------------------
            //	Attempt to extract syndication extension information
            //------------------------------------------------------------
            if(source.HasChildren)
            {
                XPathNavigator treatAsNavigator         = source.SelectSingleNode("cf:treatAs", manager);
                XPathNavigator listInformationNavigator = source.SelectSingleNode("cf:listinfo", manager);

                if (treatAsNavigator != null && String.Compare(treatAsNavigator.Value, "list", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.TreatAsList    = true;
                    wasLoaded           = true;
                }

                if (listInformationNavigator != null && listInformationNavigator.HasChildren)
                {
                    XPathNodeIterator sortIterator  = source.Select("cf:sort", manager);
                    XPathNodeIterator groupIterator = source.Select("cf:group", manager);

                    if (sortIterator != null && sortIterator.Count > 0)
                    {
                        while (sortIterator.MoveNext())
                        {
                            SimpleListSort sort = new SimpleListSort();
                            if (sort.Load(sortIterator.Current))
                            {
                                this.Sorting.Add(sort);
                                wasLoaded   = true;
                            }
                        }
                    }

                    if (groupIterator != null && groupIterator.Count > 0)
                    {
                        while (groupIterator.MoveNext())
                        {
                            SimpleListGroup group   = new SimpleListGroup();
                            if (group.Load(groupIterator.Current))
                            {
                                this.Grouping.Add(group);
                                wasLoaded   = true;
                            }
                        }
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
            if(this.TreatAsList)
            {
                writer.WriteElementString("treatAs", xmlNamespace, "list");
            }

            if(this.Grouping.Count > 0 || this.Sorting.Count > 0)
            {
                writer.WriteStartElement("listinfo", xmlNamespace);

                foreach (SimpleListSort sort in this.Sorting)
                {
                    sort.WriteTo(writer);
                }

                foreach (SimpleListGroup group in this.Grouping)
                {
                    group.WriteTo(writer);
                }

                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
