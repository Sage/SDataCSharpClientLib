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
        private readonly List<IList> _listPages;
        private int _numberOfPages;
        private int _lastPageSize;

        /// <summary>
        /// The total items available in the reader
        /// </summary>
        public int ItemsAvailable
        {
            get { return _itemsAvailable; }
        }

        private List<AtomEntry> _entries;

        /// <summary>
        /// The list of atom entries for the reader
        /// </summary>
        public List<AtomEntry> Entries
        {
            get { return _entries; }
            set { _entries = value; }
        }

        private int _entryIndex;

        /// <summary>
        /// Accessor method for the current entry index
        /// </summary>
        public int EntryIndex
        {
            get { return _entryIndex; }
            set { _entryIndex = value; }
        }

        private AtomFeed _feed;

        /// <summary>
        /// The AtomFeed the reader uses
        /// </summary>
        public AtomFeed Feed
        {
            get { return _feed; }
            set { _feed = value; }
        }

        private SDataResourceCollectionRequest _parent;

        /// <summary>
        /// The parent request 
        /// </summary>
        public SDataResourceCollectionRequest Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

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
            _parent = parent;
            _feed = feed;
            _entryIndex = 0;
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
            int lastIndex = 1;
            // caculate the number of pages
            // first get the number of items in the feed
            foreach (AtomLink link in Feed.Links)
            {
                if (link.Relation == "last")
                {
                    string query = link.Uri.Query;
                    int pos1 = query.IndexOf("startIndex=") + 11;
                    int pos2 = query.IndexOf("&", pos1);

                    if (pos2 < 0)
                    {
                        pos2 = query.Length;
                    }

                    lastIndex = int.Parse(query.Substring(pos1, pos2 - pos1));
                }
            }

            AtomFeed firstFeed = Feed;
            AtomFeed lastFeed = new AtomFeed();
            Parent.StartIndex = lastIndex;
            lastFeed.Load(new Uri(Parent.ToString()), new NetworkCredential(_service.UserName, _service.Password), null);

            _numberOfPages = _itemsAvailable/_itemsPerPage;
            _lastPageSize = _itemsAvailable%_itemsPerPage;
            if (_lastPageSize > 0)
            {
                _numberOfPages++;
            }


            // now set up our pages
            for (int x = 0; x < _numberOfPages; x++)
            {
                List<AtomEntry> list = new List<AtomEntry>();
                _listPages.Add(list);
            }

            // fill the first page
            foreach (AtomEntry entry in firstFeed.Entries)
            {
                _listPages[0].Add(entry);
            }

            // fill the last page
            foreach (AtomEntry entry in lastFeed.Entries)
            {
                _listPages[_listPages.Count - 1].Add(entry);
            }

            _entryIndex = 1;

            return true;
        }

        /// <summary>
        /// Moves the next AtomEntry in the reader. If the the reader has no more AtomEntrys the next page will be retrieved
        /// </summary>
        /// <returns>bool</returns>
        public bool MoveNext()
        {
            var hasMore = _entryIndex < _itemsAvailable;
            if (hasMore)
            {
                _entryIndex++;
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
                int currentPage = (_entryIndex - 1)/_itemsPerPage;

                if (currentPage < 0)
                {
                    currentPage = 0;
                }

                if (_listPages[currentPage].Count == 0)
                {
                    Parent.StartIndex = currentPage*_itemsPerPage + 1;
                    Feed = new AtomFeed();
                    Feed.Load(new Uri(Parent.ToString()), new NetworkCredential(_service.UserName, _service.Password), null);

                    foreach (AtomEntry entry in Feed.Entries)
                    {
                        _listPages[currentPage].Add(entry);
                    }
                }

                List<AtomEntry> list = (List<AtomEntry>) _listPages[currentPage];

                int index = (_entryIndex - 1 - (_itemsPerPage*currentPage));
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
            _entryIndex = Count;
            return true;
        }

        /// <summary>
        /// Sets the current AtomEntry to the first item in the reader
        /// </summary>
        /// <returns>bool</returns>
        public bool First()
        {
            _entryIndex = 1;
            return true;
        }

        /// <summary>
        /// Sets the current AtomEntry to the previous item in the reader.
        /// </summary>
        /// <returns></returns>
        public bool Previous()
        {
            _entryIndex--;
            if (_entryIndex < 1)
                _entryIndex = 1;
            return true;
        }
    }
}