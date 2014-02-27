using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Provider.Database
{
    /// <summary>
    /// Базовый класс провайдера
    /// </summary>
    /// <typeparam name="TConn">Тип соединения</typeparam>
    /// <typeparam name="TDA">Тип адаптера</typeparam>
    /// <typeparam name="TParam">Тип параметра</typeparam>
    public abstract class DatabaseProviderBase<TConn, TDA, TParam> : Provider, IDatabaseProvider
        where TConn : IDbConnection, new()
        where TDA : IDbDataAdapter, new()
        where TParam : IDbDataParameter, new()
    {
        private const string queryErrorMessage = "Oшибка при выполнении запроса '{0}'.";

        /// <summary>
        /// Проверяет соединение с базой данных
        /// </summary>
        /// <param name="ex">Исключение в случае неуспешного подключения</param>
        /// <returns>Результат проверки</returns>
        public override bool CheckConnection(out Exception ex)
        {
            ex = null;
            using (TConn conn = new TConn())
            {
                conn.ConnectionString = this.ConnectionString;
                try { conn.Open(); }
                catch (Exception e) { ex = e; return false; }
                finally { conn.Close(); }
            }
            return true;
        }
        /// <summary>
        /// Выполняет запрос на выборку и возвращает DbDataReader
        /// </summary>
        /// <param name="query">Запрос на выборку</param>
        /// <returns>DbDataReader с результатами запроса</returns>
        public IDataReader OpenReader(SelectQuery query)
        {
            IDataReader retval = null;

            TConn conn = new TConn();
            conn.ConnectionString = ConnectionString;
            IDbCommand command = GetCommand(conn, query.Sql, query.StoredProcedure);
            if (query.CommandTimeout.HasValue) command.CommandTimeout = query.CommandTimeout.Value;
            SetParams(command.Parameters, query.Parameters);
            try
            {
                conn.Open();
                retval = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(string.Format(queryErrorMessage, query), ex);
            }
            return retval;
        }
        /// <summary>
        /// Закрывает DbDataReader
        /// </summary>
        /// <param name="reader">Data reader</param>
        public void CloseReader(IDataReader reader)
        {
            reader.Close();
        }
        /// <summary>
        /// Выполняет запрос, возвращающий данные
        /// </summary>
        /// <param name="query">Запрос на выборку</param>
        /// <returns>Результат запроса</returns>
        public DataTable ExecuteSelect(SelectQuery query)
        {
            DataSet retval = new DataSet();
            using (TConn conn = new TConn())
            {
                conn.ConnectionString = ConnectionString;
                try
                {
                    IDbCommand command = GetCommand(conn, query.Sql, query.StoredProcedure);
                    if (query.CommandTimeout.HasValue) command.CommandTimeout = query.CommandTimeout.Value;
                    SetParams(command.Parameters, query.Parameters);
                    conn.Open();
                    IDbDataAdapter adapter = new TDA();
                    adapter.SelectCommand = command;
                    adapter.Fill(retval);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format(queryErrorMessage, query), ex);
                }
                finally { conn.Close(); }
            }
            return retval.Tables[0];
        }
        /// <summary>
        /// Выполнить SQL запрос на получение одного значения
        /// </summary>
        /// <param name="query">SQL запрос</param>        
        /// <returns>Результат выполнения</returns>    
        public object ExecuteScalar(ScalarQuery query)
        {
            object retval = null;
            using (TConn conn = new TConn())
            {
                conn.ConnectionString = ConnectionString;
                try
                {
                    conn.Open();
                    retval = ExecuteScalarInternal(query, conn);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format(queryErrorMessage, query), ex);
                }
                finally { conn.Close(); }
            }
            return retval;
        }

        private object ExecuteScalarInternal(ScalarQuery query, IDbConnection conn)
        {
            IDbCommand command = GetCommand(conn, query.Sql, query.StoredProcedure);
            if (query.CommandTimeout.HasValue) command.CommandTimeout = query.CommandTimeout.Value;
            SetParams(command.Parameters, query.Parameters);
            return command.ExecuteScalar();            
        }
        /// <summary>
        /// Выполнить SQL запрос на получение одного значения
        /// </summary>
        /// <param name="query">SQL запрос</param>     
        /// <param name="connection">Соединение с БД</param>     
        /// <returns>Результат выполнения</returns>    
        public object ExecuteScalar(ScalarQuery query, IDbConnection connection)
        {            
            try
            {
                return ExecuteScalarInternal(query, connection);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(queryErrorMessage, query), ex);
            }
        }
        /// <summary>
        /// Выполнить SQL запрос без возврещаемого результата
        /// </summary>
        /// <param name="query">SQL запрос</param>
        /// <returns>Количество обновленных записей</returns>    
        public int ExecuteNonQuery(NonSelectQuery query)
        {
            int retval = 0;
            using (TConn conn = new TConn())
            {
                conn.ConnectionString = ConnectionString;
                try
                {
                    conn.Open();
                    retval = ExecuteNonQueryInternal(query, conn);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format(queryErrorMessage, query), ex);
                }
                finally { conn.Close(); }
            }
            return retval;
        }        

        protected virtual int ExecuteNonQueryInternal(NonSelectQuery query, IDbConnection conn)
        {
            int retval = 0;
            IDbCommand command = GetCommand(conn, query.Sql, query.StoredProcedure);
            if (query.CommandTimeout.HasValue) command.CommandTimeout = query.CommandTimeout.Value;
            SetParams(command.Parameters, query.Parameters);

            foreach (object[] values in query.Values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    ((TParam)command.Parameters[i]).Value = values[i];
                }
                retval += command.ExecuteNonQuery();

                SetOutParams(query, command);
            }
            return retval;
        }        

        protected int ExecuteNonQueryInternal(IEnumerable<NonSelectQuery> queryList, IDbConnection conn)
        {
            return queryList.Sum(query => ExecuteNonQueryInternal(query, conn));
        }

        /* private bool Optimize(IEnumerable<NonSelectQuery> queryList, out IEnumerable<BulkInsert> bulkInserts, out IEnumerable<NonSelectQuery> singleQueries)
        {
            Dictionary<string, List<NonSelectQuery>> queries = new Dictionary<string, List<NonSelectQuery>>();

            foreach (NonSelectQuery query in queryList)
            {
                if (queries.ContainsKey(query.))
            }

            return true;
        }*/

        protected virtual void BulkInsertInternal(BulkInsert bulkInsertQuery, IDbConnection connection)
        {
            throw new NotImplementedException("Провайдер не поддерживает массовую вставку.");
        }
        /// <summary>
        /// Выполнить SQL запрос без возврещаемого результата
        /// </summary>
        /// <param name="query">SQL запрос</param>
        /// <param name="conn">Существующее соединение с БД</param>
        /// <returns>Количество обновленных записей</returns>        
        public int ExecuteNonQuery(NonSelectQuery query, IDbConnection conn)
        {
            int retval = 0;

            try
            {
                retval = ExecuteNonQueryInternal(query, conn);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(queryErrorMessage, query), ex);
            }

            return retval;
        }

        /// <summary>
        /// Выполняет запрос на массовую вставку
        /// </summary>
        /// <param name="bulkInsertQuery">Запрос на массовую вставку</param>
        public void BulkInsert(BulkInsert bulkInsertQuery)
        {
            using (TConn conn = new TConn())
            {
                conn.ConnectionString = ConnectionString;
                try
                {
                    conn.Open();
                    BulkInsertInternal(bulkInsertQuery, conn);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("", bulkInsertQuery.ParamTable), ex);
                }
                finally { conn.Close(); }
            }
        }
        /// <summary>
        /// Выполняет запрос на массовую вставку
        /// </summary>
        /// <param name="bulkInsertQuery">Запрос на массовую вставку</param>
        /// <param name="connection">Существующее соединение с БД</param>
        public void BulkInsert(BulkInsert bulkInsertQuery, IDbConnection connection)
        {
            try
            {
                BulkInsertInternal(bulkInsertQuery, connection);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("", bulkInsertQuery.ParamTable), ex);
            }
        }

        /// <summary>
        /// Заполняет коллекцию параметров команды
        /// </summary>
        /// <param name="parameters"></param>
        protected void SetParams(IDataParameterCollection commandParams, DbParam[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                commandParams.Clear();

                for (int i = 0; i < parameters.Length; i++)
                {
                    TParam item = new TParam {ParameterName = parameters[i].Name};
                    SetParameterType(parameters[i], ref item);
                    if (parameters[i] is DbParamValue) // проблема
                    {
                        DbParamValue param = parameters[i] as DbParamValue;
                        item.Value = param.Value;
                        item.Direction = param.Direction;
                    }
                    commandParams.Add(item);
                }
            }
        }

        /// <summary>
        /// Заполняет коллекцию параметров команды
        /// </summary>
        /// <param name="parameters"></param>
        protected void SetOutParams(IDataParameterCollection commandParams, DbParam[] parameters)
        {
            if (commandParams == null || parameters == null) return;
            foreach (var item in parameters)
            {
                if (!(item is DbParamValue)) continue;
                var param = item as DbParamValue;
                if (param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.ReturnValue)
                {                        
                    param.Value =((IDbDataParameter)commandParams[item.Name]).Value;
                }
            }
        }

        private static void SetOutParams(NonSelectQuery query, IDbCommand command)
        {
            if (query.Parameters == null || command.Parameters == null) return;
            foreach (IDataParameter outp in command.Parameters)
            {
                int idx = command.Parameters.IndexOf(outp);
                if (query.Parameters[idx] is DbParamValue && (outp.Direction == ParameterDirection.InputOutput || outp.Direction == ParameterDirection.Output || outp.Direction == ParameterDirection.ReturnValue))
                {
                    (query.Parameters[idx] as DbParamValue).Value = outp.Value;
                }
            }
        }

        /// <summary>
        /// Преобразовывает параметр из обобщенного типа к типу конкретного провайдера
        /// </summary>
        /// <param name="commonParam">Обобщенный параметр</param>
        /// <param name="concreteParam">Параметр конкретного типа провайдера</param>
        protected virtual void SetParameterType(DbParam commonParam, ref TParam concreteParam)
        {
            switch (commonParam.DbType)
            {
                case DbParamType.Binary: concreteParam.DbType = DbType.Binary;
                    break;
                case DbParamType.Boolean: concreteParam.DbType = DbType.Boolean;
                    break;
                case DbParamType.Byte: concreteParam.DbType = DbType.Byte;
                    break;
                case DbParamType.Char: concreteParam.DbType = DbType.StringFixedLength;
                    break;
                case DbParamType.Currency: concreteParam.DbType = DbType.Currency;
                    break;
                case DbParamType.DateTime: concreteParam.DbType = DbType.DateTime;
                    break;
                case DbParamType.Decimal: concreteParam.DbType = DbType.Decimal;
                    break;
                case DbParamType.Guid: concreteParam.DbType = DbType.Guid;
                    break;
                case DbParamType.Integer: concreteParam.DbType = DbType.Int32;
                    break;
                case DbParamType.Int64: concreteParam.DbType = DbType.Int64;
                    break;
                case DbParamType.String: concreteParam.DbType = DbType.String;
                    break;
                case DbParamType.Time: concreteParam.DbType = DbType.Time;
                    break;
                case DbParamType.Timestamp: concreteParam.DbType = DbType.DateTime2;
                    break;
                case DbParamType.Xml: concreteParam.DbType = DbType.Xml;
                    break;
                default: concreteParam.DbType = DbType.Object;
                    break;
            }
        }
        /// <summary>
        /// Открывает подключение к БД и возвращает объект Connection
        /// </summary>
        /// <returns>Объект Connection</returns>
        public IDbConnection OpenConnection()
        {
            TConn conn = new TConn();
            conn.ConnectionString = ConnectionString;
            conn.Open();
            return conn;
        }
        /// <summary>
        /// Закрывает открытое подключение
        /// </summary>
        /// <param name="conn">Объект Connection</param>
        public void CloseConnection(IDbConnection conn)
        {
            conn.Close();
        }

        protected string GenerateInsertSQL(string tableName, DataColumnCollection columns)
        {
            StringBuilder fields = new StringBuilder();
            StringBuilder parameters = new StringBuilder();

            for (int i = 0; i < columns.Count - 1; i++)
            {
                fields.AppendFormat("{0}, ", columns[i].ColumnName);
                parameters.AppendFormat(":{0}, ", columns[i].ColumnName);
            }
            fields.AppendFormat("{0}", columns[columns.Count - 1].ColumnName);
            parameters.AppendFormat(":{0}", columns[columns.Count - 1].ColumnName);

            StringBuilder retval = new StringBuilder();
            retval.AppendFormat("INSERT INTO {0} ({1}) VALUES ({2})", tableName, fields, parameters);
            return retval.ToString();
        }

        protected DbParam[] GenerateInsertParams(DataColumnCollection columns)
        {
            List<DbParam> retval = new List<DbParam>();

            for (int i = 0; i < columns.Count; i++)
            {
                retval.Add(new DbParam(columns[i].ColumnName, GetParamType(columns[i].DataType)));
            }

            return retval.ToArray();
        }

        protected virtual IDbCommand GetCommand(IDbConnection conn, string sql, bool isStoredProc)
        {
            IDbCommand retval = conn.CreateCommand();
            retval.CommandText = sql;
            if (isStoredProc)
                retval.CommandType = CommandType.StoredProcedure;
            return retval;
        }

        protected DbParamType GetParamType(Type t)
        {
            if (t == typeof(Int16) || t == typeof(Int32) || t == typeof(Int64)) return DbParamType.Integer;
            if (t == typeof(string)) return DbParamType.String;
            if (t == typeof(decimal)) return DbParamType.Decimal;
            if (t == typeof(bool)) return DbParamType.Boolean;
            if (t == typeof(byte)) return DbParamType.Byte;
            if (t == typeof(DateTime) || t == typeof(DateTimeOffset)) return DbParamType.Timestamp;
            if (t == typeof(Guid)) return DbParamType.Guid;
            return DbParamType.Object;
        }        
    }
}