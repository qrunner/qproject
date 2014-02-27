using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Strings
{
    /// <summary>
    /// Конвертер значений по неточному соответствию строки.
    /// Преобразует строку (INPUT STRING) в соответствующее значение заданного типа (T).
    /// Соответствие производится по вхождению регулярного выражения (REGEX) в INPUT STRING.
    /// Каждому элементу типа (T) сопоставляется свое регулярное выражение.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемых значений</typeparam>
    public class RegexMapper<T>
    {
        private readonly HashSet<KeyValuePair<Regex, T>> _dictionary = new HashSet<KeyValuePair<Regex, T>>();

        /// <summary>
        /// Создает экземпляр объекта
        /// </summary>
        /// <param name="content">Сопоставление регулярных выражений REGEX и значений, им соответствующих</param>
        public RegexMapper(IEnumerable<KeyValuePair<string, T>> content)
        {
            foreach (var item in content)
                _dictionary.Add(new KeyValuePair<Regex, T>(new Regex(item.Key), item.Value));
        }

        /// <summary>
        /// Выполняет поиск значения в указанной строке, используя заданное сопоставление значений и регулярных выражений
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <param name="value">Искомое значение</param>
        /// <returns>Результат поиска</returns>
        public bool Find(string input, out T value)
        {
            value = default(T);

            var item = _dictionary.SingleOrDefault(i => i.Key.IsMatch(input));

            if (Equals(item, default(T)))
                return false;

            value = item.Value;
            return true;
        }
    }
}