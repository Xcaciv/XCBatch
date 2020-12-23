using System;
using XCBatch.Interfaces;

namespace XCBatch.Core.UnitTests.Implementations
{
    public class ProcessorOne : IProcessor<SourceOne>
    {
        public Type SourceType => typeof(SourceOne);

        public IProcessResultState ExpectedStatus { get; set; }

        public IProcessResultState Process(ISource source)
        {
            return ExpectedStatus;
        }
    }
}
