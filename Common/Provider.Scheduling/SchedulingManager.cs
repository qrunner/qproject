using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Common.Collections;
using Common.Configuration;
using Common.Reflection;
using Common.Scheduling.Configuration;
using Common.ServiceModel;

namespace Common.Scheduling
{
    public static class SchedulingManager
    {
        private static readonly ICollection<IScheduler> Schedulers = new List<IScheduler>();

        public static void ScheduleJobs(Action<Exception, string> onError)
        {
            try
            {
                var settings = (SchedulingConfigRoot) ConfigurationManager.GetSection(SchedulingConfigRoot.SectionName);
                if (settings == null || string.IsNullOrWhiteSpace(settings.SchedulerClass)) return; // no configuration

                var provider = TypeLoader.CreateInstance<ISchedulingProvider>(settings.SchedulerClass);

                foreach (var jobConfig in settings.Jobs.Cast<ScheduledJobConfig>().Where(jobConfig => jobConfig.Scheduled))
                {
                    try
                    {
                        var schedule = settings.Schedules[jobConfig.Schedule];
                        var command = TypeLoader.CreateInstance<IParametrizedCommand>(jobConfig.JobClass);
                        command.Arguments = new DictionaryWrapper(new KeyValueConfigurationCollectionWrapper(jobConfig.Settings));
                        Schedulers.Add(provider.ScheduleJob(jobConfig.Name, provider.CreateSchedule(schedule.Type, schedule.Name, new DictionaryWrapper(new KeyValueConfigurationCollectionWrapper(schedule.Settings))), command.Excecute));
                    }
                    catch (Exception ex)
                    {
                        onError(ex, jobConfig.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                onError(ex, null);
            }
        }

        public static void StopAllJobs()
        {
            foreach (var sch in Schedulers)
            {
                sch.Stop(false);
            }
        }
    }
}