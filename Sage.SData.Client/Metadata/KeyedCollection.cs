using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Metadata
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof (CollectionDebugView<>))]
    public class KeyedCollection<TKey, TItem> : Collection<TItem>
    {
        private readonly Func<TItem, TKey> _selectKey;
        private readonly IDictionary<TKey, TItem> _keyedItems;

        internal KeyedCollection(Func<TItem, TKey> selectKey)
        {
            _selectKey = selectKey;
            _keyedItems = new Dictionary<TKey, TItem>();
        }

        public TItem this[TKey key]
        {
            get
            {
                TItem item;

                if (_keyedItems.TryGetValue(key, out item))
                {
                    var actualKey = _selectKey(item);

                    if (!Equals(actualKey, key))
                    {
                        _keyedItems[actualKey] = item;
                        _keyedItems.Remove(key);
                        item = default(TItem);
                    }
                }

                if (Equals(item, default(TItem)))
                {
                    item = this.FirstOrDefault(x => Equals(_selectKey(x), key));

                    if (!Equals(item, default(TItem)))
                    {
                        _keyedItems[key] = item;
                    }
                }

                return item;
            }
        }

        protected override void InsertItem(int index, TItem item)
        {
            Guard.ArgumentNotNull(item, "item");

            var key = _selectKey(item);
            _keyedItems[key] = item;
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, TItem item)
        {
            Guard.ArgumentNotNull(item, "item");

            var key = _selectKey(this[index]);
            _keyedItems.Remove(key);
            key = _selectKey(item);
            _keyedItems[key] = item;
            base.SetItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            var key = _selectKey(this[index]);
            _keyedItems.Remove(key);
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            _keyedItems.Clear();
            base.ClearItems();
        }
    }
}