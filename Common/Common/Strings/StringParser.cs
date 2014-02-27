using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Strings
{
    /// <summary>
    /// Вспомогательный класс для разбора строк различных форматов
    /// </summary>
    public static class StringParser
    {
        /// <summary>
        /// Преобразовывает строку вида "key1:value1; key2:value2" в словарь
        /// </summary>
        /// <param name="source">Исходная строка</param>
        /// <param name="groupsDelimeter">Разделитель групп (ключ:значение)</param>
        /// <param name="keyValueDelimeter">Разделитель ключа и значения в группе</param>
        /// <returns>Словарь ключ/значение</returns>
        public static Dictionary<string, string> GetDictionary(string source, char groupsDelimeter, char keyValueDelimeter)
        {
            return GetDictionary(source, new[] {groupsDelimeter}, new[] {keyValueDelimeter});
        }

        /// <summary>
        /// Преобразовывает строку вида "key1:value1; key2:value2" в словарь
        /// </summary>
        /// <param name="source">Исходная строка</param>
        /// <param name="groupsDelimeters">Разделители групп (ключ:значение)</param>
        /// <param name="keyValueDelimeters">Разделители ключа и значения в группе</param>
        /// <returns>Словарь ключ/значение</returns>
        public static Dictionary<string, string> GetDictionary(string source, char[] groupsDelimeters, char[] keyValueDelimeters)
        {
            return source.Split(groupsDelimeters, StringSplitOptions.RemoveEmptyEntries).Select(@group => @group.Split(keyValueDelimeters)).Where(keyValue => keyValue.Length > 0).ToDictionary(keyValue => keyValue[0].Trim(), keyValue => keyValue.Length > 1 ? keyValue[1].Trim() : null);
        }

        /// <summary> Преобразование словаря в строку вида "key1:value1; key2:value2"
        /// </summary>
        /// <typeparam name="T">Тип ключа</typeparam>
        /// <typeparam name="N">Тип значения</typeparam>
        /// <param name="source">Словарь</param>
        /// <param name="groupsDelimeter">Разделитель групп (ключ:значение)</param>
        /// <param name="keyValueDelimeter">Разделитель ключа и значения в группе</param>
        /// <returns>Строка вида "key1:value1; key2:value2"</returns>
        public static string JoinDictionary<T, N>(IDictionary<T, N> source, char groupsDelimeter, char keyValueDelimeter)
        {
            StringBuilder res = new StringBuilder();

            foreach (var item in source)
            {
                res.AppendFormat("{0}{1}{2}{3}", item.Key, keyValueDelimeter, item.Value, groupsDelimeter);
            }

            return res.ToString();
        }
    }
}