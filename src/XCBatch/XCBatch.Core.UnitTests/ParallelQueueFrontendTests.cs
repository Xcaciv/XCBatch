using Xunit;
using XCBatch.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace XCBatch.Core.UnitTests
{
    public class ParallelQueueFrontendTests
    {
        [Fact()]
        public void Enqueue_WithMany_ResultsInThreads()
        {
            var queueClient = Core.Factory.GetParallelQueueInstance();
            queueClient.EnableSaveSuccess = true;

            var dispatchedClient = Scenarios.Queue.DispatchManyLong(queueClient);

            var notExpected = System.Threading.Thread.CurrentThread.ManagedThreadId;

            var threadIds = queueClient.Successful.Select(o => ((Implementations.ThreadSuccessStatus)o).ThreadId).Distinct().ToList();

            // test that stuff happed in another thread
            Assert.Contains(queueClient.Successful, o => ((Implementations.ThreadSuccessStatus)o).ThreadId != notExpected);
        }
    }
}