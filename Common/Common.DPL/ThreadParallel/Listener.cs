using System;
using System.Collections.Generic;

namespace Common.DPL.ThreadParallel
{
    public class Listener<T> : ActionRunner where T : IEquatable<T>
    {
        private readonly IPipeline<T> _pipeline;
        private readonly Func<IEnumerable<T>> _loadTask;

        public Listener(IPipeline<T> pipeline, Func<IEnumerable<T>> loadTask, int reloadTimeout)
        {
            ReloadTimeout = reloadTimeout;
            _pipeline = pipeline;
            _loadTask = loadTask;
            ActionFactory = new OneActionFactory(LoadObjects);
        }

        public Listener(IPipeline<T> pipeline, IActionFactory actionFactory, int reloadTimeout)
        {
            ReloadTimeout = reloadTimeout;
            _pipeline = pipeline;
            //_loadTask = loadTask;
            //ActionFactory = actionFactory;
        }

        private void LoadObjects()
        {
            if (_pipeline.WaitEmpty(ReloadTimeout))
                foreach (var item in _loadTask())
                    _pipeline.TryAdd(item);
        }

        public int ReloadTimeout { get; set; }
    }
}