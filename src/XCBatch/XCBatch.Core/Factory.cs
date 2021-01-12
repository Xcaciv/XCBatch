namespace XCBatch.Core
{
    public static class Factory
    {
        public static QueueFrontend GetBasicQueueInstance()
        {
            return new QueueFrontend(new MemoryQueue());
        }

        public static ParallelQueueFrontend GetParallelQueueInstance()
        {
            return new ParallelQueueFrontend(new Queue.Concurrent.ConcurrentMemoryQueue());
        }

        public static BufferedQueueFrontend GetBufferedQueueInstance(int buffers = 3, int timeoutSeconds = 1)
        {
            return new BufferedQueueFrontend(new Queue.Concurrent.ConcurrentMemoryQueue(buffers, timeoutSeconds), timeoutSeconds, buffers);
        }

        public static BufferedQueueFrontend GetBufferedQueueBoundInstance(int boundry, int buffers = 3, int timeoutSeconds = 1)
        {
            return new BufferedQueueFrontend(new Queue.Concurrent.ConcurrentMemoryQueueBound(boundry, buffers, timeoutSeconds), timeoutSeconds, buffers);
        }
    }
}
