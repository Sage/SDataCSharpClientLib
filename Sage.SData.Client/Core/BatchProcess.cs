using System;
using System.Collections.Generic;
using System.Linq;
using Sage.SData.Client.Common;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Class used to batch process atom entries for Insert, Update, and Delete
    /// </summary>
    public sealed class BatchProcess
    {
        /// <summary>
        /// The only instance of the BatchProcess class
        /// </summary>
        public static readonly BatchProcess Instance = new BatchProcess();

        private readonly IList<SDataBatchRequest> _requests;

        private BatchProcess()
        {
            _requests = new List<SDataBatchRequest>();
        }

        /// <summary>
        /// Current stack
        /// </summary>
        public IList<SDataBatchRequest> Requests
        {
            get { return _requests; }
        }

        /// <summary>
        /// Adds a url to the batch for processing
        /// </summary>
        /// <param name="item">url for batch item</param>
        public void AddToBatch(SDataBatchRequestItem item)
        {
            Guard.ArgumentNotNull(item, "item");

            var uri = new SDataUri(item.Url)
                      {
                          CollectionPredicate = null,
                          Query = null
                      };

            if (uri.PathSegments.Length > 3)
            {
                uri.PathSegments = uri.PathSegments.Take(3).ToArray();
            }

            var baseUri = uri.ToString();
            var request = _requests.LastOrDefault(x => x.ToString() == baseUri);

            if (request == null)
            {
                throw new InvalidOperationException("Unable to find an appropriate batch request in progress");
            }

            request.Requests.Add(item);
        }
    }
}