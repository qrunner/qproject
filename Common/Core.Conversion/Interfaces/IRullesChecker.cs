using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Conversion.Interfaces
{
    public interface IRullesChecker
    {
        bool PatternCheck(IConversionDiscription pattern);
    }
}
