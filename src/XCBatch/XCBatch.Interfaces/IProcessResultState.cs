
namespace XCBatch.Interfaces
{
    /// <summary>
    /// Describes state of source in processed
    /// </summary>
    /// <remarks>
    /// <para>Using a state interface allows for extended states for specific circumstances
    /// or types.</para>
    /// </remarks>
    public interface IProcessResultState
    {
        /// <summary>
        /// unique name of the state
        /// </summary>
        /// <remarks>
        /// single word English names used to translate for UI
        /// </remarks>
        string Name { get; }
        /// <summary>
        /// single word English code used to translate explanation of reason for status in UIgit
        /// </summary>
        string ReasonCode { get; }
        /// <summary>
        /// indicates no states to follow
        /// </summary>
        bool IsFinal { get; }
    }
}
