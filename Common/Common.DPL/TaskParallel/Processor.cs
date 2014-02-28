using System;

namespace Common.DPL.TaskParallel
{
   /* public class Processor<T> : TaskProcessorBase<T> where T : IEquatable<T>
    {
        private readonly Action<IBag<T>> _task;

        public Processor(IPipeline<T> pipeline, Action<IBag<T>> task) : base(pipeline)
        {
            _task = task;
        }

        protected override void Process()
        {
            var item = Pipeline.TakeForProcess();
            System.Threading.Tasks.Task.Factory.StartNew(obj => _task(item), null);
        }
    }*/
}