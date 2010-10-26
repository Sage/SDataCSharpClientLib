// Copyright (c) Sage (UK) Limited 2007. All rights reserved.
// This code may not be copied or used, except as fset out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use
// this code. Please contact [email@sage.com] if you do not have such a licence.
// Sage will take appropriate legal action against those who make unauthorised use of this
// code.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Provides the Metadata for an associated <see cref="Type"/>.
    /// </summary>
    public class SDataResource : SMEProperty
    {
        #region Xml Constants

        internal static class XmlConstants
        {
            public const string Element = "element";
            public const string Name = "name";
            public const string Sequence = "sequence";
            public const string Type = "type";
            public const string SingleSuffix = "--type";
            public const string ListSuffix = "--list";
            public const string EnumSuffix = "--enum";
            public const string ComplexType = "complexType";
            public const string Base = "base";
            public const string All = "all";
            public const string SimpleType = "simpleType";
            public const string Restriction = "restriction";
            public const string MaxLength = "maxLength";
            public const string MinLength = "minLength";
            public const string Length = "length";
            public const string Value = "value";
            public const string Nillable = "nillable";
            public const string MinOccurs = "minOccurs";
            public const string MaxOccurs = "maxOccurs";
            public const string MinInclusive = "minInclusive";
            public const string MaxInclusive = "MaxInclusive";
            public const string Pattern = "pattern";
            public const string FractionDigits = "fractionDigits";
            public const string TotalDigits = "totalDigits";
            public const string Unbounded = "unbounded";
        }

        #endregion

        #region Static Methods

        internal static string FormatSME(string name)
        {
            return String.Format("{0}:{1}",
                                 Framework.Common.SME.Prefix,
                                 name);
        }

        internal static string FormatSingleType(string name, string suffix)
        {
            return String.Format("{0}{1}",
                                 name,
                                 suffix);
        }

        private static string FormatListType(string name)
        {
            return String.Format("{0}{1}",
                                 name,
                                 XmlConstants.ListSuffix);
        }

        #endregion

        #region Fields

        private SMEResource _oResourceInfo;
        private List<SMEProperty> _oProperties;
        private Dictionary<string, SMEProperty> _oNameToProperty;
        private XmlNamespaceManager _oNamespaces;
        private SMEProperty _oBaseType;
        private string _strTypeName;
        private SMERelationshipProperty _oRelationshipInformation;
        private bool _bLoading;
        private readonly SDataResource _oSource;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SDataResource"/> class.
        /// </summary>
        public SDataResource()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SDataResource"/> class
        /// by cloning the details of the specified instance.
        /// </summary>
        /// <param name="source">Instance of the <see cref="SDataResource"/> to clone.</param>
        private SDataResource(SDataResource source) :
            base(source)
        {
            // Back casting, yuck!
            _oSource = source;

            if (_oSource != null && _oSource._bLoading)
                _oSource.Loaded += Clone_Loaded;
            else
                Clone(_oSource);
        }

        #region Methods

        /// <summary>
        /// Loads the Metadata using combination of the specified <see cref="Type"/> and Xml schema type.
        /// </summary>
        /// <param name="name">The qualified name of the xml type.</param>
        /// <param name="schemaSet">Collection of schemas.</param>
        public void Load(XmlQualifiedName name, XmlSchemaSet schemaSet)
        {
            _bLoading = true;

            // Holds the namespaces associated with this meta data
            _oNamespaces = new XmlNamespaceManager(new NameTable());

            // Holds the SME resource details
            _oResourceInfo = new SMEResource();

            // If we have a specified name we can get the default type name and namespace
            if (TypeInfoHelper.IsValidQualifiedName(name))
            {
                _strTypeName = name.Name;
                Namespace = name.Namespace;
                Name = name.Name;
            }

            // Next find the associated schema type
            var schemaObject = GetSchemaObject(name, schemaSet);
            var complexType = schemaObject as XmlSchemaComplexType;
            XmlSchemaParticle particle;
            var properties = new List<SMEProperty>();

            // If its a complex type load the details
            if (complexType != null)
                particle = BeginLoadComplexType(properties, complexType, schemaSet);
            else
                particle = null;

            // If we don't have a name at this point we just use the type name
            if (String.IsNullOrEmpty(Name))
                Name = _strTypeName;

            // At this point we can build the list of properties using any of the following
            // 1) The particles for the associated schema type (preferred)
            // 2) The properties on the type
            if (particle != null)
            {
                // XmlSchemaChoice or XmlSchemaSequence
                var schemaGroup = particle as XmlSchemaGroupBase;

                if (schemaGroup != null)
                {
                    foreach (var groupItem in schemaGroup.Items)
                    {
                        var element = groupItem as XmlSchemaElement;

                        if (element == null)
                            continue;

                        var originalElement = element;

                        // Check for a reference name
                        if (!element.RefName.IsEmpty)
                        {
                            // Find the referenced type
                            foreach (var schema in GetSchemas(element.RefName, schemaSet))
                            {
                                var possible = schema.Elements[element.RefName] as XmlSchemaElement;

                                if (possible != null)
                                {
                                    // We process the referenced type
                                    element = possible;
                                    break;
                                }
                            }
                        }

                        var metaDataProperty = LoadProperty(element, originalElement, schemaSet);

                        if (metaDataProperty != null)
                            properties.Add(metaDataProperty);
                    }
                }
            }

            _oProperties = new List<SMEProperty>();
            _oNameToProperty = new Dictionary<string, SMEProperty>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var property in properties)
                AddProperty(property);

            OnLoaded();
        }

        private void AddProperty(SMEProperty property)
        {
            _oProperties.Add(property);
            _oNameToProperty[property.Name] = property;
        }

        #endregion

        #region Local Methods

        private SMEProperty LoadProperty(XmlSchemaElement element, XmlSchemaElement originalElement, XmlSchemaSet schemaSet)
        {
            SMEProperty metaDataProperty = null;

            if (element != null)
            {
                // Load the metadata from the schema type
                metaDataProperty = GetMetadataProperty(element.SchemaTypeName, element.QualifiedName, schemaSet);

                if (metaDataProperty != null)
                {
                    var hasRelationship = false;

                    metaDataProperty.Nillable = element.IsNillable;

                    // Load SData/SME attributes which are applied at the resource level
                    if (element.UnhandledAttributes != null && element.UnhandledAttributes.Length > 0)
                    {
                        foreach (var attribute in element.UnhandledAttributes)
                        {
                            if (SMERelationshipProperty.IsRelationshipAttribute(attribute.LocalName))
                                hasRelationship = true;

                            metaDataProperty.LoadUnhandledAttribute(attribute);
                        }
                    }

                    if (hasRelationship)
                    {
                        var relationship = new SMERelationshipProperty(metaDataProperty.Label, RelationshipType.Association);
                        relationship.Name = metaDataProperty.Name;
                        relationship.Namespace = metaDataProperty.Namespace;
                        foreach (var attribute in originalElement.UnhandledAttributes)
                        {
                            relationship.LoadUnhandledAttribute(attribute);
                        }

                        if (metaDataProperty is SDataResource)
                        {
                            ((SDataResource) metaDataProperty).RelationshipInformation = relationship;
                        }
                        else
                        {
                            metaDataProperty = relationship;
                        }
                    }

                    if (originalElement != null && originalElement != element)
                    {
                        // Load SData/SME attributes which are applied at the resource level
                        if (originalElement.UnhandledAttributes != null && originalElement.UnhandledAttributes.Length > 0)
                        {
                            foreach (var attribute in originalElement.UnhandledAttributes)
                                metaDataProperty.LoadUnhandledAttribute(attribute);
                        }
                    }

                    // Check for a collection
                    if (element.MaxOccursString == XmlConstants.Unbounded)
                    {
                        if (_oRelationshipInformation == null)
                        {
                            _oRelationshipInformation = new SMERelationshipProperty();
                            _oRelationshipInformation.IsCollection = true;
                        }
                    }
                }
            }

            return metaDataProperty;
        }

        private XmlSchemaParticle BeginLoadComplexType(IList<SMEProperty> properties, XmlSchemaComplexType complexType, XmlSchemaSet schemaSet)
        {
            // The typename defaults to the complex type
            _strTypeName = complexType.Name;

            // And the namespace defaults to the complex types
            Namespace = complexType.QualifiedName.Namespace;

            // Find the namespaces in use
            foreach (var schema in GetSchemas(complexType.QualifiedName, schemaSet))
            {
                foreach (var name in schema.Namespaces.ToArray())
                {
                    // Don't add default namespaces
                    if (!String.IsNullOrEmpty(name.Name))
                        _oNamespaces.AddNamespace(name.Name, name.Namespace);
                }
            }

            // Now find out what sort out content the Complex Type has
            var content = complexType.ContentModel;
            var complexContent = content as XmlSchemaComplexContent;
            var simpleContent = content as XmlSchemaSimpleContent;
            var particle = complexType.Particle;

            if (complexType.Attributes.Count > 0)
                LoadAttributes(properties, complexType.Attributes, schemaSet, Namespace);

            // Load base type information if any is present
            if (complexContent != null)
            {
                var complexExtension = complexContent.Content as XmlSchemaComplexContentExtension;

                if (complexExtension != null && TypeInfoHelper.IsValidQualifiedName(complexExtension.BaseTypeName))
                {
                    try
                    {
                        _oBaseType = MetadataManager.GetMetadata(complexExtension.BaseTypeName, schemaSet);
                    }
                    catch
                    {
                        _oBaseType = null; // Any error and we just set the base type as empty
                    }

                    particle = complexExtension.Particle;

                    if (complexExtension.Attributes.Count > 0)
                        LoadAttributes(properties, complexExtension.Attributes, schemaSet, Namespace);
                }
            }

            if (simpleContent != null)
            {
                var simpleExtension = simpleContent.Content as XmlSchemaSimpleContentExtension;

                if (simpleExtension != null)
                {
                    if (TypeInfoHelper.IsValidQualifiedName(simpleExtension.BaseTypeName))
                        _oBaseType = GetXSDMetadataProperty(simpleExtension.BaseTypeName, simpleExtension.BaseTypeName);

                    if (simpleExtension.Attributes.Count > 0)
                        LoadAttributes(properties, simpleExtension.Attributes, schemaSet, Namespace);
                }
            }

            // Load SData/SME attributes which are applied at the resource level
            if (complexType.UnhandledAttributes != null && complexType.UnhandledAttributes.Length > 0)
            {
                foreach (var attribute in complexType.UnhandledAttributes)
                    LoadUnhandledAttribute(attribute);
            }

            return particle;
        }

        private void LoadAttributes(IList<SMEProperty> properties, XmlSchemaObjectCollection schemaProperties, XmlSchemaSet schemaSet, string defaultNamespace)
        {
            foreach (var item in schemaProperties)
            {
                var groupRef = item as XmlSchemaAttributeGroupRef;

                if (groupRef != null)
                {
                    var attrs = GetSchemaObject(groupRef.RefName, schemaSet) as XmlSchemaAttributeGroup;

                    if (attrs != null)
                        LoadAttributes(properties, attrs.Attributes, schemaSet, defaultNamespace);
                }
                else
                {
                    var attr = item as XmlSchemaAttribute;

                    if (attr != null)
                    {
                        var name = attr.QualifiedName.Name;

                        if (String.IsNullOrEmpty(name))
                            continue;

                        XmlQualifiedName typeName;

                        if (!attr.RefName.IsEmpty)
                            typeName = attr.RefName;
                        else
                            typeName = attr.SchemaTypeName;

                        var metaDataProperty = GetMetadataProperty(typeName, attr.QualifiedName, schemaSet);

                        if (metaDataProperty != null)
                        {
                            metaDataProperty.IsAttribute = true;
                            metaDataProperty.Namespace = !string.IsNullOrEmpty(attr.QualifiedName.Namespace) ? attr.QualifiedName.Namespace : defaultNamespace;
                            properties.Add(metaDataProperty);
                        }
                    }
                }
            }
        }

        private SMEProperty GetMetadataProperty(XmlQualifiedName type, XmlQualifiedName name, XmlSchemaSet schemaSet)
        {
            SMEProperty metaDataProperty = null;

            if (string.IsNullOrEmpty(type.Namespace) && string.IsNullOrEmpty(type.Name))
            {
                metaDataProperty = new SMEProperty
                                   {
                                       Name = name.Name,
                                       Namespace = name.Namespace
                                   };
            }
            else if (type.Namespace == Framework.Common.XS.Namespace)
            {
                metaDataProperty = GetXSDMetadataProperty(type, name);
            }
            else
            {
                // Must be a simple or extended type
                var schemaObject = GetSchemaObject(type, schemaSet);
                var attr = schemaObject as XmlSchemaAttribute;

                if (attr != null)
                {
                    metaDataProperty = GetMetadataProperty(attr.SchemaTypeName, name, schemaSet);
                }
                else
                {
                    var simpleType = schemaObject as XmlSchemaSimpleType;

                    if (simpleType != null)
                    {
                        metaDataProperty = GetMetadataProperty(simpleType, name, schemaSet);
                    }
                    else
                    {
                        var complexType = schemaObject as XmlSchemaComplexType;

                        if (complexType != null)
                            metaDataProperty = GetMetadataProperty(complexType, name, schemaSet);
                    }
                }
            }

            return metaDataProperty;
        }

        private SMEProperty GetMetadataProperty(XmlSchemaSimpleType simpleType, XmlQualifiedName name, XmlSchemaSet schemaSet)
        {
            // Simple schema type
            SMEProperty metaDataProperty = null;
            var simpleTypeRestriction = simpleType.Content as XmlSchemaSimpleTypeRestriction;

            if (simpleTypeRestriction != null)
            {
                metaDataProperty = GetMetadataProperty(simpleTypeRestriction.BaseTypeName, name, schemaSet);

                if (metaDataProperty != null)
                {
                    // Load any facets
                    foreach (XmlSchemaFacet facet in simpleTypeRestriction.Facets)
                        metaDataProperty.LoadFacet(facet);
                }
            }

            return metaDataProperty;
        }

        private static SMEProperty GetMetadataProperty(XmlSchemaComplexType complexType, XmlQualifiedName name, XmlSchemaSet schemaSet)
        {
            // Find the metadata for the complex type
            var existing = MetadataManager.GetMetadata(complexType.QualifiedName, schemaSet);
            SMEProperty metaDataProperty;

            if (existing != null)
            {
                // Create a delegate property
                metaDataProperty = new SDataResource(existing);

                // Change the relevant properties (name and namespace)
                metaDataProperty.Name = name.Name;
                metaDataProperty.Namespace = name.Namespace;
            }
            else
            {
                metaDataProperty = null;
            }

            return metaDataProperty;
        }

        private static SMEProperty GetXSDMetadataProperty(XmlQualifiedName type, XmlQualifiedName name)
        {
            // The name referes to an XS Data Type
            var constructor = TypeInfoHelper.GetMetaDataConstructorFromXSDType(type.Name);

            if (constructor == null)
                return null;

            var property = (SMEProperty) constructor.Invoke(null);
            property.Name = name.Name;
            property.Namespace = name.Namespace;
            property.DataType = type.Name;

            return property;
        }

        private XmlSchemaObject GetSchemaObject(XmlQualifiedName name, XmlSchemaSet schemaSet)
        {
            XmlSchemaObject schemaObject = null;

            if (TypeInfoHelper.IsValidQualifiedName(name) && schemaSet != null)
            {
                foreach (var schema in GetSchemas(name, schemaSet))
                {
                    schemaObject = schema.SchemaTypes[name];

                    if (schemaObject == null)
                        schemaObject = schema.Elements[name];

                    if (schemaObject == null)
                        schemaObject = schema.Attributes[name];

                    if (schemaObject == null)
                        schemaObject = schema.AttributeGroups[name];

                    var schemaElement = schemaObject as XmlSchemaElement;

                    if (schemaElement != null)
                    {
                        if (schemaElement.UnhandledAttributes != null)
                        {
                            // We can have sme attributes on this element
                            foreach (var attr in schemaElement.UnhandledAttributes)
                                LoadUnhandledAttribute(attr);
                        }

                        schemaObject = schemaElement.SchemaType;

                        if (schemaObject == null)
                            schemaObject = GetSchemaObject(schemaElement.SchemaTypeName, schemaSet);
                    }

                    var schemaAttribute = schemaObject as XmlSchemaAttribute;

                    if (schemaAttribute != null)
                    {
                        schemaObject = schemaAttribute.SchemaType;

                        if (schemaObject == null)
                        {
                            if (schemaAttribute.SchemaTypeName.Namespace == Framework.Common.XS.Namespace)
                                schemaObject = schemaAttribute;
                            else
                                schemaObject = GetSchemaObject(schemaAttribute.SchemaTypeName, schemaSet);
                        }
                    }

                    if (schemaObject != null)
                        break;
                }
            }

            return schemaObject;
        }

        private static IEnumerable<XmlSchema> GetSchemas(XmlQualifiedName name, XmlSchemaSet schemaSet)
        {
            var schemas = new List<XmlSchema>();

            foreach (XmlSchema schema in schemaSet.Schemas(name.Namespace))
                schemas.Add(schema);

            return schemas.ToArray();
        }

        #endregion

        #region SDataResource Members

        /// <summary>
        /// Contains information about the resource type represented by
        /// this runtime object information
        /// </summary>
        public SMEResource ResourceInformation
        {
            get { return _oResourceInfo; }
        }

        /// <summary>
        /// Returns the <see cref="XmlNamespaceManager"/> for this property.
        /// </summary>
        /// <value>The <see cref="XmlNamespaceManager"/> for this property.</value>
        public XmlNamespaceManager Namespaces
        {
            get { return _oNamespaces; }
            set { _oNamespaces = value; }
        }

        /// <summary>
        /// Returns the <see cref="Array"/> of properties within this object.
        /// </summary>
        /// <value><see cref="Array"/> of properties within this object.</value>
        public SMEProperty[] Properties
        {
            get { return _oProperties.ToArray(); }
        }

        /// <summary>
        /// Returns the property for the specified name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The property for the specified name.</returns>
        public SMEProperty this[string name]
        {
            get
            {
                SMEProperty property;

                if (!_oNameToProperty.TryGetValue(name, out property))
                {
                    var nextPart = name.IndexOf("/");

                    if (nextPart != -1)
                    {
                        var part = name.Substring(0, nextPart);

                        if (_oNameToProperty.TryGetValue(part, out property))
                        {
                            var runtimeObject = property as SDataResource;

                            if (runtimeObject != null)
                                return runtimeObject[name.Substring(nextPart + 1)];
                            return null;
                        }
                    }

                    var baseType = _oBaseType as SDataResource;

                    if (baseType != null)
                        return baseType[name];
                    return null;
                }
                return property;
            }
        }

        /// <summary>
        /// Returns the default average length (in characters) to use for this property.
        /// </summary>
        /// <returns>The default average length (in characters) to use for this property.</returns>
        protected override int GetDefaultAverageLength()
        {
            return 0;
        }

        /// <summary>
        /// Gets or sets the base type of this runtime property.
        /// </summary>
        public SMEProperty BaseType
        {
            get { return _oBaseType; }
            set { _oBaseType = value; }
        }

        /// <summary>
        /// Gets or sets the type name of the Xml element or attribute.
        /// </summary>
        /// <value>The type name of the Xml element or attribute.</value>
        public string TypeName
        {
            get { return _strTypeName; }
        }

        /// <summary>
        /// Gets or sets the relationship information for the Xml element or attribute.
        /// </summary>
        /// <value>The relationship information for the Xml element or attribute</value>
        public SMERelationshipProperty RelationshipInformation
        {
            get { return _oRelationshipInformation; }
            set { _oRelationshipInformation = value; }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns the XS Data Type for this property.
        /// </summary>
        protected override string GetXSDType()
        {
            if (!String.IsNullOrEmpty(TypeName))
            {
                var xsdType = TypeName;

                if (!xsdType.EndsWith(XmlConstants.SingleSuffix) && !xsdType.EndsWith(XmlConstants.ListSuffix))
                {
                    if (_oRelationshipInformation == null || !_oRelationshipInformation.IsCollection)
                        xsdType = FormatSingleType(xsdType, Suffix);
                    else
                        xsdType = FormatListType(xsdType);
                }

                return xsdType;
            }

            return FormatSingleType(Name, Suffix);
        }

        /// <summary>
        /// Formats the schema for this property.
        /// </summary>
        /// <param name="relatedTypes">Contains the schema details for any related types.</param>
        /// <param name="isInline">Indicates that the generated schema is to be inlined.</param>
        /// <param name="isRoot">Indicates that the current property is the root resource in the schema.</param>
        /// <returns>The schema for this property.</returns>
        public string GetSchema(IDictionary<string, string> relatedTypes, bool isInline, bool isRoot)
        {
            return OnGetSchema(relatedTypes, isInline, isRoot);
        }

        /// <summary>
        /// Formats the schema for this property.
        /// </summary>
        /// <param name="relatedTypes">Contains the schema details for any related types.</param>
        /// <returns>The schema for this property.</returns>
        protected override string OnGetSchema(IDictionary<string, string> relatedTypes)
        {
            return OnGetSchema(relatedTypes, true, true);
        }

        /// <summary>
        /// Formats the schema for this property.
        /// </summary>
        /// <param name="relatedTypes">Contains the schema details for any related types.</param>
        /// <param name="isInline">Indicates that the generated schema is to be inlined.</param>
        /// <param name="isRoot">Indicates that the current property is the root resource in the schema.</param>
        /// <returns>The schema for this property.</returns>
        protected virtual string OnGetSchema(IDictionary<string, string> relatedTypes, bool isInline, bool isRoot)
        {
            var prefix = Namespaces.LookupPrefix(Namespace);
            var xsdType = GetXSDType();
            var prefixXSDType = FormatWithPrefix(prefix, xsdType);
            var relationshipSchema = SMERelationshipProperty.GetSchemaAttributes(_oRelationshipInformation, _oResourceInfo);

            if (relationshipSchema.Length > 0)
                relationshipSchema = " " + relationshipSchema;

            var schema = String.Format("<{0} {1}=\"{2}\" {3}=\"{4}\"{5}/>",
                                       TypeInfoHelper.FormatXS(XmlConstants.Element),
                                       XmlConstants.Name,
                                       NormaliseName(Name),
                                       XmlConstants.Type,
                                       prefixXSDType,
                                       relationshipSchema);

            OnGetRelatedSchemaTypes(NormaliseName(_oSource == null ? TypeName : _oSource.TypeName), relatedTypes, isInline, isRoot);

            return schema;
        }

        private static string NormaliseName(string name)
        {
            if (name.EndsWith(XmlConstants.ListSuffix))
                name = name.Substring(0, name.Length - XmlConstants.ListSuffix.Length);
            else if (name.EndsWith(XmlConstants.SingleSuffix))
                name = name.Substring(0, name.Length - XmlConstants.SingleSuffix.Length);

            return name;
        }

        private string GetElementName()
        {
            var name = Name;

            if (_oSource != null)
            {
                var possibleName = _oSource.Name;

                // If the name is the default name as specified by the framework, we
                // will just use the name as specified on the instance
                if (!Framework.Common.IsFrameworkNamespace(_oSource.Namespace))
                {
                    if (_oSource._oSource == null ||
                        _oSource.Name != _oSource.BaseType.Name)
                    {
                        name = possibleName;
                    }
                }
            }

            return NormaliseName(name);
        }

        /// <summary>
        /// Returns the related types for this property.
        /// </summary>
        /// <param name="relatedTypes">On exit contains the schema details for any related types.</param>
        protected override void OnGetRelatedSchemaTypes(string xsdType, IDictionary<string, string> relatedTypes)
        {
            OnGetRelatedSchemaTypes(xsdType, relatedTypes, true, true);
        }

        /// <summary>
        /// Returns the related types for this property.
        /// </summary>
        /// <param name="relatedTypes">On exit contains the schema details for any related types.</param>
        /// <param name="isInline">Indicates that the generated schema is to be inlined.</param>
        /// <param name="isRoot">Indicates that the current property is the root resource in the schema.</param>
        protected virtual void OnGetRelatedSchemaTypes(string xsdType, IDictionary<string, string> relatedTypes, bool isInline, bool isRoot)
        {
            if (relatedTypes.ContainsKey(xsdType))
                return; // Nothing to do...

            // Add a blank entry to stop recursion...
            relatedTypes[xsdType] = String.Empty;

            // At this point we create the following
            // 1) The element for the single type
            // 2) The complex type for the single type
            // 3) The complex type for the list type
            string resourceSchema;

            if (_oResourceInfo != null)
            {
                resourceSchema = SMEResource.GetSchemaAttributes(_oResourceInfo);

                if (resourceSchema.Length > 0)
                    resourceSchema = " " + resourceSchema;
            }
            else
            {
                resourceSchema = String.Empty;
            }

            var prefix = Namespaces.LookupPrefix(Namespace);
            var builder = new StringBuilder();
            var singleType = FormatSingleType(xsdType, Suffix);
            var listType = FormatListType(xsdType);
            var prefixSingleType = FormatWithPrefix(prefix, singleType);
            var name = GetElementName();

            if (!isInline || isRoot)
            {
                // ### Element for single type

                // <xs:element name="x" type="x" sme:resource.../>
                builder.AppendFormat("<{0} {1}=\"{2}\" {3}=\"{4}\"{5}/>",
                                     TypeInfoHelper.FormatXS(XmlConstants.Element),
                                     XmlConstants.Name,
                                     name,
                                     XmlConstants.Type,
                                     prefixSingleType,
                                     resourceSchema);

                // ###Complex type for the list type

                if (ResourceInformation.Role == RoleType.ResourceKind && !isInline)
                {
                    // <xs:complexType name="x">
                    builder.AppendFormat("<{0} {1}=\"{2}\">",
                                         TypeInfoHelper.FormatXS(XmlConstants.ComplexType),
                                         XmlConstants.Name,
                                         listType);

                    // <xs:sequence>
                    builder.AppendFormat("<{0}>", TypeInfoHelper.FormatXS(XmlConstants.Sequence));

                    // <xs:element minOccurs="0" maxOccurs="unbounded" name="x" type="x" />
                    builder.AppendFormat("<{0} {1}=\"{2}\" {3}=\"{4}\" {5}=\"{6}\" {7}=\"{8}\"/>",
                                         TypeInfoHelper.FormatXS(XmlConstants.Element),
                                         XmlConstants.MinOccurs, 0,
                                         XmlConstants.MaxOccurs, XmlConstants.Unbounded,
                                         XmlConstants.Name, Name,
                                         XmlConstants.Type, prefixSingleType);

                    // </xs:sequence>
                    builder.AppendFormat("</{0}>", TypeInfoHelper.FormatXS(XmlConstants.Sequence));

                    //</xs:complexType>
                    builder.AppendFormat("</{0}>", TypeInfoHelper.FormatXS(XmlConstants.ComplexType));
                }
            }

            // ###Complex type for the single type

            // <xs:complexType name="x">
            builder.AppendFormat("<{0} {1}=\"{2}\">",
                                 TypeInfoHelper.FormatXS(XmlConstants.ComplexType),
                                 XmlConstants.Name,
                                 singleType);

            // <xs:all>
            builder.AppendFormat("<{0}>", TypeInfoHelper.FormatXS(XmlConstants.All));

            foreach (var metaData in MetadataManager.GetMetadataChain(this))
            {
                foreach (var property in metaData.Properties)
                {
                    // Suppress properties declared within the Atom, Http or SData namespace
                    if (Framework.Common.IsFrameworkNamespace(property.Namespace))
                        continue;

                    var obj = property as SDataResource;

                    if (obj != null)
                        builder.Append(obj.GetSchema(relatedTypes, isInline, false));
                    else
                        builder.Append(property.GetSchema(relatedTypes));
                }
            }

            // </xs:all>
            builder.AppendFormat("</{0}>", TypeInfoHelper.FormatXS(XmlConstants.All));

            //</xs:complexType>
            builder.AppendFormat("</{0}>", TypeInfoHelper.FormatXS(XmlConstants.ComplexType));

            relatedTypes[xsdType] = builder.ToString();
        }

        private static string FormatWithPrefix(string prefix, string name)
        {
            return String.IsNullOrEmpty(prefix) ? name : prefix + ":" + name;
        }

        /// <summary>
        /// Returns a value indicating if the property schema has facets.
        /// </summary>
        /// <value><b>true</b> if the property schema has facets; otherwise, <b>false</b>.</value>
        protected override bool HasFacets
        {
            get { return false; }
        }

        protected override void OnLoadUnhandledAttribute(XmlAttribute attribute)
        {
            base.OnLoadUnhandledAttribute(attribute);

            // The 'resource information' stores information about the type,
            // which can be specified using attributes on the xml type definition
            if (_oResourceInfo != null)
                _oResourceInfo.LoadUnhandledAttribute(attribute);
        }

        /// <summary>
        /// Validates the specified value against the constraints of the property.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        protected override void OnValidate(object value)
        {
            OnValidate(value, value == null ? null : value.GetType());

            if (_oRelationshipInformation != null && _oRelationshipInformation.IsCollection)
            {
                if (_oRelationshipInformation.MinOccurs != 0 || _oRelationshipInformation.MaxOccurs > 0)
                {
                    if (value == null && _oRelationshipInformation.MinOccurs > 0)
                        ThrowValidationFailed(XmlConstants.MinOccurs);

                    var list = value as IList;

                    if (list == null)
                    {
                        var feed = value as AtomFeed;

                        if (feed != null)
                            list = new List<AtomEntry>(feed.Entries);
                    }

                    if (list != null)
                    {
                        if (list.Count < _oRelationshipInformation.MinOccurs)
                            ThrowValidationFailed(XmlConstants.MinOccurs);

                        if (_oRelationshipInformation.MaxOccurs != 0 && list.Count > _oRelationshipInformation.MaxOccurs)
                            ThrowValidationFailed(XmlConstants.MaxOccurs);
                    }
                }
            }
        }

        #endregion

        #region Delayed Loading

        internal delegate void LoadedEventDelegate(SDataResource metadata);

        internal event LoadedEventDelegate Loaded;

        private void OnLoaded()
        {
            _bLoading = false;

            if (Loaded != null)
                Loaded(this);
        }

        private void Clone_Loaded(SDataResource metadata)
        {
            var original = metadata;

            original.Loaded -= Clone_Loaded;
            Clone(metadata);
        }

        private void Clone(SDataResource source)
        {
            _strTypeName = source.TypeName;
            _oBaseType = source.BaseType;

            if (source.RelationshipInformation != null && _oRelationshipInformation == null)
                _oRelationshipInformation = new SMERelationshipProperty(source.RelationshipInformation);

            if (source.ResourceInformation != null)
                _oResourceInfo = new SMEResource(source.ResourceInformation);

            _oProperties = new List<SMEProperty>();
            _oNameToProperty = new Dictionary<string, SMEProperty>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var property in source.Properties)
                AddProperty(property);

            _oNamespaces = new XmlNamespaceManager(new NameTable());

            foreach (var ns in source.Namespaces.GetNamespacesInScope(XmlNamespaceScope.All))
            {
                // Don't add default namespaces
                if (!String.IsNullOrEmpty(ns.Key))
                    _oNamespaces.AddNamespace(ns.Key, ns.Value);
            }
        }

        #endregion
    }
}