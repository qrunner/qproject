using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.ServiceModel;

namespace Common.DPL.ThreadParallel
{
    /// <summary>
    /// Выполняет заданное действие циклически в нескольких потоках
    /// </summary>
    public class ActionRunner : IParallelRunner, IDisposable
    {
        private struct ThreadInfo : IRunner
        {
            public ThreadInfo(ParameterizedThreadStart threadStart, Action action)
            {
                Thread = new Thread(threadStart);
                Run = true;
                Action = action;
            }

            public readonly Action Action;
            public readonly Thread Thread;
            internal volatile bool Run;

            public void Stop()
            {
                Run = false;
            }

            public void Start()
            {
                Thread.Start(this);
            }
        }

        protected class OneActionFactory : IFactory<Action>
        {
            private readonly Action _action;

            public OneActionFactory(Action action)
            {
                _action = action;
            }

            public Action Create()
            {
                return _action;
            }
        }

        protected IFactory<Action> ActionFactory;
        private bool _started = false;
        private readonly List<ThreadInfo> _threads = new List<ThreadInfo>();

        protected ActionRunner()
        {

        }

        public ActionRunner(Action action)
        {
            ActionFactory = new OneActionFactory(action);
        }

        public ActionRunner(IFactory<Action> actionFactory)
        {
            ActionFactory = actionFactory;
        }

        private static void MainLoop(object threadInfo)
        {
            ThreadInfo ti = (ThreadInfo)threadInfo;
            while (ti.Run)
                ti.Action();
        }

        public void Start()
        {
            foreach (var ti in _threads.Where(ti => ti.Thread.ThreadState == ThreadState.Unstarted))
                ti.Start();
            _started = true;
        }

        public void Stop()
        {
            Parallelizm = 0;
            _started = false;
        }

        public void Dispose()
        {
            Stop();
        }

        private void AddThread()
        {
            ThreadInfo ti = new ThreadInfo(MainLoop, ActionFactory.Create());
            _threads.Add(ti);
            if (_started)
                ti.Start();
        }

        private void RemoveThread()
        {
            if (_threads.Count == 0) return;
            
            _threads[0].Thread.Join();
            if (_threads[0].Thread.IsAlive)
                _threads[0].Thread.Abort();
            _threads.RemoveAt(0);
        }

        public int Parallelizm
        {
            get { return _threads.Count; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value");
                int delta = value - _threads.Count;
                if (delta == 0) return;
                if (delta > 0)
                {
                    for (int i = 0; i < delta; i++)
                        AddThread();
                }
                else
                {
                    for (int i = 0; i < delta; i++)
                        _threads[i].Stop();

                    for (int i = 0; i < delta; i++)
                        RemoveThread();
                }
            }
        }
    }
}