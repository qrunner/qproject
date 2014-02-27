using System;

namespace Common.DPL
{
    /// <summary>
    /// Object processing queue 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPipeline< T> where T : IEquatable<T>
    {
        /// <summary>
        /// Push object to the queue
        /// </summary>
        /// <param name="item"></param>
        bool TryAdd(T item);

        /// <summary>
        /// Push object to the queue 
        /// </summary>
        // bool TryAdd(T item, int timeout);

        /// <summary>
        /// Get object from queue and mark it as "in process"
        /// </summary>
        /// <returns></returns>
        IBag<T> TakeForProcess();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemBag"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
       // bool TryTakeForProcess(out IBag<T> itemBag, int timeout);

        /// <summary>
        /// Mark object as "processed" and remove from pipeline
        /// </summary>
        //void Commit(IBag<T> itemBag);

        /// <summary>
        /// Decline object procesing and return it to the queue
        /// </summary>
        /// <param name="itemBag"></param>
        /// <param name="delay"></param>
        //void Rollback(IBag<T> itemBag, uint delay);

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

        bool WaitEmpty();
    }
}