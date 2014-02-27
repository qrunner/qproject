using System;
using System.Collections.Generic;
using System.Configuration;
using Common.Configuration;
using Configuration;

namespace Configuration
{
    /// <summary>
    /// Настройки пула
    /// </summary>
    public class PoolConfig : ConfigurationElement
    {
        public PoolConfig()
        {
            _poolSettingsWrapper = new KeyValueConfigurationCollectionWrapper(PoolSettingsInternal);
        }

        public static PoolConfig Load(string poolName)
        {
            PoolsConfigRoot section = (PoolsConfigRoot) ConfigurationManager.GetSection(PoolsConfigRoot.SectionName);
            PoolConfig retval = section.JobsPools[poolName];
            if (retval == null)
                throw new Exception(string.Format("Невозможно найти настройки пула <{0}> в конфигурационном файле.", poolName));
            return retval;
        }

        private const string _name = "name";

        [ConfigurationProperty(_name, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string PoolName
        {
            get { return ((string) (base[_name])); }
            set { base[_name] = value; }
        }

        private const string _class = "class";

        [ConfigurationProperty(_class, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string PoolClass
        {
            get { return ((string) (base[_class])); }
            set { base[_class] = value; }
        }

        private const string _reloadTimeout = "reloadTimeout";

        [ConfigurationProperty(_reloadTimeout, DefaultValue = 5000, IsKey = false, IsRequired = false)]
        public int ReloadTimeout
        {
            get { return ((int) (base[_reloadTimeout])); }
            set { base[_reloadTimeout] = value; }
        }

        private const string _reloadMargin = "reloadMargin";

        [ConfigurationProperty(_reloadMargin, DefaultValue = 5, IsKey = false, IsRequired = false)]
        public int ReloadMargin
        {
            get { return ((int) (base[_reloadMargin])); }
            set { base[_reloadMargin] = value; }
        }

        private const string _threadsCount = "threadsCount";

        [ConfigurationProperty(_threadsCount, DefaultValue = 5, IsKey = false, IsRequired = false)]
        public int ThreadsCount
        {
            get { return ((int) (base[_threadsCount])); }
            set { base[_threadsCount] = value; }
        }

        private const string _minThreadsCount = "minThreadsCount";

        [ConfigurationProperty(_minThreadsCount, DefaultValue = 1, IsKey = false, IsRequired = false)]
        public int MinThreadsCount
        {
            get { return ((int) (base[_minThreadsCount])); }
            set { base[_minThreadsCount] = value; }
        }

        private const string _maxThreadsCount = "maxThreadsCount";

        [ConfigurationProperty(_maxThreadsCount, DefaultValue = 100, IsKey = false, IsRequired = false)]
        public int MaxThreadsCount
        {
            get { return ((int) (base[_maxThreadsCount])); }
            set { base[_maxThreadsCount] = value; }
        }

        private const string _maxPickCount = "maxPickCount";

        [ConfigurationProperty(_maxPickCount, DefaultValue = 0, IsKey = false, IsRequired = false)]
        public int MaxPickCount
        {
            get { return ((int) (base[_maxPickCount])); }
            set { base[_maxPickCount] = value; }
        }

        private const string _errorRetryCount = "errorRetryCount";

        /// <summary>
        /// Количество отк
        /// </summary>
        [ConfigurationProperty(_errorRetryCount, DefaultValue = 0, IsKey = false, IsRequired = false)]
        public int ErrorRetryCount
        {
            get { return ((int) (base[_errorRetryCount])); }
            set { base[_errorRetryCount] = value; }
        }

        /*
        const string _rollbackToQueue = "rollbackToQueue";
        [ConfigurationProperty(_rollbackToQueue, DefaultValue = false, IsKey = false, IsRequired = false)]
        public bool RollbackToQueue
        {
            get { return ((bool)(base[_rollbackToQueue])); }
            set { base[_rollbackToQueue] = value; }
        }
        */
        private const string _statInterval = "statInterval";

        [ConfigurationProperty(_statInterval, DefaultValue = 10000, IsKey = false, IsRequired = false)]
        public int StatInterval
        {
            get { return ((int) (base[_statInterval])); }
            set { base[_statInterval] = value; }
        }

        /*const string _poolId = "poolId";
        [ConfigurationProperty(_poolId, IsKey = true, IsRequired = true)]
        public int PoolId
        {
            get { return ((int)(base[_poolId])); }
            set { base[_poolId] = value; }
        }*/

        //const string _dbReloadTimeout = "dbReloadTimeout";
        [ConfigurationProperty("path", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Path
        {
            get { return (string) (base["path"]); }
            set { base["path"] = value; }
        }

        private const string _nextPools = "nextPools";

        [ConfigurationProperty(_nextPools)]
        public ToPoolsCollection NextPools
        {
            get { return (ToPoolsCollection) base[_nextPools]; }
        }

        private const string _poolSettings = "settings";

        [ConfigurationProperty(_poolSettings)]
        protected KeyValueConfigurationCollection PoolSettingsInternal
        {
            get { return (KeyValueConfigurationCollection) base[_poolSettings]; }
            set { base[_poolSettings] = value; }
        }

        private readonly KeyValueConfigurationCollectionWrapper _poolSettingsWrapper;

        public IDictionary<string, string> PoolSettings
        {
            get { return _poolSettingsWrapper; }
        }
    }
}