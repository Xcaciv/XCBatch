using System;
using System.Collections.Generic;
using System.Text;
using XCBatch.Interfaces;

namespace XCBatch.Core.UnitTests.TestImplementations
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
