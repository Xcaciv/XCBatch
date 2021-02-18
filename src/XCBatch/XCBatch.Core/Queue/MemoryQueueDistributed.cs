using System.Collections.Generic;
using System.Linq;
using XCBatch.Interfaces;
using XCBatch.Interfaces.Adapters;

namespace XCBatch.Core
{
    /// <summary>
    /// default in memory queue
    /// </summary>
    public class MemoryQueueDistributed : IQueueBackendDistributed
    {
        /// <summary>
        /// current queue quantity
        /// </summary>
        public bool IsEmpty => !sourceQueue.Any(o => o.Value.Count > 0);

        /// <summary>
        /// sources indexed distribution id then by type
        /// </summary>
        protected readonly Dictionary<int, Queue<ISource>> sourceQueue = new Dictionary<int, Queue<ISource>>();

        /// <summary>
        /// queue for generic round-robin dequeue
        /// </summary>
        protected Queue<int> sourceIndexQueue = new Queue<int>();

        /// <summary>
        /// dequeue next source from next source list
        /// </summary>
        /// <remarks>
        /// <para>The effect of removing the list from the dictionary and reading it should put it at the end</para>
        /// </remarks>
        /// <returns></returns>
        /// <summary>
        /// retrieve and remove the first item from the queue
        /// </summary>
        /// <returns></returns>
        public ISource Dequeue()
        {
            ISource item = null;
            while (item == null && !this.IsEmpty && this.sourceIndexQueue.Count > 0)
            {
                int key = sourceIndexQueue.Dequeue();
                item = this.Dequeue(key);
                sourceIndexQueue.Enqueue(key);
            }

            return item;
        }

        public ISource Dequeue(int distributionId)
        {
            return sourceQueue[distributionId].Dequeue();
        }

        /// <summary>
        /// add and index source to the FIFO queue
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public void Enqueue(ISource source)
        {
            var queue = this.InitializeAndReturnStorage(source.DistributionId);
            queue.Enqueue(source);
            
        }

        /// <summary>
        /// add and index source collection to FIFO queue
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public void EnqueueRange(IEnumerable<ISource> sources)
        {
            foreach(var source in sources)
            {
                Enqueue(source);
            }

        }

        /// <summary>
        /// ensure there is a list for the given distribution and source type
        /// </summary>
        /// <param name="sourceTypeName"></param>
        /// <param name="distributionId"></param>
        protected Queue<ISource> InitializeAndReturnStorage(int distributionId)
        {
            if (!sourceQueue.ContainsKey(distributionId))
            {
                sourceQueue[distributionId] = new Queue<ISource>();
                sourceIndexQueue.Enqueue(distributionId);
            }

            return sourceQueue[distributionId];
        }

        public void Dispose()
        {
            // nothing to clean up
        }
    }
}
