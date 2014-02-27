using System.Data;

namespace Provider.Database
{
    /// <summary>
    /// Массовая вставка записей
    /// </summary>
    public class BulkInsert : Query
    {
        private readonly DataTable _paramTable = null;
        /// <summary>
        /// Таблица со значениями параметров
        /// </summary>
        public DataTable ParamTable
        {
            get { return _paramTable; }
        }
        /// <summary>
        /// Инициализирует новый запрос на массовую вставку записей в таблицу
        /// </summary>
        /// <param name="tableName">Имя таблицы в БД</param>
        /// <param name="table">DataTable со значениями параметров</param>
        public BulkInsert(string tableName, DataTable table) :
            base(QueryType.TableBulkInsert, tableName, false)
        {
            _paramTable = table;
        }
    }
}