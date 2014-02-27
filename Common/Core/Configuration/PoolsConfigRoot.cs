using System.Configuration;
using Configuration.ScheduledPools;
using Configuration.ScheduledPools.Triggers;

namespace Configuration
{
    /// <summary>
    /// Корневой элемент конфигурации
    /// </summary>
    public class PoolsConfigRoot : ConfigurationSection
    {
        internal const string _masterPrefix = "jobPool";
        internal const string _masterSchedPrefix = "jobScheduledPool";
        public const string SectionName = _masterPrefix + "sConfig";

        const string _JobPools = _masterPrefix + "s";
        /// <summary>
        /// Настройки стандартных пулов
        /// </summary>
        [ConfigurationProperty(_JobPools)]
        public PoolConfigCollection JobsPools { get { return (PoolConfigCollection)base[_JobPools]; } }
        
        const string _JobSchedPools = _masterSchedPrefix + "s";
        /// <summary>
        /// Настройки запланированных пулов
        /// </summary>
        [ConfigurationProperty(_JobSchedPools)]
        public ScheduledPoolConfigCollection JobSchedPools { get { return (ScheduledPoolConfigCollection)base[_JobSchedPools]; } }

        
        internal const string _startPoolMasterPrefix = "startPool";
        const string _StartPools = _startPoolMasterPrefix + "s";
        /// <summary>
        /// Запуск стандартных пулов
        /// </summary>
        [ConfigurationProperty(_StartPools)]
        public StartPoolConfigCollection StartPools { get { return (StartPoolConfigCollection)base[_StartPools]; } }
        //flushInterval="10000" flushCount="200"

        internal const string _startSchedPoolMasterPrefix = "startScheduledPool";
        const string _StartSchedPools = _startSchedPoolMasterPrefix + "s";
        /// <summary>
        /// Запуск запланированных пулов
        /// </summary>
        [ConfigurationProperty(_StartSchedPools)]
        public StartScheduledPoolConfigCollection StartScheduledPools { get { return (StartScheduledPoolConfigCollection)base[_StartSchedPools]; } }

        internal const string _triggerMasterPrefix = "trigger";
        const string _Trigger = _triggerMasterPrefix + "s";
        /// <summary>
        /// Настройки триггеров
        /// </summary>
        [ConfigurationProperty(_Trigger)]
        public TriggerConfigCollection Triggers { get { return (TriggerConfigCollection)base[_Trigger]; } }

        const string _poolSequence = "poolSequence";
        [ConfigurationProperty(_poolSequence)]
        public PoolSequenceConfigCollection PoolSequence { get { return (PoolSequenceConfigCollection)base[_poolSequence]; } }
        /*
        const string _flushInterval = "flushInterval";
        [ConfigurationProperty(_flushInterval, DefaultValue = 10000, IsKey = false, IsRequired = false)]
        public int FlushInterval
        {
            get { return ((int)(base[_flushInterval])); }
            set { base[_flushInterval] = value; }
        }

        const string _flushCount = "flushCount";
        [ConfigurationProperty(_flushCount, DefaultValue = 200, IsKey = false, IsRequired = false)]
        public int FlushCount
        {
            get { return ((int)(base[_flushCount])); }
            set { base[_flushCount] = value; }
        }
        */
        const string _statProvider = "statProvider";
        [ConfigurationProperty(_statProvider, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string StatProviderName
        {
            get { return ((string)(base[_statProvider])); }
            set { base[_statProvider] = value; }
        }

        const string _saveStat = "saveStat";
        [ConfigurationProperty(_saveStat, DefaultValue = false, IsKey = false, IsRequired = false)]
        public bool SaveStat
        {
            get { return (bool)base[_saveStat]; }
            set { base[_saveStat] = value; }
        }
    }
}