using System;
using XCBatch.Interfaces;

namespace XCBatch.Core.Source
{
    /// <summary>
    /// default source implementation
    /// </summary>
    public abstract class Source : ISource
    {
        /// <summary>
        /// arbitrary identifier for tracking an operation
        /// </summary>
        public Guid TransferId { get; set; }
        /// <summary>
        /// value for distributing operations
        /// </summary>
        public int DistributionId { get; set; } = -1;

        /// <summary>
        /// signifies potential processing cost
        /// </summary>
        public int Burden { get; set; } = -1;

        /// <summary>
        /// identifies source
        /// </summary>
        public long SubjectId { get; set; } = -1;
    }
}
