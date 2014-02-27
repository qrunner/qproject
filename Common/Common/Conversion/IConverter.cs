namespace Common.Conversion
{
    /// <summary>
    /// Предоставляет методы конвертации
    /// </summary>
    /// <typeparam name="TFrom">Тип исходного значения</typeparam>
    /// <typeparam name="TO">Тип результирующего значения</typeparam>
    public interface IConverter<in TFrom, out TO>
    {
        /// <summary>
        /// Конвертирует значение типа TFrom в значение типа TO
        /// </summary>
        /// <param name="source">Исходное значение</param>
        /// <returns>Результат</returns>
        TO Convert(TFrom source);
    }
}