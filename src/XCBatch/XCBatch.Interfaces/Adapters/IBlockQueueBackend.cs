namespace XCBatch.Interfaces.Adapters
{
    /// <summary>
    /// process queue of blocks
    /// </summary>
    public interface IBlockQueueBackend
    {
        /// <summary>
        /// number of source items in the queue
        /// </summary>
        int Count { get; }
        /// <summary>
        /// add a source block to the queue
        /// </summary>
        /// <param name="source"></param>
        /// <returns>queue count</returns>
        void Enqueue(ISourceBlock<ISource> sourceBlock);
        /// <summary>
        /// remove and return the next block from the queue
        /// </summary>
        /// <returns></returns>
        ISourceBlock<ISource> Dequeue();
    }
}
