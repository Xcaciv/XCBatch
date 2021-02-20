using System.Collections;
using System.Collections.Generic;
using XCBatch.Interfaces;
using XCBatch.Interfaces.Adapters;

namespace XCBatch.Core.Queue
{
    public class Enumerator : IEnumerable<ISource>, IEnumerator<ISource>
    {
        protected IQueueBackend backend;
        protected bool isDistribution;
        protected int distributionId;

        public Enumerator(IQueueBackend queue)
        {
            backend = queue;
        }

        public Enumerator(IQueueBackend queue, int distribution) : this(queue)
        {
            isDistribution = true;
            distributionId = distribution;
            if (!(backend is IQueueBackendDistributed))
            {
                throw new System.ArgumentException("Queue backend must be distributed when passing distribution id.", nameof(queue));
            }
        }

        public ISource Current { get; protected set; }

        object IEnumerator.Current => Current;


        public void Dispose()
        {
            // Queue not disposable
        }

        public IEnumerator<ISource> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            var canDequeue = false;
            
            if (isDistribution)
            {
                canDequeue = (backend is IQueueBackendSignaled signaled) ? !signaled.IsDistributionComplete(distributionId) : !((IQueueBackendDistributed)backend).IsDistributionEmpty(distributionId);
            }
            else
            {
                canDequeue = (backend is IQueueBackendSignaled signaled) ? !signaled.IsComplete : !backend.IsEmpty;
            }

            this.Current = (canDequeue) ? Dequeue() : null;

            return (this.Current != null);
        }

        protected ISource Dequeue()
        {
            return isDistribution ? ((IQueueBackendDistributed)backend).Dequeue(distributionId) : backend.Dequeue();
        }

        public void Reset()
        {
            // only move forward
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
