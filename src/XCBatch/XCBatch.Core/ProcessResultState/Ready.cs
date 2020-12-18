namespace XCBatch.Core.ProcessResultState
{
    /// <summary>
    /// default unprocessed state
    /// </summary>
    public class Ready : AbstractBase
    {
        public Ready(string reasonCode) : base(reasonCode) { }
        public override string Name => "Ready";

        public override bool IsFinal => false;
    }
}
