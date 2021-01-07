using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
    /// <para>The default buffer uses a bound memory queue that will wait to enqueue when 
    /// the queue hits 30k. To raise this limit configure your own IQueueBackendSignaled and 
    /// pass it in.</para>
    /// </remarks>
    public class BufferedQueueFrontend : ParallelQueueFrontend
    {
        /// <summary>
        /// fast thread safe queue
        /// </summary>
        protected IQueueBackendSignaled bufferQueue;

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
        /// <param name="timeoutSeconds"></param>
        /// <param name="bufferNodes"></param>
        /// <param name="flushJobs"></param>
        public BufferedQueueFrontend(IQueueBackendSignaled backendQueue, int timeoutSeconds = 1, int bufferNodes = 3, int flushJobs = 2, IQueueBackendSignaled buffer = null) : base(backendQueue)
        {
            bufferQueue = buffer ?? new Queue.Concurrent.ConcurrentMemoryQueueBound(collectionNodes: bufferNodes, timeoutSeconds: timeoutSeconds);

            timeout = timeoutSeconds;

            for (int i = 0; i < flushJobs; i++)
            {
                var t = timeoutSeconds;
                flushThreads.Add(Task.Factory.StartNew(() => Flush()));
            }
             
        }

        /// <summary>
        /// pass buffered source to backend
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        protected void Flush()
        {
            while (!bufferQueue.IsEmpty)
            {
                ISource source = bufferQueue.Dequeue();
                base.backend.Enqueue(source);
            }

            backend.CompleteEnqueue();
        }

        /// <summary>
        /// buffer the source to be passed to backend   Alton
        /// </summary>
        /// <param name="source"></param>
        new public void Enqueue(ISource source)
        {
            bufferQueue.Enqueue(source);
        }

        /// <summary>
        /// buffer the source to be passed to backend
        /// </summary>
        /// <param name="sources"></param>
        new public void EnqueueRange(IEnumerable<ISource> sources)
        {
            foreach(var source in sources)
            {
                bufferQueue.Enqueue(source);
            }
        }

        /// <summary>
        /// signal no more incoming sources
        /// </summary>
        new public void CompleteEnqueue()
        {
            bufferQueue.CompleteEnqueue();
        }

        /// <summary>
        /// make sure the buffer gets disposed too
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            bufferQueue.Dispose();
            base.Dispose(disposing);
        }
    }
}
