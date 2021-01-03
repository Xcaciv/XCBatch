namespace XCBatch.Core.UnitTests.Implementations
{
    public class ThreadSuccessStatus : ProcessResultState.Success
    {
        public int ThreadId { get; set; }
        public ThreadSuccessStatus(string reason) : base(reason) { }
    }
}
