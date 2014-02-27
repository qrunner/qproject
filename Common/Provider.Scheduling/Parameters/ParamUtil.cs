using System;
using System.Collections.Generic;

namespace Provider.Scheduling.Parameters
{
    internal static class ParamUtil
    {
        public static object GetParameter(string paramName, IDictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey(paramName)) throw new KeyNotFoundException("В настройках расписания не найден параметр " + paramName);
            if (parameters[paramName] == null) throw new NullReferenceException("В настройках расписания не задано значение параметра " + paramName);
            return parameters[paramName];
        }
    }
}