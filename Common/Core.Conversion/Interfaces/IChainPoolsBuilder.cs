using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Conversion.Enums;

namespace Core.Conversion.Interfaces
{
    public interface IChainPoolsBuilder
    {
        IConvertersPool Build(IEnumerable<EqualsConverterRulleType> rullesSequence);

        IConvertersPool Build();
    }
}
