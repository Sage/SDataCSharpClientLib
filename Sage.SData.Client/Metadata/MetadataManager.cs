// Copyright (c) Sage (UK) Limited 2007. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use
// this code. Please contact [email@sage.com] if you do not have such a licence.
// Sage will take appropriate legal action against those who make unauthorised use of this
// code.

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Manages the Metadata for types.
    /// </summary>
    public static class MetadataManager
    {
        #region XmlConstants

        private static class XmlConstants
        {
            public const string XmlDeclaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            public const string TargetNamespace = "targetNamespace";
            public const string Schema = "schema";
        }

        #endregion

        #region Static Fields

        #region XmlQualifiedNameEqualityComparer

        private class XmlQualifiedNameEqualityComparer : IEqualityComparer<XmlQualifiedName>
        {
            #region IEqualityComparer<XmlQualifiedName> Members

            bool IEqualityComparer<XmlQualifiedName>.Equals(XmlQualifiedName x, XmlQualifiedName y)
            {
                return x.ToString().ToLower() == y.ToString().ToLower();
            }

            int IEqualityComparer<XmlQualifiedName>.GetHashCode(XmlQualifiedName obj)
            {
                var lower = new XmlQualifiedName(obj.Name.ToLower(), obj.Namespace.ToLower());
                return lower.GetHashCode();
            }

            #endregion
        }

        #endregion

        private static readonly Dictionary<XmlQualifiedName, SDataResource> _oNameToMetadata;
        private static readonly Dictionary<XmlSchemaSet, bool> _oLoadingSchemas;

        static MetadataManager()
        {
            _oNameToMetadata = new Dictionary<XmlQualifiedName, SDataResource>(new XmlQualifiedNameEqualityComparer());
            _oLoadingSchemas = new Dictionary<XmlSchemaSet, bool>();
        }

        #endregion

        #region Static Metadata Methods

        /// <summary>
        /// Returns the Metadata for the specified <see cref="Type"/> and qualified name.
        /// </summary>
        /// <param name="name">The qualified name for the associated schema type.</param>
        /// <param name="schemaSet">The schema set containing the specified schema type.</param>
        /// <returns>The Metadata for the specified <see cref="Type"/> and qualified name.</returns>
        public static SDataResource GetMetadata(XmlQualifiedName name, XmlSchemaSet schemaSet)
        {
            lock (_oNameToMetadata)
            {
                SDataResource metaData;

                // See if we have already loaded the details
                if (!_oNameToMetadata.TryGetValue(name, out metaData))
                    metaData = null;

                if (metaData != null) return metaData;

                var runtimeObject = new SDataResource();

                // NOTE: we only add the type name if it has a valid schema
                if (TypeInfoHelper.IsValidQualifiedName(name) && schemaSet != null && schemaSet.Count > 0)
                    _oNameToMetadata[name] = runtimeObject;

                try
                {
                    runtimeObject.Load(name, schemaSet);

                    if (TypeInfoHelper.IsValidQualifiedName(name) && schemaSet != null && !_oLoadingSchemas.ContainsKey(schemaSet))
                    {
                        // Flag this schema as being loaded
                        _oLoadingSchemas[schemaSet] = true;

                        // We might as well load the rest of the schema types at this point
                        foreach (XmlSchema schema in schemaSet.Schemas())
                        {
                            foreach (XmlQualifiedName otherType in schema.Elements.Names)
                            {
                                // Don't load framework types this way as the associated
                                // classes can influence the details (XmlGroupDerived)
                                if (Framework.Common.IsFrameworkNamespace(otherType.Namespace))
                                    continue;

                                if (!_oNameToMetadata.ContainsKey(otherType))
                                {
                                    var otherMetadata = new SDataResource();

                                    _oNameToMetadata[otherType] = otherMetadata;

                                    otherMetadata.Load(otherType, schemaSet);
                                }
                            }
                        }

                        _oLoadingSchemas.Remove(schemaSet);
                    }
                }
                catch
                {
                    if (TypeInfoHelper.IsValidQualifiedName(name))
                        _oNameToMetadata.Remove(name);

                    throw;
                }

                return runtimeObject;
            }
        }

        public static string GetSchema(string targetNs, XmlNamespaceManager namespaceManager, IEnumerable<SDataResource> metaData)
        {
            var builder = new StringBuilder();

            // <?xml version="1.0" encoding="utf-8"?>
            builder.Append(XmlConstants.XmlDeclaration);

            // <xs:schema targetNamespace="http://schemas.sage.com/accounts50/2007" xmlns:atom="http://www.w3.org/2005/Atom" xmlns:sdata="http://schemas.sage.com/sdata/2008/1" xmlns:xs="http://www.w3.org/2001/XMLSchema" elementFormDefault="qualified" xmlns="http://schemas.sage.com/accounts50/2007">
            var namespaceList = new Dictionary<string, string>();
            builder.AppendFormat("<");

            var prefix = namespaceManager.LookupPrefix(targetNs);
            string typeDeclaration;

            if (String.IsNullOrEmpty(prefix) || prefix == Framework.Common.SData.Prefix)
                typeDeclaration = String.Empty;
            else
                typeDeclaration = String.Format("{0}=\"{1}\" ", FormatXMLNS(namespaceManager.LookupPrefix(targetNs)), targetNs);

            // Probably not very performance friendly but we need to check for namespaces used for each property of each type
            foreach (var prop in metaData)
            {
                var baseProp = prop.BaseType as SDataResource;

                if (baseProp == null)
                    continue;

                foreach (var p in baseProp.Properties)
                {
                    if (p.Namespace == null || namespaceList.ContainsValue(p.Namespace) || p.Namespace.Equals(targetNs, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    var bp = prop.Namespaces.LookupPrefix(p.Namespace);

                    if (bp != null)
                        namespaceList.Add(bp, p.Namespace);
                }
            }

            string targetNamespace;

            if (String.IsNullOrEmpty(targetNs))
                targetNamespace = "";
            else
                targetNamespace = String.Format(" {0}=\"{1}\"", XmlConstants.TargetNamespace, targetNs);

            builder.AppendFormat("{0}{1} {2}=\"{3}\" {4}",
                                 TypeInfoHelper.FormatXS(XmlConstants.Schema),
                                 targetNamespace,
                                 Framework.Common.XmlNS,
                                 targetNs,
                                 typeDeclaration);

            foreach (var ns in namespaceList.Keys)
            {
                builder.AppendFormat("{0}=\"{1}\" ", FormatXMLNS(ns), namespaceList[ns]);
            }

            builder.AppendFormat("{0}=\"{1}\" {2}=\"{3}\" {4}=\"{5}\" {6}=\"{7}\" elementFormDefault=\"qualified\"",
                                 FormatXMLNS(Framework.Common.Atom.Prefix),
                                 Framework.Common.Atom.Namespace,
                                 FormatXMLNS(Framework.Common.SData.Prefix),
                                 Framework.Common.SData.Namespace,
                                 FormatXMLNS(Framework.Common.SME.Prefix),
                                 Framework.Common.SME.Namespace,
                                 FormatXMLNS(Framework.Common.XS.Prefix),
                                 Framework.Common.XS.Namespace
                );

            builder.AppendFormat(">");

            var types = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var next in metaData)
                next.GetSchema(types);

            foreach (var type in types.Values)
                builder.Append(type);

            //</xs:schema>
            builder.AppendFormat("</{0}>",
                                 TypeInfoHelper.FormatXS(XmlConstants.Schema));

            return builder.ToString();
        }

        private static string FormatXMLNS(string name)
        {
            return String.Format("{0}:{1}",
                                 Framework.Common.XmlNS,
                                 name);
        }

        #endregion
    }
}