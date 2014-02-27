using System.Xml.Linq;
using Common.Logic.XML.Interfaces;

namespace Common.Logic.XML.SimpleLogic
{
    public class Constants
    {
        public const string Check = "Check";

        public const string Checks = "Checks";
        public const string Condition = "Condition";

        public const string Conditions = "Conditions";

        public const string Rule = "Rule";

        public const string Rules = "Rules";

        public static readonly ILogicFactory Factory = new LogicFactory();

        public const string NameAttrName = "Name";

        public const string DefaultValueAttrName = "DefaultValue";

        public const string PropertyNameAttrName = "PropertyName";

        public const string CheckValueAttrName = "CheckValue";

        public const string IsEqualAttrName = "IsEqual";

        public const string OperationAttrName = "Operation";
    }
}
