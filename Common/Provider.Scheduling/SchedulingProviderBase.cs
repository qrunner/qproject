using System;
using System.Collections.Generic;

namespace Common.Scheduling
{
    public abstract class SchedulingProviderBase : ISchedulingProvider
    {
        public abstract IScheduler CreateScheduler();

        public IScheduler ScheduleJob(string jobName, ISchedule schedule, Action action)
        {
            var scheduler = CreateScheduler();
            scheduler.ScheduleJob(jobName, schedule, action);
            return scheduler;
        }

        public abstract ISchedule CreateSchedule(ScheduleType scheduleType, string name, IDictionary<string, object> parameters);
    }
}
