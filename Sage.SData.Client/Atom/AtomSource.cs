using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.XPath;

using Sage.SData.Client.Common;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Atom
{
    /// <summary>
    /// Represents the meta-data of the source feed that an <see cref="AtomEntry"/> was copied from.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If an <see cref="AtomEntry"/> is copied from one feed into another feed, then the source feed's metadata (all child elements of feed other than the entry elements) <i>may</i> be preserved 
    ///         within the copied entry by specifying an <see cref="AtomSource"/>, if it is not already present in the entry, and including some or all of the source feed's meta-data elements as the 
    ///         source's children. Such metadata <i>should</i> be preserved if the source <see cref="AtomFeed">feed</see> contains any of the child elements author, contributor, rights, or category 
    ///         and those child elements are not present in the source <see cref="AtomEntry">entry</see>.
    ///     </para>
    ///     <para>
    ///         The <see cref="AtomSource"/> is designed to allow the aggregation of entries from different feeds while retaining information about an entry's source feed. 
    ///         For this reason, Atom Processors that are performing such aggregation <i>should</i> include at least the required feed-level meta-data elements 
    ///         (<see cref="AtomFeed.Id">id</see>, <see cref="AtomFeed.Title">title</see>, and <see cref="AtomFeed.UpdatedOn">updated</see>) in the <see cref="AtomSource"/>.
    ///     </para>
    /// </remarks>
    [Serializable()]
    public class AtomSource : IAtomCommonObjectAttributes, IComparable, IExtensibleSyndicationObject
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the base URI other than the base URI of the document or external entity.
        /// </summary>
        private Uri commonObjectBaseUri;
        /// <summary>
        /// Private member to hold the natural or formal language in which the content is written.
        /// </summary>
        private CultureInfo commonObjectLanguage;
        /// <summary>
        /// Private member to hold the collection of syndication extensions that have been applied to this syndication entity.
        /// </summary>
        private IEnumerable<ISyndicationExtension> objectSyndicationExtensions;
        /// <summary>
        /// Private member to hold the collection of authors of the source.
        /// </summary>
        private Collection<AtomPersonConstruct> sourceAuthors;
        /// <summary>
        /// Private member to hold the collection of categories associated with the source.
        /// </summary>
        private Collection<AtomCategory> sourceCategories;
        /// <summary>
        /// Private member to hold the collection of contributors of the source.
        /// </summary>
        private Collection<AtomPersonConstruct> sourceContributors;
        /// <summary>
        /// Private member to hold the agent used to generate the source.
        /// </summary>
        private AtomGenerator sourceGenerator;
        /// <summary>
        /// Private member to hold an image that provides iconic visual identification for the source.
        /// </summary>
        private AtomIcon sourceIcon;
        /// <summary>
        /// Private member to hold a permanent, universally unique identifier for the source.
        /// </summary>
        private AtomId sourceId;
        /// <summary>
        /// Private member to hold eferences from the source to one or more Web resources.
        /// </summary>
        private Collection<AtomLink> sourceLinks;
        /// <summary>
        /// Private member to hold an image that provides visual identification for the source.
        /// </summary>
        private AtomLogo sourceLogo;
        /// <summary>
        /// Private member to hold information about rights held in and over the source.
        /// </summary>
        private AtomTextConstruct sourceRights;
        /// <summary>
        /// Private member to hold inofmration that conveys a human-readable description or subtitle for the source.
        /// </summary>
        private AtomTextConstruct sourceSubtitle;
        /// <summary>
        /// Private member to hold information that conveys a human-readable title for the source.
        /// </summary>
        private AtomTextConstruct sourceTitle;
        /// <summary>
        /// Private member to hold a value indicating the most recent instant in time when the source was modified in a way the publisher considers significant.
        /// </summary>
        private DateTime sourceUpdatedOn    = DateTime.MinValue;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomSource()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomSource"/> class.
        /// </summary>
        public AtomSource()
        {
            //------------------------------------------------------------
            //	
            //------------------------------------------------------------
        }
        #endregion

        #region AtomSource(AtomId id, AtomTextConstruct title, DateTime utcUpdatedOn)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomSource"/> class using the supplied <see cref="AtomId"/>, <see cref="AtomTextConstruct"/>, and <see cref="DateTime"/>.
        /// </summary>
        /// <param name="id">A <see cref="AtomId"/> object that represents a permanent, universally unique identifier for this source.</param>
        /// <param name="title">A <see cref="AtomTextConstruct"/> object that represents information that conveys a human-readable title for this source.</param>
        /// <param name="utcUpdatedOn">
        ///     A <see cref="DateTime"/> that indicates the most recent instant in time when this source was modified in a way the publisher considers significant. 
        ///     The <see cref="DateTime"/> should be provided in Coordinated Universal Time (UTC).
        /// </param>
        public AtomSource(AtomId id, AtomTextConstruct title, DateTime utcUpdatedOn)
        {
            //------------------------------------------------------------
            //	Initialize class state using properties
            //------------------------------------------------------------
            this.Id         = id;
            this.Title      = title;
            this.UpdatedOn  = utcUpdatedOn;
        }
        #endregion

        //============================================================
        //	COMMON PROPERTIES
        //============================================================
        #region BaseUri
        /// <summary>
        /// Gets or sets the base URI other than the base URI of the document or external entity.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents a base URI other than the base URI of the document or external entity. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     <para>
        ///         The value of this property is interpreted as a URI Reference as defined in <a href="http://www.ietf.org/rfc/rfc2396.txt">RFC 2396: Uniform Resource Identifiers</a>, 
        ///         after processing according to <a href="http://www.w3.org/TR/xmlbase/#escaping">XML Base, Section 3.1 (URI Reference Encoding and Escaping)</a>.</para>
        /// </remarks>
        public Uri BaseUri
        {
            get
            {
                return commonObjectBaseUri;
            }

            set
            {
                commonObjectBaseUri = value;
            }
        }
        #endregion

        #region Language
        /// <summary>
        /// Gets or sets the natural or formal language in which the content is written.
        /// </summary>
        /// <value>A <see cref="CultureInfo"/> that represents the natural or formal language in which the content is written. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     <para>
        ///         The value of this property is a language identifier as defined by <a href="http://www.ietf.org/rfc/rfc3066.txt">RFC 3066: Tags for the Identification of Languages</a>, or its successor.
        ///     </para>
        /// </remarks>
        public CultureInfo Language
        {
            get
            {
                return commonObjectLanguage;
            }

            set
            {
                commonObjectLanguage = value;
            }
        }
        #endregion

        //============================================================
        //	EXTENSIBILITY PROPERTIES
        //============================================================
        #region Extensions
        /// <summary>
        /// Gets or sets the syndication extensions applied to this syndication entity.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}"/> collection of <see cref="ISyndicationExtension"/> objects that represent syndication extensions applied to this syndication entity.</value>
        /// <remarks>
        ///     This <see cref="IEnumerable{T}"/> collection of <see cref="ISyndicationExtension"/> objects is internally represented as a <see cref="Collection{T}"/> collection.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public IEnumerable<ISyndicationExtension> Extensions
        {
            get
            {
                if (objectSyndicationExtensions == null)
                {
                    objectSyndicationExtensions = new Collection<ISyndicationExtension>();
                }
                return objectSyndicationExtensions;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                objectSyndicationExtensions = value;
            }
        }
        #endregion

        #region HasExtensions
        /// <summary>
        /// Gets a value indicating if this syndication entity has one or more syndication extensions applied to it.
        /// </summary>
        /// <value><b>true</b> if the <see cref="Extensions"/> collection for this entity contains one or more <see cref="ISyndicationExtension"/> objects, otherwise returns <b>false</b>.</value>
        public bool HasExtensions
        {
            get
            {
                return ((Collection<ISyndicationExtension>)this.Extensions).Count > 0;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Authors
        /// <summary>
        /// Gets or sets the authors of this source.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomPersonConstruct"/> objects that represent the authors of this source.</value>
        public Collection<AtomPersonConstruct> Authors
        {
            get
            {
                if (sourceAuthors == null)
                {
                    sourceAuthors = new Collection<AtomPersonConstruct>();
                }
                return sourceAuthors;
            }
        }
        #endregion

        #region Categories
        /// <summary>
        /// Gets or sets the categories associated with this source.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomCategory"/> objects that represent the categories associated with this source.</value>
        public Collection<AtomCategory> Categories
        {
            get
            {
                if (sourceCategories == null)
                {
                    sourceCategories = new Collection<AtomCategory>();
                }
                return sourceCategories;
            }
        }
        #endregion

        #region Contributors
        /// <summary>
        /// Gets or sets the entities who contributed to this source.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomPersonConstruct"/> objects that represent the entities who contributed to this source.</value>
        public Collection<AtomPersonConstruct> Contributors
        {
            get
            {
                if (sourceContributors == null)
                {
                    sourceContributors = new Collection<AtomPersonConstruct>();
                }
                return sourceContributors;
            }
        }
        #endregion

        #region Generator
        /// <summary>
        /// Gets or sets the agent used to generate this source.
        /// </summary>
        /// <value>A <see cref="AtomGenerator"/> object that represents the agent used to generate this source. The default value is a <b>null</b> reference.</value>
        public AtomGenerator Generator
        {
            get
            {
                return sourceGenerator;
            }

            set
            {
                sourceGenerator = value;
            }
        }
        #endregion

        #region Icon
        /// <summary>
        /// Gets or sets an image that provides iconic visual identification for this source.
        /// </summary>
        /// <value>A <see cref="AtomIcon"/> object that represents an image that provides iconic visual identification for this source. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     The image <i>should</i> have an aspect ratio of one (horizontal) to one (vertical) and <i>should</i> be suitable for presentation at a small size.
        /// </remarks>
        public AtomIcon Icon
        {
            get
            {
                return sourceIcon;
            }

            set
            {
                sourceIcon = value;
            }
        }
        #endregion

        #region Id
        /// <summary>
        /// Gets or sets a permanent, universally unique identifier for this source.
        /// </summary>
        /// <value>A <see cref="AtomId"/> object that represents a permanent, universally unique identifier for this source.</value>
        public AtomId Id
        {
            get
            {
                return sourceId;
            }

            set
            {
                sourceId = value;
            }
        }
        #endregion

        #region Links
        /// <summary>
        /// Gets or sets references from this source to one or more Web resources.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomLink"/> objects that represent references from this source to one or more Web resources.</value>
        public Collection<AtomLink> Links
        {
            get
            {
                if (sourceLinks == null)
                {
                    sourceLinks = new Collection<AtomLink>();
                }
                return sourceLinks;
            }
        }
        #endregion

        #region Logo
        /// <summary>
        /// Gets or sets an image that provides visual identification for this source.
        /// </summary>
        /// <value>A <see cref="AtomLogo"/> object that represents an image that provides visual identification for this source. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     The image <i>should</i> have an aspect ratio of 2 (horizontal) to 1 (vertical).
        /// </remarks>
        public AtomLogo Logo
        {
            get
            {
                return sourceLogo;
            }

            set
            {
                sourceLogo = value;
            }
        }
        #endregion

        #region Rights
        /// <summary>
        /// Gets or sets information about rights held in and over this source.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information about rights held in and over this source.</value>
        /// <remarks>
        ///     The <see cref="Rights"/> property <i>should not</i> be used to convey machine-readable licensing information.
        /// </remarks>
        public AtomTextConstruct Rights
        {
            get
            {
                return sourceRights;
            }

            set
            {
                sourceRights = value;
            }
        }
        #endregion

        #region Subtitle
        /// <summary>
        /// Gets or sets information that conveys a human-readable description or subtitle for this source.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information that conveys a human-readable description or subtitle for this source.</value>
        public AtomTextConstruct Subtitle
        {
            get
            {
                return sourceSubtitle;
            }

            set
            {
                sourceSubtitle = value;
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets or sets information that conveys a human-readable title for this source.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information that conveys a human-readable title for this source.</value>
        public AtomTextConstruct Title
        {
            get
            {
                return sourceTitle;
            }

            set
            {
                sourceTitle = value;
            }
        }
        #endregion

        #region UpdatedOn
        /// <summary>
        /// Gets or sets a date-time indicating the most recent instant in time when this source was modified in a way the publisher considers significant.
        /// </summary>
        /// <value>
        ///     A <see cref="DateTime"/> that indicates the most recent instant in time when this source was modified in a way the publisher considers significant. 
        ///     Publishers <i>may</i> change the value of this element over time. The default value is <see cref="DateTime.MinValue"/>, which indicates that no update time was provided.
        /// </value>
        /// <remarks>
        ///     The <see cref="DateTime"/> should be provided in Coordinated Universal Time (UTC).
        /// </remarks>
        public DateTime UpdatedOn
        {
            get
            {
                return sourceUpdatedOn;
            }

            set
            {
                sourceUpdatedOn = value;
            }
        }
        #endregion

        //============================================================
        //	EXTENSIBILITY METHODS
        //============================================================
        #region AddExtension(ISyndicationExtension extension)
        /// <summary>
        /// Adds the supplied <see cref="ISyndicationExtension"/> to the current instance's <see cref="IExtensibleSyndicationObject.Extensions"/> collection.
        /// </summary>
        /// <param name="extension">The <see cref="ISyndicationExtension"/> to be added.</param>
        /// <returns><b>true</b> if the <see cref="ISyndicationExtension"/> was added to the <see cref="IExtensibleSyndicationObject.Extensions"/> collection, otherwise <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="extension"/> is a null reference (Nothing in Visual Basic).</exception>
        public bool AddExtension(ISyndicationExtension extension)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool wasAdded   = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(extension, "extension");

            //------------------------------------------------------------
            //	Add syndication extension to collection
            //------------------------------------------------------------
            ((Collection<ISyndicationExtension>)this.Extensions).Add(extension);
            wasAdded    = true;

            return wasAdded;
        }
        #endregion

        #region FindExtension(Predicate<ISyndicationExtension> match)
        /// <summary>
        /// Searches for a syndication extension that matches the conditions defined by the specified predicate, and returns the first occurrence within the <see cref="Extensions"/> collection.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{ISyndicationExtension}"/> delegate that defines the conditions of the <see cref="ISyndicationExtension"/> to search for.</param>
        /// <returns>
        ///     The first syndication extension that matches the conditions defined by the specified predicate, if found; otherwise, the default value for <see cref="ISyndicationExtension"/>.
        /// </returns>
        /// <remarks>
        ///     The <see cref="Predicate{ISyndicationExtension}"/> is a delegate to a method that returns <b>true</b> if the object passed to it matches the conditions defined in the delegate. 
        ///     The elements of the current <see cref="Extensions"/> are individually passed to the <see cref="Predicate{ISyndicationExtension}"/> delegate, moving forward in 
        ///     the <see cref="Extensions"/>, starting with the first element and ending with the last element. Processing is stopped when a match is found.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="match"/> is a null reference (Nothing in Visual Basic).</exception>
        public ISyndicationExtension FindExtension(Predicate<ISyndicationExtension> match)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(match, "match");

            //------------------------------------------------------------
            //	Perform predicate based search
            //------------------------------------------------------------
            List<ISyndicationExtension> list = new List<ISyndicationExtension>(this.Extensions);
            return list.Find(match);
        }
        #endregion

        #region RemoveExtension(ISyndicationExtension extension)
        /// <summary>
        /// Removes the supplied <see cref="ISyndicationExtension"/> from the current instance's <see cref="IExtensibleSyndicationObject.Extensions"/> collection.
        /// </summary>
        /// <param name="extension">The <see cref="ISyndicationExtension"/> to be removed.</param>
        /// <returns><b>true</b> if the <see cref="ISyndicationExtension"/> was removed from the <see cref="IExtensibleSyndicationObject.Extensions"/> collection, otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     If the <see cref="Extensions"/> collection of the current instance does not contain the specified <see cref="ISyndicationExtension"/>, will return <b>false</b>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="extension"/> is a null reference (Nothing in Visual Basic).</exception>
        public bool RemoveExtension(ISyndicationExtension extension)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool wasRemoved = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(extension, "extension");

            //------------------------------------------------------------
            //	Remove syndication extension from collection
            //------------------------------------------------------------
            if (((Collection<ISyndicationExtension>)this.Extensions).Contains(extension))
            {
                ((Collection<ISyndicationExtension>)this.Extensions).Remove(extension);
                wasRemoved  = true;
            }

            return wasRemoved;
        }
        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================
        #region Load(XPathNavigator source)
        /// <summary>
        /// Loads this <see cref="AtomSource"/> using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <returns><b>true</b> if the <see cref="AtomSource"/> was initialized using the supplied <paramref name="source"/>, otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     This method expects the supplied <paramref name="source"/> to be positioned on the XML element that represents a <see cref="AtomSource"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        public bool Load(XPathNavigator source)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool wasLoaded              = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            //------------------------------------------------------------
            //	Initialize XML namespace resolver
            //------------------------------------------------------------
            XmlNamespaceManager manager = AtomUtility.CreateNamespaceManager(source.NameTable);

            //------------------------------------------------------------
            //	Attempt to extract common attributes information
            //------------------------------------------------------------
            if (AtomUtility.FillCommonObjectAttributes(this, source))
            {
                wasLoaded   = true;
            }

            //------------------------------------------------------------
            //	Attempt to extract syndication information
            //------------------------------------------------------------
            XPathNavigator idNavigator              = source.SelectSingleNode("atom:id", manager);
            XPathNavigator titleNavigator           = source.SelectSingleNode("atom:title", manager);
            XPathNavigator updatedNavigator         = source.SelectSingleNode("atom:updated", manager);

            if (idNavigator != null)
            {
                this.Id         = new AtomId();
                if (this.Id.Load(idNavigator))
                {
                    wasLoaded   = true;
                }
            }

            if (titleNavigator != null)
            {
                this.Title      = new AtomTextConstruct();
                if (this.Title.Load(titleNavigator))
                {
                    wasLoaded   = true;
                }
            }

            if (updatedNavigator != null)
            {
                DateTime updatedOn;
                if (SyndicationDateTimeUtility.TryParseRfc3339DateTime(updatedNavigator.Value, out updatedOn))
                {
                    this.UpdatedOn  = updatedOn;
                    wasLoaded       = true;
                }
            }

            if(this.LoadOptionals(source, manager))
            {
                wasLoaded   = true;
            }

            if (this.LoadCollections(source, manager))
            {
                wasLoaded   = true;
            }

            return wasLoaded;
        }
        #endregion

        #region Load(XPathNavigator source, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Loads this <see cref="AtomSource"/> using the supplied <see cref="XPathNavigator"/> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="source">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> used to configure the load operation.</param>
        /// <returns><b>true</b> if the <see cref="AtomSource"/> was initialized using the supplied <paramref name="source"/>, otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     This method expects the supplied <paramref name="source"/> to be positioned on the XML element that represents a <see cref="AtomSource"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        public bool Load(XPathNavigator source, SyndicationResourceLoadSettings settings)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool wasLoaded  = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(settings, "settings");

            //------------------------------------------------------------
            //	Attempt to extract syndication information
            //------------------------------------------------------------
            wasLoaded   = this.Load(source);

            //------------------------------------------------------------
            //	Attempt to extract syndication extension information
            //------------------------------------------------------------
            SyndicationExtensionAdapter adapter = new SyndicationExtensionAdapter(source, settings);
            adapter.Fill(this);

            return wasLoaded;
        }
        #endregion

        #region WriteTo(XmlWriter writer)
        /// <summary>
        /// Saves the current <see cref="AtomSource"/> to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to which you want to save.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public void WriteTo(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");

            //------------------------------------------------------------
            //	Write XML representation of the current instance
            //------------------------------------------------------------
            writer.WriteStartElement("source", AtomUtility.AtomNamespace);
            AtomUtility.WriteCommonObjectAttributes(this, writer);

            if(this.Id != null)
            {
                this.Id.WriteTo(writer);
            }

            if (this.Title != null)
            {
                this.Title.WriteTo(writer, "title");
            }

            if (this.UpdatedOn != DateTime.MinValue)
            {
                writer.WriteElementString("updated", AtomUtility.AtomNamespace, SyndicationDateTimeUtility.ToRfc3339DateTime(this.UpdatedOn));
            }

            if (this.Generator != null)
            {
                this.Generator.WriteTo(writer);
            }

            if (this.Icon != null)
            {
                this.Icon.WriteTo(writer);
            }

            if (this.Logo != null)
            {
                this.Logo.WriteTo(writer);
            }

            if (this.Rights != null)
            {
                this.Rights.WriteTo(writer, "rights");
            }

            if (this.Subtitle != null)
            {
                this.Subtitle.WriteTo(writer, "subtitle");
            }

            foreach(AtomPersonConstruct author in this.Authors)
            {
                author.WriteTo(writer, "author");
            }

            foreach (AtomCategory category in this.Categories)
            {
                category.WriteTo(writer);
            }

            foreach (AtomPersonConstruct contributor in this.Contributors)
            {
                contributor.WriteTo(writer, "contributor");
            }

            foreach (AtomLink link in this.Links)
            {
                link.WriteTo(writer);
            }

            //------------------------------------------------------------
            //	Write the syndication extensions of the current instance
            //------------------------------------------------------------
            SyndicationExtensionAdapter.WriteExtensionsTo(this.Extensions, writer);

            writer.WriteEndElement();
        }
        #endregion

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="AtomSource"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="AtomSource"/>.</returns>
        /// <remarks>
        ///     This method returns the XML representation for the current instance.
        /// </remarks>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Build the string representation
            //------------------------------------------------------------
            using(MemoryStream stream = new MemoryStream())
            {
                XmlWriterSettings settings  = new XmlWriterSettings();
                settings.ConformanceLevel   = ConformanceLevel.Fragment;
                settings.Indent             = true;
                settings.OmitXmlDeclaration = true;

                using(XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    this.WriteTo(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        #endregion

        //============================================================
        //	ICOMPARABLE IMPLEMENTATION
        //============================================================
        #region CompareTo(object obj)
        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared.</returns>
        /// <exception cref="ArgumentException">The <paramref name="obj"/> is not the expected <see cref="Type"/>.</exception>
        public int CompareTo(object obj)
        {
            //------------------------------------------------------------
            //	If target is a null reference, instance is greater
            //------------------------------------------------------------
            if (obj == null)
            {
                return 1;
            }

            //------------------------------------------------------------
            //	Determine comparison result using property state of objects
            //------------------------------------------------------------
            AtomSource value = obj as AtomSource;

            if (value != null)
            {
                int result  = AtomFeed.CompareSequence(this.Authors, value.Authors);
                result      = result | AtomFeed.CompareSequence(this.Categories, value.Categories);
                result      = result | AtomFeed.CompareSequence(this.Contributors, value.Contributors);

                if (this.Generator != null)
                {
                    result  = result | this.Generator.CompareTo(value.Generator);
                }
                else if(value.Generator != null)
                {
                    result  = result | -1;
                }

                if (this.Icon != null)
                {
                    result  = result | this.Icon.CompareTo(value.Icon);
                }
                else if(value.Icon != null)
                {
                    result  = result | -1;
                }

                if (this.Id != null)
                {
                    result  = result | this.Id.CompareTo(value.Id);
                }
                else if(value.Id != null)
                {
                    result  = result | -1;
                }

                result      = result | AtomFeed.CompareSequence(this.Links, value.Links);
                
                if (this.Logo != null)
                {
                    result  = result | this.Logo.CompareTo(value.Logo);
                }
                else if(value.Logo != null)
                {
                    result  = result | -1;
                }

                if (this.Rights != null)
                {
                    result  = result | this.Rights.CompareTo(value.Rights);
                }
                else if(value.Rights != null)
                {
                    result  = result | -1;
                }

                if (this.Subtitle != null)
                {
                    result  = result | this.Subtitle.CompareTo(value.Subtitle);
                }
                else if (value.Subtitle != null)
                {
                    result  = result | -1;
                }

                if (this.Title != null)
                {
                    result  = result | this.Title.CompareTo(value.Title);
                }
                else if (value.Title != null)
                {
                    result  = result | -1;
                }

                result      = result | this.UpdatedOn.CompareTo(value.UpdatedOn);

                result      = result | AtomUtility.CompareCommonObjectAttributes(this, value);

                return result;
            }
            else
            {
                throw new ArgumentException(String.Format(null, "obj is not of type {0}, type was found to be '{1}'.", this.GetType().FullName, obj.GetType().FullName), "obj");
            }
        }
        #endregion

        #region Equals(Object obj)
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current instance.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current instance.</param>
        /// <returns><b>true</b> if the specified <see cref="Object"/> is equal to the current instance; otherwise, <b>false</b>.</returns>
        public override bool Equals(Object obj)
        {
            //------------------------------------------------------------
            //	Determine equality via type then by comparision
            //------------------------------------------------------------
            if (!(obj is AtomSource))
            {
                return false;
            }

            return (this.CompareTo(obj) == 0);
        }
        #endregion

        #region GetHashCode()
        /// <summary>
        /// Returns a hash code for the current instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            //------------------------------------------------------------
            //	Generate has code using unique value of ToString() method
            //------------------------------------------------------------
            char[] charArray    = this.ToString().ToCharArray();

            return charArray.GetHashCode();
        }
        #endregion

        #region == operator
        /// <summary>
        /// Determines if operands are equal.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>true</b> if the values of its operands are equal, otherwise; <b>false</b>.</returns>
        public static bool operator ==(AtomSource first, AtomSource second)
        {
            //------------------------------------------------------------
            //	Handle null reference comparison
            //------------------------------------------------------------
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return true;
            }
            else if (object.Equals(first, null) && !object.Equals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }
        #endregion

        #region != operator
        /// <summary>
        /// Determines if operands are not equal.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>false</b> if its operands are equal, otherwise; <b>true</b>.</returns>
        public static bool operator !=(AtomSource first, AtomSource second)
        {
            return !(first == second);
        }
        #endregion

        #region < operator
        /// <summary>
        /// Determines if first operand is less than second operand.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>true</b> if the first operand is less than the second, otherwise; <b>false</b>.</returns>
        public static bool operator <(AtomSource first, AtomSource second)
        {
            //------------------------------------------------------------
            //	Handle null reference comparison
            //------------------------------------------------------------
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return false;
            }
            else if (object.Equals(first, null) && !object.Equals(second, null))
            {
                return true;
            }

            return (first.CompareTo(second) < 0);
        }
        #endregion

        #region > operator
        /// <summary>
        /// Determines if first operand is greater than second operand.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>true</b> if the first operand is greater than the second, otherwise; <b>false</b>.</returns>
        public static bool operator >(AtomSource first, AtomSource second)
        {
            //------------------------------------------------------------
            //	Handle null reference comparison
            //------------------------------------------------------------
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return false;
            }
            else if (object.Equals(first, null) && !object.Equals(second, null))
            {
                return false;
            }

            return (first.CompareTo(second) > 0);
        }
        #endregion

        //============================================================
        //	PRIVATE METHODS
        //============================================================
        #region LoadCollections(XPathNavigator source, XmlNamespaceManager manager)
        /// <summary>
        /// Loads this <see cref="AtomSource"/> collection elements using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <param name="manager">The <see cref="XmlNamespaceManager"/> used to resolve XML namespace prefixes.</param>
        /// <returns><b>true</b> if the <see cref="AtomSource"/> collection entities were initialized using the supplied <paramref name="source"/>, otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     This method expects the supplied <paramref name="source"/> to be positioned on the XML element that represents a <see cref="AtomSource"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="manager"/> is a null reference (Nothing in Visual Basic).</exception>
        private bool LoadCollections(XPathNavigator source, XmlNamespaceManager manager)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool wasLoaded              = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(manager, "manager");

            //------------------------------------------------------------
            //	Attempt to extract syndication information
            //------------------------------------------------------------
            XPathNodeIterator authorIterator        = source.Select("atom:author", manager);
            XPathNodeIterator contributorIterator   = source.Select("atom:contributor", manager);
            XPathNodeIterator categoryIterator      = source.Select("atom:category", manager);
            XPathNodeIterator linkIterator          = source.Select("atom:link", manager);

            if (authorIterator != null && authorIterator.Count > 0)
            {
                while (authorIterator.MoveNext())
                {
                    AtomPersonConstruct author  = new AtomPersonConstruct();
                    if (author.Load(authorIterator.Current))
                    {
                        this.Authors.Add(author);
                        wasLoaded   = true;
                    }
                }
            }

            if (categoryIterator != null && categoryIterator.Count > 0)
            {
                while (categoryIterator.MoveNext())
                {
                    AtomCategory category   = new AtomCategory();
                    if (category.Load(categoryIterator.Current))
                    {
                        this.Categories.Add(category);
                        wasLoaded   = true;
                    }
                }
            }

            if (contributorIterator != null && contributorIterator.Count > 0)
            {
                while (contributorIterator.MoveNext())
                {
                    AtomPersonConstruct contributor = new AtomPersonConstruct();
                    if (contributor.Load(contributorIterator.Current))
                    {
                        this.Contributors.Add(contributor);
                        wasLoaded   = true;
                    }
                }
            }

            if (linkIterator != null && linkIterator.Count > 0)
            {
                while (linkIterator.MoveNext())
                {
                    AtomLink link   = new AtomLink();
                    if (link.Load(linkIterator.Current))
                    {
                        this.Links.Add(link);
                        wasLoaded   = true;
                    }
                }
            }

            return wasLoaded;
        }
        #endregion

        #region LoadOptionals(XPathNavigator source, XmlNamespaceManager manager)
        /// <summary>
        /// Loads this <see cref="AtomSource"/> optional elements using the supplied <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="source">The <see cref="XPathNavigator"/> to extract information from.</param>
        /// <param name="manager">The <see cref="XmlNamespaceManager"/> used to resolve XML namespace prefixes.</param>
        /// <returns><b>true</b> if the <see cref="AtomSource"/> optional entities were initialized using the supplied <paramref name="source"/>, otherwise <b>false</b>.</returns>
        /// <remarks>
        ///     This method expects the supplied <paramref name="source"/> to be positioned on the XML element that represents a <see cref="AtomSource"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="manager"/> is a null reference (Nothing in Visual Basic).</exception>
        private bool LoadOptionals(XPathNavigator source, XmlNamespaceManager manager)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool wasLoaded              = false;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(manager, "manager");

            //------------------------------------------------------------
            //	Attempt to extract syndication information
            //------------------------------------------------------------
            XPathNavigator generatorNavigator       = source.SelectSingleNode("atom:generator", manager);
            XPathNavigator iconNavigator            = source.SelectSingleNode("atom:icon", manager);
            XPathNavigator logoNavigator            = source.SelectSingleNode("atom:logo", manager);
            XPathNavigator rightsNavigator          = source.SelectSingleNode("atom:rights", manager);
            XPathNavigator subtitleNavigator        = source.SelectSingleNode("atom:subtitle", manager);

            if (generatorNavigator != null)
            {
                this.Generator  = new AtomGenerator();
                if (this.Generator.Load(generatorNavigator))
                {
                    wasLoaded   = true;
                }
            }

            if (iconNavigator != null)
            {
                this.Icon       = new AtomIcon();
                if (this.Icon.Load(iconNavigator))
                {
                    wasLoaded   = true;
                }
            }

            if (logoNavigator != null)
            {
                this.Logo       = new AtomLogo();
                if (this.Logo.Load(logoNavigator))
                {
                    wasLoaded   = true;
                }
            }

            if (rightsNavigator != null)
            {
                this.Rights     = new AtomTextConstruct();
                if (this.Rights.Load(rightsNavigator))
                {
                    wasLoaded   = true;
                }
            }

            if (subtitleNavigator != null)
            {
                this.Subtitle   = new AtomTextConstruct();
                if (this.Subtitle.Load(subtitleNavigator))
                {
                    wasLoaded   = true;
                }
            }

            return wasLoaded;
        }
        #endregion
    }
}
