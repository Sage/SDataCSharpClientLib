using System;
using System.Diagnostics;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Metadata
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof (CollectionDebugView<>))]
    public class KeyedObjectCollection<T> : KeyedCollection<string, T>
        where T : SDataSchemaObject
    {
        private readonly SDataSchemaObject _owner;

        internal KeyedObjectCollection(SDataSchemaObject owner, Func<T, string> selectKey)
            : base(obj => selectKey(obj) ?? "\0")
        {
            _owner = owner;
        }

        protected override void InsertItem(int index, T item)
        {
            Guard.ArgumentNotNull(item, "item");
            item.Parent = _owner;
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, T item)
        {
            Guard.ArgumentNotNull(item, "item");
            this[index].Parent = null;
            item.Parent = _owner;
            base.SetItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            this[index].Parent = null;
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                item.Parent = null;
            }

            base.ClearItems();
        }
    }
}