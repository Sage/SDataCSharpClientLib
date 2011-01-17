using System.Collections.Generic;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
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
            Type = new SDataSchemaTypeReference(element.SchemaTypeName);
            base.Read(obj);
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            var element = (XmlSchemaElement) obj;
            element.Name = ElementName;
            element.SchemaTypeName = Type.QualifiedName;
            base.Write(obj);
        }
    }
}