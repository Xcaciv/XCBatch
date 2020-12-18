using System;
using System.Collections.Generic;
using System.Linq;
using XCBatch.Interfaces;

namespace XCBatch.Core
{
    /// <summary>
    /// concrete block queue using a List preserving block order
    /// </summary>
    public class MemoryBlockQueue : IBlockQueueBackend
    {
        /// <summary>
        /// memory list of blocks
        /// preserves global block order
        /// </summary>
        protected readonly Queue<ISourceBlock<ISource>> blockSourceQueue = new Queue<ISourceBlock<ISource>>();

        /// <summary>
        /// current number of source blocks
        /// </summary>
        public int Count => blockSourceQueue.Count;

        /// <summary>
        /// add a source block to the queue 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public int Enqueue(ISourceBlock<ISource> sourceBlock)
        {
            if (!CheckBlockSourceAreValid(sourceBlock))
            {
                throw new Exception.BlockValidationException("Source Block contains inconsistent source types in it's collection.");
            }

            blockSourceQueue.Enqueue(sourceBlock);
            return Count;
        }

        /// <summary>
        /// Add a queue block
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public int Enqueue(IEnumerable<ISource> source)
        {
            var block = new Source.SourceBlock<ISource>(source);
            return this.Enqueue((ISourceBlock<ISource>)block);
        }

        /// <summary>
        /// retrieve the first source block
        /// </summary>
        /// <returns></returns>
        public ISourceBlock<ISource> Dequeue()
        {
            return blockSourceQueue.Dequeue();
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
