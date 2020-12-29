using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using XCBatch.Core.Exceptions;
using XCBatch.Interfaces;

namespace XCBatch.Core.Queue.Concurrent
{
    public class ConcurrentMemoryBlockQueue : IBlockQueueBackend
    {
        /// <summary>
        /// current queue quantity
        /// </summary>
        public int Count => blockSourceQueue.Count;

        /// <summary>
        /// list used as queue to provide random access
        /// </summary>
        protected readonly ConcurrentQueue<ISourceBlock<ISource>> blockSourceQueue = new ConcurrentQueue<ISourceBlock<ISource>>();

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

            blockSourceQueue.Enqueue(sourceBlock);
        }

        /// <summary>
        /// Add a queue block
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public void Enqueue(IEnumerable<ISource> source)
        {

        }

        /// <summary>
        /// retrieve the first source block
        /// </summary>
        /// <returns></returns>
        public ISourceBlock<ISource> Dequeue()
        {
            ISourceBlock<ISource> block;
            return (blockSourceQueue.TryDequeue(out block)) ? block : null;
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
