namespace XCBatch.Core
{
    public static class Factory
    {
        public static QueueClientLight GetBasicQueueInstance()
        {
            return new QueueClientLight(new MemoryQueue());
        }

        public static QueueClientLight GetConcurrentQueueInstance()
        {
            return new QueueClientLight(new ConcurrentMemoryQueue());
        }
    }
}
