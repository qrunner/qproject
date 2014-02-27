using System;
using Core.Conversion.Interfaces;

namespace Core.Conversion.Classes
{
    /// <summary>
    /// Базовый класс конвертера
    /// </summary>
    /// <typeparam name="From"></typeparam>
    /// <typeparam name="To"></typeparam>
    public abstract class Converter<From, To> : IConverter<From, To>
    {
        public abstract To Convert(From source);

        public object Convert(object source)
        {
            if (!(source is From))
                throw new Exception("Тип передаваемого значения не отвечает спецификации конвертера.");
            return Convert((From)source);
        }

        public Type FromType
        {
            get { return typeof(From); }
        }

        public Type ToType
        {
            get { return typeof(To); }
        }
    }
}