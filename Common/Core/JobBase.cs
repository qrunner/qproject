using Statistics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntegrationService
{
    public abstract class JobBase<TQueueObj>
        //where TQueueObj : class
    {
        #region FIELDS

        protected bool stop = false;
        internal Thread thread = null;
        public IPipeline<TQueueObj> ppl = null;
        public IJobsPool pool = null;
        #endregion

        #region EVENTS

        public event EventHandler<JobEventArgs<TQueueObj>> OnStarted;
        protected void RaiseOnStarted()
        {
            RaiseEvent(OnStarted);
        }

        public event EventHandler<JobEventArgs<TQueueObj>> OnStoping;
        protected void RaiseOnStoping()
        {
            RaiseEvent(OnStoping);
        }

        public event EventHandler<JobEventArgs<TQueueObj>> OnStopped;
        protected void RaiseOnStopped()
        {
            RaiseEvent(OnStopped);
        }

        private void RaiseEvent(EventHandler<JobEventArgs<TQueueObj>> eventhandler)
        {
            EventHandler<JobEventArgs<TQueueObj>> handler = eventhandler; // для исключения состояния гонки делаем локальную копию
            if (handler != null)
                handler(this, new JobEventArgs<TQueueObj>(this));
        }

        private void RaiseEventAsync(EventHandler<JobEventArgs<TQueueObj>> eventhandler)
        {
            EventHandler<JobEventArgs<TQueueObj>> handler = eventhandler; // для исключения состояния гонки делаем локальную копию
            if (handler != null)
                handler.BeginInvoke(this, new JobEventArgs<TQueueObj>(this), null, null);
        }

        #endregion
        /// <summary>
        /// Обновляет статистику
        /// </summary>
        /// <param name="statRecord">Текущая запись статистики</param>
        public virtual void FlushStat(ref StatRecord statRecord)
        {

        }

        public JobBase()
        {

        }
        /// <summary>
        /// Действия по инициализации
        /// </summary>
        protected abstract void Initialize();
        /// <summary>
        /// Действия по деинициализации
        /// </summary>
        protected abstract void Deinitialize();

        public JobBase(IPipeline<TQueueObj> pipeline)
        {
            this.ppl = pipeline;
        }
        /// <summary>
        /// Прекращает работу задачи
        /// </summary>
        public void End()
        {
            stop = true;
        }
    }
}