using Configuration;
using Statistics;
using System;

namespace IntegrationService
{
    public interface IJobsPool
    {
        int Id { get; }
        void Start();
        void Stop();
        void SetSize(int count);

        PoolStatus Status { get; }
        PoolConfig Settings { get; }
        string PoolName { get; set; }

        bool PushObject(object obj);

        TimeSpan AverageProcessTime { get; }

        long Errors { get; }

        void FlushStat(ref StatRecord statRecord);
        //event EventHandler<JobEventArgs<TQueueObj>> OnJobStarted;
        event EventHandler<ObjectsPickedEventArgs> OnObjectsLoaded;
        event EventHandler<ObjectsPickErrorEventArgs> OnPickError;
        event EventHandler<ObjectProcessedEventArgs> OnObjectProcessed;
        event EventHandler OnPoolStarted;
        event EventHandler<ObjectProcessErrorEventArgs> OnObjectProcessError;
        /// <summary>
        /// Объект поставлен в очередь
        /// </summary>
        event EventHandler<ObjectEnquedEventArgs> OnObjectEnqueued;
    }
}