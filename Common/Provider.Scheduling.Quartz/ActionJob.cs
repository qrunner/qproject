using System;
using Quartz;

namespace Common.Scheduling.QuartzNET
{
    class ActionJob : IJob
    {
        public const string ActionParamName = "action";

        public void Execute(IJobExecutionContext context)
        {
            var action = (Action)context.JobDetail.JobDataMap[ActionParamName];
            action();
        }
    }
}