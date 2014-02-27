using System.Configuration;

namespace Configuration
{
    /// <summary>
    /// Настройки режима запуска пула
    /// </summary>
    public class StartScheduledPoolConfig : ConfigurationElement
    {
        const string _name = "name";
        [ConfigurationProperty(_name, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string PoolName
        {
            get { return ((string)(base[_name])); }
            set { base[_name] = value; }
        }

        const string _start = "start";
        [ConfigurationProperty(_start, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Start
        {
            get { return ((string)(base[_start])); }
            set { base[_start] = value; }
        }
    }
}
