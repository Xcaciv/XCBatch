using System.Collections.Generic;

namespace XCBatch.Interfaces
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
        /// number of source items in the queue
        /// </summary>
        int Count { get; }
        /// <summary>
        /// add a source to the queue
        /// </summary>
        /// <param name="source"></param>
        /// <returns>queue count</returns>
        int Enqueue(ISource source);
        /// <summary>
        /// add a source collection to the queue
        /// </summary>
        /// <param name="sources"></param>
        /// <returns>queue count</returns>
        int Enqueue(IEnumerable<ISource> sources);
        /// <summary>
        /// remove and return the next item from the queue
        /// </summary>
        /// <returns></returns>
        ISource Dequeue();
    }
}
