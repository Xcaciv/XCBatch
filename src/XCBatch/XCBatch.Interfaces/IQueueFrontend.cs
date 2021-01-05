using System.Collections.Generic;

namespace XCBatch.Interfaces
{
    /// <summary>
    /// minimum queue and dispatch facade
    /// </summary>
    public interface IQueueFrontend : System.IDisposable
    {
        /// <summary>
        /// source that did not have a processor during dispatch
        /// </summary>
        IEnumerable<ISource> DeadLetters { get; }

        /// <summary>
        /// allow collecting of un-handled source
        /// </summary>
        bool EnableDeadLetter { get; set; }

        /// <summary>
        /// process results that were not a success (extend Success class)
        /// </summary>
        IEnumerable<IProcessResultState> Unsuccessful { get; }

        /// <summary>
        /// process queue using registered processors
        /// </summary>
        void Dispatch();

        /// <summary>
        /// find and use the appropriate processor
        /// </summary>
        /// <param name="source"></param>
        void Dispatch(ISource source);

        /// <summary>
        /// add several source at once
        /// </summary>
        /// <param name="source"></param>
        void EnqueueRange(IEnumerable<ISource> source);
        /// <summary>
        /// add source to the queue to be processed
        /// </summary>
        /// <param name="source"></param>
        void Enqueue(ISource source);

        /// <summary>
        /// add a processor to handle source from the queue
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
        bool RegisterProcessor(IProcessor<ISource> processor);
    }
}