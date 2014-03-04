using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace Provider.Database.MySQL
{
    /// <summary>
    /// MySQL data provider
    /// </summary>
    public class MySqlProvider : DatabaseProviderBase<MySqlConnection, MySqlDataAdapter, MySqlParameter>
    {
        protected override void SetParameterType(DbParam commonParam, ref MySqlParameter concreteParam)
        {
            switch (commonParam.DbType)
            {                
                case DbParamType.Timestamp: concreteParam.MySqlDbType = MySqlDbType.Timestamp;
                    break;
                default: base.SetParameterType(commonParam, ref concreteParam);
                    break;
            }
        }

        protected override void BulkInsertInternal(BulkInsert bulkInsertQuery, IDbConnection connection)
        {
            throw new Exception("Bulk load currently is not available for this provider");
        }

        protected override void Initialize(IDictionary<string, string> settings)
        {

        }
    }
}
