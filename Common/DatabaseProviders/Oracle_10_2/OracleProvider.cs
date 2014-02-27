using System.Collections.Generic;
using Oracle.DataAccess.Client;
using System.Data;

namespace Provider.Database.Oracle
{
    /// <summary>
    /// ODP Oracle provider
    /// </summary>
    public class OracleProvider : DatabaseProviderBase<OracleConnection, OracleDataAdapter, OracleParameter>
    {
        protected override void SetParameterType(DbParam commonParam, ref OracleParameter concreteParam)
        {
            switch (commonParam.DbType)
            {
                case DbParamType.Timestamp: concreteParam.OracleDbType = OracleDbType.TimeStamp;
                    break;
                default: base.SetParameterType(commonParam, ref concreteParam);
                    break;
            }
        }

        protected override IDbCommand GetCommand(IDbConnection conn, string sql, bool isStoredProc)
        {
            OracleCommand retval = (OracleCommand)base.GetCommand(conn, sql, isStoredProc);
            retval.BindByName = true;
            return retval;
        }

        protected override void BulkInsertInternal(BulkInsert bulkInsertQuery, IDbConnection connection)
        {
            NonSelectQuery bulkQuery = new NonSelectQuery(GenerateInsertSQL(bulkInsertQuery.Sql, bulkInsertQuery.ParamTable.Columns),
                false,
                GenerateInsertParams(bulkInsertQuery.ParamTable.Columns));

            for (int i = 0; i < bulkInsertQuery.ParamTable.Rows.Count; i++)
            {
                bulkQuery.AddParamValues(bulkInsertQuery.ParamTable.Rows[i].ItemArray);
            }

            ExecuteNonQuery(bulkQuery, connection);
        }

        protected override int ExecuteNonQueryInternal(NonSelectQuery query, IDbConnection conn)
        {
            OracleCommand command = (OracleCommand)GetCommand(conn, query.Sql, query.StoredProcedure);
            if (query.CommandTimeout.HasValue) command.CommandTimeout = query.CommandTimeout.Value;
            if (query.IsBatch)
            {
                int rowsCount = query.Values.Count;

                if (rowsCount > 0)
                {
                    int paramCount = query.Values[0].Length;
                    command.ArrayBindCount = rowsCount;
                    for (int pc = 0; pc < paramCount; pc++)
                    {
                        object[] paramValues = new object[rowsCount];
                        for (int rc = 0; rc < rowsCount; rc++)
                        {
                            paramValues[rc] = query.Values[rc][pc];
                        }
                        OracleParameter op = new OracleParameter();
                        op.ParameterName = query.Parameters[pc].Name;
                        SetParameterType(query.Parameters[pc], ref op);
                        op.Value = paramValues;
                        command.Parameters.Add(op);
                    }
                }
            }
            else
            {
                SetParams(command.Parameters, query.Parameters);
            }

            var rowCount = command.ExecuteNonQuery();
            SetOutParams(command.Parameters, query.Parameters);

            return rowCount;
        }

        protected override void Initialize(IDictionary<string, string> settings)
        {
            
        }
    }
}