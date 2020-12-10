using System;
using System.Collections.Generic;
using System.Text;

namespace XCBatch.Interfaces
{
    /// <summary>
    /// Describes state of source in processed
    /// </summary>
    /// <remarks>
    /// <para>Using a state interface allows for extended states for specific circumstances
    /// or types.</para>
    /// </remarks>
    public interface IState
    {
        /// <summary>
        /// unique name of the state
        /// </summary>
        /// <remarks>
        /// single word English names used to translate for UI
        /// </remarks>
        string Name { get; }
        /// <summary>
        /// single word English code used to translate explination of reason for status in UI
        /// </summary>
        string ReasonCode { get; }
        /// <summary>
        /// indicates no states to follow
        /// </summary>
        bool IsFinal { get; }
    }
}
