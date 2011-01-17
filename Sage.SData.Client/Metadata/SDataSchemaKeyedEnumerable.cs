using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaKeyedEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _objects;
        private readonly Func<T, string> _selectKey;

        public SDataSchemaKeyedEnumerable(IEnumerable<T> objects, Func<T, string> selectKey)
        {
            _objects = objects;
            _selectKey = selectKey;
        }

        public T this[string name]
        {
            get { return _objects.SingleOrDefault(type => _selectKey(type) == name); }
        }

        #region IEnumerable Members

        public IEnumerator<T> GetEnumerator()
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