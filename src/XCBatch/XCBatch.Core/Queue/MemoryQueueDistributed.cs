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
        public bool IsEmpty => !indexedSources.Any(o => o.Value.Count > 0);

        /// <summary>
        /// sources indexed distribution id then by type
        /// </summary>
        protected readonly Dictionary<int, Queue<ISource>> indexedSources = new Dictionary<int, Queue<ISource>>();

        /// <summary>
        /// dequeue next source from next source list
        /// </summary>
        /// <remarks>
        /// <para>The effect of removing the list from the dictionary and reading it should put it at the end</para>
        /// </remarks>
        /// <returns></returns>
        public ISource Dequeue()
        {
            if (indexedSources.Count == 0) return null;

            var distributionId = indexedSources.Keys.FirstOrDefault();
            
            return Dequeue(distributionId);
        }

        public ISource Dequeue(int distributionId)
        {
            return indexedSources[distributionId].Dequeue();
        }

        /// <summary>
        /// add and index source to the FIFO queue
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public void Enqueue(ISource source)
        {
            var queue = this.InitializeStorage(source.DistributionId);
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
        protected Queue<ISource> InitializeStorage(int distributionId)
        {
            if (!indexedSources.ContainsKey(distributionId))
            {
                indexedSources[distributionId] = new Queue<ISource>();
            }

            return indexedSources[distributionId];
        }

    }
}
