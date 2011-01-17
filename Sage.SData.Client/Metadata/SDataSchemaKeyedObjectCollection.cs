using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaKeyedObjectCollection<T> : Collection<T>
        where T : SDataSchemaObject
    {
        private readonly SDataSchemaObject _owner;
        private readonly Func<T, string> _selectKey;
        private readonly IDictionary<string, T> _keyedItems;

        internal SDataSchemaKeyedObjectCollection(SDataSchemaObject owner, Func<T, string> selectKey)
        {
            _owner = owner;
            _selectKey = selectKey;
            _keyedItems = new Dictionary<string, T>();
        }

        public T this[string key]
        {
            get
            {
                T item;

                if (_keyedItems.TryGetValue(key, out item) && _selectKey(item) != key)
                {
                    _keyedItems[_selectKey(item)] = item;
                    _keyedItems.Remove(key);
                    item = null;
                }

                if (item == null)
                {
                    item = this.FirstOrDefault(x => _selectKey(x) == key);

                    if (item != null)
                    {
                        _keyedItems[key] = item;
                    }
                }

                return item;
            }
        }

        protected override void InsertItem(int index, T item)
        {
            Guard.ArgumentNotNull(item, "item");
            _keyedItems[_selectKey(item)] = item;
            item.Parent = _owner;
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, T item)
        {
            Guard.ArgumentNotNull(item, "item");
            _keyedItems.Remove(_selectKey(this[index]));
            _keyedItems[_selectKey(item)] = item;
            this[index].Parent = null;
            item.Parent = _owner;
            base.SetItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            _keyedItems.Remove(_selectKey(this[index]));
            this[index].Parent = null;
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            _keyedItems.Clear();

            foreach (var item in this)
            {
                item.Parent = null;
            }

            base.ClearItems();
        }
    }
}