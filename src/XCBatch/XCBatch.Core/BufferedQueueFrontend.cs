using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XCBatch.Interfaces;
using XCBatch.Interfaces.Adapters;

namespace XCBatch.Core
{
    /// <summary>
    /// Parallel Queue client featuring a dual queue system for handling network delay and retry to
    /// a secondary queue that may be remote or slower than a memory queue.
    /// </summary>
    /// <remarks>
    /// <para></para>
    /// </remarks>
    public class BufferedQueueFrontend : ParallelQueueFrontend
    {
        /// <summary>
        /// fast thread safe queue
        /// </summary>
        protected BlockingCollection<ISource>[] bufferQueue;

        /// <summary>
        /// tasks for moving source to backend queue
        /// </summary>
        protected List<Task> flushThreads;

        /// <summary>
        /// timeout for collection reads
        /// </summary>
        protected int timeout;

        /// <summary>
        /// construct fronted with a fast bufferQueue and a slower queue
        /// </summary>
        /// <param name="backendQueue">thread safe queue adapter</param>
        /// <param name="maxEnqueue"></param>
        /// <param name="timeoutSeconds"></param>
        /// <param name="collectionNodes"></param>
        public BufferedQueueFrontend(IQueueBackendSignaled backendQueue, int timeoutSeconds = 1, int collectionNodes = 3, int flushJobs = 2) : base(backendQueue)
        {
            bufferQueue = Queue.Concurrent.ConcurrentMemoryQueue.BuildCollectionNodes(collectionNodes);
            timeout = timeoutSeconds;

            for (int i = 0; i < flushJobs; i++)
            {
                var t = timeoutSeconds;
                flushThreads.Add(Task.Factory.StartNew(() => Flush(t)));
            }
             
        }

        /// <summary>
        /// pass buffered source to backend
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        protected void Flush(int timeoutSeconds)
        {
            while (!bufferQueue.All(o => o.IsCompleted))
            {
                ISource source;
                BlockingCollection<ISource>.TryTakeFromAny(bufferQueue, out source, TimeSpan.FromSeconds(timeoutSeconds));
                base.backend.Enqueue(source);
            }

            backend.CompleteEnqueue();
        }

        /// <summary>
        /// buffer the source to be passed to backend
        /// </summary>
        /// <param name="source"></param>
        new public void Enqueue(ISource source)
        {
            BlockingCollection<ISource>.TryAddToAny(bufferQueue, source, timeout);
        }

        /// <summary>
        /// buffer the source to be passed to backend
        /// </summary>
        /// <param name="sources"></param>
        new public void EnqueueRange(IEnumerable<ISource> sources)
        {
            foreach(var source in sources)
            {
                BlockingCollection<ISource>.TryAddToAny(bufferQueue, source, timeout);
            }
        }

        /// <summary>
        /// signal no more incoming sources
        /// </summary>
        new public void CompleteEnqueue()
        {
            foreach(var collection in bufferQueue)
            {
                collection.CompleteAdding();
            }
        }
    }
}
