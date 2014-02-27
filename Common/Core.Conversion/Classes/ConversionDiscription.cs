using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Conversion.Enums;
using Core.Conversion.Interfaces;

namespace Core.Conversion.Classes
{
    /// <summary>
    /// Описание конвертера. Содержит функцию отложенной загрузки конвертера. Знает как создать конвертер.
    /// создает при первом запросе.
    /// </summary>
    /// <typeparam name="From"></typeparam>
    /// <typeparam name="To"></typeparam>
    public class ConversionDiscription<From, To> : LightConversionDiscription, IConversionDiscription<From, To>
    {

        IConverter _converter;
        private readonly Func<IConverter<From, To>> _createConverterFunc;

        public ConversionDiscription()
         {
             DestType = typeof(To);
             SourceType = typeof(From);
         }

        public ConversionDiscription(Func<IConverter<From, To>> getConvFunc)
        {
            DestType = typeof(To);
            SourceType = typeof(From);
            _createConverterFunc = getConvFunc;
        }



         IConverter<From, To> IConversionDiscription<From, To>.Converter
        {
            get
            {
                return (IConverter<From, To>)Converter;
                   
            }
        }

        public override IConverter Converter
        {
            get
            {
                if (_converter == null)
                    _converter = _createConverterFunc();
                return _converter;
            }
        }


        



    }
}
