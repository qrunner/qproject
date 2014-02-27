using System;
using System.Configuration;

namespace Common.Scheduling.Configuration
{
    /// <summary>
    /// Настройки режима запуска пула
    /// </summary>
    public class ScheduleConfig : ConfigurationElement
    {
        const string NamePrefix = "name";
        [ConfigurationProperty(NamePrefix, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string)(base[NamePrefix])); }
            set { base[NamePrefix] = value; }
        }

        const string TypePrefix = "type";
        [ConfigurationProperty(TypePrefix, DefaultValue = ScheduleType.CronBased, IsKey = false, IsRequired = true)]
        public ScheduleType Type
        {
            get { return (ScheduleType)base[TypePrefix]; }
            set { base[TypePrefix] = value; }
        }

        const string DescriptionPrefix = "description";
        [ConfigurationProperty(DescriptionPrefix, DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Description
        {
            get { return ((string)(base[DescriptionPrefix])); }
            set { base[DescriptionPrefix] = value; }
        }

        private const string SettingsPrefix = "settings";

        [ConfigurationProperty(SettingsPrefix)]
        public KeyValueConfigurationCollection Settings
        {
            get { return (KeyValueConfigurationCollection)base[SettingsPrefix]; }
            set { base[SettingsPrefix] = value; }
        }
    }
}