// Copyright (c) Sage (UK) Limited 2010. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use this
// code. Please contact Sage (UK) if you do not have such a licence. Sage will take
// appropriate legal action against those who make unauthorised use of this code.

using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Handles the mapping of <see cref="MediaType"/> values to names and vica-versa.
    /// </summary>
    public static class MediaTypeNames
    {
        #region Constants

        /// <summary>
        /// ATOM feed content type.
        /// </summary>
        public const string AtomMediaType = "application/atom+xml";

        /// <summary>
        /// ATOM feed content type.
        /// </summary>
        public const string AtomFeedMediaType = "application/atom+xml;type=feed";

        /// <summary>
        /// ATOM entry content type.
        /// </summary>
        public const string AtomEntryMediaType = "application/atom+xml;type=entry";

        /// <summary>
        /// RSS content type.
        /// </summary>
        public const string RssMediaType = "application/rss+xml";

        /// <summary>
        /// XML content type.
        /// </summary>
        public const string XmlMediaType = "application/xml";

        /// <summary>
        /// HTML content type.
        /// </summary>
        public const string HtmlMediaType = "text/html";

        /// <summary>
        /// Javascript Object Notation (JSON) content type
        /// </summary>
        public const string JsonMediaType = "application/json";

        /// <summary>
        /// Form content type
        /// </summary>
        public const string FormMediaType = "application/x-www-form-urlencoded";

        /// <summary>
        /// Text content type.
        /// </summary>
        public const string TextMediaType = "text/plain";

        /// <summary>
        /// PNG Image content type.
        /// </summary>
        public const string ImagePngMediaType = "image/png";

        /// <summary>
        /// JPEG Image content type.
        /// </summary>
        public const string ImageJpegMediaType = "image/jpeg";

        /// <summary>
        /// GIF Image content type.
        /// </summary>
        public const string ImageGifMediaType = "image/gif";

        /// <summary>
        /// TIFF Image content type.
        /// </summary>
        public const string ImageTiffMediaType = "image/tiff";

        /// <summary>
        /// BMP Image content type.
        /// </summary>
        public const string ImageBmpMediaType = "image/bmp";

        /// <summary>
        /// XSLT content type.
        /// </summary>
        public const string XsltMediaType = "application/xsl";

        /// <summary>
        /// CSS content type.
        /// </summary>
        public const string CssMediaType = "text/css";

        /// <summary>
        /// Short Javascript Object Notation (JSON) content type
        /// </summary>
        public const string ShortJsonMediaType = "json";

        /// <summary>
        /// Short ATOM feed content type.
        /// </summary>
        public const string ShortAtomMediaType = "atom";

        /// <summary>
        /// Short ATOM entry content type.
        /// </summary>
        public const string ShortAtomEntryMediaType = "atomentry";

        /// <summary>
        /// Short RSS content type.
        /// </summary>
        public const string ShortRssMediaType = "rss";

        /// <summary>
        /// XML content type.
        /// </summary>
        public const string ShortXmlMediaType = "xml";

        /// <summary>
        /// HTML content type.
        /// </summary>
        public const string ShortHtmlMediaType = "html";

        /// <summary>
        /// Text content type.
        /// </summary>
        public const string ShortTextMediaType = "text";

        /// <summary>
        /// Form content type
        /// </summary>
        public const string ShortFormMediaType = "form";

        /// <summary>
        /// Image PNG content type.
        /// </summary>
        public const string ShortImagePngMediaType = "png";

        /// <summary>
        /// Image JPEG content type.
        /// </summary>
        public const string ShortImageJpegMediaType = "jpeg";

        /// <summary>
        /// Image GIF content type.
        /// </summary>
        public const string ShortImageGifMediaType = "gif";

        /// <summary>
        /// Image TIFF content type.
        /// </summary>
        public const string ShortImageTiffMediaType = "tiff";

        /// <summary>
        /// Image BMP content type.
        /// </summary>
        public const string ShortImageBmpMediaType = "bmp";

        /// <summary>
        /// XSLT content type.
        /// </summary>
        public const string ShortXsltMediaType = "xslt";

        /// <summary>
        /// CSS content type.
        /// </summary>
        public const string ShortCssMediaType = "css";

        /// <summary>
        /// Returns the default <see cref="MediaType"/>.
        /// </summary>
        /// <value>The default <see cref="MediaType"/>, which is <b>Atom</b>.</value>
        public const MediaType DefaultMediaType = MediaType.Atom;

        #endregion

        #region Fields

        private static readonly IDictionary<MediaType, string> MediaTypeToName;
        private static readonly IDictionary<ContentType, MediaType> NameToMediaType;
        private static readonly IDictionary<MediaType, string> MediaTypeToShortName;
        private static readonly IDictionary<string, MediaType> ShortNameToMediaType;

        #endregion

        static MediaTypeNames()
        {
            MediaTypeToName = new Dictionary<MediaType, string>();
            MediaTypeToName[MediaType.Text] = TextMediaType;
            MediaTypeToName[MediaType.Html] = HtmlMediaType;
            MediaTypeToName[MediaType.Atom] = AtomMediaType;
            MediaTypeToName[MediaType.AtomEntry] = AtomEntryMediaType;
            MediaTypeToName[MediaType.Rss] = RssMediaType;
            MediaTypeToName[MediaType.Xml] = XmlMediaType;
            MediaTypeToName[MediaType.ImagePng] = ImagePngMediaType;
            MediaTypeToName[MediaType.ImageGif] = ImageGifMediaType;
            MediaTypeToName[MediaType.ImageTiff] = ImageTiffMediaType;
            MediaTypeToName[MediaType.ImageBmp] = ImageBmpMediaType;
            MediaTypeToName[MediaType.ImageJpeg] = ImageJpegMediaType;
            MediaTypeToName[MediaType.Xslt] = XsltMediaType;
            MediaTypeToName[MediaType.Css] = CssMediaType;
            MediaTypeToName[MediaType.Json] = JsonMediaType;
            MediaTypeToName[MediaType.Form] = FormMediaType;

            NameToMediaType = new Dictionary<ContentType, MediaType>(new ContentTypeComparer());
            NameToMediaType[new ContentType(TextMediaType)] = MediaType.Text;
            NameToMediaType[new ContentType(HtmlMediaType)] = MediaType.Html;
            NameToMediaType[new ContentType(AtomMediaType)] = MediaType.Atom;
            NameToMediaType[new ContentType(AtomFeedMediaType)] = MediaType.Atom;
            NameToMediaType[new ContentType(AtomEntryMediaType)] = MediaType.AtomEntry;
            NameToMediaType[new ContentType(RssMediaType)] = MediaType.Rss;
            NameToMediaType[new ContentType(XmlMediaType)] = MediaType.Xml;
            NameToMediaType[new ContentType(ImagePngMediaType)] = MediaType.ImagePng;
            NameToMediaType[new ContentType(ImageJpegMediaType)] = MediaType.ImageJpeg;
            NameToMediaType[new ContentType(ImageGifMediaType)] = MediaType.ImageGif;
            NameToMediaType[new ContentType(ImageTiffMediaType)] = MediaType.ImageTiff;
            NameToMediaType[new ContentType(ImageBmpMediaType)] = MediaType.ImageBmp;
            NameToMediaType[new ContentType(XsltMediaType)] = MediaType.Xslt;
            NameToMediaType[new ContentType(CssMediaType)] = MediaType.Css;
            NameToMediaType[new ContentType(JsonMediaType)] = MediaType.Json;
            NameToMediaType[new ContentType(FormMediaType)] = MediaType.Form;

            MediaTypeToShortName = new Dictionary<MediaType, string>();
            MediaTypeToShortName[MediaType.Text] = ShortTextMediaType;
            MediaTypeToShortName[MediaType.Html] = ShortHtmlMediaType;
            MediaTypeToShortName[MediaType.Atom] = ShortAtomMediaType;
            MediaTypeToShortName[MediaType.AtomEntry] = ShortAtomEntryMediaType;
            MediaTypeToShortName[MediaType.Rss] = ShortRssMediaType;
            MediaTypeToShortName[MediaType.Xml] = ShortXmlMediaType;
            MediaTypeToShortName[MediaType.ImagePng] = ShortImagePngMediaType;
            MediaTypeToShortName[MediaType.ImageJpeg] = ShortImageJpegMediaType;
            MediaTypeToShortName[MediaType.ImageGif] = ShortImageGifMediaType;
            MediaTypeToShortName[MediaType.ImageTiff] = ShortImageTiffMediaType;
            MediaTypeToShortName[MediaType.ImageBmp] = ShortImageBmpMediaType;
            MediaTypeToShortName[MediaType.Xslt] = ShortXsltMediaType;
            MediaTypeToShortName[MediaType.Css] = ShortCssMediaType;
            MediaTypeToShortName[MediaType.Json] = ShortJsonMediaType;
            MediaTypeToShortName[MediaType.Form] = ShortFormMediaType;

            ShortNameToMediaType = new Dictionary<string, MediaType>(StringComparer.InvariantCultureIgnoreCase);
            ShortNameToMediaType[ShortTextMediaType] = MediaType.Text;
            ShortNameToMediaType[ShortHtmlMediaType] = MediaType.Html;
            ShortNameToMediaType[ShortAtomMediaType] = MediaType.Atom;
            ShortNameToMediaType[ShortAtomEntryMediaType] = MediaType.AtomEntry;
            ShortNameToMediaType[ShortRssMediaType] = MediaType.Rss;
            ShortNameToMediaType[ShortXmlMediaType] = MediaType.Xml;
            ShortNameToMediaType[ShortImagePngMediaType] = MediaType.ImagePng;
            ShortNameToMediaType[ShortImageJpegMediaType] = MediaType.ImageJpeg;
            ShortNameToMediaType[ShortImageGifMediaType] = MediaType.ImageGif;
            ShortNameToMediaType[ShortImageTiffMediaType] = MediaType.ImageTiff;
            ShortNameToMediaType[ShortImageBmpMediaType] = MediaType.ImageBmp;
            ShortNameToMediaType[ShortXsltMediaType] = MediaType.Xslt;
            ShortNameToMediaType[ShortCssMediaType] = MediaType.Css;
            ShortNameToMediaType[ShortJsonMediaType] = MediaType.Json;
            ShortNameToMediaType[ShortFormMediaType] = MediaType.Form;
        }

        /// <summary>
        /// Returns the name of the content type for the specified <see cref="MediaType"/>.
        /// </summary>
        /// <param name="type">One of the <see cref="MediaType"/> values.</param>
        /// <returns>A <see cref="string"/> containing the name of the specified <see cref="MediaType"/>.</returns>
        public static string GetMediaType(MediaType type)
        {
            return MediaTypeToName[type];
        }

        /// <summary>
        /// Returns the <see cref="MediaType"/> for the specified name.
        /// </summary>
        /// <param name="name">The name of the content type.</param>
        /// <returns>The <see cref="MediaType"/> that matches the specified name.</returns>
        public static MediaType GetMediaType(string name)
        {
            return NameToMediaType[new ContentType(name)];
        }

        /// <summary>
        /// Returns the <see cref="MediaType"/> for the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mediaType">On return contains the <see cref="MediaType"/> for the specified name.</param>
        /// <returns><b>true</b> if the content type was found; otherwise, <b>false</b>.</returns>
        public static bool TryGetMediaType(string name, out MediaType mediaType)
        {
            if (string.IsNullOrEmpty(name))
            {
                mediaType = DefaultMediaType;
                return false;
            }

            var found = NameToMediaType.TryGetValue(new ContentType(name), out mediaType);

            if (!found)
                mediaType = DefaultMediaType;

            return found;
        }

        /// <summary>
        /// Returns the short name of the content type for the specified <see cref="MediaType"/>.
        /// </summary>
        /// <param name="type">One of the <see cref="MediaType"/> values.</param>
        /// <returns>A <see cref="string"/> containing the short name of the specified <see cref="MediaType"/>.</returns>
        public static string GetShortMediaType(MediaType type)
        {
            return MediaTypeToShortName[type];
        }

        /// <summary>
        /// Returns the <see cref="MediaType"/> for the specified short name.
        /// </summary>
        /// <param name="name">The short name of the content type.</param>
        /// <returns>The <see cref="MediaType"/> that matches the specified name.</returns>
        public static MediaType GetShortMediaType(string name)
        {
            return ShortNameToMediaType[name];
        }

        /// <summary>
        /// Returns the <see cref="MediaType"/> for the specified short name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mediaType">On return contains the <see cref="MediaType"/> for the specified name.</param>
        /// <returns><b>true</b> if the content type was found; otherwise, <b>false</b>.</returns>
        public static bool TryGetShortMediaType(string name, out MediaType mediaType)
        {
            mediaType = DefaultMediaType;

            if (!ShortNameToMediaType.ContainsKey(name))
                return false;

            mediaType = ShortNameToMediaType[name];
            return true;
        }

        private class ContentTypeComparer : IEqualityComparer<ContentType>
        {
            #region IEqualityComparer<ContentType> Members

            public bool Equals(ContentType x, ContentType y)
            {
                return string.Equals(x.MediaType, y.MediaType, StringComparison.InvariantCultureIgnoreCase) &&
                       string.Equals(x.Parameters["type"], y.Parameters["type"], StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(ContentType obj)
            {
                var code = obj.MediaType.ToLowerInvariant().GetHashCode();
                var type = obj.Parameters["type"];

                if (type != null)
                {
                    code ^= type.ToLowerInvariant().GetHashCode();
                }

                return code;
            }

            #endregion
        }
    }
}