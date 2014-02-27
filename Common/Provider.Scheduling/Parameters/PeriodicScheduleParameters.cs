using System;
using System.Collections.Generic;

namespace Provider.Scheduling.Parameters
{
    public class PeriodicScheduleParameters
    {
        public const string StartParamName = "Start";
        public const string IntervalParamName = "Interval";
        public const string RepeatCountParamName = "RepeatCount";

        public PeriodicScheduleParameters(IDictionary<string, object> parameters)
        {
            Start = Convert.ToDateTime(ParamUtil.GetParameter(StartParamName, parameters));
            Interval = TimeSpan.Parse(Convert.ToString(ParamUtil.GetParameter(IntervalParamName, parameters)));
            RepeatCount = Convert.ToInt32(ParamUtil.GetParameter(RepeatCountParamName, parameters));
        }

        public DateTime Start { get; private set; }
        public TimeSpan Interval { get; private set; }
        public int RepeatCount { get; private set; }
    }
}