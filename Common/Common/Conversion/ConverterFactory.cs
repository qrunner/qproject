using System;
using System.Collections.Generic;

namespace Common.Conversion
{
    /// <summary>
    /// Кеш конвертеров
    /// </summary>
    public static class Conversion
    {
        private static readonly Dictionary<Type, object> Converters = new Dictionary<Type, object>();
        /// <summary>
        /// Получает экземпляр конвертера указанного типа. При первом запросе создается, при последующих берется из кеша.
        /// </summary>
        /// <typeparam name="TConverter">Тип конвертера</typeparam>
        /// <returns>Экземпляр конвертера</returns>
        public static TConverter Use<TConverter>() where TConverter : class, new()
        {
            Type converterType = typeof (TConverter);
            if (!Converters.ContainsKey(converterType))
            {
                lock (Converters)
                {
                    if (!Converters.ContainsKey(converterType))
                        Converters.Add(converterType, Activator.CreateInstance<TConverter>());
                }
            }

            return Converters[converterType] as TConverter;
        }
    }
}