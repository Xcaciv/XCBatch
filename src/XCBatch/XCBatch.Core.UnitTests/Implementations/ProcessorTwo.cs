using System;
using XCBatch.Interfaces;

namespace XCBatch.Core.UnitTests.Implementations
{
    public class ProcessorTwo : IProcessor<ISource>
    {
        public Type SourceType => typeof(SourceTwo);

        public IProcessResultState ExpectedStatus { get; set; }

        public IProcessResultState Process(ISource source)
        {
            return ExpectedStatus;
        }
    }
}
