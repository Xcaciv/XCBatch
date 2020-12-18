using Xunit;
using XCBatch.Core;
using System;
using System.Collections.Generic;
using XCBatch.Interfaces;
using XCBatch.Core;
using XCBatch.Core.UnitTests.TestImplementations;

namespace XCBatch.Core.UnitTests
{
    public class QueueClientLightTests
    {
        [Fact()]
        public void Dispatch_WithDeadletter_AddsDeadLetter()
        {
            var queueClient = Core.Factory.GetBasicQueueInstance();
            queueClient.EnableDeadLetter = true;

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

            Assert.True(queueClient.DeadLetters.Count == 1);
        }
    }
}