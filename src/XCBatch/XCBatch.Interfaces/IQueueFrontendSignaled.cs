using System;
using System.Collections.Generic;
using System.Text;

namespace XCBatch.Interfaces
{
    /// <summary>
    /// extension to frontend that allows to signal the end of the queue for use in treading
    /// </summary>
    public interface IQueueFrontendSignaled : IQueueFrontend
    {
        /// <summary>
        /// Signal no more items will be added to the queue
        /// </summary>
        /// <remarks><para>This allows for the dispatcher to complete and prevents any more items from being added</para></remarks>
        void CompleteEnqueue();

        /// <summary>
        /// Signal no more items will be added to the queue for a distribution id
        /// </summary>
        /// <remarks><para>This allows for the dispatcher to complete</para></remarks>
        void CompleteEnqueue(int distributionId);
    }
}
