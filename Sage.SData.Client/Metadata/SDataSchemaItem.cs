using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public abstract class SDataSchemaItem : SDataSchemaObject
    {
        private static readonly XmlDocument XmlDoc = new XmlDocument();
        private SDataSchemaAttributeCollection _unhandledAttributes;

        public SDataSchema Schema
        {
            get
            {
                if (Parent == null)
                {
                    return null;
                }

                var schema = Parent as SDataSchema;

                if (schema != null)
                {
                    return schema;
                }

                var item = Parent as SDataSchemaItem;

                if (item != null)
                {
                    return item.Schema;
                }

                return null;
            }
        }

        public XmlSchemaAnnotation Annotation { get; set; }

        public SDataSchemaAttributeCollection UnhandledAttributes
        {
            get { return _unhandledAttributes ?? (_unhandledAttributes = new SDataSchemaAttributeCollection()); }
        }

        /// <summary>
        /// A friendly name for the element (localized).
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Is the element part of a global contract that it is not supported by this specific provider?
        /// </summary>
        public bool Unsupported { get; set; }

        /// <summary>
        /// Applies to elements that are part of a global contract.
        /// Defines the compliance requirement for providers that implement the contract.
        /// </summary>
        public ComplianceLevel? Compliance { get; set; }

        /// <summary>
        /// List of tags that apply to the definition.
        /// SData does not impose any predefined list of tags.
        /// Instead, each contract is free to define its own list of tags.
        /// </summary>
        public string Tags { get; set; }

        protected internal override void Read(XmlSchemaObject obj)
        {
            var annotated = (XmlSchemaAnnotated) obj;
            Annotation = annotated.Annotation;
            ReadSmeAttributes(obj);
        }

        protected virtual void ReadSmeAttributes(XmlSchemaObject obj)
        {
            var annotated = (XmlSchemaAnnotated) obj;

            if (annotated.UnhandledAttributes != null)
            {
                foreach (var attribute in annotated.UnhandledAttributes)
                {
                    if (attribute.NamespaceURI != SmeNamespaceUri || !ReadSmeAttribute(attribute))
                    {
                        UnhandledAttributes.Add(attribute);
                    }
                }
            }
        }

        protected virtual bool ReadSmeAttribute(XmlAttribute attribute)
        {
            switch (attribute.LocalName)
            {
                case "label":
                    Label = attribute.Value;
                    return true;
                case "unsupported":
                    Unsupported = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "compliance":
                    Compliance = EnumEx.Parse<ComplianceLevel>(attribute.Value, true);
                    return true;
                case "tags":
                    Tags = attribute.Value;
                    return true;
                default:
                    return false;
            }
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            var annotated = (XmlSchemaAnnotated) obj;
            annotated.Annotation = Annotation;
            WriteSmeAttributes(obj);
        }

        protected virtual void WriteSmeAttributes(XmlSchemaObject obj)
        {
            var annotated = (XmlSchemaAnnotated) obj;
            var attributes = new List<XmlAttribute>(UnhandledAttributes);
            WriteSmeAttributes(attributes);
            annotated.UnhandledAttributes = attributes.ToArray();
        }

        protected virtual void WriteSmeAttributes(ICollection<XmlAttribute> attributes)
        {
            WriteSmeAttribute("label", Label, attributes);
            WriteSmeAttribute("unsupported", Unsupported, attributes);
            WriteSmeAttribute("compliance", Compliance, attributes);
            WriteSmeAttribute("tags", Tags, attributes);
        }

        protected static void WriteSmeAttribute(string name, bool value, ICollection<XmlAttribute> attributes)
        {
            WriteSmeAttribute(name, value ? XmlConvert.ToString(true) : null, attributes);
        }

        protected static void WriteSmeAttribute(string name, int value, ICollection<XmlAttribute> attributes)
        {
            WriteSmeAttribute(name, value != 0 ? XmlConvert.ToString(value) : null, attributes);
        }

        protected static void WriteSmeAttribute(string name, bool? value, ICollection<XmlAttribute> attributes)
        {
            WriteSmeAttribute(name, value != null ? XmlConvert.ToString(value.Value) : null, attributes);
        }

        protected static void WriteSmeAttribute(string name, int? value, ICollection<XmlAttribute> attributes)
        {
            WriteSmeAttribute(name, value != null ? XmlConvert.ToString(value.Value) : null, attributes);
        }

        protected static void WriteSmeAttribute<T>(string name, T? value, ICollection<XmlAttribute> attributes)
            where T : struct
        {
            WriteSmeAttribute(name, value != null ? EnumEx.Format(value.Value) : null, attributes);
        }

        protected static void WriteSmeAttribute<T>(string name, T value, ICollection<XmlAttribute> attributes)
            where T : struct
        {
            WriteSmeAttribute(name, !Equals(value, default(T)) ? EnumEx.Format(value) : null, attributes);
        }

        protected static void WriteSmeAttribute(string name, string value, ICollection<XmlAttribute> attributes)
        {
            if (value == null)
            {
                return;
            }

            var attr = XmlDoc.CreateAttribute(name, SmeNamespaceUri);
            attr.Value = value;
            attributes.Add(attr);
        }
    }
}