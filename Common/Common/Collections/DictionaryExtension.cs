using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Collections
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// Проверяет наличие значений заданных параметров в словаре. В случае отсутствия - вызывается исключение ArgumentException.
        /// </summary>
        /// <param name="dictionary">Словарь параметров</param>
        /// <param name="keys">Список параметров</param>
        public static void CheckValuesNotEmpty(this IDictionary<string, string> dictionary, params string[] keys)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");

            StringBuilder sb = new StringBuilder();

            foreach (var key in keys)
            {
                if (!dictionary.ContainsKey(key))
                    sb.AppendFormat("В словаре отсутствует параметр <{0}>\r\n", key);

                if (string.IsNullOrWhiteSpace(dictionary[key]))
                    sb.AppendFormat("Не задано значение параметра <{0}>\r\n", key);
            }

            if (sb.Length > 0)
                throw new ArgumentException(sb.ToString());
        }
    }
}