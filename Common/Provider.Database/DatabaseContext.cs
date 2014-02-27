using System;
using System.Collections.Generic;
using System.Data;

namespace Provider.Database
{
    public class DatabaseContext
    {
        /// <summary>
        /// Текущий провайдер БД
        /// </summary>
        public IDatabaseProvider Provider
        {
            get { return DbProvider; }
        }

        protected IDatabaseProvider DbProvider = null;

        /// <summary>
        /// Инициализирует новый объект DatabaseContext
        /// </summary>
        /// <param name="providerName">Имя провайдера в файле конфигурации</param>
        public DatabaseContext(string providerName)
        {
            DbProvider = (IDatabaseProvider) ProviderFactory.GetProvider(providerName, null);
        }

        /// <summary>
        /// Инициализирует новый объект DatabaseContext
        /// </summary>
        /// <param name="providerName">Имя провайдера в файле конфигурации</param>
        /// <param name="connectionString">Имя строки соединения</param>
        public DatabaseContext(string providerName, string connectionString)
        {
            DbProvider = (IDatabaseProvider) ProviderFactory.GetProvider(providerName, connectionString);
        }

        /// <summary>
        /// Инициализирует новый объект DatabaseContext
        /// </summary>
        /// <param name="provider">Провайдер БД</param>
        public DatabaseContext(IDatabaseProvider provider)
        {
            DbProvider = provider;
        }

        /// <summary>
        /// Возвращает единичный объект
        /// </summary>
        /// <typeparam name="TResult">Тип объекта данных</typeparam>
        /// <param name="sql">Запрос на выборку</param>
        /// <param name="creatorMethod">Метод инициализации объекта TResult</param>
        /// <param name="parameters">Список параметров запроса</param>
        /// <returns>Объект TResult</returns>
        public TResult SelectSingle<TResult>(string sql, Func<IDataRecord, TResult> creatorMethod, params DbParamValue[] parameters) where TResult : class
        {
            return SelectSingle(sql, false, creatorMethod, parameters);
        }

        /// <summary>
        /// Возвращает единичный объект
        /// </summary>
        /// <typeparam name="TResult">Тип объекта данных</typeparam>
        /// <param name="sql">Запрос на выборку</param>
        /// <param name="isStoredProcedure">Является ли хранимой процедурой</param>
        /// <param name="creatorMethod">Метод инициализации объекта TResult</param>
        /// <param name="parameters">Список параметров запроса</param>
        /// <returns>Объект TResult</returns>
        public TResult SelectSingle<TResult>(string sql, bool isStoredProcedure, Func<IDataRecord, TResult> creatorMethod, params DbParamValue[] parameters) where TResult : class
        {
            return SelectSingle(new SelectQuery(sql, isStoredProcedure, parameters), creatorMethod);
        }

        /// <summary>
        /// Возвращает единичный объект
        /// </summary>
        /// <typeparam name="TResult">Тип объекта данных</typeparam>
        /// <param name="query">Запрос на выборку</param>
        /// <param name="creatorMethod">Метод инициализации объекта TResult</param>
        /// <returns>Объект</returns>
        public TResult SelectSingle<TResult>(SelectQuery query, Func<IDataRecord, TResult> creatorMethod) where TResult : class
        {
            //TResult retval = default(TResult); // sometimes returns System.DBNull
            TResult retval = null;
            IDataReader reader = DbProvider.OpenReader(query);
            if (reader.Read())
            {
                retval = creatorMethod(reader);
            }
            DbProvider.CloseReader(reader);
            return retval;
        }

        /// <summary>
        /// Возвращает набор объектов, заполненных данными
        /// </summary>
        /// <typeparam name="TResult">Тип объекта данных</typeparam>
        /// <param name="sql">Запрос на выборку</param>
        /// <param name="creatorMethod">Метод инициализации объекта TResult</param>
        /// <param name="parameters">Список параметров запроса</param>
        /// <returns>Список объектов</returns>
        public IEnumerable<TResult> Select<TResult>(string sql, Func<IDataRecord, TResult> creatorMethod, params DbParamValue[] parameters)
        {
            return Select(sql, false, creatorMethod, parameters);
        }

        /// <summary>
        /// Возвращает набор объектов, заполненных данными
        /// </summary>
        /// <typeparam name="TResult">Тип объекта данных</typeparam>
        /// <param name="sql">Запрос на выборку</param>
        /// <param name="isStoredProcedure">Является ли хранимой процедурой</param>
        /// <param name="creatorMethod">Метод инициализации объекта TResult</param>
        /// <param name="parameters">Список параметров запроса</param>
        /// <returns>Список объектов</returns>
        public IEnumerable<TResult> Select<TResult>(string sql, bool isStoredProcedure, Func<IDataRecord, TResult> creatorMethod, params DbParamValue[] parameters)
        {
            return Select(new SelectQuery(sql, isStoredProcedure, parameters), creatorMethod);
        }

        /// <summary>
        /// Возвращает набор объектов, заполненных данными
        /// </summary>
        /// <typeparam name="TResult">Тип объекта данных</typeparam>
        /// <param name="query">Запрос на выборку</param>
        /// <param name="creatorMethod">Метод инициализации объекта</param>
        /// <returns>Список объектов</returns>
        public IEnumerable<TResult> Select<TResult>(SelectQuery query, Func<IDataRecord, TResult> creatorMethod)
        {
            List<TResult> retval = new List<TResult>();
            IDataReader reader = DbProvider.OpenReader(query);
            while (reader.Read())
            {
                retval.Add(creatorMethod(reader));
            }
            DbProvider.CloseReader(reader);
            return retval;
        }

        /// <summary>
        /// Выполняет запрос на выборку и возвращает DataTable заполненный результатами запроса 
        /// </summary>
        /// <param name="sql">Запрос на выборку</param>
        /// <param name="parameters">Список параметров запроса</param>
        /// <returns>DataTable с результатами запроса</returns>
        public DataTable Select(string sql, params DbParamValue[] parameters)
        {
            return Select(sql, false, parameters);
        }

        /// <summary>
        /// Выполняет запрос на выборку и возвращает DataTable заполненный результатами запроса 
        /// </summary>
        /// <param name="sql">Запрос на выборку</param>
        /// <param name="isStoredProcedure">Является ли хранимой процедурой</param>
        /// <param name="parameters">Список параметров запроса</param>
        /// <returns>DataTable с результатами запроса</returns>
        public DataTable Select(string sql, bool isStoredProcedure, params DbParamValue[] parameters)
        {
            return Select(new SelectQuery(sql, isStoredProcedure, parameters));
        }

        /// <summary>
        /// Выполняет запрос на выборку и возвращает DataTable заполненный результатами запроса 
        /// </summary>
        /// <param name="query">Запрос на выборку</param>
        /// <returns>DataTable с результатами запроса</returns>
        public DataTable Select(SelectQuery query)
        {
            return DbProvider.ExecuteSelect(query);
        }

        /// <summary>
        /// Выполняет запрос на выборку и вызывает заданный обработчик для каждой записи
        /// </summary>
        /// <param name="query">Запрос на выборку</param>
        /// <param name="processAction">Обработчик</param>
        public void ProcessReader(SelectQuery query, Action<IDataRecord> processAction)
        {
            IDataReader reader = DbProvider.OpenReader(query);
            while (reader.Read())
            {
                processAction(reader);
            }
            DbProvider.CloseReader(reader);
        }

        /// <summary>
        /// Возвращает результат скалярного запроса
        /// </summary>
        /// <param name="query">Скалярный запрос</param>
        /// <returns>Значение - результат запроса</returns>
        public object SelectValue(ScalarQuery query)
        {
            return DbProvider.ExecuteScalar(query);
        }
        
        /// <summary>
        /// Возвращает результат скалярного запроса
        /// </summary>
        /// <param name="sql">SQL-запрос</param>
        /// <param name="parameters">Параметры запроса</param>
        /// <returns>Возвращаемое значение</returns>
        public object SelectValue(string sql, params DbParamValue[] parameters)
        {
            return SelectValue(sql, false, parameters);
        }

        public object SelectValue(string sql,bool isStoredProcedure, params DbParamValue[] parameters)
        {
            return SelectValue(new ScalarQuery(sql, isStoredProcedure, parameters));
        }

        /// <summary>
        /// Возвращает результат скалярного запроса
        /// </summary>
        /// <param name="query">Скалярный запрос</param>
        /// <param name="connection">Существующее соединение с БД</param>
        /// <returns>Значение - результат запроса</returns>
        public object SelectValue(ScalarQuery query, IDbConnection connection)
        {
            return DbProvider.ExecuteScalar(query, connection);
        }

        /// <summary>
        /// Выполняет запрос
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <returns>Количество модифицированных записей</returns>
        public int Execute(NonSelectQuery query)
        {
            return DbProvider.ExecuteNonQuery(query);
        }

        /// <summary>
        /// Выполняет SQL команду, не возвращающую результатов
        /// </summary>
        /// <param name="sql">Команда SQL, не являющейся хранимой процедурой</param>
        /// <returns>Количество модифицированных записей</returns>
        public int Execute(string sql)
        {
            return DbProvider.ExecuteNonQuery(new NonSelectQuery(sql, false));
        }

        /// <summary>
        /// Выполняет SQL команду или хранимую процедуру, не возвращающую результатов
        /// </summary>
        /// <param name="sql">Команда SQL или хранимая процедура</param>
        /// <param name="isStoredProcedure">Является ли хранимой процедурой</param>
        /// <returns>Количество модифицированных записей</returns>
        public int Execute(string sql, bool isStoredProcedure)
        {
            return DbProvider.ExecuteNonQuery(new NonSelectQuery(sql, isStoredProcedure));
        }

        /// <summary>
        /// Выполняет SQL команду или хранимую процедуру, не возвращающую результатов
        /// </summary>
        /// <param name="sql">Команда SQL или хранимая процедура</param>
        /// <param name="isStoredProcedure">Является ли хранимой процедурой</param>
        /// <param name="parameters">Список параметров</param>
        /// <returns>Количество модифицированных записей</returns>
        public int Execute(string sql, bool isStoredProcedure, params DbParam[] parameters)
        {
            return DbProvider.ExecuteNonQuery(new NonSelectQuery(sql, isStoredProcedure, parameters));
        }

        /// <summary>
        /// Выполняет SQL команду, не возвращающую результатов
        /// </summary>
        /// <param name="sql">Команда SQL, не являющейся хранимой процедурой</param>
        /// <param name="parameters">Список параметров</param>
        /// <returns>Количество модифицированных записей</returns>
        public int Execute(string sql, params DbParam[] parameters)
        {
            return DbProvider.ExecuteNonQuery(new NonSelectQuery(sql, false, parameters));
        }

        /// <summary>
        /// Выполняет запрос, не возвращающий результатов
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="connection">Имеющееся соединение с БД</param>
        /// <returns>Количество модифицированных записей</returns>
        public int Execute(NonSelectQuery query, IDbConnection connection)
        {
            return DbProvider.ExecuteNonQuery(query, connection);
        }

        /// <summary>
        /// Выполняет запрос на массовую вставку
        /// </summary>
        /// <param name="bulkInsertQuery">Запрос на массовую вставку</param>
        /// <param name="connection">Существующее соединение с БД</param>
        public void BulkInsert(BulkInsert bulkInsertQuery, IDbConnection connection)
        {
            DbProvider.BulkInsert(bulkInsertQuery, connection);
        }

        /// <summary>
        /// Выполняет запрос на массовую вставку
        /// </summary>
        /// <param name="bulkInsertQuery">Запрос на массовую вставку</param>
        public void BulkInsert(BulkInsert bulkInsertQuery)
        {
            DbProvider.BulkInsert(bulkInsertQuery);
        }

        public IDbConnection OpenConnection()
        {
            return DbProvider.OpenConnection();
        }

        public void CloseConnection(IDbConnection conn)
        {
            DbProvider.CloseConnection(conn);
        }

        /// <summary>
        /// Получает значение первичного ключа записи. Если запись отсутствует - предварительно создает ее.
        /// </summary>
        /// <param name="selectIdQuery">Запрос на получение первичного ключа записи.</param>
        /// <param name="getNewIdQuery">Запрос для генерации первичного ключа.</param>
        /// <param name="insertRecordQuery">Запрос на вставку новой записи.
        /// Внимание! Первым параметром в запросе должен быть первичный ключ (идентификатор записи).
        /// </param>
        /// <returns>Значение первичного ключа.</returns>
        [Obsolete("Убрать отсюда")]
        public object GetRecordIdOrInsertIfNotExsists(ScalarQuery selectIdQuery, ScalarQuery getNewIdQuery, NonSelectQuery insertRecordQuery)
        {
            object retval = this.SelectValue(selectIdQuery);

            if (retval == null)
            {
                retval = this.SelectValue(getNewIdQuery);
                if (retval == null) throw new Exception("Идентификатор для новой записи не получен.");
                if (insertRecordQuery.IsBatch) throw new Exception("Использование пакетного запроса не разрешено в данном контексте.");
                if (insertRecordQuery.Parameters.Length == 0) throw new ArgumentException("Запрос должен содержать хотя бы один параметр для вставки первичного ключа.");
                (insertRecordQuery.Parameters[0] as DbParamValue).Value = retval;
                Execute(insertRecordQuery);
            }

            return retval;
        }
    }
}