using System.Configuration;

namespace Configuration.ScheduledPools.Triggers
{
    [ConfigurationCollection(typeof(TriggerConfig), AddItemName = PoolsConfigRoot._triggerMasterPrefix)]
    public class TriggerConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TriggerConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TriggerConfig)(element)).TriggerName;
        }

        public TriggerConfig this[int idx]
        {
            get { return (TriggerConfig)BaseGet(idx); }
        }

        public TriggerConfig this[string name]
        {
            get { return (TriggerConfig)BaseGet(name); }
        }
    }
}
