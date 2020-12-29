

namespace XCBatch.Interfaces
{
    /// <summary>
    /// back-end that allows for secondary criteria for distribution of tasks
    /// </summary>
    public interface IQueueBackendDistributed : IQueueBackend
    {
        /// <summary>
        /// remove and return the next item for a particular distribution
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        ISource Dequeue(int distributionId);
    }
}
