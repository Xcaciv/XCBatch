using System.Collections.Concurrent;
using System.Collections.Generic;
using XCBatch.Interfaces;
using XCBatch.Interfaces.Adapters;

namespace XCBatch.Core
{
    /// <summary>
    /// thread safe serial memory queue
    /// </summary>
    /// 
    /// <remarks>
    /// <para>This queue type is FIFO only</para>
    /// </remarks>
    public class ConcurrentMemoryQueue : IQueueBackend
    {
        /// <summary>
        /// current queue status
        /// </summary>
        public bool IsEmpty => sourceQueue.IsEmpty;

        /// <summary>
        /// list used as queue to provide random access
        /// </summary>
        protected readonly ConcurrentQueue<ISource> sourceQueue = new ConcurrentQueue<ISource>();

        /// <summary>
        /// retrieve and remove the first item from the queue
        /// </summary>
        /// <returns></returns>
        public ISource Dequeue()
        {
            ISource item = null;
            sourceQueue.TryDequeue(out item);
            return item;
        }

        /// <summary>
        /// add source to the FIFO queue
        /// </summary>
        /// <param name="source"></param>
        /// <returns>void for performance reasons</returns>
        public void Enqueue(ISource source)
        {
            sourceQueue.Enqueue(source);
        }

        /// <summary>
        /// add source collection to FIFO queue
        /// </summary>
        /// <param name="source"></param>
        /// <returns>void for performance reasons</returns>
        public void EnqueueRange(IEnumerable<ISource> sources)
        {
            foreach (var item in sources)
            {
                sourceQueue.Enqueue(item);
            }
        }

        /// <summary>
        /// convert the queue to a block
        /// </summary>
        /// <returns></returns>
        public ISourceBlock<ISource> ToBlock()
        {
            return new Source.SourceBlock<ISource>(sourceQueue);
        }
    }
}
