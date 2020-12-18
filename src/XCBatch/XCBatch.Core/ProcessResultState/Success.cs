namespace XCBatch.Core.ProcessResultState
{
    /// <summary>
    /// signifies a successful processor run
    /// all success states must extend this state
    /// </summary>
    public class Success : AbstractBase
    {
        public Success(string reasonCode) : base(reasonCode) { }
        public override string Name => "Success";

        public override bool IsFinal => true;
    }
}
