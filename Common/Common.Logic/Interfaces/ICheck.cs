using System.Collections.Generic;

namespace Common.Logic.XML.Interfaces
{
    public interface ICheck : IChckBase
    {
        IRuleList Rules { get; }
    }
}