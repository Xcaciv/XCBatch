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
        public void Enumerator_WithConcurrentMemoryQueue_Finishes()
        {
            var queue = new ConcurrentMemoryQueue();
            queue.ShouldFinishEnumeration();
        }

        [TestMethod()]
        public void Enumerator_WithConcurrentMemoryQueueBound_Finishes()
        {
            var queue = new ConcurrentMemoryQueueBound();
            queue.ShouldFinishEnumeration();
        }


        [TestMethod()]
        public void Enumerator_WithConcurrentMemoryQueue_FinishesInParallel()
        {
            var queue = new ConcurrentMemoryQueue();
            queue.ShouldFinishEnumerationInParallel();
        }

        [TestMethod()]
        public void Enumerator_WithoutComplete_DoesNotFinish()
        {
            var queue = new ConcurrentMemoryQueue();
            queue.ShouldNotFinishEnumerationWithoutComplete();
        }

        [TestMethod()]
        public void Enumerator_WithConcurrentMemoryQueueBound_FinishesInParallel()
        {
            var queue = new ConcurrentMemoryQueueBound();
            queue.ShouldFinishEnumerationInParallel();
        }

        [TestMethod()]
        public void BoundEnumerator_WithoutComplete_DoesNotFinish()
        {
            var queue = new ConcurrentMemoryQueueBound();
            queue.ShouldNotFinishEnumerationWithoutComplete();
        }
    }
}