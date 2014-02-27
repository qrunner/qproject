using System;
using System.Collections.Generic;

namespace Provider.Scheduling.Parameters
{
    public class CronScheduleParameters
    {
        public const string CronStringParamName = "CronString";

        public CronScheduleParameters(IDictionary<string, object> parameters)
        {
            CronString = Convert.ToString(ParamUtil.GetParameter(CronStringParamName, parameters));
        }

        public string CronString { get; private set; }
    }
}