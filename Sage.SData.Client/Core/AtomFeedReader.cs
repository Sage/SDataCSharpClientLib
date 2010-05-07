using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// A standard list that fetches pages of feed entries as the user
    /// iterates over, indexes into or navigates through it.
    /// </summary>
    public class AtomFeedReader : IList<AtomEntry>
    {
        private readonly SDataResourceCollectionRequest _request;
        private int _itemsAvailable;
        private int _itemsPerPage;
        private IList<IList<AtomEntry>> _listPages;
        private int _currentIndex;

        /// <summary>
        /// The total number of items available.
        /// </summary>
        [Obsolete("Use the Count property instead.")]
        public int ItemsAvailable
        {
            get { return Count; }
        }

        /// <summary>
        /// The current index of the current entry. This value is 1-based.
        /// </summary>
        [Obsolete("Use the CurrentIndex property instead.")]
        public int EntryIndex
        {
            get { return (int) CurrentIndex + 1; }
        }

        /// <summary>
        /// The request used to fetch pages of feed entries.
        /// </summary>
        public SDataResourceCollectionRequest Request
        {
            get { return _request; }
        }

        /// <summary>
        /// The request used to fetch pages of feed entries.
        /// </summary>
        [Obsolete("Use the Request property instead.")]
        public SDataResourceCollectionRequest Parent
        {
            get { return Request; }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="AtomFeedReader"/> class.
        /// </summary>
        /// <param name="request">The request used to fetch pages of feed entries.</param>
        internal AtomFeedReader(SDataResourceCollectionRequest request)
        {
            Guard.ArgumentNotNull(request, "request");

            _request = request;
        }

        #region IList<T> Members

        public void Insert(int index, AtomEntry value)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public int IndexOf(AtomEntry value)
        {
            Guard.ArgumentNotNull(value, "value");

            for (var i = 0; i < _listPages.Count; i++)
            {
                var page = _listPages[i];

                if (page != null)
                {
                    for (var j = 0; j < page.Count; j++)
                    {
                        var entry = page[j];

                        if (entry == value)
                        {
                            return i*_itemsPerPage + j;
                        }
                    }
                }
            }

            return -1;
        }

        public void Add(AtomEntry value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(AtomEntry value)
        {
            Guard.ArgumentNotNull(value, "value");

            return _listPages.Where(page => page != null)
                .SelectMany(page => page)
                .Any(entry => entry == value);
        }

        public AtomEntry this[int index]
        {
            get { return GetItem(index); }
            set { throw new NotSupportedException(); }
        }

        public void CopyTo(AtomEntry[] entries, int index)
        {
            Guard.ArgumentNotNull(entries, "entries");
            Guard.ArgumentNotLessThan(index, "index", 0);

            if (index + _itemsAvailable > entries.Length)
            {
                throw new ArgumentException("Target array is not large enought", "entries");
            }

            foreach (var entry in this)
            {
                entries[index++] = entry;
            }
        }

        public bool Remove(AtomEntry entry)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get { return _itemsAvailable; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public IEnumerator<AtomEntry> GetEnumerator()
        {
            return _listPages.Select((page, i) => GetPage(i))
                .SelectMany(page => page)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Loads the reader and configures the internal pages.
        /// </summary>
        /// <returns>A value indicating whether the reader was successfully configured.</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the AtomFeedReader class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ ResourceCollection using AtomFeedReader" 
        ///         />
        ///     </code>
        /// </example>
        public bool Read()
        {
            _request.StartIndex = 1;
            var firstFeed = _request.Read();
            _itemsAvailable = firstFeed.GetOpenSearchTotalResults() ?? 0;
            _itemsPerPage = firstFeed.GetOpenSearchItemsPerPage() ?? 0;

            if (_itemsAvailable == 0 || _itemsPerPage == 0)
            {
                _currentIndex = -1;
                return false;
            }

            var pageCount = (_itemsAvailable + _itemsPerPage - 1)/_itemsPerPage;
            _listPages = new List<IList<AtomEntry>>(pageCount);

            for (var i = 0; i < pageCount; i++)
            {
                _listPages.Add(i == 0 ? new List<AtomEntry>(firstFeed.Entries) : null);
            }

            _currentIndex = 0;
            return true;
        }

        /// <summary>
        /// Sets the current entry to the first item in the reader.
        /// </summary>
        /// <returns>A value indicating whether the navigation was successful.</returns>
        [Obsolete("Use the MoveFirst method instead.")]
        public bool First()
        {
            return MoveFirst();
        }

        /// <summary>
        /// Sets the current entry to the first item in the reader.
        /// </summary>
        /// <returns>A value indicating whether the navigation was successful.</returns>
        public bool MoveFirst()
        {
            _currentIndex = 0;
            return true;
        }

        /// <summary>
        /// Sets the current entry to the last item in the reader.
        /// </summary>
        /// <returns>A value indicating whether the navigation was successful.</returns>
        [Obsolete("Use the MoveLast method instead.")]
        public bool Last()
        {
            return MoveLast();
        }

        /// <summary>
        /// Sets the current entry to the last item in the reader.
        /// </summary>
        /// <returns>A value indicating whether the navigation was successful.</returns>
        public bool MoveLast()
        {
            _currentIndex = _itemsAvailable - 1;
            return true;
        }

        /// <summary>
        /// Sets the current entry to the next item in the reader.
        /// If the end of the current page is reached then the next page will be retrieved.
        /// </summary>
        /// <returns>A value indicating whether the navigation was successful.</returns>
        public bool MoveNext()
        {
            var hasMore = _currentIndex + 1 < _itemsAvailable;
            if (hasMore)
            {
                _currentIndex++;
            }
            return hasMore;
        }

        /// <summary>
        /// Sets the current entry to the previous item in the reader.
        /// </summary>
        /// <returns>A value indicating whether the navigation was successful.</returns>
        [Obsolete("Use the MovePrevious method instead.")]
        public bool Previous()
        {
            return MovePrevious();
        }

        /// <summary>
        /// Sets the current entry to the previous item in the reader.
        /// </summary>
        /// <returns>A value indicating whether the navigation was successful.</returns>
        public bool MovePrevious()
        {
            var hasMore = _currentIndex > 0;
            if (hasMore)
            {
                _currentIndex--;
            }
            return hasMore;
        }

        /// <summary>
        /// The current entry.
        /// </summary>
        public AtomEntry Current
        {
            get { return GetItem(_currentIndex); }
        }

        /// <summary>
        /// The current index of the current entry. This value is 0-based.
        /// </summary>
        public long CurrentIndex
        {
            get { return _currentIndex; }
        }

        private AtomEntry GetItem(int index)
        {
            var pageIndex = index/_itemsPerPage;
            var entryIndex = index%_itemsPerPage;
            return GetPage(pageIndex)[entryIndex];
        }

        private IList<AtomEntry> GetPage(int pageIndex)
        {
            var page = _listPages[pageIndex];

            if (page == null)
            {
                _request.StartIndex = pageIndex*_itemsPerPage + 1;
                var feed = _request.Read();
                _listPages[pageIndex] = page = new List<AtomEntry>(feed.Entries);
            }

            return page;
        }
    }
}