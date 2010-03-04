using System.Collections;

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

        private readonly Stack _stack;

        /// <summary>
        /// Current stack
        /// </summary>
        public Stack CurrentStack
        {
            get { return _stack; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        private BatchProcess()
        {
            _stack = new Stack();
        }

        /// <summary>
        /// Adds a url to the batch for processing
        /// </summary>
        /// <param name="item">url for batch item</param>
        public void AddToBatch(SDataBatchRequestItem item)
        {
            var batchRequest = (SDataBatchRequest) _stack.Peek();
            batchRequest.Requests.Enqueue(item);
        }
    }
}