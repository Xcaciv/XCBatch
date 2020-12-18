using System.Collections.Generic;
using System.Linq;
using XCBatch.Interfaces;

namespace XCBatch.Core
{
    /// <summary>
    /// default in memory queue
    /// </summary>
    public class MemoryBlockQueueDistributed : IBlockQueueBackendDistributed
    {
        /// <summary>
        /// current queue quantity
        /// </summary>
        public int Count => indexedSources.SelectMany(o => o.Value).Count();

        /// <summary>
        /// sources indexed distribution id then by type
        /// </summary>
        protected readonly Dictionary<int, Queue<ISourceBlock<ISource>>> indexedSources = new Dictionary<int, Queue<ISourceBlock<ISource>>>();

        /// <summary>
        /// dequeue next source from next source list
        /// </summary>
        /// <remarks>
        /// <para>The effect of removing the list from the dictionary and reading it should put it at the end</para>
        /// </remarks>
        /// <returns></returns>
        public ISourceBlock<ISource> Dequeue()
        {
            if (indexedSources.Count == 0) return null;

            var distributionId = indexedSources.Keys.FirstOrDefault();
            
            return Dequeue(distributionId);
        }

        public ISourceBlock<ISource> Dequeue(int distributionId)
        {
            return indexedSources[distributionId].Dequeue();
        }

        /// <summary>
        /// add and index source to the FIFO queue
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public int Enqueue(ISourceBlock<ISource> source)
        {
            var queue = this.InitializeStorage(source.DistributionId);
            queue.Enqueue(source);
            return this.Count;
        }

        /// <summary>
        /// add and index source collection to FIFO queue
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public int Enqueue(IEnumerable<ISourceBlock<ISource>> sources)
        {
            foreach(var source in sources)
            {
                Enqueue(source);
            }

            return this.Count;
        }

        /// <summary>
        /// ensure there is a list for the given distribution and source type
        /// </summary>
        /// <param name="sourceTypeName"></param>
        /// <param name="distributionId"></param>
        protected Queue<ISourceBlock<ISource>> InitializeStorage(int distributionId)
        {
            if (!indexedSources.ContainsKey(distributionId))
            {
                indexedSources[distributionId] = new Queue<ISourceBlock<ISource>>();
            }

            return indexedSources[distributionId];
        }

    }
}
