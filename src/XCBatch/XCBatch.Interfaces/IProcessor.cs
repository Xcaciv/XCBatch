
namespace XCBatch.Interfaces
{
    /// <summary>
    /// use a dequeued source
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProcessor<out T> where T : class, ISource
    {
        /// <summary>
        /// Used on a dequeued source
        /// Given the source type, the processor must know how to retrieve any data to be 
        /// processed and any dependent data in addition to how it should be handled.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>The implementation must implicitly share knowledge of what is required to
        /// process the source.</para>
        /// 
        /// <para>This method may be called with several different source objects during
        /// the life time of the Processor.</para>
        /// 
        /// <para>This class should be stateless but can take advantage of being warmed up 
        /// before being called to process several source.</para>
        /// </remarks>
        /// <param name="source"></param>
        /// <returns></returns>
        IProcessResultState Process(ISource source);

        /// <summary>
        /// target type to be processed
        /// </summary>
        /// 
        /// <remarks>
        /// <para>This should not use introspection as it would impact performance.</para>
        /// </remarks>
        System.Type SourceType { get; }
    }
}
