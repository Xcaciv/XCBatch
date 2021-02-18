using Xunit;
using System;
using System.Collections.Generic;
using System.Text;

namespace XCBatch.Core.UnitTests.Queue
{
    public class MemoryQueueTests
    {
        [Fact()]
        public void Dequeue_OneHundred_InOrder()
        {
            var queue = new MemoryQueue();

            queue.ShouldDequeueInOrder();
        }
    }
}