using System.Collections.Generic;

namespace XCBatch.Interfaces.Adapters
{
    /// <summary>
    /// unified interface for queue operations
    /// </summary>
    /// <remarks>
    /// The back-end
    /// </remarks>
    public interface IQueueBackend
    {
        /// <summary>
        /// Gets a value that indicates whether the queue is empty.
        /// </summary>
        bool IsEmpty { get; }
        /// <summary>
        /// add a source to the queue
        /// </summary>
        /// <param name="source"></param>
        /// <returns>queue count</returns>
        void Enqueue(ISource source);
        /// <summary>
        /// add a source collection to the queue
        /// </summary>
        /// <param name="sources"></param>
        /// <returns>queue count</returns>
        void EnqueueRange(IEnumerable<ISource> sources);
        /// <summary>
        /// remove and return the next item from the queue
        /// </summary>
        /// <returns>null when empty</returns>
        ISource Dequeue();
    }
}
