using System.Collections.Generic;
using System.Linq;

namespace Common.Comparing
{
    /// <summary>
    /// Класс для сравнения переменной с несколькими значениями
    /// </summary>
    public static class MultiCompare
    {
        /// <summary>
        /// Проверяет равенство переменной одному из заданных значений, используя компаратор по умолчанию для типа Т
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых элементов</typeparam>
        /// <param name="variable">Переменная</param>
        /// <param name="constants">Список проверяемых значений</param>
        /// <returns>True, если переменная равна хотя бы одному из значений. False - если совпадений не найдено</returns>
        public static bool OneOf<T>(this T variable, params T[] constants)
        {
            return OneOf(variable, EqualityComparer<T>.Default, constants);
        }
        /// <summary>
        /// Проверяет равенство переменной одному из заданных значений, используя заданный компаратор
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых элементов</typeparam>
        /// <param name="variable">Переменная</param>
        /// <param name="constants">Список проверяемых значений</param>
        /// <param name="comparer">Компаратор</param>
        /// <returns>True, если переменная равна хотя бы одному из значений. False - если совпадений не найдено</returns>
        public static bool OneOf<T>(this T variable, IEqualityComparer<T> comparer, params T[] constants)
        {
            return constants.Any(c => comparer.Equals(c, variable));
        }

        /// <summary>
        /// Проверяет неравенство переменной ни одному из заданных значений, используя компаратор по умолчанию для типа Т
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых элементов</typeparam>
        /// <param name="variable">Переменная</param>
        /// <param name="constants">Список проверяемых значений</param>
        /// <returns>True, если переменная не равна ни одному из значений. False - если есть совпадения</returns>
        public static bool NotIn<T>(this T variable, params T[] constants)
        {
            return NotIn(variable, EqualityComparer<T>.Default, constants);
        }
        /// <summary>
        /// Проверяет неравенство переменной ни одному из заданных значений, используя заданный компаратор
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых элементов</typeparam>
        /// <param name="variable">Переменная</param>
        /// <param name="constants">Список проверяемых значений</param>
        /// <param name="comparer">Компаратор</param>
        /// <returns>True, если переменная не равна ни одному из значений. False - если есть совпадения</returns>
        public static bool NotIn<T>(this T variable, IEqualityComparer<T> comparer, params T[] constants)
        {
            return !constants.Any(c => comparer.Equals(c, variable));
        }
        /// <summary>
        /// Проверяет неравенство переменной значению по умолчанию для ее типа
        /// </summary>
        /// <typeparam name="T">Тип переменной</typeparam>
        /// <param name="variable">Переменная</param>
        /// <returns>True - если значение переменной отличается от значения по умолчанию для ее типа, в противном случае - False</returns>
        public static bool NotDefault<T>(this T variable)
        {
            return !Equals(variable, default(T));
        }
    }
}