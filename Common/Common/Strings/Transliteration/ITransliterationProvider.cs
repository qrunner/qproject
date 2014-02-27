using System.Collections.Generic;

namespace Common.Strings.Transliteration
{
    /// <summary>
    /// Представляет базовые методы для транслитерации
    /// </summary>
    public interface ITransliterationProvider
    {
        /// <summary>
        /// Выполняет транслит строки из кириллицы в латиницу
        /// </summary>
        /// <param name="text">Исходная строка</param>
        /// <returns>Строка после транслитерации</returns>
        string Translite(IEnumerable<char> text);

        /// <summary>
        /// Выполняет транслит символа
        /// </summary>
        /// <param name="c">Исходный символ</param>
        /// <returns>Транслитерированный символ</returns>
        string Translite(char c);

        /// <summary>
        /// Вычисляет длину строки после транслитерации, не выполняя саму транслитерацию
        /// </summary>
        /// <param name="text">Исходная строка</param>
        /// <returns>Длина транслитерированной строки</returns>
        int TransliteLength(IEnumerable<char> text);

        /// <summary>
        /// Вычисляет длину символа после транслитерации, не выполняя саму транслитерацию
        /// </summary>
        /// <param name="c">Исходный символ</param>
        /// <returns>Длина транслитерированной строки</returns>
        int TransliteLength(char c);
    }
}