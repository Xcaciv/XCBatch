using System;

namespace XCBatch.Interfaces
{
    /// <summary>
    /// describes a operation/job source
    /// </summary>
    /// <remarks>
    /// <para>This interface is a purposely terse description of a work source. This source
    /// must have a registered processor to be dequeued. Avoid defining a generic processor
    /// as this would cause duplication of functionality.</para>
    /// </remarks>
    public interface ISource
    {
        /// <summary>
        /// arbitrary identifier for tracking an operation
        /// </summary>
        /// 
        /// <remarks>
        /// <para>This is not meant to be unique. However it is to be used to track a process 
        /// through subsequent queuing of child data to be processed to it's entirety.</para>
        /// 
        /// <para>For example a file may be queued for parsing. Once Parsed, each entity in 
        /// the file can then be re-queued to process to the next step. The next step may also
        /// re-queue its output or other operations. All of these can share the same TransferId.
        /// The whole series of operations can then be followed when the Id is used in 
        /// logging.</para>
        /// 
        /// </remarks>
        Guid TransferId { get; }

        /// <summary>
        /// used in distributing shared resources
        /// </summary>
        /// 
        /// <remarks>
        /// <para>To be used in dequeue filters. An ENUM is not used here to prevent unnecessary 
        /// dependencies.</para>
        /// </remarks>
        /// 
        /// <example>
        /// // assign processors on a per-customer basis
        /// mySource.DistributionId = user.CustomerId;
        /// </example>
        int DistributionId { get; }

        /// <summary>
        /// used by queue and processor to determine how to handle resources.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>The processor may decide to use more memory or preprocess some part of the
        /// source to optimize based on the burden score.</para>
        /// <para>If the processor has the resources it could choose to process many high Burden
        /// source in parallel or use async calls to allow for long network waits depending on 
        /// the source type.</para>
        /// </remarks>
        int Burden { get; }

        /// <summary>
        /// used by Processor to locate job 
        /// </summary>
        /// 
        /// <remarks>
        /// <para>Since the processor is intended on being type specific, it is left with
        /// the responsibility of knowing what this id means.</para>
        /// </remarks>
        long SubjectId { get; } 
    }
}