using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// reader that returns an AtomEntry for AtomEntries within an AtomFeed.  The AtomFeedReader automatically handles paging 
    /// </summary>
    public class AtomFeedReader : IList<AtomEntry>
    {
        private readonly ISDataService _service;
        private readonly int _itemsPerPage;
        private readonly int _itemsAvailable;
        private readonly IList<IList> _listPages;
        private int _numberOfPages;
        private int _lastPageSize;

        /// <summary>
        /// The total items available in the reader
        /// </summary>
        public int ItemsAvailable
        {
            get { return _itemsAvailable; }
        }

        /// <summary>
        /// The list of atom entries for the reader
        /// </summary>
        public IList<AtomEntry> Entries { get; set; }

        /// <summary>
        /// Accessor method for the current entry index
        /// </summary>
        public int EntryIndex { get; set; }

        /// <summary>
        /// The AtomFeed the reader uses
        /// </summary>
        public AtomFeed Feed { get; set; }

        /// <summary>
        /// The parent request 
        /// </summary>
        public SDataResourceCollectionRequest Parent { get; set; }

        /// <summary>
        /// Create the reader for the specified AtomFeed
        /// </summary>
        /// <param name="service">the ISDataService used by the reader</param>
        /// <param name="feed">the atomfeed that is being read</param>
        /// <param name="parent">the SDataResourceCollectionRequest that specifies the reader</param>
        public AtomFeedReader(ISDataService service, AtomFeed feed, SDataResourceCollectionRequest parent)
        {
            if (feed == null)
            {
                throw new SDataClientException("AtomFeed is null");
            }
            Parent = parent;
            Feed = feed;
            EntryIndex = 0;
            _itemsPerPage = feed.GetOpenSearchItemsPerPage() ?? 0;
            _service = service;
            _itemsAvailable = feed.GetOpenSearchTotalResults() ?? 0;
            _listPages = new List<IList>();
            _numberOfPages = 0;
            _lastPageSize = 0;
        }

        #region IList Members

        /// <summary>
        /// IList implementation
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert(int index, AtomEntry value)
        {
            Entries.Insert(index, value);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            Entries.RemoveAt(index);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(AtomEntry value)
        {
            return Entries.IndexOf(value);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        /// <param name="value"></param>
        public void Add(AtomEntry value)
        {
            Entries.Add(value);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        public void Clear()
        {
            Entries.Clear();
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        public bool Contains(AtomEntry value)
        {
            return Entries.Contains(value);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        public AtomEntry this[int index]
        {
            get { return Entries[index]; }
            set { Entries[index] = value; }
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        public void CopyTo(AtomEntry[] entries, int index)
        {
            Entries.CopyTo(entries, index);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        public bool Remove(AtomEntry entry)
        {
            return Entries.Remove(entry);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        public int Count
        {
            get { return ItemsAvailable; }
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// IEnumerable implementation
        /// </summary>
        /// <returns></returns>
        IEnumerator<AtomEntry> IEnumerable<AtomEntry>.GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        /// <summary>
        /// IEnumerable implementation
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Initial load of the reader
        /// </summary>
        /// <returns>bool</returns>
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
            var lastIndex = 1;
            // caculate the number of pages
            // first get the number of items in the feed
            foreach (var link in Feed.Links)
            {
                if (link.Relation == "last")
                {
                    var query = link.Uri.Query;
                    var pos1 = query.IndexOf("startIndex=") + 11;
                    var pos2 = query.IndexOf("&", pos1);

                    if (pos2 < 0)
                    {
                        pos2 = query.Length;
                    }

                    lastIndex = int.Parse(query.Substring(pos1, pos2 - pos1));
                }
            }

            var firstFeed = Feed;
            var lastFeed = new AtomFeed();
            Parent.StartIndex = lastIndex;
            lastFeed.Load(new Uri(Parent.ToString()), new NetworkCredential(_service.UserName, _service.Password), null);

            _numberOfPages = _itemsAvailable/_itemsPerPage;
            _lastPageSize = _itemsAvailable%_itemsPerPage;
            if (_lastPageSize > 0)
            {
                _numberOfPages++;
            }

            // now set up our pages
            for (var x = 0; x < _numberOfPages; x++)
            {
                var list = new List<AtomEntry>();
                _listPages.Add(list);
            }

            // fill the first page
            foreach (var entry in firstFeed.Entries)
            {
                _listPages[0].Add(entry);
            }

            // fill the last page
            foreach (var entry in lastFeed.Entries)
            {
                _listPages[_listPages.Count - 1].Add(entry);
            }

            EntryIndex = 1;

            return true;
        }

        /// <summary>
        /// Moves the next AtomEntry in the reader. If the the reader has no more AtomEntrys the next page will be retrieved
        /// </summary>
        /// <returns>bool</returns>
        public bool MoveNext()
        {
            var hasMore = EntryIndex < _itemsAvailable;
            if (hasMore)
            {
                EntryIndex++;
            }
            return hasMore;
        }

        /// <summary>
        /// The current AtomEntry for the reader
        /// </summary>
        public AtomEntry Current
        {
            get
            {
                var currentPage = (EntryIndex - 1)/_itemsPerPage;

                if (currentPage < 0)
                {
                    currentPage = 0;
                }

                if (_listPages[currentPage].Count == 0)
                {
                    Parent.StartIndex = currentPage*_itemsPerPage + 1;
                    Feed = new AtomFeed();
                    Feed.Load(new Uri(Parent.ToString()), new NetworkCredential(_service.UserName, _service.Password), null);

                    foreach (var entry in Feed.Entries)
                    {
                        _listPages[currentPage].Add(entry);
                    }
                }

                var list = (List<AtomEntry>) _listPages[currentPage];

                var index = (EntryIndex - 1 - (_itemsPerPage*currentPage));
                if (index < 0)
                    index = 0;
                return list[index];
            }
        }

        /// <summary>
        /// Gets the last AtomEntry contained in the reader
        /// NOTE: this does not retrieve the last page of data for the feed
        /// </summary>
        /// <returns></returns>
        public bool Last()
        {
            EntryIndex = Count;
            return true;
        }

        /// <summary>
        /// Sets the current AtomEntry to the first item in the reader
        /// </summary>
        /// <returns>bool</returns>
        public bool First()
        {
            EntryIndex = 1;
            return true;
        }

        /// <summary>
        /// Sets the current AtomEntry to the previous item in the reader.
        /// </summary>
        /// <returns></returns>
        public bool Previous()
        {
            if (EntryIndex > 1)
                EntryIndex--;
            else if (EntryIndex < 1)
                EntryIndex = 1;
            return true;
        }
    }
}