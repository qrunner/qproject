using System;
using Quartz.Impl;

namespace Common.Scheduling.QuartzNET
{
    internal class SchedulerWrapper : IScheduler
    {
        private readonly Quartz.IScheduler _quartzScheduler;

        public SchedulerWrapper(Quartz.IScheduler quartzScheduler)
        {
            _quartzScheduler = quartzScheduler;
        }

        public void ScheduleJob(string jobName, ISchedule schedule, Action action)
        {
            var jobDetail = new JobDetailImpl(jobName, typeof (ActionJob));
            jobDetail.JobDataMap[ActionJob.ActionParamName] = action;
            _quartzScheduler.ScheduleJob(jobDetail, ((TriggerWrapper) schedule).QuartzTrigger);
            _quartzScheduler.Start();
        }

        public void Stop(bool waitToComplete)
        {
            _quartzScheduler.Shutdown(false);
        }
    }
}