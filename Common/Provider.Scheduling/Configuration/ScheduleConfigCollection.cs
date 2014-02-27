using System.Configuration;

namespace Common.Scheduling.Configuration
{
    [ConfigurationCollection(typeof(ScheduleConfig), AddItemName = SchedulingConfigRoot.ScheduleMasterPrefix)]
    public class ScheduleConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ScheduleConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ScheduleConfig)(element)).Name;
        }

        public ScheduleConfig this[int idx]
        {
            get { return (ScheduleConfig)BaseGet(idx); }
        }

        public ScheduleConfig this[string name]
        {
            get { return (ScheduleConfig)BaseGet(name); }
        }
    }
}