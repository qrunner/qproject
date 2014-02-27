using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Provider.Database.MsSql
{
    /// <summary>
    /// MS SQL data provider
    /// </summary>
    public class MsSqlProvider : DatabaseProviderBase<SqlConnection, SqlDataAdapter, SqlParameter>
    {
        protected override void SetParameterType(DbParam commonParam, ref SqlParameter concreteParam)
        {
            switch (commonParam.DbType)
            {
                case DbParamType.DataTable: concreteParam.SqlDbType = SqlDbType.Structured;
                    break;
                case DbParamType.Timestamp: concreteParam.SqlDbType = SqlDbType.Timestamp;
                    break;
                default: base.SetParameterType(commonParam, ref concreteParam);
                    break;
            }
        }

        protected override void BulkInsertInternal(BulkInsert bulkInsertQuery, IDbConnection connection)
        {
            using (SqlBulkCopy bc = new SqlBulkCopy((SqlConnection)connection))
            {
                bc.DestinationTableName = bulkInsertQuery.Sql;
                bc.WriteToServer(bulkInsertQuery.ParamTable);
            }
        }

        protected override void Initialize(IDictionary<string, string> settings)
        {
         
        }
    }
}