using System.Data;

namespace Provider.Database
{
    /// <summary>
    /// Провайдер доступа к реляционным СУБД
    /// </summary>
    public interface IDatabaseProvider : IProvider
    {
        /// <summary>
        /// Выполнить SQL запрос
        /// </summary>
        /// <param name="query">SQL запрос</param>
        /// <param name="dataTable">Таблица результата запроса</param>
        /// <returns>Набор данных</returns>        
        DataTable ExecuteSelect(SelectQuery query);
        /// <summary>
        /// Выполнить SQL запрос на получение одного значения
        /// </summary>
        /// <param name="query">SQL запрос</param>        
        /// <returns>Результат выполнения</returns>        
        object ExecuteScalar(ScalarQuery query);
        /// <summary>
        /// Выполнить SQL запрос на получение одного значения
        /// </summary>
        /// <param name="query">SQL запрос</param>
        /// <param name="connection">Существующее соединение с БД</param>
        /// <returns>Результат выполнения</returns>
        object ExecuteScalar(ScalarQuery query, IDbConnection connection);
        /// <summary>
        /// Выполнить SQL запрос без возврещаемого результата
        /// </summary>
        /// <param name="query">SQL запрос</param>
        /// <returns>Количество обновленных записей</returns>
        int ExecuteNonQuery(NonSelectQuery query);
        /// <summary>
        /// Выполнить SQL запрос без возврещаемого результата
        /// </summary>
        /// <param name="query">SQL запрос</param>
        /// <param name="connection">Существующее соединение с БД</param>
        /// <returns>Количество обновленных записей</returns>
        int ExecuteNonQuery(NonSelectQuery query, IDbConnection connection);
        /// <summary>
        /// Выполняет запрос на массовую вставку
        /// </summary>
        /// <param name="bulkInsertQuery">Запрос на массовую вставку</param>
        void BulkInsert(BulkInsert bulkInsertQuery);
        /// <summary>
        /// Выполняет запрос на массовую вставку
        /// </summary>
        /// <param name="bulkInsertQuery">Запрос на массовую вставку</param>
        /// <param name="connection">Существующее соединение с БД</param>
        void BulkInsert(BulkInsert bulkInsertQuery, IDbConnection connection);
        /// <summary>
        /// Выполняет запрос на выборку и возвращает DbDataReader
        /// </summary>
        /// <param name="query">Запрос на выборку</param>
        /// <returns>DbDataReader с результатами запроса</returns>
        IDataReader OpenReader(SelectQuery query);
        /// <summary>
        /// Закрывает DbDataReader
        /// </summary>
        /// <param name="reader">Data reader</param>
        void CloseReader(IDataReader reader);
        /// <summary>
        /// Открывает подключение к БД и возвращает объект Connection
        /// </summary>
        /// <returns>Объект Connection</returns>
        IDbConnection OpenConnection();
        /// <summary>
        /// Закрывает открытое подключение
        /// </summary>
        /// <param name="conn">Объект Connection</param>
        void CloseConnection(IDbConnection conn);

        //int ExecuteNonQuery(string query, DbParam[] parameters);
        //DataTable ExecuteStoredProcedureQuery(string procedure_name, IEnumerable<DbParam> parameters);
        //object ExecuteStoredProcedureScalar(string procedure_name, IEnumerable<DbParam> parameters);
        //int ExecuteStoredProcedure(string procedure_name, Array parameters);
        //int ExecuteStoredProcedure(DbConnection conn, string procedure_name, DbParam[] parameters);
        //object ExecuteScalar(string query, DbParam[] parameters);
        //DataTable ExecuteQuery(string query, DbParam[] parameters);
        //DbDataReader OpenReader(string sql, DbParam[] parameters);
    }
}