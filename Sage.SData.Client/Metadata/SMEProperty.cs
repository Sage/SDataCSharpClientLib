using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Common properties for providing Metadata against objects.
    /// </summary>
    public class SMEProperty
    {
        #region Constants

        private const string CanSortName = "canSort";
        private const string CanFilterName = "canFilter";
        private const string CanGroupName = "canGroup";
        private const string PrecedenceName = "precedence";
        private const string LabelName = "label";
        private const string GroupNameName = "groupName";
        private const string IsMandatoryName = "isMandatory";
        private const string IsUniqueKeyName = "isUniqueKey";
        private const string IsGlobalIdName = "isGlobalId";
        private const string IsLocalizedName = "isLocalized";
        private const string IsIdentifierName = "isIdentifier";
        private const string IsDescriptorName = "isDescriptor";
        private const string IsReadOnlyName = "isReadOnly";
        private const string CopiedFromName = "copiedFrom";
        private const string UnsupportedName = "unsupported";

        #endregion

        #region Fields

        private string _strLabel;
        private string _strPattern;
        private int _iAverageLength;
        private bool _bCanSort;
        private bool _bCanGroup;
        private bool _bCanFilter;
        private int _iPrecedence;
        private string _strGroupName;
        private bool _bIsMandatory;
        private bool _bIsUniqueKey;
        private bool _bIsGlobalId;
        private bool _bIsLocalized;
        private bool _bIsIdentifier;
        private bool _bIsDescriptor;
        private bool _bIsReadOnly;
        private string _strCopiedFrom;
        private bool _bUnsupported;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        protected SMEProperty(string label)
        {
            _strLabel = label;

            // NOTE: we differ to the SData specification here as
            // we default to 1, which means that the property is always
            // visible.  To hide the property the precedence will need to
            // be set to 0.
            _iPrecedence = 1;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEProperty"/> class.
        /// </summary>
        protected internal SMEProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SDataResource"/> class
        /// using the details of the specified instance.
        /// </summary>
        /// <param name="copy">The instance to copy.</param>
        internal SMEProperty(SMEProperty copy)
        {
            _strLabel = copy.Label;
            Default = copy.Default;
            Nillable = copy.Nillable;
            _strPattern = copy.Pattern;
            _iAverageLength = copy.AverageLength;
            IsAttribute = copy.IsAttribute;
            Name = copy.Name;
            Namespace = copy.Namespace;
            DataType = copy.DataType;
            _bCanSort = copy.CanSort;
            _bCanGroup = copy.CanGroup;
            _bCanFilter = copy.CanFilter;
            _iPrecedence = copy.Precedence;
            _strGroupName = copy.GroupName;
            _bIsMandatory = copy.IsMandatory;
            _bIsUniqueKey = copy.IsUniqueKey;
            _bIsGlobalId = copy.IsGlobalId;
            _bIsLocalized = copy.IsLocalized;
            _bIsIdentifier = copy.IsIdentifier;
            _bIsDescriptor = copy.IsDescriptor;
            _bIsReadOnly = copy.IsReadOnly;
            _strCopiedFrom = copy.CopiedFrom;
            _bUnsupported = copy.Unsupported;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the label for the property.
        /// </summary>
        /// <value>The label for the property.</value>
        public string Label
        {
            get { return _strLabel ?? Name; }
            set { _strLabel = value; }
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public object Default { get; set; }

        /// <summary>
        /// Returns or sets a value indicating if <b>null</b> values are acceptable.
        /// </summary>
        /// <value><b>true</b> if a null value is accepted; otherwise, <b>false</b>.</value>
        public bool Nillable { get; set; }

        /// <summary>
        /// Gets or sets a Regular Expression pattern that can be used as
        /// a constraint on the property.
        /// </summary>
        /// <value>A <see cref="String"/> containing the Regular Expression that can be used as a constraint on the property.</value>
        public string Pattern
        {
            get { return _strPattern; }
            set { _strPattern = value; }
        }

        /// <summary>
        /// Gets or sets the average length (in characters) of this property.
        /// </summary>
        /// <value>The average length (in characters) of this property.</value>
        public int AverageLength
        {
            get { return _iAverageLength <= 0 ? GetDefaultAverageLength() : _iAverageLength; }
            set { _iAverageLength = value; }
        }

        /// <summary>
        /// Gets or sets a flag indicating if this property is an attribute.
        /// </summary>
        /// <value><b>true</b> if this property is an attribute; otherwise, <b>false</b> if the property is an element</value>
        public bool IsAttribute { get; set; }

        /// <summary>
        /// Gets or sets the name of the element or attribute.
        /// </summary>
        /// <value>The name of the element or attribute.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the namespace for the Xml element or attribute.
        /// </summary>
        /// <value>The namespace for the Xml element or attribute.</value>
        public string Namespace { get; set; }

        /// <summary>
        /// The XS Data Type for this Xml element or attribute.
        /// </summary>
        /// <value>The XS Data Type for this Xml element or attribute.</value>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if this property can be sorted against.
        /// </summary>
        /// <value><b>true</b> if this property can be sorted against; otherwise, <b>false</b>.</value>
        public bool CanSort
        {
            get { return _bCanSort; }
            set { _bCanSort = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating if this property can be filtered against.
        /// </summary>
        /// <value><b>true</b> if this property can be filtered against; otherwise, <b>false</b>.</value>
        public bool CanFilter
        {
            get { return _bCanFilter; }
            set { _bCanFilter = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating if this property can be grouped against.
        /// </summary>
        /// <value><b>true</b> if this property can be grouped against; otherwise, <b>false</b>.</value>
        public bool CanGroup
        {
            get { return _bCanGroup; }
            set { _bCanGroup = value; }
        }

        /// <summary>
        /// A group (category) name to group related properties.
        /// </summary>
        public string GroupName
        {
            get { return _strGroupName; }
            set { _strGroupName = value; }
        }

        /// <summary>
        /// Gets or sets a value controlling how the value should be displayed on screens
        /// with limited space.
        /// </summary>
        /// <value><b>1</b> means always visible, <b>2</b> and above means visible if there is 
        /// enough space (the higher the value, the lower the priority), <b>0</b> means
        /// no precedence.</value>
        public int Precedence
        {
            get { return _iPrecedence; }
            set { _iPrecedence = value; }
        }

        /// <summary>
        /// Indicates whether the property value is mandatory or not when creating a new resource
        /// </summary>
        public bool IsMandatory
        {
            get { return _bIsMandatory; }
            set { _bIsMandatory = value; }
        }

        /// <summary>
        /// Indicates if the property is a key that identifies a unique resource.
        /// </summary>
        public bool IsUniqueKey
        {
            get { return _bIsUniqueKey; }
            set { _bIsUniqueKey = value; }
        }

        /// <summary>
        /// Indicates if the property is a global identifier for the resource.
        /// </summary>
        [Obsolete("Depreciated in SData version 1.0")]
        public bool IsGlobalId
        {
            get { return _bIsGlobalId; }
            set { _bIsGlobalId = value; }
        }

        /// <summary>
        /// Indicates if the property contains localized text.
        /// </summary>
        public bool IsLocalized
        {
            get { return _bIsLocalized; }
            set { _bIsLocalized = value; }
        }

        /// <summary>
        /// Indicates if the property is an identifier for the resource.
        /// </summary>
        [Obsolete("Depreciated in SData version 1.0")]
        public bool IsIdentifier
        {
            get { return _bIsIdentifier; }
            set { _bIsIdentifier = value; }
        }

        /// <summary>
        /// Indicates if the property is a descriptor for the resource.
        /// </summary>
        [Obsolete("Depreciated in SData version 1.0")]
        public bool IsDescriptor
        {
            get { return _bIsDescriptor; }
            set { _bIsDescriptor = value; }
        }

        /// <summary>
        /// Indicates if the property is read-only.  For example, an ID 
        /// set by the provider or a computed property.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _bIsReadOnly; }
            set { _bIsReadOnly = value; }
        }

        /// <summary>
        /// Indicates that the property is dependent upon a related resource 
        /// and gives the XPath expression for the corresponding property in the related resouce 
        /// </summary>
        [Obsolete("Depreciated in SData version 1.0")]
        public string CopiedFrom
        {
            get { return _strCopiedFrom; }
            set { _strCopiedFrom = value; }
        }

        /// <summary>
        /// Indicates that the element is part of a global contract but 
        /// that it is not supported by this specific provider.
        /// </summary>
        public bool Unsupported
        {
            get { return _bUnsupported; }
            set { _bUnsupported = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads a facet for this property.
        /// </summary>
        /// <param name="facet">The facet to load for this property.</param>
        public void LoadFacet(XmlSchemaFacet facet)
        {
            OnLoadFacet(facet);
        }

        /// <summary>
        /// Loads an unhandled attribute.
        /// </summary>
        /// <param name="attribute">The unhandled attribute.</param>
        public void LoadUnhandledAttribute(XmlAttribute attribute)
        {
            OnLoadUnhandledAttribute(attribute);
        }

        /// <summary>
        /// Validates the specified value against the constraints of the property.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        public void Validate(object value)
        {
            OnValidate(value);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1}, {2})", Name, DataType, Namespace);
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Validates the specified value against the constraints of the property.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        protected virtual void OnValidate(object value)
        {
        }

        /// <summary>
        /// Validates the specified value against the constraints of the property.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        protected void OnValidate(object value, Type type)
        {
            if (value == null)
            {
                if (IsMandatory)
                    ThrowValidationFailed(IsMandatoryName);

                if (!Nillable)
                    ThrowValidationFailed(SDataResource.XmlConstants.Nillable);
            }
            else
            {
                if (!type.IsAssignableFrom(value.GetType()))
                    ThrowValidationFailed("type");

                var pattern = Pattern;

                if (!String.IsNullOrEmpty(pattern))
                {
                    if (!Regex.Match(value.ToString(), pattern).Success)
                        ThrowValidationFailed(SDataResource.XmlConstants.Pattern);
                }
            }
        }

        protected void ThrowValidationFailed(string reason)
        {
            throw new InvalidOperationException(string.Format("Validation of property {0} failed on {1} constraint", Name, reason));
        }

        /// <summary>
        /// Loads a facet for this property.
        /// </summary>
        /// <param name="facet">The facet to load for this property.</param>
        protected virtual void OnLoadFacet(XmlSchemaFacet facet)
        {
            var patternFacet = facet as XmlSchemaPatternFacet;

            if (patternFacet != null)
                _strPattern = patternFacet.Value;
        }

        /// <summary>
        /// Loads an unhandled attribute.
        /// </summary>
        /// <param name="attribute">The unhandled attribute.</param>
        protected virtual void OnLoadUnhandledAttribute(XmlAttribute attribute)
        {
            if (attribute.NamespaceURI == Framework.Common.SME.Namespace)
            {
                switch (attribute.LocalName)
                {
                    case CanSortName:
                        bool.TryParse(attribute.Value, out _bCanSort);
                        break;

                    case CanFilterName:
                        bool.TryParse(attribute.Value, out _bCanFilter);
                        break;

                    case CanGroupName:
                        bool.TryParse(attribute.Value, out _bCanGroup);
                        break;

                    case PrecedenceName:
                        int.TryParse(attribute.Value, out _iPrecedence);
                        break;

                    case GroupNameName:
                        _strGroupName = attribute.Value;
                        break;

                    case IsMandatoryName:
                        bool.TryParse(attribute.Value, out _bIsMandatory);
                        break;

                    case IsUniqueKeyName:
                        bool.TryParse(attribute.Value, out _bIsUniqueKey);
                        break;

                    case IsGlobalIdName:
                        bool.TryParse(attribute.Value, out _bIsGlobalId);
                        break;

                    case IsLocalizedName:
                        bool.TryParse(attribute.Value, out _bIsLocalized);
                        break;

                    case IsIdentifierName:
                        bool.TryParse(attribute.Value, out _bIsIdentifier);
                        break;

                    case IsDescriptorName:
                        bool.TryParse(attribute.Value, out _bIsDescriptor);
                        break;

                    case IsReadOnlyName:
                        bool.TryParse(attribute.Value, out _bIsReadOnly);
                        break;

                    case CopiedFromName:
                        _strCopiedFrom = attribute.Value;
                        break;

                    case UnsupportedName:
                        bool.TryParse(attribute.Value, out _bUnsupported);
                        break;

                    case LabelName:
                        _strLabel = attribute.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// Returns the default average length (in characters) to use for this property.
        /// </summary>
        /// <returns>The default average length (in characters) to use for this property.</returns>
        protected virtual int GetDefaultAverageLength()
        {
            return 0;
        }

        /// <summary>
        /// Returns a value indicating if the property schema has facets.
        /// </summary>
        /// <value><b>true</b> if the property schema has facets; otherwise, <b>false</b>.</value>
        protected virtual bool HasFacets
        {
            get { return !String.IsNullOrEmpty(Pattern); }
        }

        protected virtual string Suffix
        {
            get { return SDataResource.XmlConstants.SingleSuffix; }
        }

        /// <summary>
        /// Returns the XS Data Type for this property.
        /// </summary>
        /// <returns>The XSD Data Type for this property.</returns>
        protected virtual string GetXSDType()
        {
            return TypeInfoHelper.GetXSDataType(GetType());
        }

        /// <summary>
        /// Returns the XS Data Type for this property.
        /// </summary>
        /// <param name="relatedTypes">Contains the schema details for any related types.</param>
        /// <returns>The XSD Data Type for this property.</returns>
        protected virtual string GetSchemaXSDType(IDictionary<string, string> relatedTypes)
        {
            return HasFacets ? GetUniqueTypeName(relatedTypes, Name) : GetXSDType();
        }

        /// <summary>
        /// Constructs a unique type name.
        /// </summary>
        /// <param name="relatedTypes">Contains the schema details for any related types.</param>
        /// <param name="name">Name to base the type name on.</param>
        /// <returns>A unique type name.</returns>
        private string GetUniqueTypeName(IDictionary<string, string> relatedTypes, string name)
        {
            //find the parent resource name and use it as a property name prefix
            foreach (var relatedType in relatedTypes)
            {
                if (relatedType.Value == string.Empty)
                {
                    var prefix = relatedType.Key;

                    if (prefix.EndsWith(SDataResource.XmlConstants.SingleSuffix))
                    {
                        prefix = prefix.Substring(0, prefix.Length - SDataResource.XmlConstants.SingleSuffix.Length);
                    }

                    name = string.Format("{0}-{1}", prefix, name);
                    break;
                }
            }

            name = SDataResource.FormatSingleType(name, Suffix);

            if (relatedTypes.ContainsKey(name))
            {
                var suffix = String.Empty;

                if (name.EndsWith(SDataResource.XmlConstants.SingleSuffix))
                {
                    name = name.Substring(0, name.Length - SDataResource.XmlConstants.SingleSuffix.Length);
                    suffix = SDataResource.XmlConstants.SingleSuffix;
                }

                for (var i = 0;; i++)
                {
                    var localName = name + i + suffix;

                    if (!relatedTypes.ContainsKey(localName))
                    {
                        name = localName;
                        break;
                    }
                }
            }

            return name;
        }

        /// <summary>
        /// Formats the schema for this property.
        /// </summary>
        /// <param name="relatedTypes">Contains the schema details for any related types.</param>
        /// <returns>The schema for this property.</returns>
        public string GetSchema(IDictionary<string, string> relatedTypes)
        {
            return OnGetSchema(relatedTypes);
        }

        /// <summary>
        /// Formats the schema for this property.
        /// </summary>
        /// <param name="relatedTypes">Contains the schema details for any related types.</param>
        /// <returns>The schema for this property.</returns>
        protected virtual string OnGetSchema(IDictionary<string, string> relatedTypes)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("<{0} ", TypeInfoHelper.FormatXS(SDataResource.XmlConstants.Element));

            var xsdType = GetSchemaXSDType(relatedTypes);

            OnGetRelatedSchemaTypes(xsdType, relatedTypes);

            builder.AppendFormat("{0}=\"{1}\" {2}=\"{3}\" {4}=\"{5}\"",
                                 SDataResource.XmlConstants.Name,
                                 Name,
                                 SDataResource.XmlConstants.Type,
                                 xsdType,
                                 SDataResource.XmlConstants.MinOccurs,
                                 0
                );

            foreach (var pair in OnGetSchemaAttributes())
            {
                builder.AppendFormat(" {0}=\"{1}\"", pair.Key, pair.Value);
            }

            builder.Append("/>");

            return builder.ToString();
        }

        protected virtual IDictionary<string, string> OnGetSchemaAttributes()
        {
            IDictionary<string, string> attributes = new Dictionary<string, string>();

            if (Label != null)
            {
                attributes.Add(
                    SDataResource.FormatSME(LabelName),
                    TypeInfoHelper.Escape(Label)
                    );
            }

            if (Nillable)
            {
                attributes.Add(
                    SDataResource.XmlConstants.Nillable,
                    Nillable.ToString().ToLower()
                    );
            }

            if (Precedence > 0)
            {
                attributes.Add(
                    SDataResource.FormatSME(PrecedenceName),
                    Precedence.ToString()
                    );
            }

            if (CanGroup)
            {
                attributes.Add(
                    SDataResource.FormatSME(CanGroupName),
                    CanGroup.ToString().ToLower()
                    );
            }

            if (CanFilter)
            {
                attributes.Add(
                    SDataResource.FormatSME(CanFilterName),
                    CanFilter.ToString().ToLower()
                    );
            }

            if (CanSort)
            {
                attributes.Add(
                    SDataResource.FormatSME(CanSortName),
                    CanSort.ToString().ToLower()
                    );
            }

            if (IsDescriptor)
            {
                attributes.Add(
                    SDataResource.FormatSME(IsDescriptorName),
                    IsDescriptor.ToString().ToLower()
                    );
            }

            if (IsIdentifier)
            {
                attributes.Add(
                    SDataResource.FormatSME(IsIdentifierName),
                    IsIdentifier.ToString().ToLower()
                    );
            }

            if (IsReadOnly)
            {
                attributes.Add(
                    SDataResource.FormatSME(IsReadOnlyName),
                    IsReadOnly.ToString().ToLower()
                    );
            }

            if (IsGlobalId)
            {
                attributes.Add(
                    SDataResource.FormatSME(IsGlobalIdName),
                    IsGlobalId.ToString().ToLower()
                    );
            }

            if (IsMandatory)
            {
                attributes.Add(
                    SDataResource.FormatSME(IsMandatoryName),
                    IsMandatory.ToString().ToLower()
                    );
            }

            if (IsUniqueKey)
            {
                attributes.Add(
                    SDataResource.FormatSME(IsUniqueKeyName),
                    IsUniqueKey.ToString().ToLower()
                    );
            }

            if (Unsupported)
            {
                attributes.Add(
                    SDataResource.FormatSME(UnsupportedName),
                    Unsupported.ToString().ToLower()
                    );
            }

            if (!String.IsNullOrEmpty(GroupName))
            {
                attributes.Add(
                    SDataResource.FormatSME(GroupNameName),
                    GroupName
                    );
            }

            if (!String.IsNullOrEmpty(CopiedFrom))
            {
                attributes.Add(
                    SDataResource.FormatSME(CopiedFromName),
                    CopiedFrom
                    );
            }

            if (IsLocalized)
            {
                attributes.Add(
                    SDataResource.FormatSME(IsLocalizedName),
                    IsLocalized.ToString().ToLower()
                    );
            }

            return attributes;
        }

        /// <summary>
        /// Returns the related types for this property.
        /// </summary>
        /// <param name="xsdType">The XSD Data Type.</param>
        /// <param name="relatedTypes">On exit contains the schema details for any related types.</param>
        protected virtual void OnGetRelatedSchemaTypes(string xsdType, IDictionary<string, string> relatedTypes)
        {
            if (!HasFacets)
                return;

            // Construct a simpleText
            var builder = new StringBuilder();

            // <xs:simpleType name="x">
            builder.AppendFormat("<{0} {1}=\"{2}\">",
                                 TypeInfoHelper.FormatXS(SDataResource.XmlConstants.SimpleType),
                                 SDataResource.XmlConstants.Name,
                                 xsdType
                );

            // <xs:restriction base="xs:string">
            builder.AppendFormat("<{0} {1}=\"{2}\"",
                                 TypeInfoHelper.FormatXS(SDataResource.XmlConstants.Restriction),
                                 SDataResource.XmlConstants.Base,
                                 TypeInfoHelper.GetXSDataType(GetType()));

            builder.Append(">");

            OnGetSchemaFacets(builder);

            // </xs:restriction>
            builder.AppendFormat("</{0}>",
                                 TypeInfoHelper.FormatXS(SDataResource.XmlConstants.Restriction)
                );

            //</xs:simpleType>
            builder.AppendFormat("</{0}>",
                                 TypeInfoHelper.FormatXS(SDataResource.XmlConstants.SimpleType)
                );

            relatedTypes[xsdType] = builder.ToString();
        }

        /// <summary>
        /// Adds the schema facets for this property.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to add the facets to.</param>
        protected virtual void OnGetSchemaFacets(StringBuilder builder)
        {
            if (!String.IsNullOrEmpty(Pattern))
            {
                // <xs:pattern value="x" />
                builder.AppendFormat("<{0} {1}=\"{2}\"/>",
                                     TypeInfoHelper.FormatXS(SDataResource.XmlConstants.Pattern),
                                     SDataResource.XmlConstants.Value,
                                     Pattern
                    );
            }
        }

        #endregion
    }
}