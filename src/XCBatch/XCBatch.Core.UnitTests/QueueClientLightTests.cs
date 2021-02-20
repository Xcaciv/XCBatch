using Xunit;

namespace XCBatch.Core.UnitTests
{
    public class QueueClientLightTests
    {
        [Fact()]
        public void Dispatch_WithDeadletter_AddsDeadLetter()
        {
            var queueClient = Core.Factory.GetBasicQueueInstance();
            queueClient.ShouldProduceDeadLetterEnabled();
        }

        [Fact()]
        public void Dispatch_WithDeadLetterDisabled_NoDeadLetter()
        {
            var queueClient = Core.Factory.GetBasicQueueInstance();
            queueClient.ShouldNotProduceDealdLetterDisabled();
        }
    }
}