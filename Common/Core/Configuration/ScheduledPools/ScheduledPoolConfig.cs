using System.Configuration;

namespace Configuration.ScheduledPools
{
    public class ScheduledPoolConfig : ConfigurationElement
    {
        const string _name = "name";
        [ConfigurationProperty(_name, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string PoolName
        {
            get { return ((string)(base[_name])); }
            set { base[_name] = value; }
        }

        const string _class = "class";
        [ConfigurationProperty(_class, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string PoolClass
        {
            get { return ((string)(base[_class])); }
            set { base[_class] = value; }
        }

        const string _trigger = "trigger";
        [ConfigurationProperty(_trigger, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Trigger
        {
            get { return ((string)(base[_trigger])); }
            set { base[_trigger] = value; }
        }

        const string _poolSettings = "settings";
        [ConfigurationProperty(_poolSettings)]
        public KeyValueConfigurationCollection PoolSettings
        {
            get { return (KeyValueConfigurationCollection)base[_poolSettings]; }
            set { base[_poolSettings] = value; }
        }
    }
}
