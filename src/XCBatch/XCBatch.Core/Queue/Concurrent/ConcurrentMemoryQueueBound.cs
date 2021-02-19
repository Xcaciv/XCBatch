using System.Collections.Concurrent;
using System.Collections.Generic;
using XCBatch.Interfaces;

namespace XCBatch.Core.Queue.Concurrent
{
    /// <summary>
    /// thread safe queue that blocks on an upper-bound limit
    /// </summary>
    /// <remarks>
    /// <para>The total limit is calculated upperNodeBoundry x collectionNodes. The
    /// default is 30,000 because 10,000 per node on 3 nodes.</para>
    /// </remarks>
    public class ConcurrentMemoryQueueBound : ConcurrentMemoryQueue
    {
        protected readonly int upperBoundry;

        public ConcurrentMemoryQueueBound(int upperNodeBoundry = 10000, int collectionNodes = 3, int timeoutSeconds = 1)
            :base(collectionNodes, timeoutSeconds)
        {
            upperBoundry = upperNodeBoundry;
        }

        new protected BlockingCollection<ISource>[] BuildCollectionNodes(int collectionNodeCount, ISource source = null)
        {
            var nodes = new List<BlockingCollection<ISource>>();
            for (int i = 0; i < collectionNodeCount; i++)
            {
                nodes.Add(new BlockingCollection<ISource>(upperBoundry));
            }

            if (source != null) nodes[0].Add(source);

            return nodes.ToArray();
        }
    }
}
