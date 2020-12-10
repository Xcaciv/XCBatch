using System;
using System.Collections.Generic;
using System.Text;

namespace XCBatch.Interfaces
{
    /// <summary>
    /// unified interface for queue operations
    /// </summary>
    /// <remarks>
    /// The backend
    /// </remarks>
    public interface IQueue
    {
        /// <summary>
        /// add a source to the queue
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        bool Enqueue(ISource source);
        /// <summary>
        /// remove and return the next item from the queue
        /// </summary>
        /// <param name="type">value coresponding to ISource.Type</param>
        /// <returns></returns>
        ISource Dequeue(string type);
        /// <summary>
        /// remove and return the next item for a particular distribution from the queue
        /// </summary>
        /// <param name="type">value coresponding to ISource.Type</param>
        /// <param name="DistrobutionId">exact distrobution filter</param>
        /// <returns></returns>
        ISource Dequeue(string type, int DistrobutionId);
    }
}
