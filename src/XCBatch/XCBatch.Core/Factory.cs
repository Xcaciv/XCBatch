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
    }
}
