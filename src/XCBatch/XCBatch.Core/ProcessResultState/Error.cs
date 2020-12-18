using System.Collections.Generic;
using XCBatch.Interfaces;

namespace XCBatch.Core.ProcessResultState
{
    /// <summary>
    /// signifies a fatal error in processing
    /// most error states should extend this class
    /// </summary>
    /// <remarks>
    /// <para>This object is placed in the Error queue for handling, logging and reprocessing.
    /// Thus there should be some information to fix any issues or reprocess the source itself.</para>
    /// </remarks>
    public class Error : AbstractBase
    {
        public Error(string reasonCode) : base(reasonCode) { }
        public override string Name => "Error";

        public override bool IsFinal => true;
        /// <summary>
        /// source that is in error state (optional) for reprocessing
        /// </summary>
        public ISource Source { get; set; }
        /// <summary>
        /// messages from processing (optional) 
        /// </summary>
        public List<string> Messages { get; private set; } = new List<string>();
        /// <summary>
        /// exception thrown during processing (optional) 
        /// </summary>
        public System.Exception Ex { get; set; }
    }
}
