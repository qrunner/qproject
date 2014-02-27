using System;
using System.Collections.Generic;
using Provider.Scheduling.Parameters;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace Common.Scheduling.QuartzNET
{
    public class SchedulingProvider : SchedulingProviderBase
    {
        public override IScheduler CreateScheduler()
        {
            return new SchedulerWrapper(StdSchedulerFactory.GetDefaultScheduler());
        }

        public override ISchedule CreateSchedule(ScheduleType scheduleType, string name, IDictionary<string, object> parameters)
        {
            switch (scheduleType)
            {
                case ScheduleType.Periodic:
                    var triggerSettings = new PeriodicScheduleParameters(parameters);
                    var trigger = new SimpleTriggerImpl
                    {
                        Name = name,
                        RepeatCount = triggerSettings.RepeatCount, 
                        StartTimeUtc = triggerSettings.Start, 
                        RepeatInterval = triggerSettings.Interval
                    };
                    return new TriggerWrapper(trigger, scheduleType);
                
                case ScheduleType.CronBased:
                    var triggerSettingsCron = new CronScheduleParameters(parameters);
                    var triggerCron = new CronTriggerImpl
                    {
                        Name = name,
                        CronExpressionString = triggerSettingsCron.CronString
                    };
                    return new TriggerWrapper(triggerCron, scheduleType);

                default:
                    throw new Exception("Unsupported schedule type " + scheduleType.ToString());
            }
        }
    }
}
