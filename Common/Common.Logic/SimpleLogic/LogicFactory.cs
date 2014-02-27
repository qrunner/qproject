using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Common.Logic.XML.Interfaces;

namespace Common.Logic.XML.SimpleLogic
{
    public class LogicFactory : ILogicFactory
    {
        public CheckItem CheckCreate()
        {
            return new CheckItem();
        }

        public IChecks ChecksCreate()
        {
            return new Checks(); //List<ICheck>();
        }

        public IRule RuleCreate()
        {
            return new Rule();
        }

        public IRuleList RulesCreate()
        {
             return new RuleList();
        }

        public IConditions ConditionsCreate()
        {
            return new Conditions();
        }

        public ICondition ConditionCreate()
        {
            return new Condition();
        }


        public ICheckable Create(string entityName)
        {
            switch (entityName)
            {
                case Constants.Check: return CheckCreate();
                case Constants.Checks: return ChecksCreate();
                case Constants.Condition: return ConditionCreate();
                case Constants.Conditions: return ConditionsCreate();
                case Constants.Rule: return RuleCreate();
                case Constants.Rules: return RulesCreate();
            }
            throw new Exception(string.Format("Фабрика не может создать объект по имени {0}", entityName));
        }

        public ICheckable Deserialize(XElement xLogicElem)
        {
            var emptyItem = Create(xLogicElem.Name.ToString());
            emptyItem.DeSerialize(xLogicElem);
            return emptyItem;
        }

    }
}