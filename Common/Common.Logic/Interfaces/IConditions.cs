using System.Collections.Generic;

namespace Common.Logic.XML.Interfaces
{
    public interface IConditions : ICheckable
    {
        bool Check(string conditionName, IDictionary<string, string> circs);

        void Serialize(string pathToDoc);
    }
}