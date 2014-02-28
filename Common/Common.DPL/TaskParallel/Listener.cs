using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.DPL.TaskParallel
{
   /* public class Listener<T> : TaskProcessorBase<T> where T : IEquatable<T>
    {
        private readonly Func<IEnumerable<T>> _loadTask;
        private readonly int _reloadTimeout;

        public Listener(IPipeline<T> pipeline, Func<IEnumerable<T>> loadTask, int reloadTimeout)
            : base(pipeline)
        {
            _loadTask = loadTask;
            _reloadTimeout = reloadTimeout;
        }

        protected override void Process()
        {
            if (Pipeline.WaitEmpty(_reloadTimeout))
            {
                Task.Factory.StartNew(obj =>
                    {
                        foreach (var item in _loadTask())
                        {
                            Pipeline.TryAdd(item);
                        }
                    }, null);
            }
        }
    }*/
}