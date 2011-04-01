using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    [DebuggerDisplay("{Name}")]
    public abstract class SDataSchemaProperty : SDataSchemaItem
    {
        private SDataSchemaTypeReference _type;

        public SDataSchemaProperty()
        {
        }

        public SDataSchemaProperty(string name)
        {
            Name = name;
            MinOccurs = 0;
        }

        public string Name { get; set; }
        public decimal? MinOccurs { get; set; }

        public SDataSchemaTypeReference Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    value.Parent = this;
                }
            }
        }

        public bool IsNillable { get; set; }

        /// <summary>
        /// Is the property value mandatory when creating a new resource?
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Is the property read-only?
        /// For example, an ID set by the provider or a calculated property.
        /// </summary>
        public bool IsReadOnly { get; set; }

        public override IEnumerable<SDataSchemaObject> Children
        {
            get { return Type != null ? new[] {Type} : base.Children; }
        }

        protected internal override void Read(XmlSchemaObject obj)
        {
            var element = (XmlSchemaElement) obj;
            Name = element.Name;
            MinOccurs = element.MinOccursString != null ? element.MinOccurs : (decimal?) null;
            Type = element.SchemaTypeName;

            if (element.SchemaTypeName.IsEmpty)
            {
                var simpleType = element.SchemaType as XmlSchemaSimpleType;

                if (simpleType != null)
                {
                    var restriction = simpleType.Content as XmlSchemaSimpleTypeRestriction;

                    if (restriction != null)
                    {
                        Type = restriction.BaseTypeName;
                    }
                }
            }

            IsNillable = element.IsNillable;
            base.Read(obj);
        }

        protected override bool ReadSmeAttribute(XmlAttribute attribute)
        {
            switch (attribute.LocalName)
            {
                case "isMandatory":
                    IsMandatory = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "isReadOnly":
                    IsReadOnly = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                default:
                    return base.ReadSmeAttribute(attribute);
            }
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            var element = (XmlSchemaElement) obj;
            element.Name = Name;

            if (MinOccurs != null)
            {
                element.MinOccurs = MinOccurs.Value;
            }

            element.SchemaTypeName = GetTypeName();
            element.IsNillable = IsNillable;
            base.Write(obj);
        }

        protected override void WriteSmeAttributes(ICollection<XmlAttribute> attributes)
        {
            WriteSmeAttribute("isMandatory", IsMandatory, attributes);
            WriteSmeAttribute("isReadOnly", IsReadOnly, attributes);
            base.WriteSmeAttributes(attributes);
        }

        protected virtual XmlQualifiedName GetTypeName()
        {
            return Type != null ? Type.QualifiedName : null;
        }
    }
}