using System.Configuration;

namespace Common.Scheduling.Configuration
{
    /// <summary>
    /// Корневой элемент конфигурации
    /// </summary>
    public class SchedulingConfigRoot : ConfigurationSection
    {
        private const string SchedulerClassPrefix = "schedulerClass";
        public const string SectionName = "scheduling";

        /// <summary>
        /// Класс, реализующий ISchedulingProvider
        /// </summary>
        [ConfigurationProperty(SchedulerClassPrefix, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string SchedulerClass
        {
            get { return (string) base[SchedulerClassPrefix]; }
        }

        internal const string ScheduleMasterPrefix = "schedule";
        private const string SchedulesMasterPrefix = ScheduleMasterPrefix + "s";

        /// <summary>
        /// Список расписаний
        /// </summary> 
        [ConfigurationProperty(SchedulesMasterPrefix)]
        public ScheduleConfigCollection Schedules
        {
            get { return (ScheduleConfigCollection) base[SchedulesMasterPrefix]; }
        }

        internal const string JobMasterPrefix = "job";
        private const string JobsMasterPrefix = JobMasterPrefix + "s";

        /// <summary>
        /// Список задач
        /// </summary>
        [ConfigurationProperty(JobsMasterPrefix)]
        public ScheduledJobConfigCollection Jobs
        {
            get { return (ScheduledJobConfigCollection) base[JobsMasterPrefix]; }
        }
    }
}