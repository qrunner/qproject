using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Conversion.Enums;

namespace Core.Conversion.Interfaces
{
    public interface IMultiConverter
    {
        IConvertersPool ConvertersPool { get; set; }

        ThirdPartyServiceType? ThirdPartyServiceType { get; set; }

        bool? ToThirdPartyServiceDirection { get; set; }

        void AddConverters(IConvertersProvider provider);

        To Convert<From, To>(From source);

        To Convert<From, To>(From source, ConversionType conversionType);
    }
}
