using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public abstract class SDataSchemaType : SDataSchemaItem
    {
        public string Name { get; set; }

        public XmlQualifiedName QualifiedName
        {
            get { return new XmlQualifiedName(Name, Schema != null ? Schema.TargetNamespace : null); }
        }

        protected internal override void Read(XmlSchemaObject obj)
        {
            if (obj is XmlSchemaType)
            {
                var type = (XmlSchemaType) obj;
                Name = type.Name;
            }

            base.Read(obj);
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            if (obj is XmlSchemaType)
            {
                var type = (XmlSchemaType) obj;
                type.Name = Name;
            }

            base.Write(obj);
        }
    }
}