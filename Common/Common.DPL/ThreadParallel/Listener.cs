using System;
using System.Collections.Generic;
using Common.ServiceModel;

namespace Common.DPL.ThreadParallel
{
    public sealed class Listener<T> : IParallelRunner
    {
        private readonly IPipeline<T> _pipeline;
        private readonly Func<IEnumerable<T>> _loadTask;
        private readonly ActionRunner _runner;

        private Listener(IPipeline<T> pipeline, int reloadTimeout)
        {
            ReloadTimeout = reloadTimeout;
            _pipeline = pipeline;
        }

        public Listener(IPipeline<T> pipeline, Func<IEnumerable<T>> loadTask, int reloadTimeout, int stopTimeout) :
            this(pipeline, reloadTimeout)
        {
            _runner = new ActionRunner(new OneActionFactory(LoadObjects), stopTimeout);
            _loadTask = loadTask;
        }

        public Listener(IPipeline<T> pipeline, IFactory<Action> actionFactory, int reloadTimeout, int stopTimeout) :
            this(pipeline, reloadTimeout)
        {
            _runner = new ActionRunner(actionFactory, stopTimeout);
        }

        private void LoadObjects()
        {
            if (_pipeline.WaitEmpty(ReloadTimeout))
                foreach (var item in _loadTask())
                    _pipeline.TryAdd(item);
        }

        public int ReloadTimeout { get; set; }

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