namespace Provider.Database
{
    /// <summary>
    /// Тип запроса
    /// </summary>
    public enum QueryType
    {
        /// <summary>
        /// Запрос на выборку данных
        /// </summary>
        SelectQuery,
        /// <summary>
        /// Запрос возвращающий одно значение
        /// </summary>
        ScalarQuery,
        /// <summary>
        /// Запрос, не возвращающий данных
        /// </summary>
        NonSelectQuery,
        /// <summary>
        /// Массовая вставка данных в таблицу
        /// </summary>
        TableBulkInsert
    }
}