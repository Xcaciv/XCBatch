using Xunit;

namespace XCBatch.Core.UnitTests
{
    public class BufferedQueueFrontendTests
    {
        [Fact()]
        public void BufferedQueueFrontend_WithMany_Finishes()
        {
            using (var frontend = Factory.GetBufferedQueueBoundInstance(10))
            {
                frontend.EnableSaveSuccess = true;

                Scenarios.Queue.EnqueueManyProcessParallel(frontend);

                var notExpected = System.Threading.Thread.CurrentThread.ManagedThreadId;

                // test that stuff happed in another thread
                Assert.Contains(frontend.Successful, o => ((Implementations.ThreadSuccessStatus)o).ThreadId != notExpected);
            }
        }
    }
}