using Xunit;
using System.Linq;
using XCBatch.Core.UnitTests.Implementations;

namespace XCBatch.Core.UnitTests
{
    public class ParallelQueueFrontendTests
    {
        [Fact()]
        public void Dispatch_WithDeadletter_AddsDeadLetter()
        {
            var queueClient = Core.Factory.GetParallelQueueInstance();
            queueClient.ShouldProduceDeadLetterEnabled();
        }

        [Fact()]
        public void Dispatch_WithDeadLetterDisabled_NoDeadLetter()
        {
            var queueClient = Core.Factory.GetParallelQueueInstance();
            queueClient.ShouldNotProduceDealdLetterDisabled();
        }

        [Fact()]
        public void Enqueue_WithMany_ResultsInThreads()
        {
            using (var frontend = Core.Factory.GetParallelQueueInstance())
            {
                frontend.EnableSaveSuccess = true;

                Scenarios.Queue.EnqueueMany(frontend);
                frontend.CompleteEnqueue();
                frontend.Dispatch();

                var notExpected = System.Threading.Thread.CurrentThread.ManagedThreadId;

                var threadIds = frontend.Successful.Select(o => ((Implementations.ThreadSuccessStatus)o).ThreadId).Distinct().ToList();

                // test that stuff happed in another thread
                Assert.Contains(frontend.Successful, o => ((Implementations.ThreadSuccessStatus)o).ThreadId != notExpected);
            }
        }

        [Fact()]
        public void Enqueue_WithDeadLetter_Routed()
        {
            using (var frontend = Core.Factory.GetParallelQueueInstance())
            {
                frontend.EnableDeadLetter = true;

                Scenarios.Queue.EnqueueMany(frontend, 10);
                frontend.Enqueue(new SourceTwo()); // item to be deadlettered
                frontend.CompleteEnqueue();

                frontend.Dispatch();

                // test that stuff happed in another thread
                Assert.NotEmpty(frontend.DeadLetters);
            }
        }

        [Fact()]
        public void Enqueue_WithProcessorException_Routed()
        {
            using (var frontend = Core.Factory.GetParallelQueueInstance())
            {
                frontend.EnableDeadLetter = true;

                Scenarios.Queue.EnqueueMany(frontend, 10);
                frontend.RegisterProcessor(new ExceptionProcessor());
                frontend.Enqueue(new SourceThree());
                frontend.CompleteEnqueue();

                frontend.Dispatch();

                // test that stuff happed in another thread
                Assert.NotEmpty(frontend.Unsuccessful);
                Assert.IsType<ProcessResultState.Error>(frontend.Unsuccessful.FirstOrDefault());
            }
        }
    }
}