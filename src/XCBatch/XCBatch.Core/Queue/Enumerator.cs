using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using XCBatch.Interfaces;

namespace XCBatch.Core.Queue
{
    public class Enumerator : IEnumerable<ISource>, IEnumerator<ISource>
    {
        protected IQueueBackend backend;

        public Enumerator(IQueueBackend queue)
        {
            backend = queue;
        }

        public ISource Current => backend.Dequeue();

        object IEnumerator.Current => backend.Dequeue();

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
            // automatic
            return !backend.IsEmpty;
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
