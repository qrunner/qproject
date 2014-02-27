namespace Provider.Database
{
    /// <summary>
    /// Запрос возвращающий одно значение
    /// </summary>
    public class ScalarQuery : Query
    {
        /// <summary>
        /// Инициализирует новый скалярный запрос
        /// </summary>
        /// <param name="sql">Команда SQL </param>
        /// <param name="storedProcedure">Является ли хранимой процедурой</param>
        /// <param name="parameters">Список параметров со значениями</param>
        public ScalarQuery(string sql, bool storedProcedure, params DbParamValue[] parameters)
            : base(QueryType.ScalarQuery, sql, storedProcedure, parameters) { }

        public new DbParamValue[] Parameters { get { return (DbParamValue[])base.Parameters; } }
    }
}