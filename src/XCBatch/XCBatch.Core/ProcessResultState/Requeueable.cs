using System.Collections.Generic;
using XCBatch.Interfaces;

namespace XCBatch.Core.ProcessResultState
{
    /// <summary>
    /// status that specifies the processor has output that needs to be processed further
    /// </summary>
    public class Requeueable : Success, IStateRequeueable
    {
        private readonly IEnumerable<ISource> _sourceToQueue;

        public Requeueable(string reasonCode, IEnumerable<ISource> sourceToQueue) 
            : base(reasonCode)
        {
            this._sourceToQueue = sourceToQueue;
        }
        public Requeueable(string reasonCode) : base(reasonCode) { }
        public override string Name => "Requeueable";
        public override bool IsFinal => false;

        public IEnumerable<ISource> GetQueueableResults()
        {
            return _sourceToQueue;
        }
    }
}
