using System;
using System.Collections.Generic;
using XCBatch.Interfaces;

namespace XCBatch.Core.UnitTests.Implementations
{
    public class ProcessorOne : IProcessor<SourceOne>
    {
        public List<long> DequeuedIds { get; set; } = new List<long>();
        public Type SourceType => typeof(SourceOne);

        public IProcessResultState ExpectedStatus { get; set; }

        public IProcessResultState Process(ISource source)
        {
            DequeuedIds.Add(source.SubjectId);
            return ExpectedStatus;
        }
    }
}
