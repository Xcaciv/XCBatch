# Batch 90

## The universal batch/queue interface layer

**Who is this for?** You want to horizontally scale a workload. You want to take advantage of cloud resources where available, but you don't want to have to build different batch applications to leverage these managed cloud queues.

**What does it do?** *Throw down a little Batch 90* and adapt to a variety of running environemnts by using configuration. Flexibly use a database backend, an in-memory queue, or any combination with managed queue backends including Amazon MQ and Azure Service Bus.

## Get Started

Queue some and Do some

    var queueClient = new QueueFrontend(new MemoryQueue());
    
    // specify what to do
    queueClient.RegisterProcessor(new MyProcessor());

    // create something to do it to
    for (int i = 0; i < 10000; ++i)
    {
        myQueue.Enqueue(new MySource() { SubjectId = i });
    }

    // do it
    myQueue.Dispatch();

In this example, you need to create MyProcessor to specify what to do with the source. In this example the source is a series of integers. The most efficient implementation woud use integers representing object ids or database row ids. Once you define the data queued you need to specify how to handle that type of source:

    // Implement processor class
    public class MyProcessor
        : IProcessor<MySource>
    {
        public MyExistingClass myJob { get; set; }

        public Type SourceType => typeof(MySource);

        public IProcessResultState ExpectedStatus { get; set; }

        public IProcessResultState Process(ISource source)
        {
            Console.WriteLine($"Recieved {source.SubjectId}");
            return new ProcessResultState.Success("Winning!");
        }
    }

    // simplest implementation of source
    public class MySource : Source {}

Queue and process in seperate threads:

    // Configure your queue with a factory, IoC or what ever you want
    // *note: You can use the built in Concurrent queue or build your own thread 
    // safe queue adapter by implementing IQueueBackend.
    public void DoWorkThreaded(IQueueFrontend queueClient)
    {
        Task producerThread = Task.Factory.StartNew(() =>
        {
            for (int i = 0; i < 10000; ++i)
            {
                myQueue.Enqueue(new MySource() { SubjectId = i });
            }
            myQueue.CompleteEnqueue(); // tell other threads where the end is
        });

        Task consumerThread = Task.Factory.StartNew(() =>
        {
            myQueue.Dispatch();
        });

        Task.WaitAll(producerThread, consumerThread);
    }

## Change to an Azure Service Bus

// TODO: Code example

## Block Queue

Are your jobs faster using a warmed-up context? Group like jobs into blocks of work:

// TODO: Code example

Also, consider an advanced scenario where in you need to process larg files and take individual actions on each record in the file. You place the file to be processed in the Azure Storage Queue so that if the worker is interrupted, another can take over. Each of the resulting records is placed in a database and queued to be processed via the Azure Service Bus Queue:

// TODO: Code example

> **note:** Generally speaking a queue is not meant to store data, it is meant to ensure the processing of data.  Therefore, Batch 90 queues source information (that is what information to process) and not data. Creating any sort of complex source objects is strongly discouraged as it will degrade this frameworks performance and efficiency.

### TODO

- buffered queue
- WCF distributed queue client
- Azure Storage Queue
- Azure Message Queue
- RabbitMQ
- Redis
- Amazon MQ
- other

### FAQ

Why not use `IProducerConsumerCollection<T>`?

To keep things a bit simpler. Implementing Enqueue and Dequeue (with overloads) is more straight forward.
