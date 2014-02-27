using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Conversion.Enums;

namespace Core.Conversion.Interfaces
{
    public interface IConvertersPool
    {
        IConverter<From, To> GetConverter<From, To>(ConversionType? conversionType, ThirdPartyServiceType? serviceType, bool? toThirdPartyServiceDirection);

        IConverter<From, To> GetConverter<From, To>(ConversionType conversionType, ThirdPartyServiceType serviceType, bool toThirdPartyServiceDirection);

        IConverter<From, To> GetConverter<From, To>(ConversionType conversionType, ThirdPartyServiceType serviceType);

        IConverter<From, To> GetConverter<From, To>(ConversionType conversionType);

        IConverter<From, To> GetConverter<From, To>();

        IConverter FindConverter(IConversionDiscription discription);

        IConvertersPool ChildConvertersPool { get; set; }

        IRullesChecker RullesChecker { get; set; }

        void AddConverter(IConversionDiscription discription);

        void AddConverter<From, To>(Func<IConverter<From, To>> createConverterFunc);

    }
}
