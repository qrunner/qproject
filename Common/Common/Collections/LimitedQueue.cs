using System.Collections.Generic;

namespace Common.Collections
{
    /// <summary>
    /// Очередь с ограничением по размеру
    /// </summary>
    /// <typeparam name="T">Тип элементов в очереди</typeparam>
    public class LimitedQueue<T>
    {
        private readonly int _limit;

        public LimitedQueue(int limit)
        {
            _limit = limit;
            _queue = new Queue<T>(limit);
        }

        private readonly Queue<T> _queue;

        public void Enqueue(T item)
        {
            while (_queue.Count >= _limit)
                _queue.Dequeue();

            _queue.Enqueue(item);
        }

        public T Dequeue()
        {
            return _queue.Dequeue();
        }

        public int Count
        {
            get { return _queue.Count; }
        }

        public T Peek()
        {
            return _queue.Peek();
        }

        public bool Contains(T item)
        {
            return _queue.Contains(item);
        }
    }
}