using System;
using System.Collections.Generic;
using System.Text;

namespace XCBatch.Interfaces
{
    public interface IProcessorFactory<out T> where T : class, IProcessor<ISource>
    {
        /// <summary>
        /// factory
        /// </summary>
        /// <returns></returns>
        T CreateProcessor();

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
