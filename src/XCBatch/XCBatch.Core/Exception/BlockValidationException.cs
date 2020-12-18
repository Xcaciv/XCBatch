

namespace XCBatch.Core.Exception
{
    /// <summary>
    /// Exception thrown when a block contains elements that do not belong
    /// </summary>
    public class BlockValidationException : System.Exception
    {
        public BlockValidationException(string message) : base(message) { }
        public BlockValidationException(string message, System.Exception ex) : base(message, ex) { }

        public BlockValidationException()
        {
        }
    }
}
