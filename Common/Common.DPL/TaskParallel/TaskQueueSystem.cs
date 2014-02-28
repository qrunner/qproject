using System;
using System.Collections.Generic;
using Common.DPL.ThreadParallel;

namespace Common.DPL.TaskParallel
{
    /*public class TaskQueueSystem<T> : QueueSystem<T> where T : IEquatable<T>
    {
        public TaskQueueSystem(IPipeline<T> pipeline, Func<IEnumerable<T>> loadTask, Action<IBag<T>> task, int reloadTimeout, int stopTimeout) :
            base(new Listener<T>(pipeline, loadTask, reloadTimeout, stopTimeout), new Processor<T>(pipeline, task, stopTimeout))
        {

        }
    }*/
}