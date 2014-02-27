using System.Configuration;

namespace Configuration.ScheduledPools
{
    [ConfigurationCollection(typeof(ScheduledPoolConfig), AddItemName = PoolsConfigRoot._masterSchedPrefix)]
    public class ScheduledPoolConfigCollection : ConfigurationElementCollection
    {
        /*public PoolConfig GetByName(string jobName)
        {
            foreach (PoolConfig item in this)
            {
                if (item.PoolName == jobName)
                    return item;
            }
            throw new ArgumentException(string.Format("Элемент Job c именем <{0}> не найден.", jobName), "jobName");
        }*/

        protected override ConfigurationElement CreateNewElement()
        {
            return new ScheduledPoolConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ScheduledPoolConfig)(element)).PoolName;
        }

        public ScheduledPoolConfig this[int idx]
        {
            get { return (ScheduledPoolConfig)BaseGet(idx); }
        }

        public ScheduledPoolConfig this[string name]
        {
            get { return (ScheduledPoolConfig)BaseGet(name); }
        }
    }
}
