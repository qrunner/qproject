using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Conversion.Enums;

namespace Core.Conversion.Interfaces
{
    public interface IConversionEntitysFactory
    {
        //IConversionDiscription GetConversionDiscription();

        IConversionDiscription GetLightConversionDiscription();

        IConversionDiscription<From, To> GetConversionDiscription<From, To>(Func<IConverter<From, To>> getConvFunc);

        IConvertersPool GetConvertersPool();

        IRullesChecker GetRullesChecker(EqualsConverterRulleType rulleType);

        IChainPoolsBuilder GetChainPoolsBuilder();

        /*IConvertersProvider GetConvertersProvider();*/

        IMultiConverter GetMultiConverter();

        IMultiConverter GetMultiConverter(IConvertersProvider convertersProvider);
    }
}
