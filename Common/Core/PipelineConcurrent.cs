using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Collections;

namespace IntegrationService
{
    /// <summary>
    /// Потокобезопасный конвейер обработки объектов Ver. 2
    /// </summary>
    public class PipelineConcurrent<T> : IPipeline<T>, IDisposable // can use GetHashCode()
        //where T : class
    {
        Thread wakeupThread = null;

        public PipelineConcurrent()
        {
            wakeupThread = new Thread(WakeupCycle);
            wakeupThread.IsBackground = true;
            wakeupThread.Start();
        }

        object sync = new object();
        /// <summary>
        /// Очередь объектов на обработку
        /// </summary>
        ConcurrentQueue<T> objectsToProcess = new ConcurrentQueue<T>();
        /// <summary>
        /// Контейнер объектов в обработке
        /// </summary>
        HashSet<T> objectsInProcess = new HashSet<T>();

        private LimitedQueue<T> _lastProcessed = new LimitedQueue<T>(10);

        /// <summary>
        /// Счетчик количества попыток
        /// </summary>
        ConcurrentDictionary<int, int> delayCounter = new ConcurrentDictionary<int, int>();
        /// <summary>
        /// Контейнер отложенных объектов
        /// </summary>
        HashSet<DelayInfo> delayedObjects = new HashSet<DelayInfo>();
        /// <summary>
        /// Берет объект из очереди и переводит его в работу
        /// </summary>
        /// <returns>Первый объект из очереди на обработку</returns>
        public bool TryGetObjectForProcess(out T obj)
        {
            lock (sync)
            {
                if (objectsToProcess.TryDequeue(out obj))
                {
                    objectsToProcessCount--;
                    objectsInProcess.Add(obj);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Ставит новый объект на конвейер (в случае отсутствия)
        /// </summary>
        /// <param name="obj">Объект</param>
        public bool TryPutObject(T obj)
        {
            lock (sync)
            {
                if (!objectsToProcess.Contains(obj) && !objectsInProcess.Contains(obj) && !IsDelayed(obj) /* && !_lastProcessed.Contains(obj)*/)
                {
                    objectsToProcess.Enqueue(obj);
                    objectsToProcessCount++;
                    RaiseObjectEnqued(obj);
                    return true;
                }
                return false;
            }
        }

        private void RaiseObjectEnqued(T obj)
        {
            EventHandler<ObjectEnquedEventArgs> evh = ObjectEnqueued;
            if (evh != null) evh(this, new ObjectEnquedEventArgs(obj));
        }

        /// <summary>
        /// Подтверждает завершение процесса обработки объекта
        /// </summary>
        /// <param name="order">Объект</param>
        public void CommitObjectProcess(T order)
        {
            lock (sync)
            {
                if (!objectsInProcess.Remove(order))
                    delayedObjects.RemoveWhere(x => x.Obj.Equals(order));

                //_lastProcessed.Enqueue(order);

                /*if (!objectsInProcess.Remove(order))
                    if (!IsDelayed(order))
                        throw new Exception("Невозможно извлечь объект из очереди при выполнении Commit.");*/
            }
        }
        /// <summary>
        /// Возвращает объект в начало очереди на обработку
        /// </summary>
        /// <param name="order">Объект</param>
        public void RollbackObjectProcess(T order)
        {
            lock (sync)
            {
                if (objectsInProcess.Remove(order))
                {
                    objectsToProcess.Enqueue(order);
                    objectsToProcessCount++;
                }
                else throw new Exception("Невозможно извлечь объект из очереди при выполнении Rollback.");
            }
        }
        volatile int objectsToProcessCount = 0;
        /// <summary>
        /// Количество объектов в очереди на обработку
        /// </summary>
        public int ObjectsToProcessCount { get { return objectsToProcessCount; } }
        /// <summary>
        /// Количество объектов в обработке
        /// </summary>
        public int ObjectsInProcessCount { get { return objectsInProcess.Count; } }
        /// <summary>
        /// Объект поставлен в очередь
        /// </summary>
        public event EventHandler<ObjectEnquedEventArgs> ObjectEnqueued;

        public int ObjectsDelayedCount
        {
            get { return delayedObjects.Count; }
        }

        public void DelayObjectProcess(T obj, int interval)
        {
            lock (sync)
            {
                if (objectsInProcess.Remove(obj))
                {
                    delayedObjects.Add(new DelayInfo(obj, interval));
                    //Timer t = new Timer(DelayComplete, obj, interval, interval);
                }
                else throw new Exception("Невозможно извлечь объект из очереди при выполнении Delay.");
            }
        }

        bool IsDelayed(T obj)
        {
            return delayedObjects.Any(di => di.Obj.Equals(obj));
        }

        /*void DelayComplete(object obj)
        {
            lock (sync)
            {
                if (delayedObjects.Remove((T)obj))
                    if (TryPutObject((T)obj))
                    { }
                    else
                    { }
                else
                { }
            }
        }*/

        volatile bool runWakeUp = true;
        void WakeupCycle()
        {
            while (runWakeUp)
            {
                lock (sync)
                {
                    if (delayedObjects.Count > 0)
                    {
                        DelayInfo[] tmpArray = new DelayInfo[delayedObjects.Count];
                        delayedObjects.CopyTo(tmpArray);
                        foreach (var delayed in tmpArray)
                        {
                            if (delayed.IsFree)
                            {
                                if (delayedObjects.Remove(delayed))
                                    this.TryPutObject(delayed.Obj);
                            }
                        }
                    }
                }
                Thread.Sleep(3000);
            }
        }
        /*
        [Obsolete("Не работает")]
        public void DelayObjectProcess(T obj, int interval, int times, Action<T> finalAction)
        {
            throw new NotImplementedException();

            lock (sync)
            {
                // если объект находится в списке отложенных, то ничего не делаем
                if (delayedObjects.Contains(obj)) return;
                // если объект сейчас в обработке, но уже когда-то откладывался
                if (objectsInProcess.Contains(obj) && delayCounter.ContainsKey(obj.GetHashCode()))
                {
                    int delayedCount = delayCounter[obj.GetHashCode()];
                    // если количество раз
                    if (delayedCount >= times)
                    {
                        try { finalAction(obj); }
                        finally
                        {
                            objectsInProcess.Remove(obj);
                            delayedObjects.Remove(obj);
                        }
                        return;
                    }
                    else
                    {

                    }
                }

                if (objectsInProcess.Remove(obj))
                {
                    delayedObjects.Add(obj);
                    Timer t = new Timer(DelayComplete, obj, interval, interval);
                }
                else throw new Exception("Невозможно извлечь объект из очереди при выполнении Delay.");
            }
        }*/
        /// <summary>
        /// Данные по отложенному объекту
        /// </summary>
        struct DelayInfo : IEquatable<T>
        {
            /*public DelayInfo(T obj, int interval, int delayCount, TimerCallback clb)
            {
                Obj = obj;
                Interval = interval;
                DelayCount = delayCount;
                Timer = new Timer(clb, Obj, interval, interval);
                DelayedTimes = 0;
            }            
            public Timer Timer;
            public int DelayedTimes;
            public int DelayCount;
            */
            public int Delay;
            public T Obj;
            private DateTime delayStarted;
            /*/// <summary>
            /// Конструктор для сравнения
            /// </summary>
            /// <param name="obj"></param>
            public DelayInfo(T obj)
            {
                Obj = obj;
                Delay = 0;
                delayStarted = DateTime.MinValue;
            }
            */
            public DelayInfo(T obj, int delay)
            {
                Obj = obj;
                Delay = delay;
                delayStarted = DateTime.UtcNow;
            }

            public bool IsFree
            {
                get { return (DateTime.UtcNow - delayStarted).TotalMilliseconds > Delay; }
            }

            public bool Equals(T other)
            {
                return this.Obj.Equals(other);
            }

            public override int GetHashCode()
            {
                return this.Obj.GetHashCode();
            }
        }

        public void Dispose()
        {
            runWakeUp = false;
            wakeupThread.Join(10000);
            if (wakeupThread.IsAlive)
                wakeupThread.Abort();
        }
    }
}