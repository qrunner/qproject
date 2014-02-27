using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Conversion.Interfaces;
using Core.Conversion.Enums;
using Core.Conversion.Classes.Rulles;

namespace Core.Conversion.Classes
{
    /// <summary>
    /// Абстрактная фабрика классов текущей сборки
    /// </summary>
    public class ConversionEntitysFactory : IConversionEntitysFactory
    {
        //public IConversionDiscription GetConversionDiscription()
        //{
        //    return new ConversionDiscription<object, object>();
        //}

        public IConversionDiscription GetLightConversionDiscription()
        {
            return new LightConversionDiscription();
        }

        public IConversionDiscription<From, To> GetConversionDiscription<From, To>(Func<IConverter<From, To>> getConvFunc)
        {
            return new ConversionDiscription<From, To>(getConvFunc);
        }

        public IConvertersPool GetConvertersPool()
        {
            return new ConvertersPool();
        }

        public IRullesChecker GetRullesChecker(EqualsConverterRulleType rulleType)
        {
            switch (rulleType)
            {
                case EqualsConverterRulleType.Full:
                    return new FullMatchChecker();
                case EqualsConverterRulleType.DirectionExcluding:
                    return new DirectionExcludingChecker();
                case EqualsConverterRulleType.ServiceNameExcluding:
                    return new ServiceNameExcludingChecker();
                case EqualsConverterRulleType.ConversionTypeExcluding:
                    return new ConversionTypeChecker();
            }
            return null;
        }

        public IChainPoolsBuilder GetChainPoolsBuilder()
        {
            return new ChainPoolsBuilder();
        }

        /*public IConvertersProvider GetConvertersProvider()
        {
            return new ConvertersProvider();
        }*/

        public IMultiConverter GetMultiConverter()
        {
            return new MultiConverter();
        }

        public IMultiConverter GetMultiConverter(IConvertersProvider convertersProvider)
        {
            return new MultiConverter(convertersProvider);
        }
    }
}
