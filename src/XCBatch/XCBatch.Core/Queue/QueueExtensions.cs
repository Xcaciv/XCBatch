using System;
using System.Collections.Generic;
using System.Text;
using XCBatch.Interfaces;
using XCBatch.Interfaces.Adapters;

namespace XCBatch.Core.Queue
{
    public static class QueueExtensions
    {
        public static IEnumerator<ISource> GetEnumerator(this IQueueBackend queue)
        {
            return new Enumerator(queue);
        }
    }
}
