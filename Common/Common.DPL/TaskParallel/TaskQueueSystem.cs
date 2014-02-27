using System;
using System.Collections.Generic;

namespace Common.DPL.TaskParallel
{
    public class TaskQueueSystem<T> : QueueSystem<T> where T : IEquatable<T>
    {
        public TaskQueueSystem(IPipeline<T> pipeline, Func<IEnumerable<T>> loadTask, Action<IBag<T>> task, int reloadTimeout) :
            base(new Listener<T>(pipeline, loadTask, reloadTimeout), new Processor<T>(pipeline, task))
        {

        }
    }
}