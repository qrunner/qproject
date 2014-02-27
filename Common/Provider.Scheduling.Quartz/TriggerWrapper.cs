using Quartz;

namespace Common.Scheduling.QuartzNET
{
    internal class TriggerWrapper : ISchedule
    {
        private readonly ITrigger _quartzTrigger;

        public TriggerWrapper(ITrigger quartzTrigger, ScheduleType type)
        {
            _quartzTrigger = quartzTrigger;
            ScheduleType = type;
            //Parameters = parameters;
        }

        public string Name { get; set; }

        public ITrigger QuartzTrigger
        {
            get { return _quartzTrigger; }
        }


        public ScheduleType ScheduleType
        {
            get;
            private set;
        }
    }
}