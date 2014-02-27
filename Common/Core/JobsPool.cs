using Common.ServiceModel;
using Configuration;
using Statistics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntegrationService
{
    /// <summary>
    /// Пул задач. Сделать еще версию с использованием Task.
    /// </summary>    
    public class JobsPool<TQueueObj, TPickJob, TJob> : IService, IJobsPool
        where TPickJob : PickJob<TQueueObj>//, new()
        where TJob : Job<TQueueObj>
        where TQueueObj : //class, 
        IEquatable<TQueueObj>
    {
        public JobsPool(string poolName)
        {
            this.poolName = poolName;
        }
        public event EventHandler<JobEventArgs<TQueueObj>> OnJobStarted;
        public event EventHandler<ObjectsPickedEventArgs> OnObjectsLoaded;
        public event EventHandler<ObjectsPickErrorEventArgs> OnPickError;
        public event EventHandler<ObjectProcessedEventArgs> OnObjectProcessed;
        public event EventHandler OnPoolStarted;
        public event EventHandler<ObjectProcessErrorEventArgs> OnObjectProcessError;
        /// <summary>
        /// Объект поставлен в очередь
        /// </summary>
        public event EventHandler<ObjectEnquedEventArgs> OnObjectEnqueued;
        private void RaisePoolStarted()
        {
            EventHandler evh = OnPoolStarted;
            if (evh != null)
                evh(this, EventArgs.Empty);
        }
        string poolName = "Undefined";
        /// <summary>
        /// Имя пула
        /// </summary>
        public string PoolName
        {
            get { return poolName; }
            set { poolName = value; }
        }
        /// <summary>
        /// Задача постановки в очередь
        /// </summary>
        protected TPickJob pickJob = null;
        /// <summary>
        /// Конвейер заявок для данного пула задач
        /// </summary>
        protected IPipeline<TQueueObj> pipeline = new PipelineConcurrent<TQueueObj>();
        // protected IPipeline<TQueueObj> pipeline = new Pipeline<TQueueObj>();
        /// <summary>
        /// Настройки пула задач
        /// </summary>
        protected PoolConfig jobSettings = null;
        /*/// <summary>
        /// Статусы заявки, которые обрабатываются данной задачей
        /// </summary>
        public HashSet<string> processStatuses = new HashSet<string>();*/
        /*/// <summary>
        /// Типы заявок, которые обрабатывает задача
        /// </summary>
        public HashSet<OrderType> processOrderTypes = new HashSet<OrderType>();*/
        private Timer statTimer = null;
        /// <summary>
        /// Настройки пула задач
        /// </summary>
        public PoolConfig Settings
        {
            get
            {
                if (jobSettings == null)
                    jobSettings = LoadSettings(poolName);

                return jobSettings;
            }
        }

        public bool PushObject(object obj)
        {
            return pipeline.TryPutObject((TQueueObj)obj);
        }

        protected static PoolConfig LoadSettings(string poolName)
        {
            PoolsConfigRoot section = (PoolsConfigRoot)ConfigurationManager.GetSection(PoolsConfigRoot.SectionName);
            PoolConfig jobSettings = section.JobsPools[poolName];
            if (jobSettings == null)
                //jobSettings = new PoolConfig(); // по дефолту
                throw new Exception(string.Format("Невозможно найти настройки пула <{0}> в конфигурационном файле.", poolName));
            return jobSettings;
        }
                
        /// <summary>
        /// Коллекция задач (активных потоков)
        /// </summary>
        HashSet<TJob> workJobs = new HashSet<TJob>();

        PoolStatus status = PoolStatus.Stopped;

        public PoolStatus Status
        {
            get { return status; }            
        }

        public JobsPool()
        {
            this.Id = 1;//this.ToString().GetHashCode();

            pipeline.ObjectEnqueued += pipeline_ObjectEnqueued;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        void pipeline_ObjectEnqueued(object sender, ObjectEnquedEventArgs e)
        {
            EventHandler<ObjectEnquedEventArgs> handler = OnObjectEnqueued;
            if (handler != null)
                handler(this, e);
        }

        void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            CollectStatistics(null);
        }

        public int Id { get; private set; }

        private void CollectStatistics(Object stateInfo)
        {
            StatRecord stat = new StatRecord(this.ToString(), this.JobsCount, this.pipeline.ObjectsInProcessCount, this.pipeline.ObjectsToProcessCount, PoolStatus.Working);
            if (pickJob != null)
                pickJob.FlushStat(ref stat);

            foreach (var job in workJobs)
                if (job != null)
                    job.FlushStat(ref stat);

            ServiceStat.WriteStat(stat);
        }

        private void InitializeInternal()
        {
            InitPickJob();
            statTimer = new Timer(CollectStatistics, null, Settings.StatInterval, Settings.StatInterval);
        }

        private void InitPickJob()
        {
            pickJob = CreatePickJob();
            pickJob.ppl = this.pipeline;
            pickJob.pool = this;
            pickJob.ReloadMargin = Settings.ReloadMargin;
            pickJob.ReloadTimeout = Settings.ReloadTimeout;
            pickJob.MaxPeeksCount = Settings.MaxPickCount;
        }

        protected virtual TPickJob CreatePickJob()
        {
            throw new NotImplementedException();
        }

        protected virtual void Initialize()
        {
            
        }

        protected virtual void PostInitialize()
        {

        }

        protected virtual void Deinitialize()
        {

        }
        /// <summary>
        /// Проверяет наличие указанных параметров в конфигурации пула. В случае отсутствия возникает исключение.
        /// </summary>
        /// <param name="mandatoryParams">Список имен параметров</param>
        protected void CheckConfigParametersExists(params string[] mandatoryParams)
        {
            foreach (string parName in mandatoryParams)
            {
                if (!Settings.PoolSettings.ContainsKey(parName) || string.IsNullOrWhiteSpace(Settings.PoolSettings[parName]))
                    throw new ArgumentException("В настройках пула не задан параметр <" + parName + ">.", parName);
            }
        }

        public virtual void Start()
        {
            Initialize();

            PostInitialize();

            InitializeInternal();

            for (int i = 0; i < Settings.ThreadsCount; i++)
            {
                Thread.Sleep(100); // сдвигаем время начала работы задач, для рассинхронизации блокировок
                AddJob();
            }

            if (pickJob != null)
            {
                pickJob.OnStarted += job_OnStarted;
                pickJob.OnObjectsPicked += peekJob_OnObjectsLoaded;
                pickJob.OnObjectsPickError += pickJob_OnObjectsPickError;

                pickJob.thread = StartAsync(pickJob.Begin, pickJob.ToString());
            }

            status = PoolStatus.Working;

            RaisePoolStarted();
        }

        private void pickJob_OnObjectsPickError(object sender, ObjectsPickErrorEventArgs e)
        {
            EventHandler<ObjectsPickErrorEventArgs> evh = OnPickError;
            if (evh != null)
                evh(sender, e);
        }

        public void Stop()
        {
            StopPickJob(pickJob);            

            foreach (var job in workJobs)
            {
                StopJob(job);                
            }

            workJobs.Clear();

            Deinitialize();

            status = PoolStatus.Stopped;
        }

        public string Name
        {
            get { return PoolName; }
            set { PoolName = value; }
        }

        void peekJob_OnObjectsLoaded(object sender, ObjectsPickedEventArgs e)
        {
            EventHandler<ObjectsPickedEventArgs> evh = OnObjectsLoaded;
            if (evh != null) evh(sender, e);
        }
        /// <summary>
        /// Задает количество потоков в пуле
        /// </summary>
        /// <param name="newSize">Количество потоков</param>
        public void SetSize(int newSize)
        {
            if (newSize != workJobs.Count)
            {
                if (newSize > Settings.MaxThreadsCount) throw new InvalidOperationException("Ошибка при изменении размера пула. Максимальное количество потоков в пуле - " + Settings.MaxThreadsCount.ToString());
                if (newSize < Settings.MinThreadsCount) throw new InvalidOperationException("Ошибка при изменении размера пула. Минимальное количество потоков в пуле - " + Settings.MaxThreadsCount.ToString());

                if (newSize > workJobs.Count)
                {
                    int diff = newSize - workJobs.Count;
                    for (int i = 0; i < diff; i++)
                        AddJob();
                }
                else
                {
                    int diff = workJobs.Count - newSize;
                    for (int i = 0; i < diff; i++)
                        RemoveJob();
                }
            }
        }

        void AddJob()
        {
            if (workJobs.Count < Settings.MaxThreadsCount)
            {
                TJob job = Activator.CreateInstance<TJob>();
                job.pool = this;
                job.ppl = this.pipeline;

                job.OnStarted += job_OnStarted;
                job.OnObjectProcessed += job_OnObjectProcessed;
                job.OnObjectProcessError += job_OnObjectProcessError;
                                
                job.thread = StartAsync(job.Begin, string.Format("{0}", job));
                
                workJobs.Add(job);
            }
            else throw new InvalidOperationException("Нельзя добавить поток. Максимальное количество потоков в пуле - " + Settings.MaxThreadsCount.ToString());
        }

        void job_OnObjectProcessError(object sender, ObjectProcessErrorEventArgs e)
        {
            errors++;
            EventHandler<ObjectProcessErrorEventArgs> evh = OnObjectProcessError;
            if (evh != null)
                evh(sender, e);
        }

        void job_OnObjectProcessed(object sender, ObjectProcessedEventArgs e)
        {
            objectsProcessed++;
            ProcessingTime += e.Time;
            EventHandler<ObjectProcessedEventArgs> evh = OnObjectProcessed;
            if (evh != null)
                evh(this, e);
        }

        void job_OnStarted(object sender, JobEventArgs<TQueueObj> e)
        {
            EventHandler<JobEventArgs<TQueueObj>> handler = OnJobStarted;
            if (handler != null)
                handler(this, new JobEventArgs<TQueueObj>(e.job));
        }

        void RemoveJob()
        {
            if (workJobs.Count > Settings.MinThreadsCount)
            {
                TJob jobToRemove = workJobs.Last();
                StopJob(jobToRemove);
                workJobs.Remove(jobToRemove);
            }
            else throw new InvalidOperationException("Нельзя удалить поток. Минимальное количество потоков в пуле - " + Settings.MinThreadsCount.ToString());
        }

        private const int ThreadAbortTimeout = 20000;

        private void StopJob(TJob job)
        {
            if (job != null)
            {
                job.End();

                if (job.thread != null)
                    job.thread.Join(ThreadAbortTimeout);

                job.OnStarted -= job_OnStarted;
                job.OnObjectProcessed -= job_OnObjectProcessed;
                job.OnObjectProcessError -= job_OnObjectProcessError;
            }
        }

        private void StopPickJob(TPickJob pickJob)
        {
            if (pickJob != null)
            {
                pickJob.End();

                if (pickJob.thread != null)
                    pickJob.thread.Join(ThreadAbortTimeout);

                pickJob.OnStarted -= job_OnStarted;
                pickJob.OnObjectsPicked -= peekJob_OnObjectsLoaded;
                pickJob.OnObjectsPickError -= pickJob_OnObjectsPickError;
            }
        }
        /// <summary>
        /// Возвращает количество задач в пуле
        /// </summary>
        public int JobsCount { get { return workJobs.Count; } }
        /// <summary>
        /// Запускает задачу асинхронно в отдельном потоке
        /// </summary>
        /// <param name="start">Делегат для выполнения</param>
        /// <param name="threadName">Имя потока (в отладочных целях)</param>
        /// <returns>Запущенный поток</returns>
        public static Thread StartAsync(ThreadStart start, string threadName)
        {
            Thread t = new Thread(start);
            t.Name = threadName;
            t.IsBackground = false;
            t.Start();
            return t;
        }

        private object sync_ProcessingTime = new object();
        private TimeSpan processingTime = TimeSpan.Zero;
        public TimeSpan ProcessingTime
        {
            get
            {
                TimeSpan retval;
                lock (sync_ProcessingTime)
                {
                    retval = processingTime;
                }
                return retval;
            }
            set
            {
                lock (sync_ProcessingTime)
                {
                    processingTime = value;
                }
            }
        }

        private long objectsProcessed = 0;
        public long ObjectsProcessed { get { return objectsProcessed; } }

        private long errors = 0;
        public long Errors { get { return errors; } }
        /// <summary>
        /// Среднее вермя выполнения задачи (метод ProcessItem)
        /// </summary>
        public TimeSpan AverageProcessTime
        {
            get { return TimeSpan.FromMilliseconds(ProcessingTime.TotalMilliseconds / objectsProcessed); }
        }

        public void FlushStat(ref StatRecord statRecord)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return poolName;
        }
    }
}