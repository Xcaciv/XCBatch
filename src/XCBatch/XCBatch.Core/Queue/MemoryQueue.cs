using System.Collections.Generic;
using XCBatch.Interfaces;
using XCBatch.Interfaces.Adapters;

namespace XCBatch.Core
{
    /// <summary>
    /// default in memory queue with global order
    /// </summary>
    public class MemoryQueue : IQueueBackend
    {
        /// <summary>
        /// current queue state
        /// </summary>
        public bool IsEmpty => sourceQueue.Count == 0;

        /// <summary>
        /// list used as queue to provide random access
        /// </summary>
        protected readonly Queue<ISource> sourceQueue = new Queue<ISource>();

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
        /// <returns></returns>
        public void Enqueue(ISource source)
        {
            sourceQueue.Enqueue(source);
        }

        /// <summary>
        /// add source collection to FIFO queue
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public void EnqueueRange(IEnumerable<ISource> sources)
        {
            foreach(var item in sources)
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
