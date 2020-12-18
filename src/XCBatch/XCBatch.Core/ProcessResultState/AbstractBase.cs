using XCBatch.Interfaces;

namespace XCBatch.Core.ProcessResultState
{
    /// <summary>
    /// default state functionality
    /// </summary>
    public abstract class AbstractBase : IProcessResultState
    {
        /// <summary>
        /// constructor injecting reason code
        /// </summary>
        /// <param name="reasonCode"></param>
        protected AbstractBase(string reasonCode)
        {
            ReasonCode = reasonCode;
        }
        /// <summary>
        /// state name
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// given reason for status
        /// </summary>
        public string ReasonCode { get; set; }
        /// <summary>
        /// signify if there can be more work done
        /// </summary>
        public abstract bool IsFinal { get; }
    }
}
