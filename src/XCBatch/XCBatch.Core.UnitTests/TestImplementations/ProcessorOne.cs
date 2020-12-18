using System;
using System.Collections.Generic;
using System.Text;
using XCBatch.Interfaces;

namespace XCBatch.Core.UnitTests.TestImplementations
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
