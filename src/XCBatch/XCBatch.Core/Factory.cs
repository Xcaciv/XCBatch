namespace XCBatch.Core
{
    public class Factory
    {
        public static QueueClientLight GetBasicQueueInstance()
        {
            return new QueueClientLight(new MemoryQueue());
        }
    }
}
