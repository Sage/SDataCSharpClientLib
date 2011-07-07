using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    [DebuggerDisplay("{Name}")]
    public abstract class SDataSchemaType : SDataSchemaItem
    {
        protected SDataSchemaType()
        {
        }

        protected SDataSchemaType(string baseName, string defaultSuffix)
        {
            Name = string.Format("{0}--{1}", baseName, defaultSuffix);
        }

        public string Name { get; set; }
        public string ListName { get; set; }
        public string ListItemName { get; set; }
        public XmlSchemaAnyAttribute ListAnyAttribute { get; set; }

        public XmlQualifiedName QualifiedName
        {
            get { return new XmlQualifiedName(Name, Schema != null ? Schema.TargetNamespace : null); }
        }

        public XmlQualifiedName ListQualifiedName
        {
            get { return new XmlQualifiedName(ListName, Schema != null ? Schema.TargetNamespace : null); }
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