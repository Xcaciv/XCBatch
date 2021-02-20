using Xunit;
using XCBatch.Core.Queue;
using XCBatch.Core.Queue.Concurrent;
using System;
using System.Collections.Generic;
using System.Text;
using XCBatch.Core.UnitTests.Implementations;

namespace XCBatch.Core.UnitTests.Queue.Concurrent
{
    public class ConcurrentMemoryQueueTests
    {

        [Fact()]
        public void Dequeue_1k_InOrder()
        {
            var queue = new ConcurrentMemoryQueue();

            queue.ShouldDequeueInOrder(1000);
        }

        [Fact()]
        public void Dequeue_Distributed_RoundRobin()
        {
            var queue = new ConcurrentMemoryQueue();

            queue.ShouldDequeueDistributedRoundRobin();
        }


        [Fact()]
        public void Dequeue_Distribution_InOrder()
        {
            var queue = new ConcurrentMemoryQueue();

            queue.ShouldDequeueDistributionInOrder();
        }

        [Fact()]
        public void DequeueDistribution_Incomplete_ShouldBlock()
        {
            var queue = new ConcurrentMemoryQueue();

            queue.ShouldDequeueDistributionInOrder();
        }

    }


}