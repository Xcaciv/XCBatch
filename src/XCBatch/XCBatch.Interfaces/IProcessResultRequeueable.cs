using System.Collections.Generic;

namespace XCBatch.Interfaces
{
    /// <summary>
    /// returned by a processor that has results to be queued
    /// </summary>
    public interface IProcessResultRequeueable : IProcessResultState
    {
        /// <summary>
        /// result to be re-queued for processing
        /// </summary>
        IEnumerable<ISource> GetQueueableResults();
    }
}
