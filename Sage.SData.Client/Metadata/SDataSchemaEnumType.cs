using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaEnumType : SDataSchemaValueType
    {
        private KeyedObjectCollection<SDataSchemaEnumItem> _items;

        public SDataSchemaEnumType()
        {
        }

        public SDataSchemaEnumType(string baseName)
            : base(baseName, "enum")
        {
            BaseType = XmlTypeCode.String;
        }

        public override IEnumerable<SDataSchemaObject> Children
        {
            get { return Items.Cast<SDataSchemaObject>(); }
        }

        public KeyedObjectCollection<SDataSchemaEnumItem> Items
        {
            get { return _items ?? (_items = new KeyedObjectCollection<SDataSchemaEnumItem>(this, item => item.Value)); }
        }

        protected internal override void Read(XmlSchemaObject obj)
        {
            var simpleType = (XmlSchemaSimpleType) obj;
            var restriction = (XmlSchemaSimpleTypeRestriction) simpleType.Content;

            foreach (var facet in restriction.Facets)
            {
                var item = new SDataSchemaEnumItem();
                item.Read(facet);
                Items.Add(item);
            }

            base.Read(obj);
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            var simpleType = (XmlSchemaSimpleType) obj;
            var restriction = new XmlSchemaSimpleTypeRestriction();

            foreach (var item in Items)
            {
                var facet = new XmlSchemaEnumerationFacet();
                item.Write(facet);
                restriction.Facets.Add(facet);
            }

            simpleType.Content = restriction;
            base.Write(obj);
        }
    }
}