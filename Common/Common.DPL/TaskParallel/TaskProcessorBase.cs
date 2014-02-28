using System;
using Common.DPL.ThreadParallel;

namespace Common.DPL.TaskParallel
{
    /*public abstract class TaskProcessorBase<T> : IParallelRunner where T : IEquatable<T>
    {
        protected IPipeline<T> Pipeline;
        private readonly ActionRunner _runner;

        protected TaskProcessorBase(IPipeline<T> pipeline)
        {
            Pipeline = pipeline;
            _runner = new ActionRunner(Process) {ThreadsCount = 1};
        }

        public int ThreadsCount { get; set; }

        protected abstract void Process();

        public void Start()
        {
            _runner.Start();
        }

        public void Stop()
        {
            _runner.Stop();
        }
    }*/
}