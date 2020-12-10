using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XCBatch.Interfaces
{
    public interface IProcessor<T> where T : ISource
    {
        IState Process(T source);
    }
}
