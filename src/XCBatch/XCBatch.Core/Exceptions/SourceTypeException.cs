namespace XCBatch.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a the given source type does not match the current context
    /// </summary>
    public class SourceTypeException : System.Exception
    {
        public SourceTypeException(string message) : base(message) { }
        public SourceTypeException(string message, System.Exception ex) : base(message, ex) { }

        public SourceTypeException()
        {
        }
    }
}
