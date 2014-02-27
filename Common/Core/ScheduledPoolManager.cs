using System;
using System.Collections.Generic;
using System.Configuration;
using Common.Reflection;
using Common.Scheduling;
using Common.ServiceModel;
using Configuration;
using Configuration.ScheduledPools;
using Provider.Scheduling;

namespace IntegrationService
{
    /// <summary>
    /// Менеджер запланированных пулов. Управляет запуском по расписанию и работой всех пулов в приложении.
    /// </summary>
    /*public static class ScheduledPoolManager
    {
        private static Dictionary<string, IService> scheduledPools = new Dictionary<string, IService>();
        public static PoolsConfigRoot Settings = null;
        private static IScheduler _scheduler = null;

        static ScheduledPoolManager()
        {
            Settings = (PoolsConfigRoot) ConfigurationManager.GetSection(PoolsConfigRoot.SectionName);

            var schedulingContext = new SchedulingContext("QuartzSchedulingProvider");
            var schedulerfactory = schedulingContext.Provider.GetSchedulerFactory();
            _scheduler = schedulerfactory.GetScheduler();
        }

        /// <summary>
        /// Запланированные пулы
        /// </summary>
        public static Dictionary<string, IService> ScheduledPools
        {
            get { return scheduledPools; }
        }

        /// <summary>
        /// Включает в план запуска по расписанию все пулы, отмеченные в настройках
        /// </summary>
        /// <param name="onStartAction">Действие, выполняемое при запуске каждого пула</param>
        public static void SchedulePools()
        {
            foreach (StartScheduledPoolConfig kv in Settings.StartScheduledPools)
            {
                if (Convert.ToBoolean(kv.Start))
                {
                    try
                    {
                        SchedulePool(kv.PoolName);
                    }
                    catch (Exception ex)
                    {
                        //if (onError != null) onError(ex, kv.PoolName);
                    }
                }
            }
        }

        public static void SchedulePool(string poolName)
        {
            ScheduledPoolConfig config = Settings.JobSchedPools[poolName];
            if (config == null) throw new Exception(string.Format("Невозможно найти настройки пула <{0}> в конфигурационном файле.", poolName));
            Type poolClass = null;
            if (!TypeLoader.TryGetType(config.PoolClass, out poolClass))
                throw new Exception(string.Format("Невозможно найти тип <{0}>.", config.PoolClass));
            IService pool = (IService)Activator.CreateInstance(poolClass);
            pool.Name = poolName;
            scheduledPools.Add(poolName, pool);

            _scheduler.ScheduleJob(poolName, pool.Start, config.Trigger);
        }
    }*/
}
