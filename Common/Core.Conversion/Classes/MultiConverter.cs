using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Conversion.Interfaces;
using Core.Conversion.Enums;

namespace Core.Conversion.Classes
{
    /// <summary>
    /// Преобразует данные, является общим фасадом для поиска конвертеров для заданного преобразования
    /// </summary>
    public class MultiConverter : IMultiConverter
    {
        public MultiConverter(IConvertersProvider provider) : this()
        {
            AddConverters(provider);
        }

        public MultiConverter()
        {
            var factory = new ConversionEntitysFactory();
            var poolBuilder = factory.GetChainPoolsBuilder();
            ConvertersPool = poolBuilder.Build();
        }

        public void AddConverters(IConvertersProvider provider)
        {
            provider.ConvertersPool = ConvertersPool;
            provider.Provide();
            provider.ConvertersPool = null;
        }

        public IConvertersPool ConvertersPool { get; set; }

        public ThirdPartyServiceType? ThirdPartyServiceType { get; set; }

        public bool? ToThirdPartyServiceDirection { get; set; }

        public To Convert<From, To>(From source)
        {
            if (ConvertersPool == null)
                throw new Exception("Сначало необходимо задать ConvertersPool.");
            var converter = ConvertersPool.GetConverter<From, To>();
            return converter.Convert(source);
        }

        public To Convert<From, To>(From source, ConversionType conversionType)
        {
            if (ConvertersPool == null)
                throw new Exception("Сначало необходимо задать ConvertersPool.");
            var converter = ConvertersPool.GetConverter<From, To>(conversionType, ThirdPartyServiceType, ToThirdPartyServiceDirection);
            return converter.Convert(source);
        }
    }
}
