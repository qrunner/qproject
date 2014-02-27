using System.Configuration;

namespace Common.Scheduling.Configuration
{
    [ConfigurationCollection(typeof(ScheduledJobConfig), AddItemName = SchedulingConfigRoot.JobMasterPrefix)]
    public class ScheduledJobConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ScheduledJobConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ScheduledJobConfig)(element)).Name;
        }

        public ScheduledJobConfig this[int idx]
        {
            get { return (ScheduledJobConfig)BaseGet(idx); }
        }

        public ScheduledJobConfig this[string name]
        {
            get { return (ScheduledJobConfig)BaseGet(name); }
        }
    }
}