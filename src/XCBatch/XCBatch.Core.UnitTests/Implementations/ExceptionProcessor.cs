using System;
using System.Threading.Tasks;
using XCBatch.Interfaces;

namespace XCBatch.Core.UnitTests.Implementations
{
    public class ExceptionProcessor
        : IProcessor<SourceOne>
    {
        public Type SourceType => typeof(SourceThree);

        public IProcessResultState ExpectedStatus { get; set; }

        public IProcessResultState Process(ISource source)
        {
            Task.Delay((new Random()).Next(188));
            throw new Exception("just to make sure the exception handling works as intended");
        }
    }
}