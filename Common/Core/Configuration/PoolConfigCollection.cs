using System.Configuration;
using Configuration;

namespace Configuration
{
    /// <summary>
    /// Список
    /// </summary>
    [ConfigurationCollection(typeof(PoolConfig), AddItemName = PoolsConfigRoot._masterPrefix)]
    public class PoolConfigCollection : ConfigurationElementCollection
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
            return new PoolConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PoolConfig)(element)).PoolName;
        }

        public PoolConfig this[int idx]
        {
            get { return (PoolConfig)BaseGet(idx); }
        }

        public PoolConfig this[string name]
        {
            get { return (PoolConfig)BaseGet(name); }
        }
    }
}
