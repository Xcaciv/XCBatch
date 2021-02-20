using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using XCBatch.Core.Queue.Concurrent;

namespace XCBatch.Core.UnitTests.Queue
{
    [TestClass()]
    public class EnumeratorTests
    {
        [TestMethod()]
        public void Enumerator_WithMemoryQueue_Finishes()
        {
            var queue = new MemoryQueue();
            queue.ShouldFinishEnumeration();
        }

        [TestMethod()]
        public void Enumerator_WithMemoryQueueDistributed_Finishes()
        {
            var queue = new MemoryQueueDistributed();
            queue.ShouldFinishEnumeration();
        }

        [TestMethod()]
        public void DistributedEnumerator_DistributedEnumerator_Finishes()
        {
            var queue = new MemoryQueueDistributed();
            queue.ShouldDequeueDistribution();
        }

        [TestMethod()]
        public void ConcurrentMemoryQueue_WithConcurrentMemoryQueue_Finishes()
        {
            var queue = new ConcurrentMemoryQueue();
            queue.ShouldFinishEnumeration();
        }

        [TestMethod()]
        public void ConcurrentMemoryQueue_DistributedEnumerator_Finishes()
        {
            var queue = new ConcurrentMemoryQueue();
            queue.ShouldDequeueDistribution();
        }

        [TestMethod()]
        public void ConcurrentMemoryQueue_InParallel_Finishes()
        {
            var queue = new ConcurrentMemoryQueue();
            queue.ShouldFinishEnumerationInParallel();
        }

        [TestMethod()]
        public void ConcurrentMemoryQueue_WithoutComplete_DoesNotFinish()
        {
            var queue = new ConcurrentMemoryQueue();
            queue.Should_Not_FinishEnumerationWithoutComplete();
        }

        [TestMethod()]
        public void ConcurrentMemoryQueueBound_Enumerator_Finishes()
        {
            var queue = new ConcurrentMemoryQueueBound();
            queue.ShouldFinishEnumeration();
        }

        [TestMethod()]
        public void ConcurrentMemoryQueueBound_DistributedEnumerator_Finishes()
        {
            var queue = new ConcurrentMemoryQueueBound();
            queue.ShouldDequeueDistribution();
        }


        [TestMethod()]
        public void ConcurrentMemoryQueueBound_ParallelEnumerator_Finishes()
        {
            var queue = new ConcurrentMemoryQueueBound();
            queue.ShouldFinishEnumerationInParallel();
        }

        [TestMethod()]
        public void ConcurrentMemoryQueueBound_WithoutComplete_DoesNotFinish()
        {
            var queue = new ConcurrentMemoryQueueBound();
            queue.Should_Not_FinishEnumerationWithoutComplete();
        }
    }
}