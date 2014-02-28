using System;

namespace Common.DPL
{
    /// <summary>
    /// Object processing queue 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPipeline<T>
    {
        /// <summary>
        /// Push object to the queue
        /// </summary>
        /// <param name="item"></param>
        bool TryAdd(T item);
        
        /// <summary>
        /// Get object from queue and mark it as "in process"
        /// </summary>
        /// <returns></returns>
        IBag<T> TakeForProcess();
        
        /// <summary>
        /// Objects, currently waiting in queue count
        /// </summary>
        int InQueueCount { get; }

        /// <summary>
        /// Objects, currently processing
        /// </summary>
        int InProcessCount { get; }

        /// <summary>
        /// Delayed objects count
        /// </summary>
        int DelayedCount { get; }

        /// <summary>
        /// Block calling thread until reload needed
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        bool WaitEmpty(int timeout);

        /// <summary>
        /// Block calling thread until reload needed
        /// </summary>
        /// <returns></returns>
        bool WaitEmpty();
    }
}