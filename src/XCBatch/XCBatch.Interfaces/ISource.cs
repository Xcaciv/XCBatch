using System;
using System.Collections.Generic;
using System.Text;

namespace XCBatch.Interfaces
{
    /// <summary>
    /// describes a operation/job source
    /// </summary>
    /// <remarks>
    /// <para>This interface is a purposly turse description of a work source. This source
    /// must have a registered processor to be dequeued. Avoid defining a generic processor
    /// as this would cause duplication of functionality.</para>
    /// </remarks>
    public interface ISource
    {
        /// <summary>
        /// arbitrary identifier generated
        /// </summary>
        /// <remarks>
        /// <para>Used to track a process through subsiquent queueing of child data to
        /// be processed to it's entirety.</para>
        /// 
        /// <para></para>
        /// </remarks>
        Guid TransferId { get; }
        /// <summary>
        /// used in distributing shared resources
        /// </summary>
        /// <remarks>
        /// <para>To be used in dequeue filters.</para>
        /// </remarks>
        /// <example>
        /// mySource.DistrobutionId = user.CustomerId;
        /// </example>
        int DistrobutionId { get; }
        /// <summary>
        /// name used for dequeue
        /// </summary>
        /// <remarks>
        /// <para>Specific to the implementation. This shal not be used in place of namespace 
        /// which will be decided by the queue.</para>
        /// </remarks>
        string Type { get; }
        /// <summary>
        /// used by Processor to locate job 
        /// </summary>
        /// <remarks>
        /// <para>Since the processor is intented on being type specific, it is left with
        /// the responsibility of knowing what this id means.</para>
        /// </remarks>
        long SubjectId { get; }
    }
}