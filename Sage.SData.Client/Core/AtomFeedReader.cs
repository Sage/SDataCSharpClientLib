using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// reader that returns an SDataPayload for AtomEntries within an AtomFeed.  The AtomFeedReader automatically handles paging 
    /// </summary>
    public class AtomFeedReader : IList<SDataPayload>
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

        private List<SDataPayload> _listPayloads;

        /// <summary>
        /// The list of SDataPayloads for the reader
        /// </summary>
        public List<SDataPayload> ListPayloads
        {
            get { return _listPayloads; }
            set { _listPayloads = value; }
        }

        private int _entryIndex;

        /// <summary>
        /// Accessor method for the current payload index
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
        public void Insert(int index, SDataPayload value)
        {
            ListPayloads.Insert(index, value);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            ListPayloads.RemoveAt(index);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(SDataPayload value)
        {
            return ListPayloads.IndexOf(value);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        /// <param name="value"></param>
        public void Add(SDataPayload value)
        {
            ListPayloads.Add(value);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        public void Clear()
        {
            ListPayloads.Clear();
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        public bool Contains(SDataPayload value)
        {
            return ListPayloads.Contains(value);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        public SDataPayload this[int index]
        {
            get { return ListPayloads[index]; }
            set { ListPayloads[index] = value; }
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        public void CopyTo(SDataPayload[] payloads, int index)
        {
            ListPayloads.CopyTo(payloads, index);
        }

        /// <summary>
        /// IList Implementation
        /// </summary>
        public bool Remove(SDataPayload payload)
        {
            return ListPayloads.Remove(payload);
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
        IEnumerator<SDataPayload> IEnumerable<SDataPayload>.GetEnumerator()
        {
            return ListPayloads.GetEnumerator();
        }

        /// <summary>
        /// IEnumerable implementation
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ListPayloads.GetEnumerator();
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
                List<SDataPayload> list = new List<SDataPayload>();
                _listPages.Add(list);
            }

            // fill the first page
            foreach (AtomEntry entry in firstFeed.Entries)
            {
                _listPages[0].Add(new SDataPayload(entry.GetSDataPayload(), entry));
            }

            // fill the last page
            foreach (AtomEntry entry in lastFeed.Entries)
            {
                _listPages[_listPages.Count - 1].Add(new SDataPayload(entry.GetSDataPayload(), entry));
            }

            _entryIndex = 1;

            return true;
        }

        /// <summary>
        /// Moves the next SDataPayload in the reader. If the the reader has no more SDataPayloads the next page will be retrieved
        /// </summary>
        /// <returns>bool</returns>
        public bool MoveNext()
        {
            _entryIndex++;
            if (_entryIndex > _itemsAvailable)
            {
                _entryIndex = _itemsAvailable;
            }
            return true;
        }

        /// <summary>
        /// The current SDataPayload for the reader
        /// </summary>
        public SDataPayload Current
        {
            get
            {
                int currentPage = (_entryIndex - 1)/_itemsPerPage;

                //if ((_entryIndex % _itemsPerPage) == 0)
                //    currentPage--;
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
                        _listPages[currentPage].Add(new SDataPayload(entry.GetSDataPayload(), entry));
                    }
                }

                List<SDataPayload> list = (List<SDataPayload>) _listPages[currentPage];

                int index = (_entryIndex - 1 - (_itemsPerPage*currentPage));
                if (index < 0)
                    index = 0;
                return list[index];
            }
        }

        /// <summary>
        /// Gets the last SDataPayload contained in the reader
        /// NOTE: this does not retrieve the last page of data for the feed
        /// </summary>
        /// <returns></returns>
        public bool Last()
        {
            _entryIndex = Count;
            return true;
        }

        /// <summary>
        /// Sets the current SDataPayload to the first item in the reader
        /// </summary>
        /// <returns>bool</returns>
        public bool First()
        {
            _entryIndex = 1;
            return true;
        }

        /// <summary>
        /// Sets the current SDataPayload to the previous item in the reader.
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