// Copyright (c) Sage (UK) Limited 2007. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use
// this code. Please contact [email@sage.com] if you do not have such a licence.
// Sage will take appropriate legal action against those who make unauthorised use of this
// code.

using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Provides the common elements for Syndication
    /// </summary>
    public static class Common
    {
        #region Fields

        private static readonly XmlSerializerNamespaces _oSerializerNamespaces;
        private static readonly Dictionary<string, bool> _oFrameworkNamespaces;

        static Common()
        {
            _oSerializerNamespaces = new XmlSerializerNamespaces();
            _oSerializerNamespaces.Add(SME.Prefix, SME.Namespace);
            _oSerializerNamespaces.Add(SData.Prefix, SData.Namespace);
            _oSerializerNamespaces.Add(HTTP.Prefix, HTTP.Namespace);
            _oSerializerNamespaces.Add(Sync.Prefix, Sync.Namespace);
            _oSerializerNamespaces.Add(OpenSearch.Prefix, OpenSearch.Namespace);
            _oSerializerNamespaces.Add(XSI.Prefix, XSI.Namespace);

            _oFrameworkNamespaces = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);
            _oFrameworkNamespaces[Atom.Namespace] = true;
            _oFrameworkNamespaces[SME.Namespace] = true;
            _oFrameworkNamespaces[SData.Namespace] = true;
            _oFrameworkNamespaces[HTTP.Namespace] = true;
            _oFrameworkNamespaces[Sync.Namespace] = true;
            _oFrameworkNamespaces[OpenSearch.Namespace] = true;
        }

        #endregion

        #region Namespaces

        /// <summary>
        /// Prefix for xml namespace
        /// </summary>
        public const string XmlNS = "xmlns";

        /// <summary>
        /// XS namespace
        /// </summary>
        public static class XS
        {
            /// <summary>
            /// Xml Schema namespace.
            /// </summary>
            public const string Namespace = "http://www.w3.org/2001/XMLSchema";

            /// <summary>
            /// Prefix for XmlSchema namespace
            /// </summary>
            public const string Prefix = "xs";
        }

        /// <summary>
        /// XSI namespace
        /// </summary>
        public static class XSI
        {
            /// <summary>
            /// Namespace for XSI 
            /// </summary>
            public const string Namespace = "http://www.w3.org/2001/XMLSchema-instance";

            /// <summary>
            /// Prefix for XSI namespace
            /// </summary>
            public const string Prefix = "xsi";

            /// <summary>
            /// Nil
            /// </summary>
            public const string Nil = "nil";
        }

        /// <summary>
        /// ATOM namespace
        /// </summary>
        public static class Atom
        {
            /// <summary>
            /// Uri for Atom namespace
            /// </summary>
            public const string Namespace = "http://www.w3.org/2005/Atom";

            /// <summary>
            /// Prefix for Atom namespace
            /// </summary>
            public const string Prefix = "atom";
        }

        /// <summary>
        /// SData namespace
        /// </summary>
        public static class SData
        {
            /// <summary>
            /// Uri for SData namespace
            /// </summary>
            public const string Namespace = "http://schemas.sage.com/sdata/2008/1";

            /// <summary>
            /// Prefix for SData namespace
            /// </summary>
            public const string Prefix = "sdata";
        }

        /// <summary>
        /// SME namespace
        /// </summary>
        public static class SME
        {
            /// <summary>
            /// Uri for SME namespace
            /// </summary>
            public const string Namespace = "http://schemas.sage.com/sdata/sme/2007";

            /// <summary>
            /// Prefix for SME namespace
            /// </summary>
            public const string Prefix = "sme";
        }

        /// <summary>
        /// HTTP namespace
        /// </summary>
        public static class HTTP
        {
            /// <summary>
            /// Namespace for SData HTTP elements 
            /// </summary>
            public const string Namespace = "http://schemas.sage.com/sdata/http/2008/1";

            /// <summary>
            /// Prefix for SData Http header elements.
            /// </summary>
            public const string Prefix = "http";
        }

        /// <summary>
        /// Sync namespace
        /// </summary>
        public static class Sync
        {
            /// <summary>
            /// Namespace for SData Sync elements 
            /// </summary>
            public const string Namespace = "http://schemas.sage.com/sdata/sync/2008/1";

            /// <summary>
            /// Prefix for SData Http header elements.
            /// </summary>
            public const string Prefix = "sync";
        }

        /// <summary>
        /// SLE namespace
        /// </summary>
        public static class SLE
        {
            /// <summary>
            /// URI for SLE namespace
            /// </summary>
            public const string Namespace = "http://www.microsoft.com/schemas/rss/core/2005";

            /// <summary>
            /// Prefix for SLE namespace
            /// </summary>
            public const string Prefix = "cf";
        }

        /// <summary>
        /// OpenSearch namespace
        /// </summary>
        public static class OpenSearch
        {
            /// <summary>
            /// URI for OpenSearch namespace
            /// </summary>
            public const string Namespace = "http://a9.com/-/spec/opensearch/1.1/";

            /// <summary>
            /// Prefix for OpenSearch namespace
            /// </summary>
            public const string Prefix = "opensearch";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the <see cref="XmlSerializerNamespaces"/> for the Accounts types.
        /// </summary>
        /// <returns>The <see cref="XmlSerializerNamespaces"/> for the Accounts types.</returns>
        public static XmlSerializerNamespaces GetSerializerNamespaces()
        {
            return _oSerializerNamespaces;
        }

        /// <summary>
        /// Returns a value indicating if the specified namespace is a framework namespace.
        /// </summary>
        /// <param name="ns">The namespace to check.</param>
        /// <returns><b>true</b> if the specified namespace is a framework namespace; otherwise, <b>false</b>.</returns>
        public static bool IsFrameworkNamespace(string ns)
        {
            return !String.IsNullOrEmpty(ns) && _oFrameworkNamespaces.ContainsKey(ns);
        }

        #endregion

        private class NullXmlResolver : XmlResolver
        {
            #region XmlResolver Members

            public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
            {
                return null;
            }

            public override ICredentials Credentials
            {
                set { }
            }

            #endregion
        }
    }
}