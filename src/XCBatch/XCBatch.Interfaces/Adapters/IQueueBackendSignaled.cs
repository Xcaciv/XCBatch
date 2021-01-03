using System;
using System.Collections.Generic;
using System.Text;

namespace XCBatch.Interfaces.Adapters
{
    /// <summary>
    /// interface to a queue that can be signaled as complete
    /// </summary>
    public interface IQueueBackendSignaled : IQueueBackendDistributed
    {
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
