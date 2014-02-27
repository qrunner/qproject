using System;
using System.Diagnostics;

namespace Common.DPL.InMemory
{
    internal class Bag<T> : IBag<T> where T:IEquatable<T>
    {
        private Stopwatch _stw;
        private readonly Pipeline<T> _pipeline;

        public Bag(Pipeline<T> pipeline, T item)
        {
            Object = item;
            _pipeline = pipeline;
        }

        public bool Equals(IBag<T> other)
        {
            return this.Object.Equals(other.Object);
        }

        public T Object { get; private set; }

        public ObjectState State { get; set; }

        public void Cancel()
        {
            State = ObjectState.Cancelled;
            _pipeline.Commit(this);
        }

        public void Commit()
        {
            State = ObjectState.Commited;
            _pipeline.Commit(this);
        }

        public void Rollback(uint timeout)
        {
            RollbackTimeout = timeout;
            RollbackedTimes++;
            State = timeout > 0 ? ObjectState.Delayed : ObjectState.Rollbacked;
            if (timeout <= 0) return;
            _stw = new Stopwatch();
            _stw.Start();
            _pipeline.Rollback(this, timeout);
        }

        public uint RollbackTimeout { get; set; }

        public uint RollbackedTimes { get; set; }

        public bool IsFree
        {
            get
            {
                if (_stw == null) return true;

                if (_stw.ElapsedMilliseconds >= RollbackTimeout)
                {
                    _stw.Stop();
                    _stw = null;
                    return true;
                }
                return false;
            }
        }
    }
}