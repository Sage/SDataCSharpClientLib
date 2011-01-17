using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaEnumType : SDataSchemaValueType
    {
        private SDataSchemaKeyedObjectCollection<SDataSchemaEnumItem> _items;

        public override IEnumerable<SDataSchemaObject> Children
        {
            get { return Items.Cast<SDataSchemaObject>(); }
        }

        public SDataSchemaKeyedObjectCollection<SDataSchemaEnumItem> Items
        {
            get { return _items ?? (_items = new SDataSchemaKeyedObjectCollection<SDataSchemaEnumItem>(this, item => item.Value)); }
        }

        protected internal override void Read(XmlSchemaObject obj)
        {
            var simpleType = (XmlSchemaSimpleType) obj;
            var restriction = simpleType.Content as XmlSchemaSimpleTypeRestriction;

            if (restriction == null)
            {
                throw new NotSupportedException();
            }

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