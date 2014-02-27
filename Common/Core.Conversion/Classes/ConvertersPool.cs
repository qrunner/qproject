using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Conversion.Interfaces;
using Core.Conversion.Enums;

namespace Core.Conversion.Classes
{
    /// <summary>
    /// Содержит несколько конвертеров, сгруппированных по количеству заданных параметров в описании
    /// </summary>
    public class ConvertersPool : IConvertersPool
    {

        private IConversionDiscription _lastDiscription;

        private IConversionEntitysFactory _factory;

        private IList<IConversionDiscription> _converterDiscrList;


        public IRullesChecker RullesChecker { get; set; }

        public ConvertersPool()
        {
            _factory = new ConversionEntitysFactory();
            _lastDiscription = _factory.GetLightConversionDiscription();
            _converterDiscrList = new List<IConversionDiscription>();
        }

        public IConverter<From, To> GetConverter<From, To>(ConversionType? conversionType, ThirdPartyServiceType? serviceType, bool? toThirdPartyServiceDirection)
        {
             _lastDiscription.ToThirdPartyServiceDirection = toThirdPartyServiceDirection;
             _lastDiscription.ConversionType = conversionType;
             _lastDiscription.ThirdPartyServiceType = serviceType;
            return GetConverter<From, To>();
        }

        public IConverter<From, To> GetConverter<From, To>(ConversionType conversionType, ThirdPartyServiceType serviceType, bool toThirdPartyServiceDirection)
        {
            _lastDiscription.ToThirdPartyServiceDirection = toThirdPartyServiceDirection;
            return GetConverter<From, To>(conversionType, serviceType);
        }

        public IConverter<From, To> GetConverter<From, To>(ConversionType conversionType, ThirdPartyServiceType serviceType)
        {
            _lastDiscription.ThirdPartyServiceType = serviceType;
            return GetConverter<From, To>(conversionType);
        }

        public IConverter<From, To> GetConverter<From, To>(ConversionType conversionType)
        {
            _lastDiscription.ConversionType = conversionType;
            return GetConverter<From, To>();
        }

        public IConverter<From, To> GetConverter<From, To>()
        {

            _lastDiscription.DestType = typeof(To);
            _lastDiscription.SourceType = typeof(From);

            return (IConverter<From, To>) FindConverter(_lastDiscription);
        }

        public IConverter FindConverter(IConversionDiscription discription)
        {
            try
            {

                var finded = GetConverterFromThisPool(discription);

                if (finded == null && ChildConvertersPool != null)
                    finded = ChildConvertersPool.FindConverter(discription);

                return finded;
            }
            catch (Exception e)
            {
                throw new Exception("Произошла ошибка в ходе поиска подходящего конвертера.", e);
            }
            finally
            {
                _lastDiscription = _factory.GetLightConversionDiscription();
            }
        }

        public IConvertersPool ChildConvertersPool { get; set; }

        //public void AddConverter<From, To>(IConversionDiscription discription, IConverter<From, To> converter)
        //{
        //    var pair = new KeyValuePair<IConversionDiscription, IConverter>(discription, converter);
        //    //discription.DestType = typeof(To);
        //    //discription.SourceType = typeof(From);
            

        //    if (RullesChecker.PatternCheck(discription))
        //        _pairList.Add(pair);
        //    else
        //        ChildConvertersPool.AddConverter(discription, converter);
        //}

        //public void AddConverter<From, To>(IConverter<From, To> converter)
        //{
        //    AddConverter(_factory.GetConversionDiscription(), converter);
        //}

        public void AddConverter(IConversionDiscription discription)
        {
            if (RullesChecker.PatternCheck(discription))
                _converterDiscrList.Add(discription);
            else
                ChildConvertersPool.AddConverter(discription);
        }

        private IConverter GetConverterFromThisPool(IConversionDiscription discription)
        {

            var convrters = from discrPattern in _converterDiscrList where discription.PartialEquals(discrPattern) select discrPattern.Converter;

            return convrters.FirstOrDefault();

        }

        public void AddConverter<From, To>(Func<IConverter<From, To>> createConverterFunc)
        {
            var discription = _factory.GetConversionDiscription(createConverterFunc);
            AddConverter(discription);
        }
    }
}
