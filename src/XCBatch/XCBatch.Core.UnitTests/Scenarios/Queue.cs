using XCBatch.Core.UnitTests.Implementations;
using XCBatch.Interfaces;

namespace XCBatch.Core.UnitTests.Scenarios
{
    internal static class Queue
    {
        public static IQueueFrontend DispatchOneDeadLetter(IQueueFrontend queueClient)
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

        public static IQueueFrontend DispatchManyLong(IQueueFrontendSignaled queueClient)
        {

            for (int i = 1; i <= 1000; i++)
            {
                queueClient.Enqueue(new SourceOne()
                {
                    SubjectId = i
                });
            }

            queueClient.CompleteEnqueue();

            IProcessor<ISource> aProcessor = new ParallelProcessor();

            queueClient.RegisterProcessor(aProcessor);

            queueClient.Dispatch();

            return queueClient;
        }
    }
}
