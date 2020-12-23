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

            var dispatchedClient = Scenarios.Queue.DispatchOneDeadLetter(queueClient);

            Assert.Single(dispatchedClient.DeadLetters);
        }

        [Fact()]
        public void Dispatch_WithDeadLetterDisabled_NoDeadLetter()
        {
            var queueClient = Core.Factory.GetBasicQueueInstance();

            var dispatchedClient = Scenarios.Queue.DispatchOneDeadLetter(queueClient);

            Assert.Empty(dispatchedClient.DeadLetters);
        }
    }
}