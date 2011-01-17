using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public abstract class SDataSchemaTopLevelType : SDataSchemaComplexType
    {
        public string ElementName { get; set; }

        /// <summary>
        /// Relative URL to query resources or invoke the operation.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Does the resource kind, service operation or named query provide a $template URL?
        /// </summary>
        public bool HasTemplate { get; set; }

        protected internal override void Read(XmlSchemaObject obj)
        {
            if (obj is XmlSchemaElement)
            {
                ElementName = ((XmlSchemaElement) obj).Name;
            }

            base.Read(obj);
        }

        protected override void ReadSmeAttributes(XmlSchemaObject obj)
        {
            if (obj is XmlSchemaElement)
            {
                base.ReadSmeAttributes(obj);
            }
        }

        protected override bool ReadSmeAttribute(XmlAttribute attribute)
        {
            switch (attribute.LocalName)
            {
                case "role":
                    return true;
                case "path":
                    Path = attribute.Value;
                    return true;
                case "hasTemplate":
                    HasTemplate = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                default:
                    return base.ReadSmeAttribute(attribute);
            }
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            if (obj is XmlSchemaElement)
            {
                ((XmlSchemaElement) obj).Name = ElementName;
            }

            base.Write(obj);
        }

        protected override void WriteSmeAttributes(XmlSchemaObject obj)
        {
            if (obj is XmlSchemaElement)
            {
                base.WriteSmeAttributes(obj);
            }
        }

        protected override void WriteSmeAttributes(ICollection<XmlAttribute> attributes)
        {
            WriteSmeAttribute("path", Path, attributes);
            WriteSmeAttribute("hasTemplate", HasTemplate, attributes);
            base.WriteSmeAttributes(attributes);
        }
    }
}