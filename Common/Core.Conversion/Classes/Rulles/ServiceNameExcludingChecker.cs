using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Conversion.Interfaces;

namespace Core.Conversion.Classes.Rulles
{
    public class ServiceNameExcludingChecker : IRullesChecker
    {
        public bool PatternCheck(IConversionDiscription pattern)
        {
            if (pattern.ConversionType != null && pattern.DestType != null && pattern.ToThirdPartyServiceDirection == null && pattern.ThirdPartyServiceType == null
             && pattern.SourceType != null)
                return true;
            return false;
        }
    }
}
