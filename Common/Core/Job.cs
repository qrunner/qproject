using Provider.Configuration;
using Statistics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace IntegrationService
{
    /// <summary>
    /// Рабочая задача (основная обработка объектов пула)
    /// </summary>
    /// <typeparam name="TQueueObj">Тип объектов, которые обрабатывается задачей</typeparam>
    public abstract class Job<TQueueObj> : JobBase<TQueueObj> 
        where TQueueObj : //class,
        IEquatable<TQueueObj>        
    {        
        /// <summary>
        /// Возникает после обработки каждого объекта
        /// </summary>
        public event EventHandler<ObjectProcessedEventArgs> OnObjectProcessed;
        private void RaiseObjectProcessedEvent(EventHandler<ObjectProcessedEventArgs> ev, TQueueObj obj, TimeSpan t)
        {
            EventHandler<ObjectProcessedEventArgs> handler = ev;
            if (handler != null)
                handler(this, new ObjectProcessedEventArgs(obj, t));
        }
        /// <summary>
        /// Возникает при ошибке обработки объекта
        /// </summary>
        public event EventHandler<ObjectProcessErrorEventArgs> OnObjectProcessError;
        private void RaiseObjectProcessErrorEvent(TQueueObj obj, Exception ex)
        {
            EventHandler<ObjectProcessErrorEventArgs> handler = OnObjectProcessError;
            if (handler != null)
                handler(this, new ObjectProcessErrorEventArgs(obj, ex));
        }
        Stopwatch watch = new Stopwatch();
        private Stopwatch foreingWatch = new Stopwatch();
        /// <summary>
        /// Запускает цикл обработки объектов из очереди
        /// </summary>
        public void Begin()
        {
            Initialize();

            RaiseOnStarted();

            do // основной цикл
            {
                TQueueObj obj;
                if (ppl.TryGetObjectForProcess(out obj))
                {
                    bool rollback = false;
                    try
                    {
                        ForeingWatch.Reset();
                        watch.Restart();
                        rollback = !ProcessItem(obj);
                        // то, что работает только при отсутствии ошибок
                        watch.Stop();
                        ForeingWatch.Stop();
                        ProcessedCount++;
                        //ppl.CommitObjectProcess(obj);
                        RaiseObjectProcessedEvent(OnObjectProcessed, obj, watch.Elapsed);
                        if (!rollback)
                            PushNext(obj);
                    }
                    catch (Exception ex)
                    {
                        watch.Stop();
                        ForeingWatch.Stop();
                        ErrorsCount++;
                        RaiseObjectProcessErrorEvent(obj, ex);
                        try
                        {
                            ProcessError(obj, ex, out rollback);
                        }
                        catch (Exception exProcErr) // ! exception подавляется !
                        {   
                            RaiseObjectProcessErrorEvent(obj, exProcErr);
                        }
                    }
                    finally
                    {   // то, что должно выполниться в любом случае                        
                        if (rollback)
                            ppl.RollbackObjectProcess(obj); // ставим в начало очереди
                        else
                            ppl.CommitObjectProcess(obj); // удаляем из очереди

                        executeTime += watch.Elapsed;
                        ProcessTime += (int)watch.ElapsedMilliseconds;
                        ForeignTime += (int)ForeingWatch.ElapsedMilliseconds;
                    }
                }
                else // Поток сработал в холостую (нет объектов в очереди). Продумать данную ситуацию.
                {
                    Thread.Sleep(100);
                }
            } while (!stop);

            RaiseOnStopped();

            Deinitialize();
        }
        /// <summary>
        /// Вызывается в случае ошибки
        /// </summary>
        /// <param name="item">Объект, обработка которого вызвала ошибку</param>
        /// <param name="ex">Возникшее исключение</param>
        /// <param name="rollbackToQueue">Откатывать назад в очередь или нет</param>
        protected virtual void ProcessError(TQueueObj item, Exception ex, out bool rollbackToQueue)
        {
            rollbackToQueue = false;
        }
        /*
        /// <summary>
        /// Необходимо вызвать непосредственно перед обращением к внешней системе
        /// </summary>
        protected void ExternalStart()
        {
            foreingWatch.Start();
        }
        /// <summary>
        /// Необходимо вызвать сразу после обращения к внешней системе
        /// </summary>
        protected void ExternalEnd()
        {
            foreingWatch.Stop();
        }*/
        /// <summary>
        /// Обертка для обращения к внешней системе. Используется для сбора статистики.
        /// </summary>
        /// <param name="action"></param>
        protected void external(Action action)
        {
            try { ForeingWatch.Start(); action(); }            
            finally { ForeingWatch.Stop(); }            
        }
        /*protected Exception external(Action action)
        {
            try { foreingWatch.Start(); action(); }
            catch (Exception ex) { return ex; }
            finally { foreingWatch.Stop(); }
            return null;
        }*/
        /// <summary>
        /// Переводит объект в следующий пул, если он запущен
        /// </summary>
        /// <param name="obj">Объект</param>
        protected void PushNext(TQueueObj obj)
        {
            foreach (NamedConfigurationElement config in pool.Settings.NextPools.AsParallel())
            {
                if (PoolManager.ActivePools.ContainsKey(config.Name))
                    ((IJobsPool)PoolManager.ActivePools[config.Name]).PushObject(obj);
            }
        }
        /// <summary>
        /// Параметры, которые передаются в метод ProcessItem
        /// </summary>
        //volatile object[] parameters = null;
        //internal void SetParams(object[] par) { parameters = par; }
        /// <summary>
        /// Действия по обработке одного объекта. Внимание: Не обрабатывать критичные исключения внутри, они обрабатываются в вызывающем методе!
        /// </summary>
        /// <param name="order">Объект</param>
        /// <returns>Результат выполнения (сделана работа или нет. Если нет - то откатится в начало очереди.)</returns>
        public virtual bool ProcessItem(TQueueObj item)
        {
            return true;
        }
        /// <summary>
        /// Общее время выполнения всех операций с начала работы задачи
        /// </summary>
        public TimeSpan ExecuteTime
        {
            get { return executeTime; }
        }
        TimeSpan executeTime = TimeSpan.Zero;
        /// <summary>
        /// Среднее вермя выполнения задачи (метод ProcessItem)
        /// </summary>
        public TimeSpan AverageTime
        {
            get { return TimeSpan.FromMilliseconds(executeTime.Milliseconds / ProcessedCount); }
        }
        /// <summary>
        /// Количество ошибок при обработке объектов с момента запуска
        /// </summary>
        public int ErrorsCount
        {
            set { lock (sync) { errorsCount = value; } }
            get { return errorsCount; }
        }
        int errorsCount = 0;
        /// <summary>
        /// Количество успешно обработанных объектов с момента запуска
        /// </summary>
        public int ProcessedCount
        {
            set { lock (sync) { processedCount = value; } }
            get { return processedCount; }
        }
        private int processedCount = 0;
        /// <summary>
        /// Время общей обработки объекта
        /// </summary>
        public int ProcessTime
        {
            get { return processTime; }
            set { lock (sync) { processTime = value; } }
        }
        private int processTime = 0;
        /// <summary>
        /// Время обмена с внешней системой
        /// </summary>
        public int ForeignTime
        {
            get { return foreignTime; }
            set { lock (sync) { foreignTime = value; } }
        }

        public Stopwatch ForeingWatch
        {
            get { return foreingWatch; }
        }

        private int foreignTime = 0;

        private static object sync = new object();
        public override void FlushStat(ref StatRecord statRecord)
        {
            lock (sync)
            {
                statRecord.ProcessedCount += ProcessedCount;
                processedCount = 0;

                statRecord.ErrorsCount += ErrorsCount;
                errorsCount = 0;

                statRecord.ProcessTime += ProcessTime;
                processTime = 0;

                statRecord.ForeignTime += ForeignTime;
                foreignTime = 0;
            }
        }

        protected override void Deinitialize()
        {
            
        }

        protected override void Initialize()
        {
            
        }
    }
}