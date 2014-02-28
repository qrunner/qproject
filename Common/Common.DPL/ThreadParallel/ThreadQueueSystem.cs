using System;
using System.Collections.Generic;

namespace Common.DPL.ThreadParallel
{
    public class ThreadQueueSystem<T> : QueueSystem<T> where T : IEquatable<T>
    {
        public ThreadQueueSystem(IPipeline<T> pipeline, Func<IEnumerable<T>> loadTask, Action<IBag<T>> task, int reloadTimeout, int stopTimeout) :
            base(new Listener<T>(pipeline, loadTask, reloadTimeout, stopTimeout), new Processor<T>(pipeline, task, stopTimeout))
        {

        }
    }
}