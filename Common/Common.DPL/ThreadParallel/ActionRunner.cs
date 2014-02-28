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
    public sealed class ActionRunner : IParallelRunner, IDisposable
    {
        private class ThreadInfo : IRunner
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
                // Loop until the worker thread activates.
                while (!Thread.IsAlive)
                {
                    
                }
            }
        }

        private readonly IFactory<Action> _actionFactory;
        private readonly int _stopTimeout;
        private bool _started = false;
        private readonly List<ThreadInfo> _threadInfos = new List<ThreadInfo>();

        /// <summary>
        /// Создает новый экземпляр
        /// </summary>
        /// <param name="action">Действие. Все исключения должны обрабатываться внутри!</param>
        /// <param name="stopTimeout">Таймаут при остановке потоков. Для бесконечного ожидания можно передать Timeout.Infinite</param>
        public ActionRunner(Action action, int stopTimeout) : this(new OneActionFactory(action), stopTimeout)
        {
        }

        /// <summary>
        /// Создает новый экземпляр
        /// </summary>
        /// <param name="actionFactory">Фабрика задач. Используется при создания задачи для потока.</param>
        /// <param name="stopTimeout">Таймаут при остановке потоков. Для бесконечного ожидания можно передать Timeout.Infinite</param>
        public ActionRunner(IFactory<Action> actionFactory, int stopTimeout)
        {
            _actionFactory = actionFactory;
            _stopTimeout = stopTimeout;
        }

        private static void MainLoop(object threadInfo)
        {
            ThreadInfo ti = (ThreadInfo) threadInfo;
            while (ti.Run)
            {
                ti.Action();
            }
        }

        public void Start()
        {
            foreach (var ti in _threadInfos.Where(ti => ti.Thread.ThreadState == ThreadState.Unstarted))
                ti.Start();
            _started = true;
        }
        
        public void Start(int threadsCount)
        {
            ThreadsCount = threadsCount;
            Start();
        }

        public void Stop()
        {
            ThreadsCount = 0;
            _started = false;
        }

        public void Dispose()
        {
            Stop();
        }

        private void AddThread()
        {
            ThreadInfo ti = new ThreadInfo(MainLoop, _actionFactory.Create());
            _threadInfos.Add(ti);
            if (_started)
                ti.Start();
        }

        private void RemoveThread()
        {
            if (_threadInfos.Count == 0) return;

            _threadInfos[0].Thread.Join(_stopTimeout);
            if (_threadInfos[0].Thread.IsAlive)
                _threadInfos[0].Thread.Abort();
            _threadInfos.RemoveAt(0);
        }

        public int ThreadsCount
        {
            get { return _threadInfos.Count; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value");
                int delta = value - _threadInfos.Count;
                if (delta == 0) return;
                if (delta > 0)
                {
                    for (int i = 0; i < delta; i++)
                        AddThread();
                }
                else
                {
                    delta = -delta;
                    for (int i = 0; i < delta; i++)
                        _threadInfos[i].Stop();

                    for (int i = 0; i < delta; i++)
                        RemoveThread();
                }
            }
        }
    }
}