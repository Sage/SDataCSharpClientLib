using System;
using System.Collections.Generic;
using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// SData URL for a resource collection
    /// </summary>
    public class SDataResourceCollectionRequest : SDataApplicationRequest
    {
        private string _query;

        /// <summary>
        /// Accessor method for query can be null
        /// </summary>
        /// <remarks>
        /// string used to filter data
        /// </remarks>
        /// <example>startIndex=21&amp;count=10
        ///</example>
        public string Query
        {
            get { return _query; }
            set { _query = value; }
        }

        private Dictionary<string, string> _queryValues;

        /// <summary>
        ///  Dictionary of query name value pairs
        /// </summary>
        /// <example>where, salesorderamount lt 15.00
        /// orderby, salesorderid
        /// </example>
        public Dictionary<string, string> QueryValues
        {
            get { return _queryValues; }
            set { _queryValues = value; }
        }

        private int _startIndex;

        /// <summary>
        /// Indicates the index of the first resource returned by the query. This index is 1-based (not 0-based).
        /// </summary>
        public int StartIndex
        {
            get { return _startIndex; }
            set { _startIndex = value; }
        }

        private int _count;

        /// <summary>
        /// Indicates the number of resources requested by the service consumer.
        /// The service may choose to return a different number of resources and it
        /// indicates this by setting the itemsPerPage element in the returned feed.
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }


        private AtomFeedReader _reader;

        /// <summary>
        /// AtomFeed Reader for the request
        /// </summary>
        public AtomFeedReader Reader
        {
            get { return _reader; }
            set { _reader = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        public SDataResourceCollectionRequest(ISDataService service)
            : base(service)
        {
            Count = -1;
            StartIndex = -1;

            QueryValues = new Dictionary<string, string>();
        }

        /// <summary>
        /// Reads the AtomFeed for a resource collection
        /// </summary>
        /// <returns>AtomFeed</returns>
        /// <example>
        ///     <code lang="cs" title="The following code example demonstrates the usage of the SDataResourceCollectionRequest class.">
        ///         <code 
        ///             source=".\Example.cs" 
        ///             region="READ a Resource Collection Feed" 
        ///         />
        ///     </code>
        /// </example>
        public AtomFeed Read()
        {
            Service.Url = Service.Url + ResourceKind;
            return Service.ReadFeed(this);
        }

        /// <summary>
        /// Performs initial read from AtomFeedReader
        /// </summary>
        /// <param name="feed">the feed</param>
        /// <returns></returns>
        public bool Read(AtomFeed feed)
        {
            if (Reader == null)
            {
                Reader = new AtomFeedReader(Service, feed, this);
            }
            return Reader.Read();
        }


        /// <summary>
        /// Formats URL String
        /// </summary>
        /// <returns>formatted string</returns>
        public override string ToString()
        {
            bool hasParams = false;
            string retval =
                Protocol + "://" +
                ServerName + "/" +
                VirtualDirectory + "/" +
                Application + "/" +
                ContractName + "/" +
                DataSet + "/" +
                ResourceKind;


            if (QueryValues.Count > 0)
            {
                foreach (KeyValuePair<string, string> values in QueryValues)
                {
                    if (hasParams)
                    {
                        retval += "&";
                    }
                    else
                    {
                        retval += "?";
                        hasParams = true;
                    }

                    retval += values.Key + "=" + values.Value;
                }
            }

            if (Count > -1)
            {
                if (hasParams)
                {
                    retval += "&";
                }
                else
                {
                    retval += "?";
                    hasParams = true;
                }

                retval += "count=" + Convert.ToString(Count);
            }

            if (StartIndex > -1)
            {
                if (hasParams)
                {
                    retval += "&";
                }
                else
                {
                    retval += "?";
                }

                retval += "startIndex=" + Convert.ToString(StartIndex);
            }

            return retval;
        }
    }
}