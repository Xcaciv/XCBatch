using System;
using System.Collections.Generic;
using XCBatch.Interfaces;
using XCBatch.Interfaces.Adapters;

namespace XCBatch.Core
{
    /// <summary>
    /// mediator between queue back-end and processor
    /// </summary>
    /// 
    /// <remarks>
    /// <para>CAUTION: NOT THREAD SAFE</para>
    /// <para>Use this class with IoC Container or use the factory for a memory queue.</para>
    /// </remarks>
    public class QueueFrontend : IQueueFrontend
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

        protected List<IProcessResultState> unsuccessful = new List<IProcessResultState>();

        /// <summary>
        /// collection of states that were unsuccessful
        /// </summary>
        public IEnumerable<IProcessResultState> Unsuccessful {
            get => unsuccessful;
        } 

        /// <summary>
        /// specify if a source that does not have a processor should be put in the dead letter queue
        /// </summary>
        public bool EnableDeadLetter { get; set; } = false;

        protected List<ISource> deadletters = new List<ISource>();

        /// <summary>
        /// list of source that did not have a processor during dispatch
        /// </summary>
        public IEnumerable<ISource> DeadLetters { get => deadletters; } 

        /// <summary>
        /// determines if the queue has had an item removed
        /// </summary>
        protected bool hasDequeueStarted = false;

        /// <summary>
        /// constructor requiring a back-end
        /// </summary>
        /// <param name="queue"></param>
        public QueueFrontend(IQueueBackend queue) => this.backend = queue;

        /// <summary>
        /// add item to back-end
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public void Enqueue(ISource source)
        {
            this.backend.Enqueue(source);
        }

        /// <summary>
        /// add list of items to back-end
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public void EnqueueRange(IEnumerable<ISource> source)
        {
            this.backend.EnqueueRange(source);
        }

        /// <summary>
        /// process the queue with registered processor instances
        /// </summary>
        /// <returns>number successfully processed</returns>
        public void Dispatch()
        {
            ISource source;
            while ((source = this.backend.Dequeue()) != null)
            {
                Dispatch(source);
            }
        }

        /// <summary>
        /// use the registered processor on a source
        /// </summary>
        /// <param name="source"></param>
        public void Dispatch(ISource source)
        {
            try
            {
                var sourceType = source.GetType();
                if (processors.ContainsKey(sourceType))
                {
                    var resultState = processors[sourceType].Process(source);

                    if (resultState is ProcessResultState.Success)
                    {
                        if (resultState is IProcessResultRequeueable)
                        {
                            OnRequeueable(resultState);
                        }
                        this.OnSuccess(resultState, source);
                        
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
            catch (Exception ex)
            {
                this.OnUnsuccessful(new ProcessResultState.Error("Exception while processing source.") { Ex = ex }, source);
            }
        }

        /// <summary>
        /// handle re-queue
        /// </summary>
        /// <param name="resultState"></param>
        protected void OnRequeueable(IProcessResultState resultState)
        {
            var requeueable = resultState as IProcessResultRequeueable;
            this.EnqueueRange(requeueable.GetQueueableResults());
        }

        /// <summary>
        /// simple success handler
        /// </summary>
        /// <param name="resultState"></param>
        protected virtual void OnSuccess(IProcessResultState resultState, ISource source)
        {
            // do nothing, just enjoy it
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

            this.unsuccessful.Add(resultState);
        }

        /// <summary>
        /// simple dead letter handler
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tempForce">allow add temporarily</param>
        protected virtual void OnDeadLetter(ISource source)
        {
            if (this.EnableDeadLetter) deadletters.Add(source);
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

        /// <summary>
        /// To detect redundant calls
        /// </summary>
        protected bool disposed = false;

        /// <summary>
        /// cleaning up
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                backend.Dispose();
            }

            disposed = true;
        }
    }
}
