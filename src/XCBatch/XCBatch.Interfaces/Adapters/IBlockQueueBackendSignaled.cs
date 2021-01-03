using System;
using System.Collections.Generic;
using System.Text;

namespace XCBatch.Interfaces.Adapters
{
    public interface IBlockQueueBackendSignaled : IBlockQueueBackendDistributed
    {
        /// <summary>
        /// Indicate no more queuing. Allow for dispatch to complete.
        /// </summary>
        void CompleteEnqueue();

        /// <summary>
        /// Indicate no more queuing for a distribution. Allow for dispatch of the distribution 
        /// to complete.
        /// </summary>
        void CompleteEnqueue(int DistributionId);
    }
}
