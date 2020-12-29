# Batch 90

## The universal batch/queue interface layer

You want to horizontally scale a workload. You want to do it the same way, no matter where you run while still taking advantage of available resources like managed cloud message queues or batch services.

*Dabb a little Batch 90* in your code and adapt to a variety of platforms including Amazon MQ and Azure Service Bus.

Start with a memory queue:

// TODO: Code example

Change to an Azure Service Bus:

// TODO: Code example

Are your jobs faster using a warmed-up context? Group like jobs into blocks of work:

// TODO: Code example

Also, consider an advanced scenario where in you need to process larg files and take individual actions on each record in the file. You place the file to be processed in the Azure Storage Queue so that if the worker is interrupted, another can take over. Each of the resulting records is placed in a database and queued to be processed via the Azure Service Bus Queue:

// TODO: Code example
