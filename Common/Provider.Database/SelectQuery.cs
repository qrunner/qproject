namespace Provider.Database
{
    /// <summary>
    /// Запрос на выборку данных
    /// </summary>
    public class SelectQuery : Query
    {
        /// <summary>
        /// Инициализирует новый запрос на выборку
        /// </summary>
        /// <param name="sql">Команда SQL</param>
        /// <param name="storedProcedure">Является ли хранимой процедурой</param>
        /// <param name="parameters">Список параметров со значениями</param>
        public SelectQuery(string sql, bool storedProcedure, params DbParamValue[] parameters)
            : base(QueryType.SelectQuery, sql, storedProcedure, parameters) { }

        public new DbParamValue[] Parameters { get { return (DbParamValue[])base.Parameters; } }
    }
}