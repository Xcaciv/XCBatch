using System.Threading.Tasks;
using XCBatch.Core.UnitTests.Implementations;
using XCBatch.Interfaces;

namespace XCBatch.Core.UnitTests.Scenarios
{
    internal static class Queue
    {
        public static IQueueFrontend EnqueueOneDeadLetter(IQueueFrontend queueClient)
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

            return queueClient;
        }

        public static IQueueFrontend EnqueueMany(IQueueFrontend queueClient, int queueLength = 1000)
        {

            for (int i = 1; i <= queueLength; i++)
            {
                queueClient.Enqueue(new SourceOne()
                {
                    SubjectId = i
                });
            }

            IProcessor<ISource> aProcessor = new ParallelProcessor();
            queueClient.RegisterProcessor(aProcessor);

            return queueClient;
        }


        public static IQueueFrontend EnqueueManyProcessParallel(IQueueFrontendSignaled queueClient, int queueLength = 1000)
        {

            IProcessor<ISource> aProcessor = new ParallelProcessor();
            queueClient.RegisterProcessor(aProcessor);

            var dispatchTask = Task.Factory.StartNew(() => queueClient.Dispatch());

            for (int i = 1; i <= queueLength; i++)
            {
                queueClient.Enqueue(new SourceOne()
                {
                    SubjectId = i
                });
            }

            queueClient.CompleteEnqueue();

            dispatchTask.Wait();

            return queueClient;
        }
    }
}
