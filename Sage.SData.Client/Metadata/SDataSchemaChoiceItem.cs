using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    [DebuggerDisplay("{ElementName}")]
    public class SDataSchemaChoiceItem : SDataSchemaItem
    {
        private SDataSchemaTypeReference _type;

        public string ElementName { get; set; }

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

        public override IEnumerable<SDataSchemaObject> Children
        {
            get { return Type != null ? new[] {Type} : base.Children; }
        }

        protected internal override void Read(XmlSchemaObject obj)
        {
            var element = (XmlSchemaElement) obj;
            ElementName = element.Name;
            Type = element.SchemaTypeName;
            base.Read(obj);
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            var element = (XmlSchemaElement) obj;
            element.Name = ElementName;

            if (Type != null)
            {
                element.SchemaTypeName = Type.QualifiedName;
            }

            base.Write(obj);
        }

        public static implicit operator SDataSchemaChoiceItem(SDataSchemaType type)
        {
            var resource = type as SDataSchemaResourceType;
            return new SDataSchemaChoiceItem
                   {
                       ElementName = resource != null ? resource.ElementName : null,
                       Type = type
                   };
        }
    }
}