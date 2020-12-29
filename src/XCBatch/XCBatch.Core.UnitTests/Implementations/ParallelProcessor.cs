using System;
using System.Threading.Tasks;
using XCBatch.Interfaces;

namespace XCBatch.Core.UnitTests.Implementations
{
    public class ParallelProcessor
        : IProcessor<SourceOne>
    {
        public Type SourceType => typeof(SourceOne);

        public IProcessResultState ExpectedStatus { get; set; }

        public IProcessResultState Process(ISource source)
        {
            Task.Delay(1000);
            return new ThreadSuccessStatus("test") 
            { 
                ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId
            };
        }
    }
}
