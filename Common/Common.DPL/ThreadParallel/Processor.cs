using System;

namespace Common.DPL.ThreadParallel
{
    public sealed class Processor<T> : IParallelRunner
    {
        private readonly IPipeline<T> _pipeline;
        private readonly Action<IBag<T>> _task;
        private readonly ActionRunner _runner;

        public Processor(IPipeline<T> pipeline, Action<IBag<T>> task, int stopTimeout)
        {
            _pipeline = pipeline;
            _task = task;
            _runner = new ActionRunner(Process, stopTimeout);
        }

        private void Process()
        {
            var item = _pipeline.TakeForProcess();

            _task(item);

            if (item.State == ObjectState.InProcess)
                item.Commit();
        }

        public void Start()
        {
            _runner.Start();
        }

        public void Stop()
        {
            _runner.Stop();
        }

        public int ThreadsCount
        {
            get { return _runner.ThreadsCount; }
            set { _runner.ThreadsCount = value; }
        }

        public void Start(int threadsCount)
        {
            _runner.Start(threadsCount);
        }
    }
}