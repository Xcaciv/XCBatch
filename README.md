# Batch 90

## The universal batch/queue interface layer

**Who is this for?** You want to horizontally scale a workload. You want to take advantage of cloud resources where available, but you don't want to have to build different batch applications to leverage these managed cloud queues.

**What does it do?** *Throw down a little Batch 90* and adapt to a variety of running environments by using configuration. Flexibly use a database backend, an in-memory queue, or any combination with managed queue backend including Amazon MQ and Azure Service Bus.

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
            Console.WriteLine($"Received {source.SubjectId}");
            return new ProcessResultState.Success("Winning!");
        }

        // ... to what?
        public Type SourceType => typeof(MySource);
    }

    // ISource implementation extending the Source abstract
    public class MySource : Source {}

Counter to how you may be used to viewing jobs, Batch 90 asks you to answer the question **"Do what?"** first. Here this is answered with `MyProcessor`, a IProcessor implementation. By asking the 'Do' question first, we create a purposeful relationship between the item being put into the queue and the how it is used, reducing to the smallest amount of information needed: an id.

`MySource` is the answer to **"Queue what?"**. In Batch 90 we do not use the queue as persistance, especially in memory. Thus, in this example we are showing the most basic queue information, which is an integer id. The purpose is to communicate with the queue quickly so as to parallelize the work and scale horizontally. Processes will usually need more information than an integer to run. Because of this, the process should posses the knowledge and ability to connect to and read the data attached to the id.

The id can refer to an object in a cache, document database, relational database or it could be a key in a dictionary. The point is for the queue to add as little load or delay on the process as possible.

## THREADS‼

So far you may be thanking 'That too much ritual when I could just use `System.Collections.Queue`.' If you are: bear with it, we are getting there.

The above implementation could be in a web app or stateless actor or service... somewhere you are doing something simple in a single thread. Once we convert to processing workloads in this way there are several opportunities for scaling. The first is to scale vertically via threads. Check out this example of queuing and processing in separate threads:

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

The first thing you should note is the parameter injection. Because Batch 90 makes extensive use of interfaces, it is easy to configure and change the backend when you are ready. You can even use your favorite IoC framework.

The frontend in use here is a 'signaled' frontend. This means that a signal is given, in a thread safe manner, specifying when queuing is complete. In the default implementation this means that `Dispatch()` keeps running until this signal is given and all items in the queue are processed or sorted as a dead letter.

In the example we use Tasks instead of manual threads since they are more portable and predictable across running environments. The tasks are placed in the thread pool and run asynchronously to each other until the signal is given joining them back to the main thread via a call to `WaitAll()`.

## Spread it out horizontally

Horizontal scale is core to the cloud value proposition. Horizontal scale not only increases overall throughput but can also be used for higher fault tolerance through redundancy. If you are doing large batch processing you are probably looking to scale horizontally if you are not already doing it. Batch 90 is here to ease you transition and open up additional backend possibilities.

The concept is basic: unlimited number of producers and consumers on any number of running environments. The number of each depends on the cost of the operation and this flexibility allows you to create highly optimized process chains that can self adjust based on current load. The the trade-off is that there is delay added in queueing such that very small operations will take more time even though you will be able to handle many more of these operations at one time.

Implementation looks very similar to threaded queueing except the queue itself needs to be centrally reachable by all processes. This does not mean that the queue itself cannot be horizontally scalable, it just means that the queue must reside behind a single URI or be known by a shared name.

**Application one:**

    public void AddWork(IQueueFrontendSignaled queueClient)
    {
        for (int i = 0; i < 10000; ++i)
        {
            myQueue.Enqueue(new MySource() { SubjectId = i });
        }
        myQueue.CompleteEnqueue(); // tell other threads where the end is
    }

**Application two:**

    public void DoWork(IQueueFrontendSignaled queueClient)
    {
        myQueue.Dispatch();
    }

The two applications must share `ISource` types and configure their queue backends to a shared communication chanel. This chanel can be a wide variety of things with a wide variety of availability and performance. A simple concept to understand would be a relational database. A slightly more complex and costly example could be an Enterprise Service Bus. It is also possible to use Batch 90 itself as this chanel by exposing it's API over the network using number of methods like REST or an RPC framework like WCF.

### Optimize Distribution

Once you have distributed your workload, optimization could then look at taking advantage of cache and persistance in particular processor types.  At it's most fundamental level Batch 90 facilitates this via its favorite data type: Int32. Giving your source an id for distributing jobs independent of source type allows you, the programmer, flexibility. If you or your queue backend prefers friendly names, simply associate a number to the name. Alternatively share an enum between your producers and consumers.

To use Distribution Ids, simply set them on your source and request them on your dispatch while using a Frontend that supports it:

**Shared enum:**

    public enum MyDistribution
    {
        LongRunningDistribution,
        SmallJobDistribution,
        HotJobsDistribution
    }

**Application one:**

    public void AddWork(IQueueFrontendSignaled queueClient)
    {
        for (int i = 0; i < 10000; ++i)
        {
            myQueue.Enqueue(new MySource() 
            { 
                SubjectId = i,
                DistributionId = MyDistribution.SmallJobDistribution
            });
        }
        myQueue.CompleteEnqueue(); // tell other threads where the end is
    }

**Application two:**

    public void DoWork(IQueueFrontendSignaled queueClient)
    {
        myQueue.Dispatch(MyDistribution.SmallJobDistribution);
    }

## Change to an Azure Service Bus

Building on the above example, using Azure Service Bus is as easy as implementing a IQueueBackend to send source types to to a queue.

    var queueClient = new ParallelQueueFrontend(new XcAzureQueue(myConfig));

## Block Queue

Are your jobs faster using a warmed-up context? Group like jobs into blocks of work:

    // TODO: Code example

## Processor Result Requeuing

Also, consider an advanced scenario where in you need to process large files and take individual actions on each record in the file. You place the file to be processed in the Azure Storage Queue so that if the worker is interrupted, another can take over. Each of the resulting records is placed in a database and queued to be processed via the Azure Service Bus Queue:

    // TODO: Code example

> **note:** It is important to note that generally speaking a queue is not meant to store data. It is meant to ensure the processing of data.  Therefore, Batch 90 queues source information and not object trees. Creating any sort of complex source objects is strongly discouraged as it will degrade this frameworks performance and efficiency. String parsing/serialization source data is also discouraged for this reason.

### TODO

- Implement Processor factory
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
