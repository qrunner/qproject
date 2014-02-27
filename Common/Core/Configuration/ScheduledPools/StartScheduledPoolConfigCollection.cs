using System.Configuration;

namespace Configuration.ScheduledPools
{
    [ConfigurationCollection(typeof(StartScheduledPoolConfig), AddItemName = PoolsConfigRoot._startSchedPoolMasterPrefix)]
    public class StartScheduledPoolConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new StartScheduledPoolConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StartScheduledPoolConfig)(element)).PoolName;
        }

        public StartScheduledPoolConfig this[int idx]
        {
            get { return (StartScheduledPoolConfig)BaseGet(idx); }
        }

        public StartScheduledPoolConfig this[string name]
        {
            get { return (StartScheduledPoolConfig)BaseGet(name); }
        }
    }
}
