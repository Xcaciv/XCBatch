using System;
using System.Collections.Generic;
using System.Text;
using XCBatch.Interfaces;
using XCBatch.Interfaces.Adapters;
using Xunit;

namespace XCBatch.Core.UnitTests
{
    internal static class AssertExtensions
    {
        internal static void ShouldProduceDeadLetterEnabled(this IQueueFrontend queueClient)
        {
            queueClient.EnableDeadLetter = true;

            var frontend = Scenarios.Queue.EnqueueOneDeadLetter(queueClient);

            // signal end of queuing if needed
            if (queueClient is IQueueFrontendSignaled queueSignaled)
            {
                (queueSignaled).CompleteEnqueue();
            }

            frontend.Dispatch();

            Assert.Single(frontend.DeadLetters);
        }

        internal static void ShouldNotProduceDealdLetterDisabled(this IQueueFrontend queueClient)
        {

            var frontend = Scenarios.Queue.EnqueueOneDeadLetter(queueClient);
            
            // signal end of queuing if needed
            if (queueClient is IQueueFrontendSignaled queueSignaled)
            {
                (queueSignaled).CompleteEnqueue();
            }

            frontend.Dispatch();

            Assert.Empty(frontend.DeadLetters);
        }        
    }
}
