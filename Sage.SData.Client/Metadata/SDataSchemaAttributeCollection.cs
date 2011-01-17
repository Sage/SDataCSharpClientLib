using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaAttributeCollection : Collection<XmlAttribute>
    {
        private readonly IDictionary<XmlQualifiedName, XmlAttribute> _keyedItems = new Dictionary<XmlQualifiedName, XmlAttribute>();

        public XmlAttribute this[XmlQualifiedName key]
        {
            get
            {
                XmlAttribute item;

                if (_keyedItems.TryGetValue(key, out item) && SelectKey(item) != key)
                {
                    _keyedItems[SelectKey(item)] = item;
                    _keyedItems.Remove(key);
                    item = default(XmlAttribute);
                }

                if (item == null)
                {
                    item = this.FirstOrDefault(x => SelectKey(x) == key);

                    if (item != null)
                    {
                        _keyedItems[key] = item;
                    }
                }

                return item;
            }
        }

        protected override void InsertItem(int index, XmlAttribute item)
        {
            Guard.ArgumentNotNull(item, "item");
            _keyedItems[SelectKey(item)] = item;
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, XmlAttribute item)
        {
            Guard.ArgumentNotNull(item, "item");
            _keyedItems.Remove(SelectKey(this[index]));
            _keyedItems[SelectKey(item)] = item;
            base.SetItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            _keyedItems.Remove(SelectKey(this[index]));
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            _keyedItems.Clear();
            base.ClearItems();
        }

        private static XmlQualifiedName SelectKey(XmlAttribute attr)
        {
            return new XmlQualifiedName(attr.LocalName, attr.NamespaceURI);
        }
    }
}