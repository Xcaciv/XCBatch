using Xunit;

namespace XCBatch.Core.UnitTests
{
    public class QueueClientLightTests
    {
        [Fact()]
        public void Dispatch_WithDeadletter_AddsDeadLetter()
        {
            var queueClient = Core.Factory.GetBasicQueueInstance();
            queueClient.EnableDeadLetter = true;

            var frontend = Scenarios.Queue.EnqueueOneDeadLetter(queueClient);
            frontend.Dispatch();

            Assert.Single(frontend.DeadLetters);
        }

        [Fact()]
        public void Dispatch_WithDeadLetterDisabled_NoDeadLetter()
        {
            var queueClient = Core.Factory.GetBasicQueueInstance();

            var frontend = Scenarios.Queue.EnqueueOneDeadLetter(queueClient);
            frontend.Dispatch();

            Assert.Empty(frontend.DeadLetters);
        }
    }
}