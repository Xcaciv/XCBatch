namespace XCBatch.Interfaces.Adapters
{
    /// <summary>
    /// interface to a queue that can be signaled as complete
    /// NOTE: implementations *must* be thread safe
    /// </summary>
    public interface IQueueBackendSignaled : IQueueBackendDistributed
    {
        /// <summary>
        /// Gets a value that indicates the queuing is finished
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// allows to check distribution for completeness
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        bool IsDistributionComplete(int distributionId);

        /// <summary>
        /// Signals the queue is not accepting any more additions
        /// </summary>
        void CompleteEnqueue();

        /// <summary>
        /// Signals a distribution queue is not accepting any more additions 
        /// to complete.
        /// </summary>
        void CompleteEnqueue(int DistributionId);

    }
}
