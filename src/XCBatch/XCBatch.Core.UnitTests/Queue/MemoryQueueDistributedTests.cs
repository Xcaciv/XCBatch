using Xunit;
using System;
using System.Collections.Generic;
using System.Text;

namespace XCBatch.Core.UnitTests.Queue
{
    public class MemoryQueueDistributedTests
    {
        [Fact()]
        public void Dequeue_Ten_InOrder()
        {
            var queue = new MemoryQueueDistributed();

            queue.ShouldDequeueInOrder();
        }

        [Fact()]
        public void Dequeue_Distributed_RoundRobin()
        {
            var queue = new MemoryQueueDistributed();

            queue.ShouldDequeueDistributedRoundRobin();
        }


        [Fact()]
        public void Dequeue_Distribution_InOrder()
        {
            var queue = new MemoryQueueDistributed();

            queue.ShouldDequeueDistributionInOrder();
        }
    }
}