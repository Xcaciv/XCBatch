using System.Collections.Generic;

namespace XCBatch.Interfaces
{
    /// <summary>
    /// describes a block of similar sources to be queued in a batch queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISourceBlock<out T> where T : ISource
    {
        /// <summary>
        /// collection of similar source
        /// </summary>
        IEnumerable<T> SourceList { get; }

        /// <summary>
        /// used in distributing shared resources
        /// </summary>
        /// 
        /// <remarks>
        /// <para>Block queues generally favor this value and ignore ISource.DistributionId 
        /// values save special circumstances.</para>
        /// 
        /// <para>To be used in dequeue filters. An ENUM is not used here to prevent unnecessary 
        /// dependencies.</para>
        /// </remarks>
        /// 
        /// <example>
        /// // assign processors on a per-customer basis
        /// mySourceBlock.DistributionId = user.CustomerId;
        /// </example>
        int DistributionId { get; }

        /// <summary>
        /// name used for dequeue
        /// </summary>
        /// 
        /// <remarks>
        /// <para>Single word English name of type</para>
        /// 
        /// <para>Specific to the implementation. This will not be used in place of namespace 
        /// which will be decided by the queue.</para>
        /// 
        /// <para>Do not use introspection as it would negatively impact performance.</para>
        /// </remarks>
        System.Type SourceType { get; }
    }
}
