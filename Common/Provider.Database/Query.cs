namespace Provider.Database
{
    /// <summary>
    /// Запрос к БД
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Инициализирует новый запрос
        /// </summary>
        /// <param name="type">Тип запроса</param>
        /// <param name="sql">Текст запроса</param>
        /// <param name="storedProcedure">Является ли хранимой процедурой</param>
        /// <param name="parameters">Параметры запроса</param>
        public Query(QueryType type, string sql, bool storedProcedure, params DbParam[] parameters)
        {
            Sql = sql;
            Parameters = parameters;
            Type = type;
            StoredProcedure = storedProcedure;
        }

        /// <summary>
        /// Таймаут операции
        /// </summary>
        public int? CommandTimeout { get; set; }

        /// <summary>
        /// Текст запроса
        /// </summary>
        public string Sql;

        /// <summary>
        /// Параметры запроса
        /// </summary>
        public DbParam[] Parameters = null;

        /// <summary>
        /// Тип запроса
        /// </summary>
        public QueryType Type;

        /// <summary>
        /// Является ли вызовом хранимой процедуры
        /// </summary>
        public bool StoredProcedure;

        public override string ToString()
        {
            return Sql;
        }
    }
}