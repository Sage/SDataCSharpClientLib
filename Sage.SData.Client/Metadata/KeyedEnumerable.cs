using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sage.SData.Client.Metadata
{
    public class KeyedEnumerable<TKey, TItem> : IEnumerable<TItem>
    {
        private readonly IEnumerable<TItem> _objects;
        private readonly Func<TItem, TKey> _selectKey;

        internal KeyedEnumerable(IEnumerable<TItem> objects, Func<TItem, TKey> selectKey)
        {
            _objects = objects;
            _selectKey = selectKey;
        }

        public TItem this[TKey key]
        {
            get { return _objects.SingleOrDefault(type => Equals(_selectKey(type), key)); }
        }

        #region IEnumerable Members

        public IEnumerator<TItem> GetEnumerator()
        {
            return _objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}