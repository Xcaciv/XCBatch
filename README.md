# Batch 90

## The universal batch/queue interface layer

**Who is this for?** You want to horizontally scale a workload. You want to take advantage of cloud resources where available, but you don't want to have to build different batch applications to leverage these managed cloud queues.

**What does it do?** *Throw down a little Batch 90* and adapt to a variety of running environemnts by using configuration. Flexibly use a database backend, an in-memory queue, or any combination with managed queue backends including Amazon MQ and Azure Service Bus.

## Get Started

### Queue some and Do some

The basics are to do work in an organized manner and take advantage of one of the basic implementations.

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

### Do what and queue what

In the above example, you should wonder what `MyProcessor` and `MySource` are.

    // IProcessor Implementation
    public class MyProcessor : IProcessor<MySource>
    {
        // Do what? ...
        public IProcessResultState Process(ISource source)
        {
            // output each integer
            // we could get more complex data externally
            Console.WriteLine($"Recieved {source.SubjectId}");
            return new ProcessResultState.Success("Winning!");
        }

        // ... to what?
        public Type SourceType => typeof(MySource);
    }

    // ISource implementation extending the Source abstract
    public class MySource : Source {}

Counter to how you may be used to viewing jobs, Batch 90 asks you to answer the question **"Do what?"** first. Here this is answered with `MyProcessor`, a IProcessor implementation. By asking the 'Do' question first, we create a purposeful relationship between the item being put into the queue and the how it is used, reducing to the smallest amout of information needed: an id.

`MySource` is the answer to **"Queue what?"**. In Batch 90 we do not use the queue as persistance, especially in memory. Thus, in this example we are showing the most basic queue information, which is an integer id. The purpose is to communicate with the queue quickly so as to parallelize the work and scale horizontally. Processes will usually need more information than an integer to run. Becase of this, the process should posess the knoledge and ability to connect to and read the data attached to the id.

The id can refer to an object in a cache, document database, relational database or it could be a key in a dictionary. The point is for the queue to add as little load or delay on the process as possible.

## THREADS‼

So far you may be thnking 'That too much ritual when I could just use `System.Collections.Queue`.' If you are: bear with it, we are getting there.

The above implementation could be in a web app or stateless actor or service... somewhere you are doing something simple in a single thread. Once we convert to processing workloads in this way there are several opertunities for scaling. The first is to scale vertically via threads. Check out this example of queuing and processing in seperate threads:

    public void DoWorkThreaded(IQueueFrontendSignaled queueClient)
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

The first thing you should note is the parameter injection. Because Batch 90 makes extensive use of interfaces, it is easy to configure and change backends when you are ready. You can even use your favorite IoC framework.

The frontend in use here is a 'signaled' frontend. This means that a signal is given, in a thread safe manner, specifying when queuing is complete. In the default implementation this means that `Dispatch()` keeps running untill this signal is given and all items in the queue are processed or sorted as a dead letter.

In the example we use Tasks instead of manual threads since they are more portable and predictable across running environments. The tasks are placed in the thread pool and run asycronously to each other until the signal is given joining them back to the main thread via a call to `WaitAll()`.

## Spread it out horizontally

Horizontal scale is core to the cloud value propisition. Horizontal scale not only increases overall throughput but can also be used for higher falt tollerance through redundancy. If you are doing large batch processing you are probably looking to scale horizontally or you already do it. Batch 90 is here to ease you transition

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
