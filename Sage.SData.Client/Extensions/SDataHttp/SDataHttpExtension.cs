using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Extends the atomentry to handle SDataHttp.
    /// </summary>
    [Serializable]
    public class SDataHttpExtension : SyndicationExtension
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================

        #region PRIVATE/PROTECTED/PUBLIC MEMBERS

        /// <summary>
        /// Private member to hold specific information about the extension.
        /// </summary>
        private SDataHttpExtensionContext extensionContext = new SDataHttpExtensionContext();

        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================

        #region SDataHttpExtension()

        /// <summary>
        /// Initializes a new instance of the <see cref="SDataHttpExtension"/> class.
        /// </summary>
        public SDataHttpExtension()
            : base("http", "http://schemas.sage.com/sdata/http/2008/1", new Version("1.0"))
        {
            //------------------------------------------------------------
            //	Initialization handled by base class
            //------------------------------------------------------------
        }

        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================

        #region Context

        /// <summary>
        /// Gets or sets the <see cref="SDataHttpExtensionContext"/> object associated with this extension.
        /// </summary>
        /// <value>A <see cref="SDataHttpExtensionContext"/> object that contains information associated with the current syndication extension.</value>
        /// <remarks>
        ///     The <b>Context</b> encapsulates all of the syndication extension information that can be retrieved or written to an extended syndication entity. 
        ///     Its purpose is to prevent property naming collisions between the base <see cref="SyndicationExtension"/> class and any custom properties that 
        ///     are defined for the custom syndication extension.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public SDataHttpExtensionContext Context
        {
            get { return extensionContext; }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                extensionContext = value;
            }
        }

        #endregion

        //============================================================
        //	STATIC METHODS
        //============================================================

        #region MatchByType(ISyndicationExtension extension)

        /// <summary>
        /// Predicate delegate that returns a value indicating if the supplied <see cref="ISyndicationExtension"/> 
        /// represents the same <see cref="Type"/> as this <see cref="SyndicationExtension"/>.
        /// </summary>
        /// <param name="extension">The <see cref="ISyndicationExtension"/> to be compared.</param>
        /// <returns><b>true</b> if the <paramref name="extension"/> is the same <see cref="Type"/> as this <see cref="SyndicationExtension"/>; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="extension"/> is a null reference (Nothing in Visual Basic).</exception>
        public static bool MatchByType(ISyndicationExtension extension)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(extension, "extension");

            //------------------------------------------------------------
            //	Determine if search condition was met 
            //------------------------------------------------------------
            return extension.GetType() == typeof (SDataHttpExtension);
        }

        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================

        #region Load(IXPathNavigable source)

        /// <summary>
        /// Initializes the syndication extension using the supplied <see cref="IXPathNavigable"/>.
        /// </summary>
        /// <param name="source">The <b>IXPathNavigable</b> used to load this <see cref="SDataExtension"/>.</param>
        /// <returns><b>true</b> if the <see cref="SDataExtension"/> was able to be initialized using the supplied <paramref name="source"/>; otherwise <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        public override bool Load(IXPathNavigable source)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool wasLoaded;

            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            //------------------------------------------------------------
            //	Attempt to extract syndication extension information
            //------------------------------------------------------------
            var navigator = source.CreateNavigator();
            wasLoaded = Context.Load(navigator, CreateNamespaceManager(navigator));

            //------------------------------------------------------------
            //	Raise extension loaded event
            //------------------------------------------------------------
            var args = new SyndicationExtensionLoadedEventArgs(source, this);
            OnExtensionLoaded(args);

            return wasLoaded;
        }

        #endregion

        #region Load(XmlReader reader)

        /// <summary>
        /// Initializes the syndication extension using the supplied <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to load this <see cref="SDataExtension"/>.</param>
        /// <returns><b>true</b> if the <see cref="SDataExtension"/> was able to be initialized using the supplied <paramref name="reader"/>; otherwise <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic).</exception>
        public override bool Load(XmlReader reader)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(reader, "reader");

            //------------------------------------------------------------
            //	Create navigator against reader and pass to load method
            //------------------------------------------------------------
            var document = new XPathDocument(reader);

            return Load(document.CreateNavigator());
        }

        #endregion

        #region WriteTo(XmlWriter writer)

        /// <summary>
        /// Writes the syndication extension to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to write the syndication extension.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        public override void WriteTo(XmlWriter writer)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");

            //------------------------------------------------------------
            //	Write current extension details to the writer
            //------------------------------------------------------------
            Context.WriteTo(writer, XmlNamespace);
        }

        #endregion

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================

        #region ToString()

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="SDataExtension"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="SDataExtension"/>.</returns>
        /// <remarks>
        ///     This method returns the XML representation for the current instance.
        /// </remarks>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Build the string representation
            //------------------------------------------------------------
            using (var stream = new MemoryStream())
            {
                var settings = new XmlWriterSettings();
                settings.ConformanceLevel = ConformanceLevel.Fragment;
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (var writer = XmlWriter.Create(stream, settings))
                {
                    WriteTo(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion
    }
}