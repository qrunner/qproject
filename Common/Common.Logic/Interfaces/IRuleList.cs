using System.Collections.Generic;
using Common.Logic.XML.SimpleLogic;

namespace Common.Logic.XML.Interfaces
{
    public interface IRuleList : ICheckable
    {
        OperationType Operation { get; set; }

        void Add(string propertyName, string checkValue, bool isEqual = true);

    }
}