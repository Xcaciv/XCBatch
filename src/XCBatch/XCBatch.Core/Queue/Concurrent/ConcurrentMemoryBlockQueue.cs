using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using XCBatch.Core.Exceptions;
using XCBatch.Core.Source;
using XCBatch.Interfaces;
using XCBatch.Interfaces.Adapters;

namespace XCBatch.Core.Queue.Concurrent
{
    /// <summary>
    /// block queue implementation that requires consistent typed blocks
    /// </summary>
    public class ConcurrentMemoryBlockQueue : IBlockQueueBackendSignaled
    {
        /// <summary>
        /// seconds to allow for read or write to the queue
        /// </summary>
        private readonly int timeout;

        /// <summary>
        /// queue clusters for distribution
        /// </summary>
        protected ConcurrentDictionary<int, BlockingCollection<ISourceBlock<ISource>>[]> blockSourceQueue;

        /// <summary>
        /// current queue status
        /// </summary>
        public bool IsEmpty => blockSourceQueue.IsEmpty;

        /// <summary>
        /// constructor allowing for altering of default queue cluster size and timeout
        /// </summary>
        /// <param name="queueClusterSize"></param>
        /// <param name="timeoutSeconds"></param>
        public ConcurrentMemoryBlockQueue(int queueClusterSize = 3, int timeoutSeconds = 1)
        {
            timeout = timeoutSeconds;
            blockSourceQueue[-1] = BuildCollectionNodes(queueClusterSize);
        }

        /// <summary>
        /// create a blocking collection array
        /// </summary>
        /// <param name="collectionNodeCount"></param>
        /// <returns></returns>
        public static BlockingCollection<ISourceBlock<ISource>>[] BuildCollectionNodes(int collectionNodeCount)
        {
            var nodes = new List<BlockingCollection<ISourceBlock<ISource>>>();
            for (int i = 0; i < collectionNodeCount; i++)
            {
                nodes.Add(new BlockingCollection<ISourceBlock<ISource>>());
            }

            return nodes.ToArray();
        }

        /// <summary>
        /// add a source block to the queue 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public void Enqueue(ISourceBlock<ISource> sourceBlock)
        {
            if (!CheckBlockSourceAreValid(sourceBlock))
            {
                throw new BlockValidationException("Source Block contains inconsistent source types in it's collection.");
            }

            BlockingCollection<ISourceBlock<ISource>>.TryAddToAny(blockSourceQueue[sourceBlock.DistributionId], sourceBlock, System.TimeSpan.FromSeconds(timeout));
        }

        /// <summary>
        /// Add a generic source block from an IEnumerable
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public void Enqueue(IEnumerable<ISource> source)
        {
            this.Enqueue((ISourceBlock<ISource>)new SourceBlock<ISource>(source));
        }

        /// <summary>
        /// signal no more queuing and allow for dispatch to finish
        /// </summary>
        public void CompleteEnqueue()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// signal no more queuing and allow for dispatch to finish
        /// </summary>
        public void CompleteEnqueue(int distributionId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// retrieve the first source block
        /// </summary>
        /// <returns></returns>
        public ISourceBlock<ISource> Dequeue()
        {
            return Dequeue(-1);
        }

        /// <summary>
        /// retrieve the first source block
        /// </summary>
        /// <returns></returns>
        public ISourceBlock<ISource> Dequeue(int distributionId = -1)
        {
            ISourceBlock<ISource> block;
            BlockingCollection<ISourceBlock<ISource>>.TryTakeFromAny(blockSourceQueue[distributionId], out block, System.TimeSpan.FromSeconds(timeout));
            return block;
        }
        /// <summary>
        /// Because a block should be able to be handled by a single processor they must
        /// all be of the same source type
        /// </summary>
        /// <param name="sourceBlock"></param>
        /// <returns></returns>
        protected bool CheckBlockSourceAreValid(ISourceBlock<ISource> sourceBlock)
        {
            var first = sourceBlock.SourceList.FirstOrDefault();
            if (first == null) return true;
            // if there are any source types that are not the same as the first, the block is invalid
            return !sourceBlock.SourceList.Any(o => o.GetType() != first.GetType());
        }
    }
}
