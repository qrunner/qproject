using System.Collections.Generic;

namespace Common.Logic.XML.Interfaces
{
    public interface IRule : ICheckable
    {
        string PropertyName { get; set; }

        string CheckValue { get; set; }

        bool IsEqual { get; set; }
    }
}