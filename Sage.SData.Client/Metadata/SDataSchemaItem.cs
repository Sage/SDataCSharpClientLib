using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public abstract class SDataSchemaItem : SDataSchemaObject
    {
        protected static readonly XmlDocument XmlDoc = new XmlDocument();

        private KeyedObjectCollection<SDataSchemaDocumentation> _documentation;
        private KeyedCollection<XmlQualifiedName, XmlAttribute> _unhandledAttributes;

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

        public KeyedObjectCollection<SDataSchemaDocumentation> Documentation
        {
            get { return _documentation ?? (_documentation = new KeyedObjectCollection<SDataSchemaDocumentation>(this, doc => doc.Language ?? "\0")); }
        }

        public KeyedCollection<XmlQualifiedName, XmlAttribute> UnhandledAttributes
        {
            get { return _unhandledAttributes ?? (_unhandledAttributes = new KeyedCollection<XmlQualifiedName, XmlAttribute>(attr => new XmlQualifiedName(attr.Name, attr.NamespaceURI))); }
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

        protected internal virtual void Read(XmlSchemaObject obj)
        {
            var annotated = (XmlSchemaAnnotated) obj;

            if (annotated != null && annotated.Annotation != null)
            {
                foreach (var xmlDoc in annotated.Annotation.Items.OfType<XmlSchemaDocumentation>())
                {
                    var doc = new SDataSchemaDocumentation {Language = xmlDoc.Language};

                    if (xmlDoc.Markup != null)
                    {
                        doc.Text = string.Concat(xmlDoc.Markup.Select(text => text.Value).ToArray());
                    }

                    Documentation.Add(doc);
                }
            }

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

        protected internal virtual void Write(XmlSchemaObject obj)
        {
            var annotated = (XmlSchemaAnnotated) obj;

            foreach (var doc in Documentation)
            {
                var xmlDoc = new XmlSchemaDocumentation();

                if (doc.Language != null)
                {
                    xmlDoc.Language = doc.Language;
                }

                if (doc.Text != null)
                {
                    xmlDoc.Markup = new XmlNode[] {XmlDoc.CreateTextNode(doc.Text)};
                }

                annotated.Annotation = new XmlSchemaAnnotation {Items = {xmlDoc}};
            }

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