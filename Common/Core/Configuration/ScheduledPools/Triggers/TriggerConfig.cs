using System.Configuration;

namespace Configuration.ScheduledPools.Triggers
{
    public class TriggerConfig : ConfigurationElement
    {
        const string _name = "name";
        [ConfigurationProperty(_name, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string TriggerName
        {
            get { return ((string)(base[_name])); }
            set { base[_name] = value; }
        }

        const string _class = "class";
        [ConfigurationProperty(_class, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string TriggerClass
        {
            get { return ((string)(base[_class])); }
            set { base[_class] = value; }
        }

        const string _poolSettings = "settings";
        [ConfigurationProperty(_poolSettings)]
        public KeyValueConfigurationCollection TriggerSettings
        {
            get { return (KeyValueConfigurationCollection)base[_poolSettings]; }
            set { base[_poolSettings] = value; }
        }
    }
}
