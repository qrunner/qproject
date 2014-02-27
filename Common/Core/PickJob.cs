using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Configuration;
using System.Configuration;
using System.Diagnostics;
using Statistics;

namespace IntegrationService
{
    /// <summary>
    /// Задача по постановке объектов в очередь пула
    /// </summary>
    /// <typeparam name="TQueueObj">Тип объектов, обрабатываемых задачей</typeparam>
    public abstract class PickJob<TQueueObj> : JobBase<TQueueObj>
        //where TQueueObj : class
    {
        private static object sync = new object();
        private volatile int enqueuedCount = 0;
        /// <summary>
        /// Количество поставленных в очередь объектов
        /// </summary>
        public int EnqueuedCount { set { lock (sync) { enqueuedCount = value; } } get { return enqueuedCount; } }
        private volatile int pickedCount = 0;
        /// <summary>
        /// Количество выбранных объектов
        /// </summary>
        public int PickedCount { set { lock (sync) { pickedCount = value; } } get { return pickedCount; } }
        private volatile int errosCount = 0;
        /// <summary>
        /// Количество ошибок при выборке
        /// </summary>
        public int ErrorsCount { set { lock (sync) { errosCount = value; } } get { return errosCount; } }
        private volatile int pickTime = 0;
        /// <summary>
        /// Время выполнения выборки
        /// </summary>
        public int PickTime { set { lock (sync) { pickTime = value; } } get { return pickTime; } }
        private volatile int picksCount = 0;
        /// <summary>
        /// Количество сборов
        /// </summary>
        public int PicksCount { set { lock (sync) { picksCount = value; } } get { return picksCount; } }

        public override sealed void FlushStat(ref StatRecord statRecord)
        {
            lock (sync)
            {
                statRecord.EnqueuedCount += enqueuedCount;
                enqueuedCount = 0;

                statRecord.PickedCount += pickedCount;
                pickedCount = 0;

                statRecord.PickErrors += errosCount;
                errosCount = 0;

                statRecord.PickTime += pickTime;
                pickTime = 0;

                statRecord.PicksCount += picksCount;
                picksCount = 0;
            }
        }

        public event EventHandler<ObjectsPickErrorEventArgs> OnObjectsPickError;
        void RaiseObjectsPickError(EventHandler<ObjectsPickErrorEventArgs> eventhandler, Exception ex)
        {
            EventHandler<ObjectsPickErrorEventArgs> handler = eventhandler;
            if (handler != null)
                handler(this, new ObjectsPickErrorEventArgs(ex));
        }

        public event EventHandler<ObjectsPickedEventArgs> OnObjectsPicked;
        void RaiseObjectsLoaded(EventHandler<ObjectsPickedEventArgs> eventhandler, int count)
        {
            EventHandler<ObjectsPickedEventArgs> handler = eventhandler;
            if (handler != null)
                handler(this, new ObjectsPickedEventArgs(count));
        }

        private int reloadMargin = 100;
        /// <summary>
        /// Остаток заявок, при котором начинают подгружаються новые
        /// </summary>
        public int ReloadMargin
        {
            get { return reloadMargin; }
            set { reloadMargin = value; }
        }

        /*private string peekOrdersScript = "";
        /// <summary>
        /// Скрипт, которые выбирает заявки для данной очереди из БД
        /// </summary>
        public string PeekOrdersScript
        {
            get { return peekOrdersScript; }
            set { peekOrdersScript = value; }
        }*/

        private int reloadTimeout = 100;
        /// <summary>
        /// Таймаут подгрузки заявок в очередь из базы данных
        /// </summary>
        public int ReloadTimeout
        {
            get { return reloadTimeout; }
            set { reloadTimeout = value; }
        }
                
        private int maxPeeksCount = 0;
        /// <summary>
        /// Максимальное количество загрузок. 0 - не ограничивать.
        /// </summary>
        public int MaxPeeksCount
        {
            get { return maxPeeksCount; }
            set { maxPeeksCount = value; }
        }
        
        Stopwatch pickTimer = new Stopwatch();
        //Stopwatch foreingWatch = new Stopwatch();

        public void Begin()
        {
            TimeSpan timeout = TimeSpan.Zero;
            DateTime lastPeek = DateTime.UtcNow;

            bool emptyPeek = true;

            Initialize();

            RaiseOnStarted();

            do
            {
                timeout = DateTime.UtcNow - lastPeek;                
                // если объектов в очереди остается мало или истек таймаут
                if (ppl.ObjectsToProcessCount <= reloadMargin || timeout.TotalMilliseconds > reloadTimeout)
                {
                    emptyPeek = true;
                    pickTimer.Restart();
                    try
                    {
                        IEnumerable<TQueueObj> items = PickObjects();
                        pickTimer.Stop();
                        PicksCount++;
                        int itemsCount = items.Count();
                        emptyPeek = items == null || itemsCount == 0;

                        if (!emptyPeek)
                        {
                            PickedCount += itemsCount;

                            RaiseObjectsLoaded(OnObjectsPicked, itemsCount);
                            int cycleEnq = 0;
                            foreach (TQueueObj obj in items)
                            {
                                if (obj != null)
                                {
                                    if (ppl.TryPutObject(obj))
                                    {
                                        cycleEnq++;
                                        EnqueuedCount++;
                                    }
                                }
                            }
                            emptyPeek = cycleEnq == 0;
                        }

                        lastPeek = DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        pickTimer.Stop();
                        ErrorsCount++;
                        RaiseObjectsPickError(OnObjectsPickError, ex);
                        ProcessError(ex);
                    }
                    finally
                    {
                        PickTime += (int)pickTimer.ElapsedMilliseconds;
                    }
                }

                if (maxPeeksCount > 0 && picksCount >= maxPeeksCount) // проверяем на количество загрузок
                    stop = true;

                if (!stop && emptyPeek) // если загрузка пустая - засыпаем
                    Thread.Sleep(reloadTimeout);

            } while (!stop);

            RaiseOnStopped();

            Deinitialize();
        }

        public abstract IEnumerable<TQueueObj> PickObjects();
        

        protected override void Deinitialize()
        {
            
        }

        protected override void Initialize()
        {
            
        }

        protected virtual void ProcessError(Exception ex)
        {
            
        }
    }
}