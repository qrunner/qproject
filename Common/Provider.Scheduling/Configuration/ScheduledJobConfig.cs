using System.Configuration;

namespace Common.Scheduling.Configuration
{
    public class ScheduledJobConfig : ConfigurationElement
    {
        private const string NamePrefix = "name";

        [ConfigurationProperty(NamePrefix, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return ((string) (base[NamePrefix])); }
            set { base[NamePrefix] = value; }
        }

        private const string ClassPrefix = "class";

        [ConfigurationProperty(ClassPrefix, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string JobClass
        {
            get { return ((string) (base[ClassPrefix])); }
            set { base[ClassPrefix] = value; }
        }

        private const string SchedulePrefix = "schedule";

        [ConfigurationProperty(SchedulePrefix, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Schedule
        {
            get { return ((string) (base[SchedulePrefix])); }
            set { base[SchedulePrefix] = value; }
        }

        private const string JobSettingsPrefix = "settings";

        [ConfigurationProperty(JobSettingsPrefix)]
        public KeyValueConfigurationCollection Settings
        {
            get { return (KeyValueConfigurationCollection) base[JobSettingsPrefix]; }
            set { base[JobSettingsPrefix] = value; }
        }

        public bool Scheduled
        {
            get { return !string.IsNullOrEmpty(Schedule); }
        }
    }
}