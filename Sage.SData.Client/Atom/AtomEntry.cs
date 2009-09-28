using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using System.Xml.XPath;

using Sage.SData.Client.Common;
using Sage.SData.Client.Adapter;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Atom
{
    /// <summary>
    /// Represents exactly one Atom entry, outside of the context of an <see cref="AtomFeed">Atom feed</see>.
    /// </summary>
    /// <seealso cref="AtomFeed"/>
    /// <seealso cref="AtomFeed.Entries"/>
    /// <remarks>
    ///     <para>
    ///         Atom is an an XML-based Web content and metadata syndication format that describes lists of related information known as <i>feeds</i>. 
    ///         Feeds are composed of a number of items, known as <i>entries</i>, each with an extensible set of attached metadata.
    ///     </para>
    ///     <para>
    ///         This implementation conforms to the Atom 1.0 specification, which can be found 
    ///         at <a href="http://www.atomenabled.org/developers/syndication/atom-format-spec.php">http://www.atomenabled.org/developers/syndication/atom-format-spec.php</a>.
    ///     </para>
    ///     <para>
    ///         Experience teaches that feeds that contain textual content are in general more useful than those that do not. 
    ///         Some applications (one example is full-text indexers) require a minimum amount of text or (X)HTML to function reliably and predictably. 
    ///         Feed producers should be aware of these issues. It is advisable that each <see cref="AtomEntry"/> object contain a non-empty <see cref="AtomEntry.Title"/>, 
    ///         a non-empty <see cref="AtomEntry.Content"/> when content is defined, and a non-empty <see cref="AtomEntry.Summary"/> when the entry contains does not provide a <see cref="AtomEntry.Content"/>. 
    ///         However, the absence of <see cref="AtomEntry.Summary"/> is not an error, and Atom Processors <b>must not</b> fail to function correctly as a consequence of such an absence.
    ///     </para>
    /// </remarks>
    [Serializable()]
    public class AtomEntry : ISyndicationResource, IAtomCommonObjectAttributes, IExtensibleSyndicationObject
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the syndication format for this syndication resource.
        /// </summary>
        private static SyndicationContentFormat feedFormat  = SyndicationContentFormat.Atom;
        /// <summary>
        /// Private member to hold the version of the syndication format for this syndication resource conforms to.
        /// </summary>
        private static Version feedVersion                  = new Version(1, 0);
        /// <summary>
        /// Private member to hold a value indicating if the syndication resource asynchronous load operation was cancelled.
        /// </summary>
        private bool resourceAsyncLoadCancelled;
        /// <summary>
        /// Private member to hold a value indicating if the syndication resource is in the process of loading.
        /// </summary>
        private bool resourceIsLoading;
        /// <summary>
        /// Private member to hold HTTP web request used by asynchronous load operations.
        /// </summary>
        private static WebRequest asyncHttpWebRequest;
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
        /// Private member to hold the collection of authors of the entry.
        /// </summary>
        private Collection<AtomPersonConstruct> entryAuthors;
        /// <summary>
        /// Private member to hold the collection of categories associated with the entry.
        /// </summary>
        private Collection<AtomCategory> entryCategories;
        /// <summary>
        /// Private member to hold information that contains or links to the content of the entry.
        /// </summary>
        private AtomContent entryContent;
        /// <summary>
        /// Private member to hold the collection of contributors of the entry.
        /// </summary>
        private Collection<AtomPersonConstruct> entryContributors;
        /// <summary>
        /// Private member to hold a permanent, universally unique identifier for the entry.
        /// </summary>
        private AtomId entryId;
        /// <summary>
        /// Private member to hold eferences from the entry to one or more Web resources.
        /// </summary>
        private Collection<AtomLink> entryLinks;
        /// <summary>
        /// Private member to hold a value indicating an instant in time associated with an event early in the life cycle of the entry.
        /// </summary>
        private DateTime entryPublishedOn                   = DateTime.MinValue;
        /// <summary>
        /// Private member to hold information about rights held in and over the entry.
        /// </summary>
        private AtomTextConstruct entryRights;
        /// <summary>
        /// Private member to hold the meta-data of the source feed that the entry was copied from.
        /// </summary>
        private AtomSource entrySource;
        /// <summary>
        /// Private member to hold information that conveys a short summary, abstract, or excerpt of the entry.
        /// </summary>
        private AtomTextConstruct entrySummary;
        /// <summary>
        /// Private member to hold information that conveys a human-readable title for the entry.
        /// </summary>
        private AtomTextConstruct entryTitle;
        /// <summary>
        /// Private member to hold a value indicating the most recent instant in time when the entry was modified in a way the publisher considers significant.
        /// </summary>
        private DateTime entryUpdatedOn                     = DateTime.MinValue;
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomEntry()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomEntry"/> class.
        /// </summary>
        public AtomEntry()
        {
            //------------------------------------------------------------
            //	
            //------------------------------------------------------------
        }
        #endregion

        #region AtomEntry(AtomId id, AtomTextConstruct title, DateTime utcUpdatedOn)
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomEntry"/> class using the supplied <see cref="AtomId"/>, <see cref="AtomTextConstruct"/>, and <see cref="DateTime"/>.
        /// </summary>
        /// <param name="id">A <see cref="AtomId"/> object that represents a permanent, universally unique identifier for this entry.</param>
        /// <param name="title">A <see cref="AtomTextConstruct"/> object that represents information that conveys a human-readable title for this entry.</param>
        /// <param name="utcUpdatedOn">
        ///     A <see cref="DateTime"/> that indicates the most recent instant in time when this entry was modified in a way the publisher considers significant. 
        ///     The <see cref="DateTime"/> should be provided in Coordinated Universal Time (UTC).
        /// </param>
        /// <exception cref="ArgumentNullException">The <paramref name="id"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="title"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomEntry(AtomId id, AtomTextConstruct title, DateTime utcUpdatedOn)
        {
            //------------------------------------------------------------
            //	Initialize class state using guarded properties
            //------------------------------------------------------------
            this.Id         = id;
            this.Title      = title;
            this.UpdatedOn  = utcUpdatedOn;
        }
        #endregion

        //============================================================
        //	PUBLIC EVENTS
        //============================================================
        #region Loaded
        /// <summary>
        /// Occurs when the syndication resource state has been changed by a load operation.
        /// </summary>
        /// <seealso cref="AtomEntry.Load(IXPathNavigable)"/>
        /// <seealso cref="AtomEntry.Load(XmlReader)"/>
        public event EventHandler<SyndicationResourceLoadedEventArgs> Loaded;
        #endregion

        //============================================================
        //	EVENT HANDLER DELEGATE METHODS
        //============================================================
        #region OnEntryLoaded(SyndicationResourceLoadedEventArgs e)
        /// <summary>
        /// Raises the <see cref="AtomEntry.Loaded"/> event.
        /// </summary>
        /// <param name="e">A <see cref="SyndicationResourceLoadedEventArgs"/> that contains the event data.</param>
        protected virtual void OnEntryLoaded(SyndicationResourceLoadedEventArgs e)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            EventHandler<SyndicationResourceLoadedEventArgs> handler = null;

            //------------------------------------------------------------
            //	Raise event on registered handler(s)
            //------------------------------------------------------------
            handler = this.Loaded;

            if (handler != null)
            {
                handler(this, e);
            }
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
        /// Gets or sets the authors of this entry.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomPersonConstruct"/> objects that represent the authors of this entry.</value>
        /// <remarks>
        ///     <para>
        ///        An entry <b>must</b> contain one or more authors, unless the entry contains an <see cref="AtomEntry.Source"/> object that contains an author or, 
        ///        in an Atom Feed Document, the <see cref="AtomFeed"/> contains an author itself.
        ///     </para>
        /// </remarks>
        public Collection<AtomPersonConstruct> Authors
        {
            get
            {
                if (entryAuthors == null)
                {
                    entryAuthors = new Collection<AtomPersonConstruct>();
                }
                return entryAuthors;
            }
        }
        #endregion

        #region Categories
        /// <summary>
        /// Gets or sets the categories associated with this entry.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomCategory"/> objects that represent the categories associated with this entry.</value>
        public Collection<AtomCategory> Categories
        {
            get
            {
                if (entryCategories == null)
                {
                    entryCategories = new Collection<AtomCategory>();
                }
                return entryCategories;
            }
        }
        #endregion

        #region Content
        /// <summary>
        /// Gets or sets information that contains or links to the content of this entry.
        /// </summary>
        /// <value>A <see cref="AtomContent"/> object that represents information that contains or links to the content of this entry.</value>
        public AtomContent Content
        {
            get
            {
                return entryContent;
            }

            set
            {
                entryContent = value;
            }
        }
        #endregion

        #region Contributors
        /// <summary>
        /// Gets or sets the entities who contributed to this entry.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomPersonConstruct"/> objects that represent the entities who contributed to this entry.</value>
        public Collection<AtomPersonConstruct> Contributors
        {
            get
            {
                if (entryContributors == null)
                {
                    entryContributors = new Collection<AtomPersonConstruct>();
                }
                return entryContributors;
            }
        }
        #endregion

        #region Format
        /// <summary>
        /// Gets the <see cref="SyndicationContentFormat"/> that this syndication resource implements.
        /// </summary>
        /// <value>The <see cref="SyndicationContentFormat"/> enumeration value that indicates the type of syndication format that this syndication resource implements.</value>
        public SyndicationContentFormat Format
        {
            get
            {
                return feedFormat;
            }
        }
        #endregion

        #region Id
        /// <summary>
        /// Gets or sets a permanent, universally unique identifier for this entry.
        /// </summary>
        /// <value>A <see cref="AtomId"/> object that represents a permanent, universally unique identifier for this entry.</value>
        /// <remarks>
        ///     <para>
        ///         When an <i>Atom Document</i> is relocated, migrated, syndicated, republished, exported, or imported, the content of its universally unique identifier <b>must not</b> change. 
        ///         Put another way, an <see cref="AtomId"/> pertains to all instantiations of a particular <see cref="AtomEntry"/>; revisions retain the same 
        ///         content in their <see cref="AtomId"/> properties. It is suggested that the<see cref="AtomId"/> be stored along with the associated resource.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomId Id
        {
            get
            {
                return entryId;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                entryId = value;
            }
        }
        #endregion

        #region Links
        /// <summary>
        /// Gets or sets references from this entry to one or more Web resources.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomLink"/> objects that represent references from this entry to one or more Web resources.</value>
        /// <remarks>
        ///     <para>
        ///         An entry <b>must not</b> contain more than one <see cref="AtomLink"/> with a <see cref="AtomLink.Relation"/> property of <i>alternate</i> 
        ///         that has the same combination of <see cref="AtomLink.ContentType"/> and <see cref="AtomLink.ContentLanguage"/> property values.
        ///     </para>
        /// </remarks>
        public Collection<AtomLink> Links
        {
            get
            {
                if (entryLinks == null)
                {
                    entryLinks = new Collection<AtomLink>();
                }
                return entryLinks;
            }
        }
        #endregion

        #region PublishedOn
        /// <summary>
        /// Gets or sets a date-time indicating an instant in time associated with an event early in the life cycle of this entry.
        /// </summary>
        /// <value>
        ///     A <see cref="DateTime"/> that indicates an instant in time associated with an event early in the life cycle of this entry. 
        ///     The default value is <see cref="DateTime.MinValue"/>, which indicates that no publication time was provided.
        /// </value>
        /// <remarks>
        ///     The <see cref="DateTime"/> should be provided in Coordinated Universal Time (UTC).
        /// </remarks>
        public DateTime PublishedOn
        {
            get
            {
                return entryPublishedOn;
            }

            set
            {
                entryPublishedOn = value;
            }
        }
        #endregion

        #region Rights
        /// <summary>
        /// Gets or sets information about rights held in and over this entry.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information about rights held in and over this entry.</value>
        /// <remarks>
        ///     The <see cref="Rights"/> property <i>should not</i> be used to convey machine-readable licensing information. 
        ///     If an <see cref="AtomEntry"/> does not provide any rights information, then the <see cref="AtomFeed.Rights"/> of the containing feed, if present, is considered to apply to the entry.
        /// </remarks>
        public AtomTextConstruct Rights
        {
            get
            {
                return entryRights;
            }

            set
            {
                entryRights = value;
            }
        }
        #endregion

        #region Source
        /// <summary>
        /// Gets or sets the meta-data of the source feed that this entry was copied from.
        /// </summary>
        /// <value>A <see cref="AtomSource"/> object that represents the meta-data of the source feed that this entry was copied from.</value>
        /// <remarks>
        ///     <para>
        ///         The <see cref="AtomSource"/> is designed to allow the aggregation of entries from different feeds while retaining information about an entry's source feed. 
        ///         For this reason, Atom Processors that are performing such aggregation <i>should</i> include at least the required feed-level meta-data elements 
        ///         (<see cref="AtomFeed.Id">id</see>, <see cref="AtomFeed.Title">title</see>, and <see cref="AtomFeed.UpdatedOn">updated</see>) in the <see cref="AtomSource"/>.
        ///     </para>
        /// </remarks>
        public AtomSource Source
        {
            get
            {
                return entrySource;
            }

            set
            {
                entrySource = value;
            }
        }
        #endregion

        #region Summary
        /// <summary>
        /// Gets or sets information that conveys a short summary, abstract, or excerpt for this entry.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information that conveys a short summary, abstract, or excerpt for this entry.</value>
        /// <remarks>
        ///     <para>
        ///         It is not advisable for the<see cref="Summary"/> property to duplicate <see cref="Title"/> or <see cref="Content"/> because Atom Processors might assume there is a useful summary when there is none.
        ///     </para>
        ///     <para>
        ///         Entries <b>must</b> contain a <see cref="Summary"/> in either of the following cases:
        ///         <list type="number">
        ///             <item>
        ///                 <description>
        ///                      The <see cref="AtomEntry"/> contains an <see cref="Content"/> property that has a <see cref="AtomContent.Source"/> property (and is thus empty).
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                      The <see cref="AtomEntry"/> contains content that is encoded in Base64; i.e., the <see cref="AtomContent.ContentType"/> property of <see cref="Content"/> property 
        ///                      is a <a href="http://www.ietf.org/rfc/rfc4288.txt">MIME media type</a>, but is not an <a href="http://www.ietf.org/rfc/rfc3023.txt">XML media type</a>, 
        ///                      does not begin with <b>text/</b>, and does not end with <b>/xml</b> or <b>+xml</b>.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        public AtomTextConstruct Summary
        {
            get
            {
                return entrySummary;
            }

            set
            {
                entrySummary = value;
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Gets or sets information that conveys a human-readable title for this entry.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information that conveys a human-readable title for this entry.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomTextConstruct Title
        {
            get
            {
                return entryTitle;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                entryTitle = value;
            }
        }
        #endregion

        #region UpdatedOn
        /// <summary>
        /// Gets or sets a date-time indicating the most recent instant in time when this entry was modified in a way the publisher considers significant.
        /// </summary>
        /// <value>
        ///     A <see cref="DateTime"/> that indicates the most recent instant in time when this entry was modified in a way the publisher considers significant. 
        ///     Publishers <i>may</i> change the value of this element over time. The default value is <see cref="DateTime.MinValue"/>, which indicates that no update time was provided.
        /// </value>
        /// <remarks>
        ///     The <see cref="DateTime"/> should be provided in Coordinated Universal Time (UTC).
        /// </remarks>
        public DateTime UpdatedOn
        {
            get
            {
                return entryUpdatedOn;
            }

            set
            {
                entryUpdatedOn = value;
            }
        }
        #endregion

        #region Version
        /// <summary>
        /// Gets the <see cref="Version"/> of the <see cref="SyndicationContentFormat"/> that this syndication resource conforms to.
        /// </summary>
        /// <value>The <see cref="Version"/> of the <see cref="SyndicationContentFormat"/> that this syndication resource conforms to. The default value is <b>2.0</b>.</value>
        public Version Version
        {
            get
            {
                return feedVersion;
            }
        }
        #endregion

        //============================================================
        //	INTERNAL PROPERTIES
        //============================================================
        #region AsyncLoadHasBeenCancelled
        /// <summary>
        /// Gets or sets a value indicating if the syndication resource asynchronous load operation was cancelled.
        /// </summary>
        /// <value><b>true</b> if syndication resource asynchronous load operation has been cancelled, otherwise <b>false</b>.</value>
        internal bool AsyncLoadHasBeenCancelled
        {
            get
            {
                return resourceAsyncLoadCancelled;
            }

            set
            {
                resourceAsyncLoadCancelled = value;
            }
        }
        #endregion

        #region LoadOperationInProgress
        /// <summary>
        /// Gets or sets a value indicating if the syndication resource is in the process of loading.
        /// </summary>
        /// <value><b>true</b> if syndication resource is in the process of loading, otherwise <b>false</b>.</value>
        internal bool LoadOperationInProgress
        {
            get
            {
                return resourceIsLoading;
            }

            set
            {
                resourceIsLoading = value;
            }
        }
        #endregion

        //============================================================
        //	STATIC METHODS
        //============================================================
        #region Create(Uri source)
        /// <summary>
        /// Creates a new <see cref="AtomEntry"/> instance using the specified <see cref="Uri"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <returns>An <see cref="AtomEntry"/> object loaded using the <paramref name="source"/> data.</returns>
        /// <remarks>
        ///     The <see cref="AtomEntry"/> is created using the default <see cref="SyndicationResourceLoadSettings"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        public static AtomEntry Create(Uri source)
        {
            //------------------------------------------------------------
            //	Create instance using supplied parameter and default settings
            //------------------------------------------------------------
            return AtomEntry.Create(source, (SyndicationResourceLoadSettings) null);
        }
        #endregion

        #region Create(Uri source, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Creates a new <see cref="AtomEntry"/> instance using the specified <see cref="Uri"/> and <see cref="SyndicationResourceLoadSettings"/> object.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="AtomEntry"/> instance. This value can be <b>null</b>.</param>
        /// <returns>An <see cref="AtomEntry"/> object loaded using the <paramref name="source"/> data.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        public static AtomEntry Create(Uri source, SyndicationResourceLoadSettings settings)
        {
            //------------------------------------------------------------
            //	Create instance using supplied parameters and default settings
            //------------------------------------------------------------
            return AtomEntry.Create(source, new WebRequestContext(), settings);
        }
        #endregion

        #region Create(Uri source, ICredentials credentials, IWebProxy proxy)
        /// <summary>
        /// Creates a new <see cref="AtomEntry"/> instance using the specified <see cref="Uri"/>, <see cref="ICredentials"/>, and <see cref="IWebProxy"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the <paramref name="source"/> when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="proxy">
        ///     A <see cref="IWebProxy"/> that provides proxy access to the <paramref name="source"/> when required. This value can be <b>null</b>.
        /// </param>
        /// <returns>An <see cref="AtomEntry"/> object loaded using the <paramref name="source"/> data.</returns>
        /// <remarks>
        ///     The <see cref="AtomEntry"/> is created using the default <see cref="SyndicationResourceLoadSettings"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        public static AtomEntry Create(Uri source, ICredentials credentials, IWebProxy proxy)
        {
            //------------------------------------------------------------
            //	Create instance using supplied parameters and default settings
            //------------------------------------------------------------
            return AtomEntry.Create(source, new WebRequestContext(credentials, proxy));
        }
        #endregion

        #region Create(Uri source, WebRequestContext context)
        /// <summary>
        /// Creates a new <see cref="AtomEntry"/> instance using the specified <see cref="Uri"/>, <see cref="ICredentials"/>, and <see cref="IWebProxy"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="context">A <see cref="WebRequestContext"/> that holds shared contextual information about the web request.</param>
        /// <returns>An <see cref="AtomEntry"/> object loaded using the <paramref name="source"/> data.</returns>
        /// <remarks>
        ///     The <see cref="AtomEntry"/> is created using the default <see cref="SyndicationResourceLoadSettings"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        public static AtomEntry Create(Uri source, WebRequestContext context)
        {
            //------------------------------------------------------------
            //	Create instance using supplied parameters and default settings
            //------------------------------------------------------------
            return AtomEntry.Create(source, context, null);
        }
        #endregion

        #region Create(Uri source, ICredentials credentials, IWebProxy proxy, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Creates a new <see cref="AtomEntry"/> instance using the specified <see cref="Uri"/>, <see cref="ICredentials"/>, <see cref="IWebProxy"/>, and <see cref="SyndicationResourceLoadSettings"/> object.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the <paramref name="source"/> when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="proxy">
        ///     A <see cref="IWebProxy"/> that provides proxy access to the <paramref name="source"/> when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="AtomEntry"/> instance. This value can be <b>null</b>.</param>
        /// <returns>An <see cref="AtomEntry"/> object loaded using the <paramref name="source"/> data.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        public static AtomEntry Create(Uri source, ICredentials credentials, IWebProxy proxy, SyndicationResourceLoadSettings settings)
        {
            //------------------------------------------------------------
            //	Create instance using supplied parameters
            //------------------------------------------------------------
            return AtomEntry.Create(source, new WebRequestContext(credentials, proxy), settings);
        }
        #endregion

        #region Create(Uri source, WebRequestContext context, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Creates a new <see cref="AtomEntry"/> instance using the specified <see cref="Uri"/>, <see cref="ICredentials"/>, <see cref="IWebProxy"/>, and <see cref="SyndicationResourceLoadSettings"/> object.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="context">A <see cref="WebRequestContext"/> that holds shared contextual information about the web request.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="AtomEntry"/> instance. This value can be <b>null</b>.</param>
        /// <returns>An <see cref="AtomEntry"/> object loaded using the <paramref name="source"/> data.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        public static AtomEntry Create(Uri source, WebRequestContext context, SyndicationResourceLoadSettings settings)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            AtomEntry syndicationResource = new AtomEntry();

            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            //------------------------------------------------------------
            //	Create new instance using supplied parameters
            //------------------------------------------------------------
            syndicationResource.Load(source, context, settings);

            return syndicationResource;
        }
        #endregion

        //============================================================
        //	ASYNC METHODS
        //============================================================
        #region LoadAsync(Uri source, Object userToken)
        /// <summary>
        /// Loads this <see cref="AtomEntry"/> instance asynchronously using the specified <see cref="Uri"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        /// <remarks>
        ///     <para>The <see cref="AtomEntry"/> is loaded using the default <see cref="SyndicationResourceLoadSettings"/>.</para>
        ///     <para>
        ///         To receive notification when the operation has completed or the operation has been canceled, add an event handler to the <see cref="Loaded"/> event. 
        ///         You can cancel a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> operation by calling the <see cref="LoadAsyncCancel()"/> method.
        ///     </para>
        ///     <para>
        ///         After calling <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/>, you must wait for the load operation to complete before 
        ///         attempting to load the syndication resource using the <see cref="LoadAsync(Uri, Object)"/> method.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the feed remains empty.</exception>
        /// <exception cref="InvalidOperationException">This <see cref="AtomEntry"/> has a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> call in progress.</exception>
        public void LoadAsync(Uri source, Object userToken)
        {
            //------------------------------------------------------------
            //	Create instance using supplied parameter and default settings
            //------------------------------------------------------------
            this.LoadAsync(source, null, userToken);
        }
        #endregion

        #region LoadAsync(Uri source, SyndicationResourceLoadSettings settings, Object userToken)
        /// <summary>
        /// Loads this <see cref="AtomEntry"/> instance asynchronously using the specified <see cref="Uri"/> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="AtomEntry"/> instance. This value can be <b>null</b>.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        /// <remarks>
        ///     <para>
        ///         To receive notification when the operation has completed or the operation has been canceled, add an event handler to the <see cref="Loaded"/> event. 
        ///         You can cancel a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> operation by calling the <see cref="LoadAsyncCancel()"/> method.
        ///     </para>
        ///     <para>
        ///         After calling <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/>, you must wait for the load operation to complete before 
        ///         attempting to load the syndication resource using the <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, Object)"/> method.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the feed remains empty.</exception>
        /// <exception cref="InvalidOperationException">This <see cref="AtomEntry"/> has a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> call in progress.</exception>
        public void LoadAsync(Uri source, SyndicationResourceLoadSettings settings, Object userToken)
        {
            //------------------------------------------------------------
            //	Create instance using supplied parameter and specified settings
            //------------------------------------------------------------
            this.LoadAsync(source, settings, new WebRequestContext(), userToken);
        }
        #endregion

        #region LoadAsync(Uri source, SyndicationResourceLoadSettings settings, ICredentials credentials, IWebProxy proxy, Object userToken)
        /// <summary>
        /// Loads this <see cref="AtomEntry"/> instance asynchronously using the specified <see cref="Uri"/>, <see cref="SyndicationResourceLoadSettings"/>, <see cref="ICredentials"/>, and <see cref="IWebProxy"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="AtomEntry"/> instance. This value can be <b>null</b>.</param>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the <paramref name="source"/> when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="proxy">
        ///     A <see cref="IWebProxy"/> that provides proxy access to the <paramref name="source"/> when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        /// <remarks>
        ///     <para>
        ///         To receive notification when the operation has completed or the operation has been canceled, add an event handler to the <see cref="Loaded"/> event. 
        ///         You can cancel a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> operation by calling the <see cref="LoadAsyncCancel()"/> method.
        ///     </para>
        ///     <para>
        ///         After calling <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/>, 
        ///         you must wait for the load operation to complete before attempting to load the syndication resource using the <see cref="LoadAsync(Uri, Object)"/> method.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the feed remains empty.</exception>
        /// <exception cref="InvalidOperationException">This <see cref="AtomEntry"/> has a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> call in progress.</exception>
        public void LoadAsync(Uri source, SyndicationResourceLoadSettings settings, ICredentials credentials, IWebProxy proxy, Object userToken)
        {
            //------------------------------------------------------------
            //	Create instance using supplied parameters and specified settings
            //------------------------------------------------------------
            this.LoadAsync(source, settings, new WebRequestContext(credentials, proxy), userToken);
        }
        #endregion

        #region LoadAsync(Uri source, SyndicationResourceLoadSettings settings, WebRequestContext context, Object userToken)
        /// <summary>
        /// Loads this <see cref="AtomEntry"/> instance asynchronously using the specified <see cref="Uri"/>, <see cref="SyndicationResourceLoadSettings"/>, <see cref="ICredentials"/>, and <see cref="IWebProxy"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="AtomEntry"/> instance. This value can be <b>null</b>.</param>
        /// <param name="context">A <see cref="WebRequestContext"/> that holds shared contextual information about the web request.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        /// <remarks>
        ///     <para>
        ///         To receive notification when the operation has completed or the operation has been canceled, add an event handler to the <see cref="Loaded"/> event. 
        ///         You can cancel a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> operation by calling the <see cref="LoadAsyncCancel()"/> method.
        ///     </para>
        ///     <para>
        ///         After calling <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/>, 
        ///         you must wait for the load operation to complete before attempting to load the syndication resource using the <see cref="LoadAsync(Uri, Object)"/> method.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the feed remains empty.</exception>
        /// <exception cref="InvalidOperationException">This <see cref="AtomEntry"/> has a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> call in progress.</exception>
        public void LoadAsync(Uri source, SyndicationResourceLoadSettings settings, WebRequestContext context, Object userToken)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            //------------------------------------------------------------
            //	Use default settings if none specified by the caller
            //------------------------------------------------------------
            if (settings == null)
            {
                settings    = new SyndicationResourceLoadSettings();
            }

            //------------------------------------------------------------
            //	Validate syndication resource state
            //------------------------------------------------------------
            if (this.LoadOperationInProgress)
            {
                throw new InvalidOperationException();
            }

            //------------------------------------------------------------
            //	Indicate that a load operation is in progress
            //------------------------------------------------------------
            this.LoadOperationInProgress    = true;

            //------------------------------------------------------------
            //	Reset the asynchronous load operation cancelled indicator
            //------------------------------------------------------------
            this.AsyncLoadHasBeenCancelled  = false;

            //------------------------------------------------------------
            //	Build HTTP web request used to retrieve the syndication resource
            //------------------------------------------------------------
            asyncHttpWebRequest         = SyndicationEncodingUtility.CreateWebRequest(source, context);
            asyncHttpWebRequest.Timeout = Convert.ToInt32(settings.Timeout.TotalMilliseconds, System.Globalization.NumberFormatInfo.InvariantInfo);

            //------------------------------------------------------------
            //	Get the async response to the web request
            //------------------------------------------------------------
            object[] state      = new object[6] { asyncHttpWebRequest, this, source, settings, context, userToken };
            IAsyncResult result = asyncHttpWebRequest.BeginGetResponse(new AsyncCallback(AsyncLoadCallback), state);

            //------------------------------------------------------------
            //  Register the timeout callback
            //------------------------------------------------------------
            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(AsyncTimeoutCallback), state, settings.Timeout, true);
        }
        #endregion

        #region LoadAsyncCancel()
        /// <summary>
        /// Cancels an asynchronous operation to load this syndication resource.
        /// </summary>
        /// <remarks>
        ///     Use the LoadAsyncCancel method to cancel a pending <see cref="LoadAsync(Uri, Object)"/> operation. 
        ///     If there is a load operation in progress, this method releases resources used to execute the load operation. 
        ///     If there is no load operation pending, this method does nothing.
        /// </remarks>
        public void LoadAsyncCancel()
        {
            //------------------------------------------------------------
            //	Determine if load of syndication resource call in progress 
            //  and the async operation has not already been cancelled
            //------------------------------------------------------------
            if (this.LoadOperationInProgress && !this.AsyncLoadHasBeenCancelled)
            {
                //------------------------------------------------------------
                //	Set async operation cancelled indicator
                //------------------------------------------------------------
                this.AsyncLoadHasBeenCancelled  = true;

                //------------------------------------------------------------
                //	Cancel the async load operation
                //------------------------------------------------------------
                asyncHttpWebRequest.Abort();
            }
        }
        #endregion

        //============================================================
        //	CALLBACK DELEGATE METHODS
        //============================================================
        #region AsyncLoadCallback(IAsyncResult result)
        /// <summary>
        /// Called when a corresponding asynchronous load operation completes.
        /// </summary>
        /// <param name="result">The result of the asynchronous operation.</param>
        private static void AsyncLoadCallback(IAsyncResult result)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            System.Text.Encoding encoding               = System.Text.Encoding.UTF8;
            XPathNavigator navigator                    = null;
            WebRequest httpWebRequest                   = null;
            AtomEntry entry                             = null;
            Uri source                                  = null;
            WebRequestContext context;
            SyndicationResourceLoadSettings settings    = null;

            //------------------------------------------------------------
            //	Determine if the async send operation completed
            //------------------------------------------------------------
            if (result.IsCompleted)
            {
                //------------------------------------------------------------
                //	Extract the send operations parameters from the user state
                //------------------------------------------------------------
                object[] parameters = (object[])result.AsyncState;
                httpWebRequest      = parameters[0] as WebRequest;
                entry               = parameters[1] as AtomEntry;
                source              = parameters[2] as Uri;
                settings            = parameters[3] as SyndicationResourceLoadSettings;
                context             = (parameters[4] as WebRequestContext?) ?? new WebRequestContext();
                object userToken    = parameters[5];

                //------------------------------------------------------------
                //	Verify expected parameters were found
                //------------------------------------------------------------
                if (entry != null)
                {
                    //------------------------------------------------------------
                    //	Get the response to the syndication resource request
                    //------------------------------------------------------------
                    WebResponse httpWebResponse = (WebResponse)httpWebRequest.EndGetResponse(result);

                    //------------------------------------------------------------
                    //	Load syndication resource
                    //------------------------------------------------------------
                    using (Stream stream = httpWebResponse.GetResponseStream())
                    {
                        if (settings != null)
                        {
                            encoding    = settings.CharacterEncoding;
                        }

                        using (StreamReader streamReader = new StreamReader(stream, encoding))
                        {
                            XmlReaderSettings readerSettings    = new XmlReaderSettings();
                            readerSettings.IgnoreComments       = true;
                            readerSettings.IgnoreWhitespace     = true;
                            readerSettings.ProhibitDtd          = false;

                            using (XmlReader reader = XmlReader.Create(streamReader, readerSettings))
                            {
                                if (encoding == System.Text.Encoding.UTF8)
                                {
                                    navigator   = SyndicationEncodingUtility.CreateSafeNavigator(source, context, null);
                                }
                                else
                                {
                                    navigator   = SyndicationEncodingUtility.CreateSafeNavigator(source, context, settings.CharacterEncoding);
                                }

                                //------------------------------------------------------------
                                //	Load syndication resource using the framework adapters
                                //------------------------------------------------------------
                                SyndicationResourceAdapter adapter  = new SyndicationResourceAdapter(navigator, settings);
                                adapter.Fill(entry, SyndicationContentFormat.Atom);

                                //------------------------------------------------------------
                                //	Raise Loaded event to notify registered handlers of state change
                                //------------------------------------------------------------
                                entry.OnEntryLoaded(new SyndicationResourceLoadedEventArgs(navigator, source, context, userToken));
                            }
                        }
                    }

                    //------------------------------------------------------------
                    //	Reset load operation in progress indicator
                    //------------------------------------------------------------
                    entry.LoadOperationInProgress    = false;
                }
            }
        }
        #endregion

        #region AsyncTimeoutCallback(object state, bool timedOut)
        /// <summary>
        /// Represents a method to be called when a <see cref="WaitHandle"/> is signaled or times out.
        /// </summary>
        /// <param name="state">An object containing information to be used by the callback method each time it executes.</param>
        /// <param name="timedOut"><b>true</b> if the <see cref="WaitHandle"/> timed out; <b>false</b> if it was signaled.</param>
        private void AsyncTimeoutCallback(object state, bool timedOut)
        {
            //------------------------------------------------------------
            //	Determine if asynchronous load operation timed out
            //------------------------------------------------------------
            if (timedOut)
            {
                //------------------------------------------------------------
                //	Abort asynchronous load operation
                //------------------------------------------------------------
                if (asyncHttpWebRequest != null)
                {
                    asyncHttpWebRequest.Abort();
                }
            }

            //------------------------------------------------------------
            //	Reset load operation in progress indicator
            //------------------------------------------------------------
            this.LoadOperationInProgress    = false;
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
        //	INSTANCE METHODS
        //============================================================
        #region CreateNavigator()
        /// <summary>
        /// Initializes a read-only <see cref="XPathNavigator"/> object for navigating through nodes in this <see cref="AtomEntry"/>.
        /// </summary>
        /// <returns>A read-only <see cref="XPathNavigator"/> object.</returns>
        /// <remarks>
        ///     The <see cref="XPathNavigator"/> is positioned on the root element of the <see cref="AtomEntry"/>. 
        ///     If there is no root element, the <see cref="XPathNavigator"/> is positioned on the first element in the XML representation of the <see cref="AtomEntry"/>.
        /// </remarks>
        public XPathNavigator CreateNavigator()
        {
            using(MemoryStream stream = new MemoryStream())
            {
                XmlWriterSettings settings  = new XmlWriterSettings();
                settings.ConformanceLevel   = ConformanceLevel.Document;
                settings.Indent             = true;
                settings.OmitXmlDeclaration = false;

                using(XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    this.Save(writer);
                    writer.Flush();
                }

                stream.Seek(0, SeekOrigin.Begin);

                XPathDocument document  = new XPathDocument(stream);
                return document.CreateNavigator();
            }
        }
        #endregion

        #region Load(IXPathNavigable source)
        /// <summary>
        /// Loads the syndication resource from the specified <see cref="IXPathNavigable"/>.
        /// </summary>
        /// <param name="source">The <b>IXPathNavigable</b> used to load the syndication resource.</param>
        /// <remarks>
        ///     After the load operation has successfully completed, the <see cref="AtomEntry.Loaded"/> event will be raised.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the entry remains empty.</exception>
        public void Load(IXPathNavigable source)
        {
            //------------------------------------------------------------
            //	Load syndication resource using default settings
            //------------------------------------------------------------
            this.Load(source, null);
        }
        #endregion

        #region Load(IXPathNavigable source, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Loads the syndication resource from the specified <see cref="IXPathNavigable"/> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="source">The <b>IXPathNavigable</b> used to load the syndication resource.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="AtomEntry"/> instance. This value can be <b>null</b>.</param>
        /// <remarks>
        ///     After the load operation has successfully completed, the <see cref="AtomEntry.Loaded"/> event will be raised.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the entry remains empty.</exception>
        public void Load(IXPathNavigable source, SyndicationResourceLoadSettings settings)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            //------------------------------------------------------------
            //	Use default settings if none specified by the caller
            //------------------------------------------------------------
            if (settings == null)
            {
                settings    = new SyndicationResourceLoadSettings();
            }

            //------------------------------------------------------------
            //	Load syndication resource using the framework adapters
            //------------------------------------------------------------
            XPathNavigator navigator    = source.CreateNavigator();
            this.Load(navigator, settings, new SyndicationResourceLoadedEventArgs(navigator));
        }
        #endregion

        #region Load(Stream stream)
        /// <summary>
        /// Loads the syndication resource from the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <b>Stream</b> used to load the syndication resource.</param>
        /// <remarks>
        ///     After the load operation has successfully completed, the <see cref="AtomEntry.Loaded"/> event will be raised.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="stream"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the entry remains empty.</exception>
        public void Load(Stream stream)
        {
            //------------------------------------------------------------
            //	Load syndication resource using default settings
            //------------------------------------------------------------
            this.Load(stream, null);
        }
        #endregion

        #region Load(Stream stream, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Loads the syndication resource from the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <b>Stream</b> used to load the syndication resource.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="AtomEntry"/> instance. This value can be <b>null</b>.</param>
        /// <remarks>
        ///     After the load operation has successfully completed, the <see cref="AtomEntry.Loaded"/> event will be raised.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="stream"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the entry remains empty.</exception>
        public void Load(Stream stream, SyndicationResourceLoadSettings settings)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(stream, "stream");

            //------------------------------------------------------------
            //	Use stream to create a XPathNavigator to load from
            //------------------------------------------------------------
            if (settings != null)
            {
                this.Load(SyndicationEncodingUtility.CreateSafeNavigator(stream, settings.CharacterEncoding), settings);
            }
            else
            {
                this.Load(SyndicationEncodingUtility.CreateSafeNavigator(stream), settings);
            }
        }
        #endregion

        #region Load(XmlReader reader)
        /// <summary>
        /// Loads the syndication resource from the specified <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to load the syndication resource.</param>
        /// <remarks>
        ///     After the load operation has successfully completed, the <see cref="AtomEntry.Loaded"/> event will be raised.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="reader"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the entry remains empty.</exception>
        public void Load(XmlReader reader)
        {
            //------------------------------------------------------------
            //	Load syndication resource using default settings
            //------------------------------------------------------------
            this.Load(reader, null);
        }
        #endregion

        #region Load(XmlReader reader, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Loads the syndication resource from the specified <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to load the syndication resource.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="AtomEntry"/> instance. This value can be <b>null</b>.</param>
        /// <remarks>
        ///     After the load operation has successfully completed, the <see cref="AtomEntry.Loaded"/> event will be raised.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="reader"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the entry remains empty.</exception>
        public void Load(XmlReader reader, SyndicationResourceLoadSettings settings)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(reader, "reader");

            //------------------------------------------------------------
            //	Use reader to create a IXPathNavigable to load from
            //------------------------------------------------------------
            this.Load(new XPathDocument(reader), settings);
        }
        #endregion

        #region Load(Uri source, ICredentials credentials, IWebProxy proxy)
        /// <summary>
        /// Loads the syndication resource from the supplied <see cref="Uri"/> using the specified <see cref="ICredentials">credentials</see> and <see cref="IWebProxy">proxy</see>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that points to the location of the web resource used to load the syndication resource.</param>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the <paramref name="source"/> resource when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="proxy">
        ///     A <see cref="IWebProxy"/> that provides proxy access to the <paramref name="source"/> resource when required. This value can be <b>null</b>.
        /// </param>
        /// <remarks>
        ///     <para>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                      If <paramref name="credentials"/> is <b>null</b>, request is made using the default application credentials.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     If <paramref name="proxy"/> is <b>null</b>, request is made using the <see cref="WebRequest"/> default proxy settings.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="AtomEntry.Loaded"/> event will be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the entry remains empty.</exception>
        public void Load(Uri source, ICredentials credentials, IWebProxy proxy)
        {
            //------------------------------------------------------------
            //	Load syndication resource using default settings
            //------------------------------------------------------------
            this.Load(source, new WebRequestContext(credentials, proxy));
        }
        #endregion

        #region Load(Uri source, WebRequestContext context)
        /// <summary>
        /// Loads the syndication resource from the supplied <see cref="Uri"/> using the specified <see cref="ICredentials">credentials</see> and <see cref="IWebProxy">proxy</see>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that points to the location of the web resource used to load the syndication resource.</param>
        /// <param name="context">A <see cref="WebRequestContext"/> that holds shared contextual information about the web request.</param>
        /// <remarks>
        ///     <para>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                      If <paramref name="credentials"/> is <b>null</b>, request is made using the default application credentials.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     If <paramref name="proxy"/> is <b>null</b>, request is made using the <see cref="WebRequest"/> default proxy settings.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="AtomEntry.Loaded"/> event will be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the entry remains empty.</exception>
        public void Load(Uri source, WebRequestContext context)
        {
            //------------------------------------------------------------
            //	Load syndication resource using default settings
            //------------------------------------------------------------
            this.Load(source, context, null);
        }
        #endregion

        #region Load(Uri source, ICredentials credentials, IWebProxy proxy, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Loads the syndication resource from the supplied <see cref="Uri"/> using the specified <see cref="ICredentials">credentials</see>, <see cref="IWebProxy">proxy</see> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that points to the location of the web resource used to load the syndication resource.</param>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the <paramref name="source"/> resource when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="proxy">
        ///     A <see cref="IWebProxy"/> that provides proxy access to the <paramref name="source"/> resource when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="AtomEntry"/> instance. This value can be <b>null</b>.</param>
        /// <remarks>
        ///     <para>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                      If <paramref name="credentials"/> is <b>null</b>, request is made using the default application credentials.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     If <paramref name="proxy"/> is <b>null</b>, request is made using the <see cref="WebRequest"/> default proxy settings.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     If <paramref name="settings"/> has a <see cref="SyndicationResourceLoadSettings.CharacterEncoding">character encoding</see> of <see cref="System.Text.Encoding.UTF8"/> 
        ///                     the character encoding of the <paramref name="source"/> will be attempt to be determined automatically, otherwise the specified character encoding will be used. 
        ///                     If automatic detection fails, a character encoding of <see cref="System.Text.Encoding.UTF8"/> is used by default.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="AtomEntry.Loaded"/> event will be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the entry remains empty.</exception>
        public void Load(Uri source, ICredentials credentials, IWebProxy proxy, SyndicationResourceLoadSettings settings)
        {
            //------------------------------------------------------------
            //	Load syndication resource using supplied settings
            //------------------------------------------------------------
            this.Load(source, new WebRequestContext(credentials, proxy), settings);
        }
        #endregion

        #region Load(Uri source, WebRequestContext context, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Loads the syndication resource from the supplied <see cref="Uri"/> using the specified <see cref="ICredentials">credentials</see>, <see cref="IWebProxy">proxy</see> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that points to the location of the web resource used to load the syndication resource.</param>
        /// <param name="context">A <see cref="WebRequestContext"/> that holds shared contextual information about the web request.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="AtomEntry"/> instance. This value can be <b>null</b>.</param>
        /// <remarks>
        ///     <para>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                      If <paramref name="credentials"/> is <b>null</b>, request is made using the default application credentials.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     If <paramref name="proxy"/> is <b>null</b>, request is made using the <see cref="WebRequest"/> default proxy settings.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     If <paramref name="settings"/> has a <see cref="SyndicationResourceLoadSettings.CharacterEncoding">character encoding</see> of <see cref="System.Text.Encoding.UTF8"/> 
        ///                     the character encoding of the <paramref name="source"/> will be attempt to be determined automatically, otherwise the specified character encoding will be used. 
        ///                     If automatic detection fails, a character encoding of <see cref="System.Text.Encoding.UTF8"/> is used by default.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="AtomEntry.Loaded"/> event will be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the entry remains empty.</exception>
        public void Load(Uri source, WebRequestContext context, SyndicationResourceLoadSettings settings)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            XPathNavigator navigator    = null;

            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            //------------------------------------------------------------
            //	Use default settings if none specified by the caller
            //------------------------------------------------------------
            if (settings == null)
            {
                settings = new SyndicationResourceLoadSettings();
            }

            //------------------------------------------------------------
            //	Initialize XPathNavigator for supplied Uri, credentials, and proxy
            //------------------------------------------------------------
            if (settings.CharacterEncoding == System.Text.Encoding.UTF8)
            {
                navigator    = SyndicationEncodingUtility.CreateSafeNavigator(source, context, null);
            }
            else
            {
                navigator    = SyndicationEncodingUtility.CreateSafeNavigator(source, context, settings.CharacterEncoding);
            }

            //------------------------------------------------------------
            //	Load syndication resource using the framework adapters
            //------------------------------------------------------------
            this.Load(navigator, settings, new SyndicationResourceLoadedEventArgs(navigator, source, context));
        }
        #endregion

        #region Save(Stream stream)
        /// <summary>
        /// Saves the syndication resource to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <b>Stream</b> to which you want to save the syndication resource.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication resource.</exception>
        public void Save(Stream stream)
        {
            //------------------------------------------------------------
            //	Persist syndication resource using default settings
            //------------------------------------------------------------
            this.Save(stream, null);
        }
        #endregion

        #region Save(Stream stream, SyndicationResourceSaveSettings settings)
        /// <summary>
        /// Saves the syndication resource to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <b>Stream</b> to which you want to save the syndication resource.</param>
        /// <param name="settings">The <see cref="SyndicationResourceSaveSettings"/> object used to configure the persistance of the <see cref="AtomEntry"/> instance. This value can be <b>null</b>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication resource.</exception>
        public void Save(Stream stream, SyndicationResourceSaveSettings settings)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(stream, "stream");

            if (settings == null)
            {
                settings    = new SyndicationResourceSaveSettings();
            }

            //------------------------------------------------------------
            //	Create XmlWriter against supplied stream to save entry
            //------------------------------------------------------------
            XmlWriterSettings writerSettings    = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration   = false;
            writerSettings.Indent               = !settings.MinimizeOutputSize;
            writerSettings.Encoding             = settings.CharacterEncoding;

            using (XmlWriter writer = XmlWriter.Create(stream, writerSettings))
            {
                this.Save(writer, settings);
            }
        }
        #endregion

        #region Save(XmlWriter writer)
        /// <summary>
        /// Saves the syndication resource to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to save the syndication resource.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication resource.</exception>
        public void Save(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");

            //------------------------------------------------------------
            //	Save feed using default settings
            //------------------------------------------------------------
            this.Save(writer, new SyndicationResourceSaveSettings());
        }
        #endregion

        #region Save(XmlWriter writer, SyndicationResourceSaveSettings settings)
        /// <summary>
        /// Saves the syndication resource to the specified <see cref="XmlWriter"/> and <see cref="SyndicationResourceSaveSettings"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to save the syndication resource.</param>
        /// <param name="settings">The <see cref="SyndicationResourceSaveSettings"/> object used to configure the persistance of the <see cref="AtomEntry"/> instance.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication resource.</exception>
        public void Save(XmlWriter writer, SyndicationResourceSaveSettings settings)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");
            Guard.ArgumentNotNull(settings, "settings");

            //------------------------------------------------------------
            //	Save entry
            //------------------------------------------------------------
            writer.WriteStartElement("entry", AtomUtility.AtomNamespace);

            if (settings.AutoDetectExtensions)
            {
                SyndicationExtensionAdapter.FillExtensionTypes(this, settings.SupportedExtensions);

                if(this.Content != null)
                {
                    SyndicationExtensionAdapter.FillExtensionTypes(this.Content, settings.SupportedExtensions);
                }
                if (this.Id != null)
                {
                    SyndicationExtensionAdapter.FillExtensionTypes(this.Id, settings.SupportedExtensions);
                }
                if (this.Rights != null)
                {
                    SyndicationExtensionAdapter.FillExtensionTypes(this.Rights, settings.SupportedExtensions);
                }
                if (this.Source != null)
                {
                    SyndicationExtensionAdapter.FillExtensionTypes(this.Source, settings.SupportedExtensions);
                }
                if (this.Summary != null)
                {
                    SyndicationExtensionAdapter.FillExtensionTypes(this.Summary, settings.SupportedExtensions);
                }
                if (this.Title != null)
                {
                    SyndicationExtensionAdapter.FillExtensionTypes(this.Title, settings.SupportedExtensions);
                }

                foreach (AtomPersonConstruct author in this.Authors)
                {
                    SyndicationExtensionAdapter.FillExtensionTypes(author, settings.SupportedExtensions);
                }
                foreach (AtomCategory category in this.Categories)
                {
                    SyndicationExtensionAdapter.FillExtensionTypes(category, settings.SupportedExtensions);
                }
                foreach (AtomPersonConstruct contributor in this.Contributors)
                {
                    SyndicationExtensionAdapter.FillExtensionTypes(contributor, settings.SupportedExtensions);
                }
                foreach (AtomLink link in this.Links)
                {
                    SyndicationExtensionAdapter.FillExtensionTypes(link, settings.SupportedExtensions);
                }
            }
            SyndicationExtensionAdapter.WriteXmlNamespaceDeclarations(settings.SupportedExtensions, writer);

            AtomUtility.WriteCommonObjectAttributes(this, writer);

            if (this.Id != null)
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

            this.WriteEntryOptionals(writer);
            this.WriteEntryCollections(writer);

            //------------------------------------------------------------
            //	Write the syndication extensions of the current instance
            //------------------------------------------------------------
            SyndicationExtensionAdapter.WriteExtensionsTo(this.Extensions, writer);

            writer.WriteEndElement();
        }
        #endregion

        //============================================================
        //	PRIVATE METHODS
        //============================================================
        #region Load(XPathNavigator navigator, SyndicationResourceLoadSettings settings, SyndicationResourceLoadedEventArgs eventData)
        /// <summary>
        /// Loads the syndication resource using the specified <see cref="XPathNavigator"/> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="navigator">A read-only <see cref="XPathNavigator"/> object for navigating through the syndication resource information.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the load operation of the <see cref="AtomEntry"/>.</param>
        /// <param name="eventData">A <see cref="SyndicationResourceLoadedEventArgs"/> that contains the event data used when raising the <see cref="AtomEntry.Loaded"/> event.</param>
        /// <remarks>
        ///     After the load operation has successfully completed, the <see cref="AtomEntry.Loaded"/> event is raised using the specified <paramref name="eventData"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="navigator"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="eventData"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="navigator"/> data does not conform to the expected syndication content format. In this case, the entry remains empty.</exception>
        private void Load(XPathNavigator navigator, SyndicationResourceLoadSettings settings, SyndicationResourceLoadedEventArgs eventData)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(navigator, "navigator");
            Guard.ArgumentNotNull(settings, "settings");
            Guard.ArgumentNotNull(eventData, "eventData");

            //------------------------------------------------------------
            //	Load syndication resource using the framework adapters
            //------------------------------------------------------------
            SyndicationResourceAdapter adapter  = new SyndicationResourceAdapter(navigator, settings);
            adapter.Fill(this, SyndicationContentFormat.Atom);

            //------------------------------------------------------------
            //	Raise Loaded event to notify registered handlers of state change
            //------------------------------------------------------------
            this.OnEntryLoaded(eventData);
        }
        #endregion

        #region WriteEntryCollections(XmlWriter writer)
        /// <summary>
        /// Saves the current <see cref="AtomEntry"/> collection entities to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to which you want to save.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteEntryCollections(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");

            foreach (AtomPersonConstruct author in this.Authors)
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
        }
        #endregion

        #region WriteEntryOptionals(XmlWriter writer)
        /// <summary>
        /// Saves the current <see cref="AtomEntry"/> optional entities to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> to which you want to save.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        private void WriteEntryOptionals(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");

            if (this.Content != null)
            {
                this.Content.WriteTo(writer);
            }

            if (this.PublishedOn != DateTime.MinValue)
            {
                writer.WriteElementString("published", AtomUtility.AtomNamespace, SyndicationDateTimeUtility.ToRfc3339DateTime(this.PublishedOn));
            }

            if (this.Rights != null)
            {
                this.Rights.WriteTo(writer, "rights");
            }

            if (this.Source != null)
            {
                this.Source.WriteTo(writer);
            }

            if (this.Summary != null)
            {
                this.Summary.WriteTo(writer, "summary");
            }
        }
        #endregion
    }
}
