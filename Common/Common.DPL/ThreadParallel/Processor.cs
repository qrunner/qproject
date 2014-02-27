using System;

namespace Common.DPL.ThreadParallel
{
    public class Processor<T> : ActionRunner where T : IEquatable<T>
    {
        private readonly IPipeline<T> _pipeline;
        private readonly Action<IBag<T>> _task;

        public Processor(IPipeline<T> pipeline, Action<IBag<T>> task)
        {
            _pipeline = pipeline;
            _task = task;
            ActionFactory = new OneActionFactory(Process);
        }

        private void Process()
        {
            var item = _pipeline.TakeForProcess();

            _task(item);

            if (item.State == ObjectState.InProcess)
                item.Commit();
        }
    }
}