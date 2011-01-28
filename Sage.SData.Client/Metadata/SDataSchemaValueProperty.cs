using System;
using System.Collections.Generic;
using System.Xml;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaValueProperty : SDataSchemaProperty
    {
        /// <summary>
        /// Can the feed be sorted by this property?
        /// </summary>
        public bool CanSort { get; set; }

        /// <summary>
        /// Can the feed be filtered by the property?
        /// </summary>
        public bool CanFilter { get; set; }

        /// <summary>
        /// Can feed entries be grouped by values of this property?
        /// </summary>
        public bool CanGroup { get; set; }

        /// <summary>
        /// Does the property contain localized text?
        /// </summary>
        public bool IsLocalized { get; set; }

        /// <summary>
        /// Is the property a key that identifies a unique resource.
        /// </summary>
        public bool IsUniqueKey { get; set; }

        /// <summary>
        /// Controls the visibility of properties on small screens.
        /// </summary>
        public int Precedence { get; set; }

        /// <summary>
        /// A group (category) name to group related properties.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Maximum length of a string property value.
        /// This attribute is a hint for the user interface. A service consumer may pass a string which is longer than maxLength. In this case, the value will be truncated by the provider.
        /// A more sophisticated consumer may use this value to limit the size of its edit field and/or adjust its storage requirements so that they match what the provider uses.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Average length (number of characters) of a string property value.
        /// This attribute is a hint for the user interface. A service consumer can use it to set the visible size of edit fields in a form.
        /// </summary>
        public int AverageLength { get; set; }

        /// <summary>
        /// Maximum overall number of digits for a decimal property value.
        /// This attribute is a hint for the user interface. A service consumer may ignore it and pass a decimal value with more digits. In this case the provider will round or truncate the value.
        /// </summary>
        public int TotalDigits { get; set; }

        /// <summary>
        /// Maximum number of digits to the right of the decimal point for a decimal property value.
        /// This attribute is a hint for the user interface. A service consumer may ignore it and pass a decimal value with more decimal digits. In this case the provider will round or truncate the value.
        /// </summary>
        public int FractionDigits { get; set; }

        /// <summary>
        /// Is the property a global identifier for the resource?
        /// </summary>
        [Obsolete]
        public bool IsGlobalId { get; set; }

        /// <summary>
        /// Is the property an identifier for the resource?
        /// </summary>
        [Obsolete]
        public bool IsIdentifier { get; set; }

        /// <summary>
        /// Is the property a descriptor for the resource?
        /// </summary>
        [Obsolete]
        public bool IsDescriptor { get; set; }

        /// <summary>
        /// Indicates that the property is dependent upon a related resource and gives
        /// the XPath expression for the corresponding property in the related resource.
        /// </summary>
        [Obsolete]
        public string CopiedFrom { get; set; }

        protected override bool ReadSmeAttribute(XmlAttribute attribute)
        {
            switch (attribute.LocalName)
            {
                case "canSort":
                    CanSort = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "canFilter":
                    CanFilter = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "canGroup":
                    CanGroup = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "isLocalized":
                    IsLocalized = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "isUniqueKey":
                    IsUniqueKey = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "precedence":
                    Precedence = XmlConvert.ToInt32(attribute.Value);
                    return true;
                case "groupName":
                    GroupName = attribute.Value;
                    return true;
                case "maxLength":
                    MaxLength = XmlConvert.ToInt32(attribute.Value);
                    return true;
                case "averageLength":
                    AverageLength = XmlConvert.ToInt32(attribute.Value);
                    return true;
                case "totalDigits":
                    TotalDigits = XmlConvert.ToInt32(attribute.Value);
                    return true;
                case "fractionDigits":
                    FractionDigits = XmlConvert.ToInt32(attribute.Value);
                    return true;
#pragma warning disable 612,618
                case "isGlobalId":
                    IsGlobalId = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "isIdentifier":
                    IsIdentifier = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "isDescriptor":
                    IsDescriptor = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "copiedFrom":
                    CopiedFrom = attribute.Value;
                    return true;
#pragma warning restore 612,618
                default:
                    return base.ReadSmeAttribute(attribute);
            }
        }

        protected override void WriteSmeAttributes(ICollection<XmlAttribute> attributes)
        {
            WriteSmeAttribute("canSort", CanSort, attributes);
            WriteSmeAttribute("canFilter", CanFilter, attributes);
            WriteSmeAttribute("canGroup", CanGroup, attributes);
            WriteSmeAttribute("isLocalized", IsLocalized, attributes);
            WriteSmeAttribute("isUniqueKey", IsUniqueKey, attributes);
            WriteSmeAttribute("precedence", Precedence, attributes);
            WriteSmeAttribute("groupName", GroupName, attributes);
            WriteSmeAttribute("maxLength", MaxLength, attributes);
            WriteSmeAttribute("averageLength", AverageLength, attributes);
            WriteSmeAttribute("totalDigits", TotalDigits, attributes);
            WriteSmeAttribute("fractionDigits", FractionDigits, attributes);
#pragma warning disable 612,618
            WriteSmeAttribute("isGlobalId", IsGlobalId, attributes);
            WriteSmeAttribute("isIdentifier", IsIdentifier, attributes);
            WriteSmeAttribute("isDescriptor", IsDescriptor, attributes);
            WriteSmeAttribute("copiedFrom", CopiedFrom, attributes);
#pragma warning restore 612,618
            base.WriteSmeAttributes(attributes);
        }
    }
}