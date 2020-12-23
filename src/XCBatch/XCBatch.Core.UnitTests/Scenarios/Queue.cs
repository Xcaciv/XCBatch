using XCBatch.Core.UnitTests.Implementations;
using XCBatch.Interfaces;

namespace XCBatch.Core.UnitTests.Scenarios
{
    internal static class Queue
    {
        public static IQueueClient DispatchOneDeadLetter(IQueueClient queueClient)
        {

            var aSouce = new SourceOne()
            {
                SubjectId = 1
            };

            var bSource = new SourceTwo()
            {
                SubjectId = 200
            };

            queueClient.Enqueue(aSouce);
            queueClient.Enqueue(bSource);

            IProcessor<ISource> aProcessor = new ProcessorOne();

            queueClient.RegisterProcessor(aProcessor);

            queueClient.Dispatch();

            return queueClient;
        }
    }
}
