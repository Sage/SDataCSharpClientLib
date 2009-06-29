using System.Collections;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Class used to batch process atom entries for Insert, Update, and Delete
    /// </summary>
    public sealed class BatchProcess
    {
        private static readonly BatchProcess instance = new BatchProcess();

        private readonly Stack _stack;

        /// <summary>
        /// Current stack
        /// </summary>
        public Stack CurrentStack
        {
            get { return _stack; }
        }

        /// <summary>
        /// The only instance of the BatchProcess class
        /// </summary>
        public static BatchProcess Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BatchProcess()
        {
            _stack = new Stack();
        }

        /// <summary>
        /// Adds a url to the batch for processing
        /// </summary>
        /// <param name="batchitem">url for batch item</param>
        public void AddToBatch(string[] batchitem)
        {
            SDataBatchRequest batchRequest = _stack.Peek() as SDataBatchRequest;
            batchRequest.Requests.Enqueue(batchitem);
        }
    }
}