using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Logic.XML
{
    public class ConditionalSettings
    {
        public Condition[] Conditions { get; set; }

        public bool GetValue(string attrName, IDictionary<string, string> parameters, bool defaultValue)
        {
            foreach (var cnd in Conditions)
            {
                bool ro;
                if (Applied(cnd, attrName, parameters, out ro))
                    return ro;
            }
            return defaultValue;
        }

        private bool Applied(Condition condition, string attrName, IDictionary<string, string> parameters, out bool value)
        {
            value = false;
            if (CheckCondition(condition, parameters))
            {
                if (condition.Attributes != null)
                {
                    foreach (var attr in condition.Attributes.Where(attr => attr.Name == attrName))
                    {
                        value = attr.Value;
                        break;
                    }
                }
                if (condition.NestedConditions != null)
                {
                    foreach (var cond in condition.NestedConditions)
                    {
                        bool ro;
                        if (Applied(cond, attrName, parameters, out ro))
                            value = ro;
                    }
                }
                return true;
            }
            return false;
        }

        private static bool Check(string condition, string value)
        {
            return string.IsNullOrEmpty(condition) || string.IsNullOrEmpty(value) || value == condition;
        }

        private static bool CheckCondition(Condition condition, IDictionary<string, string> parameters)
        {
            return condition.Checks.All(prm => parameters.ContainsKey(prm.Name) && Check(prm.Value, parameters[prm.Name]));
        }
    }
}
