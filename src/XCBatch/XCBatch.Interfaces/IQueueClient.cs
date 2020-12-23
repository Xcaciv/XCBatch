﻿using System.Collections.Generic;

namespace XCBatch.Interfaces
{
    public interface IQueueClient
    {
        List<ISource> DeadLetters { get; }
        bool EnableDeadLetter { get; set; }
        List<IProcessResultState> Unsuccessful { get; }

        int Dispatch();
        int Dispatch(IList<IProcessor<ISource>> processors);
        int Enqueue(IEnumerable<ISource> source);
        int Enqueue(ISource source);
        bool RegisterProcessor(IProcessor<ISource> processor);
    }
}