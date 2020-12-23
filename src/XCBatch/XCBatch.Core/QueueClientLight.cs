using System;
using System.Collections.Generic;
using XCBatch.Interfaces;

namespace XCBatch.Core
{
    /// <summary>
    /// mediator between queue back-end and processor
    /// </summary>
    /// 
    /// <remarks>
    /// <para>This implementation </para>
    /// <para>Use this class with IoC Container or use the factory for a memory queue.</para>
    /// </remarks>
    public class QueueClientLight : IQueueClient
    {
        /// <summary>
        /// queue instance
        /// </summary>
        protected IQueueBackend backend { get; set; }

        /// <summary>
        /// list of processor instances indexed by source type
        /// </summary>
        protected Dictionary<Type, IProcessor<ISource>> processors = new Dictionary<Type, IProcessor<ISource>>();

        /// <summary>
        /// indicates that the queue processing has started
        /// </summary>
        protected bool HasDequeueStarted { get => hasDequeueStarted; }

        /// <summary>
        /// collection of states that were unsuccessful
        /// </summary>
        public List<IProcessResultState> Unsuccessful { get; private set; } = new List<IProcessResultState>();

        /// <summary>
        /// specify if a source that does not have a processor should be put in the dead letter queue
        /// </summary>
        public bool EnableDeadLetter { get; set; } = false;

        /// <summary>
        /// list of source that did not have a processor during dispatch
        /// </summary>
        public List<ISource> DeadLetters { get; private set; } = new List<ISource>();

        /// <summary>
        /// determines if the queue has had an item removed
        /// </summary>
        protected bool hasDequeueStarted = false;

        /// <summary>
        /// constructor requiring a back-end
        /// </summary>
        /// <param name="queue"></param>
        public QueueClientLight(IQueueBackend queue) => backend = queue;

        /// <summary>
        /// add item to back-end
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public int Enqueue(ISource source)
        {
            return backend.Enqueue(source);
        }

        /// <summary>
        /// add list of items to back-end
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public int Enqueue(IEnumerable<ISource> source)
        {
            return backend.Enqueue(source);
        }

        /// <summary>
        /// process the queue adding the given list of processor instances to the registry
        /// </summary>
        /// <param name="processors"></param>
        /// <returns>number of sources processed</returns>
        public int Dispatch(IList<IProcessor<ISource>> processors)
        {
            foreach (var processor in processors)
            {
                this.RegisterProcessor(processor);
            }

            return this.Dispatch();
        }

        /// <summary>
        /// process the queue with registered processor instances
        /// </summary>
        /// <returns>number successfully processed</returns>
        public int Dispatch()
        {
            ISource source;
            var successCount = 0;
            while ((source = backend.Dequeue()) != null)
            {
                var sourceType = source.GetType();
                if (processors.ContainsKey(sourceType))
                {
                    var resultState = processors[sourceType].Process(source);
                    if (resultState is IStateRequeueable)
                    {
                        var requeueable = resultState as IStateRequeueable;
                        this.Enqueue(requeueable.GetQueueableResults());
                    }

                    if (resultState is ProcessResultState.Success)
                    {
                        this.OnSuccess(resultState, source);
                        successCount++;
                    }
                    else
                    {

                        this.OnUnsuccessful(resultState, source);
                    }
                }
                else
                {
                    this.OnDeadLetter(source);
                }
            }

            return successCount;
        }

        /// <summary>
        /// simple success handler
        /// </summary>
        /// <param name="resultState"></param>
        protected virtual void OnSuccess(IProcessResultState resultState, ISource source)
        {
            // do nothing
        }

        /// <summary>
        /// simple unsuccessful/error state handler
        /// </summary>
        /// <param name="resultState"></param>
        /// <param name="source"></param>
        protected virtual void OnUnsuccessful(IProcessResultState resultState, ISource source)
        {
            var errorState = resultState as ProcessResultState.Error;
            if (errorState != null && errorState.Source == null) errorState.Source = source;

            this.Unsuccessful.Add(resultState);
        }

        /// <summary>
        /// simple dead letter handler
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tempForce">allow add temporarily</param>
        protected virtual void OnDeadLetter(ISource source)
        {
            if (this.EnableDeadLetter) DeadLetters.Add(source);
        }

        /// <summary>
        /// register a processor instance
        /// </summary>
        /// <param name="processor"></param>
        /// <returns>true if a previous processor instance was replaced</returns>
        public bool RegisterProcessor(IProcessor<ISource> processor)
        {
            var replaced = this.processors.ContainsKey(processor.SourceType);
            this.processors[processor.SourceType] = processor;
            return replaced;
        }
    }
}
