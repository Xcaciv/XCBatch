namespace XCBatch.Interfaces.Adapters
{
    /// <summary>
    /// block back-end that allows for secondary criteria for distribution of tasks
    /// </summary>
    public interface IBlockQueueBackendDistributed
    {
        /// <summary>
        /// remove and return the next item for a particular distribution
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        ISourceBlock<ISource> Dequeue(int distributionId);
    }
}
