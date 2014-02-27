using Common.Reflection;
using Common.ServiceModel;
using Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationService
{
    /// <summary>
    /// Менеджер пулов. Управляет запуском и работой всех пулов в приложении.
    /// </summary>
    public static class PoolManager
    {
        private static readonly Dictionary<string, IService> activePools = new Dictionary<string, IService>();
        public static PoolsConfigRoot Settings = null;

        static PoolManager()
        {
            Settings = (PoolsConfigRoot) ConfigurationManager.GetSection(PoolsConfigRoot.SectionName);
        }

        /// <summary>
        /// Активные пулы
        /// </summary>
        public static Dictionary<string, IService> ActivePools
        {
            get { return activePools; }
        }

        /// <summary>
        /// Запускает пул с указанным именем
        /// </summary>
        /// <param name="poolName">Имя пула</param>
        /// <param name="onStartAction">Действие, выполняемое при запуске пула</param>
        public static IService StartPool(string poolName, Action<IService> onStartAction)
        {
            IService pool = GetPool(poolName);
            if (onStartAction != null)
                onStartAction(pool);
            pool.Start();
            return pool;
        }

        /// <summary>
        /// Запускает все пулы, отмеченные в настройках
        /// </summary>
        /// <param name="onStartAction">Действие, выполняемое при запуске каждого пула</param>
        public static void RunPools(Action<IService> onStartAction, Action<Exception, string> onError)
        {
            foreach (StartPoolConfig kv in Settings.StartPools.AsParallel())
            {
                if (Convert.ToBoolean(kv.Start))
                {
                    try
                    {
                       StartPool(kv.PoolName, onStartAction);
                    }
                    catch (Exception ex)
                    {
                        if (onError != null) onError(ex, kv.PoolName);
                    }
                }
            }
        }

        /// <summary>
        /// Останавливает все запущенные пулы
        /// </summary>
        public static void StopPools(Action<IService> action)
        {
            foreach (IService pool in activePools.Values.AsParallel())
            {
                pool.Stop();
                if (action != null) action(pool);
            }
        }

        private static IService GetPool(string poolName)
        {
            if (!activePools.ContainsKey(poolName))
            {
                PoolConfig config = Settings.JobsPools[poolName];
                if (config == null) throw new Exception(string.Format("Невозможно найти настройки пула <{0}> в конфигурационном файле.", poolName));
                Type poolClass = null;
                if (! TypeLoader.TryGetType(config.PoolClass, out poolClass))
                    throw new Exception(string.Format("Невозможно найти тип <{0}>.", config.PoolClass));
                IService pool = (IService)Activator.CreateInstance(poolClass);
                pool.Name = poolName;
                activePools.Add(poolName, pool);
            }
            return activePools[poolName];
        }

        public static string CoreVersion
        {
            get { return Assembly.GetAssembly(typeof (IJobsPool)).GetName().Version.ToString(); }
        }
    }
}