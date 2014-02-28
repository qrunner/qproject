using System;
using System.Collections.Generic;
using System.Threading;

namespace Common.DPL.InMemory
{
    public class PipelineUnique<T> : IPipeline<T>, IDisposable
        where T : IEquatable<T>
    {
        private readonly Queue<IBag<T>> _queue = new Queue<IBag<T>>();
        private readonly ICollection<IBag<T>> _inProcess = new List<IBag<T>>();
        private readonly ICollection<IBag<T>> _delayed = new HashSet<IBag<T>>();
        private readonly object _addSyncRoot = new object();
        private readonly Timer _wakeUpDelayedTimer;

        public PipelineUnique(int delayedWakeUpTimeout)
        {
            _wakeUpDelayedTimer = new Timer(x => WakeUpDelayed(), null, delayedWakeUpTimeout, delayedWakeUpTimeout);
        }

        public bool TryAdd(T item)
        {
            var itemBag = new BagEquatable<T>(this, item);
            lock (_queue)
            {
                if (!_queue.Contains(itemBag) && !_inProcess.Contains(itemBag) && !_delayed.Contains(itemBag))
                 {
                    _queue.Enqueue(itemBag);
                    itemBag.State = ObjectState.InQueue;
                    Monitor.PulseAll(_queue);
                    return true;
                }
            }
            return false;
        }

        public IBag<T> TakeForProcess()
        {
            lock (_queue)
            {
                while (_queue.Count == 0)
                    Monitor.Wait(_queue);

                var itemBag = _queue.Dequeue();
                _inProcess.Add(itemBag);
                itemBag.State = ObjectState.InProcess;
                lock (_addSyncRoot)
                    Monitor.PulseAll(_addSyncRoot);
                return itemBag;
            }
        }

        internal void Commit(IBag<T> itemBag)
        {
            lock (_queue)
            {
                _inProcess.Remove(itemBag);
                itemBag.State = ObjectState.Commited;
            }
        }

        internal void Rollback(IBag<T> itemBag, uint delay)
        {
            lock (_queue)
            {
                _inProcess.Remove(itemBag);
                if (delay > 0)
                {
                    _delayed.Add(itemBag);
                    itemBag.State = ObjectState.Delayed;
                }
                else
                {
                    _queue.Enqueue(itemBag);
                    itemBag.State = ObjectState.InQueue;
                    Monitor.PulseAll(_queue);
                }
            }
        }

        public int InQueueCount
        {
            get { return _queue.Count; }
        }

        public int InProcessCount
        {
            get { return _inProcess.Count; }
        }

        public int DelayedCount
        {
            get { return _delayed.Count; }
        }

        public bool WaitEmpty(int timeout)
        {
            lock (_addSyncRoot)
                if (_queue.Count > 0)
                    Monitor.Wait(_addSyncRoot, timeout);

            return true;
        }

        public bool WaitEmpty()
        {
            lock (_addSyncRoot)
                if (_queue.Count > 0)
                    Monitor.Wait(_addSyncRoot);

            return true;
        }

        private void WakeUpDelayed()
        {
            foreach (var item in _delayed)
            {
                if (item.IsFree)
                {
                    lock (_queue)
                    {
                        _queue.Enqueue(item);
                        item.State = ObjectState.InQueue;
                        Monitor.PulseAll(_queue);
                    }
                }
            }
        }

        public void Dispose()
        {
            _wakeUpDelayedTimer.Dispose();
        }
    }
}