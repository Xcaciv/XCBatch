using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XCBatch.Core.Queue.Concurrent;
using XCBatch.Core.UnitTests.Implementations;
using XCBatch.Interfaces.Adapters;
using Xunit;

namespace XCBatch.Core.UnitTests.Queue
{
    public static class AssertExtensions
    {
        internal static void ShouldDequeueInOrder(this IQueueBackend queue, int quantity = 10)
        {
            for (int i = 0; i < quantity; i++)
            {
                queue.Enqueue(new SourceOne() { SubjectId = i, DistributionId = 9 });
            }

            // signal end of queuing if needed
            if (queue is IQueueBackendSignaled queueBackendSignaled)
            {
                (queueBackendSignaled).CompleteEnqueue();
            }

            for (int i = 0; i < quantity; i++)
            {
                var source = queue.Dequeue();
                Assert.Equal(i, source.SubjectId);
            }
        }

        internal static void ShouldDequeueDistribution(this IQueueBackendDistributed queue, int sourceCount = 3, int distributions = 3)
        {
            for (int i = 0; i < distributions; i++)
            {
                for (int s = 0; s < sourceCount; s++)
                {
                    queue.Enqueue(new SourceOne() { SubjectId = s, DistributionId = i });
                }
            }
            
            // signal end of queuing if needed
            if (queue is IQueueBackendSignaled queueBackendSignaled)
            {
                (queueBackendSignaled).CompleteEnqueue();
            }

            var dequeueCount = 0;
            var toDequeue = distributions - 1;
            foreach(var source in new Core.Queue.Enumerator(queue, toDequeue))
            {
                dequeueCount++;
                Assert.Equal(toDequeue, source.DistributionId);
            }

            Assert.Equal(sourceCount, dequeueCount);
        }

        internal static void ShouldDequeueDistributedRoundRobin(this IQueueBackendDistributed queue, int sourceCount = 3, int distributions = 3)
        {
            for (int i = 0; i < distributions; i++)
            {
                for (int s = 0; s < sourceCount; s++)
                {
                    queue.Enqueue(new SourceOne() { SubjectId = s, DistributionId = i });
                }
            }

            // signal end of queuing if needed
            if (queue is IQueueBackendSignaled queueBackendSignaled)
            {
                (queueBackendSignaled).CompleteEnqueue();
            }

            var dequeueCount = 0;
            var distroId = -1;
            while(!queue.IsEmpty)
            {
                dequeueCount++;
                var source = queue.Dequeue();
                Assert.NotEqual(distroId, source.DistributionId);
                distroId = source.DistributionId;
            }

            Assert.Equal(sourceCount*distributions, dequeueCount);
        }

        internal static void ShouldDequeueDistributionInOrder(this IQueueBackendDistributed queue, int sourceCount = 3, int distributions = 3)
        {
            for (int i = 0; i < distributions; i++)
            {
                for (int s = 0; s < sourceCount; s++)
                {
                    queue.Enqueue(new SourceOne() { SubjectId = s, DistributionId = i });
                }
            }

            // signal end of queuing if needed
            if (queue is IQueueBackendSignaled queueBackendSignaled)
            {
                (queueBackendSignaled).CompleteEnqueue();
            }

            var distroId = distributions - 1;
            for (int i = 0; i < sourceCount; i++)
            {
                var source = queue.Dequeue(distroId);
                Assert.Equal(i, source.SubjectId);
            }
        }

        internal static void ShouldFinishEnumeration(this IQueueBackend queue, int sourceCount = 5, int distributions = 3)
        {
            for (int i = 0; i < distributions; i++)
            {
                for (int s = 0; s < sourceCount; s++)
                {
                    queue.Enqueue(new SourceOne() { SubjectId = s, DistributionId = i });
                }
            }

            // signal end of queuing if needed
            if (queue is IQueueBackendSignaled queueBackendSignaled)
            {
                (queueBackendSignaled).CompleteEnqueue();
            }

            foreach (var source in new XCBatch.Core.Queue.Enumerator(queue)) { Assert.NotNull(source); }
        }

        internal static void ShouldFinishEnumerationInParallel(this IQueueBackendSignaled queue, int sourceCount = 5, int distributions = 3)
        {
            Should_Not_FinishEnumerationWithoutComplete(queue, sourceCount, distributions, true);
        }

        internal static void Should_Not_FinishEnumerationWithoutComplete(this IQueueBackendSignaled queue, int sourceCount = 5, int distributions = 3, bool signal = false)
        {
            for (int i = 0; i < distributions; i++)
            {
                for (int s = 0; s < sourceCount; s++)
                {
                    queue.Enqueue(new SourceOne() { SubjectId = s, DistributionId = i });
                }
            }

            var dequeueTask = Task.Factory.StartNew(() =>
            {
                // make sure no nulls are returned
                foreach (var source in new XCBatch.Core.Queue.Enumerator(queue)) { Assert.NotNull(source); }
            });

            if (signal) queue.CompleteEnqueue();

            Thread.SpinWait(60000); // dequeue will finish by now

            Assert.Equal(signal, queue.IsComplete);
            Assert.Equal(signal, dequeueTask.IsCompleted);
        }

        internal static void ShouldNotFinishDistributionEnumerationWithoutComplete(this IQueueBackendSignaled queue, int sourceCount = 5, int distributions = 3, bool signal = false)
        {
            for (int i = 0; i < distributions; i++)
            {
                for (int s = 0; s < sourceCount; s++)
                {
                    queue.Enqueue(new SourceOne() { SubjectId = s, DistributionId = i });
                }
            }

            int targetDistribution = distributions - 1;

            var dequeueTask = Task.Factory.StartNew((distro) =>
            {
                // make sure no nulls are returned
                foreach (var source in new XCBatch.Core.Queue.Enumerator(queue, (int)distro)) { Assert.NotNull(source); }
            }, targetDistribution);

            if (signal) queue.CompleteEnqueue();

            Thread.SpinWait(60000); // dequeue will finish by now

            Assert.Equal(signal, queue.IsComplete);
            Assert.Equal(signal, dequeueTask.IsCompleted);
        }


    }
}
