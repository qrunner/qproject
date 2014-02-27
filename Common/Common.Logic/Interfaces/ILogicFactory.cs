using System.Collections.Generic;
using System.Xml.Linq;
using Common.Logic.XML.SimpleLogic;

namespace Common.Logic.XML.Interfaces
{
    public interface ILogicFactory
    {
        CheckItem CheckCreate();

        IChecks ChecksCreate();

        IRule RuleCreate();

        IRuleList RulesCreate();

        IConditions ConditionsCreate();

        ICondition ConditionCreate();

        ICheckable Deserialize(XElement xLogicElem);

    }
}