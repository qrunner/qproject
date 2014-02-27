using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Collections
{
    /// <summary>
    /// Предоставляет методы расширения для массивов
    /// </summary>
    public static class ArrayExtension
    {
        /// <summary>
        /// Проверяет массив на != null и наличие хотя бы одного элемента
        /// </summary>
        /// <typeparam name="T">Тип массива</typeparam>
        /// <param name="array">Массив</param>
        /// <returns>True - если массив не инициализирован или не содержит элементов, False - в противном случае</returns>
        public static bool IsEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }
    }
}