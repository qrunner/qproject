using System.Configuration;

namespace Configuration
{
    [ConfigurationCollection(typeof(StartPoolConfig), AddItemName = PoolsConfigRoot._startPoolMasterPrefix)]
    public class StartPoolConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new StartPoolConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StartPoolConfig)(element)).PoolName;
        }

        public StartPoolConfig this[int idx]
        {
            get { return (StartPoolConfig)BaseGet(idx); }
        }

        public StartPoolConfig this[string name]
        {
            get { return (StartPoolConfig)BaseGet(name); }
        }
    }
}
