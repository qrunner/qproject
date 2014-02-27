using Provider.Database;
using System.Collections.Generic;
using System.Linq;

namespace Provider.Logging.Database
{
    public class Writer : DbBatchWriteProvider
    {
        string spName = string.Empty;

        char[] levelCodes = new char[5] { 'I', 'E', 'W', 'D', 'F' };

        protected override void WriteBatch(IEnumerable<IEntry> records)
        {
            if (records.Count() == 0) return;

            /*DataTable tbl = new DataTable();

            foreach (LogEntry entry in records)
            {
                object poolId = null;
                if (entry.Source.PoolId > 0)
                    poolId = entry.Source.PoolId; // box/unbox

                object objectId = null;
                if (entry.object_code > 0)
                    objectId = entry.object_code; // box/unbox

                insertQuery.AddParamValues(
                        entry.Timestamp,
                        entry.Msg,
                        levelCodes[(int)entry.Level],
                        entry.process_id,
                        entry.thread_id,
                        entry.Source.AppPathId,
                        entry.Source.ServerId,
                        poolId,
                        objectId,
                        entry.additional,
                        entry.call_stack,
                        entry.ex_stack
                        );
            }

            BulkInsert insertQuery = new BulkInsert("T_INTEGRATION_LOG", false,
                new DbParam("DTM_", DbParamType.Timestamp),
                new DbParam("MSG_", DbParamType.String),
                new DbParam("LVL_", DbParamType.Char),
                new DbParam("PROCESS_ID_", DbParamType.Integer),
                new DbParam("THREAD_ID_", DbParamType.Integer),
                new DbParam("APP_ID_", DbParamType.Integer),
                new DbParam("HOST_ID_", DbParamType.Integer),
                new DbParam("POOL_ID_", DbParamType.Integer),
                new DbParam("CODE_", DbParamType.Integer),
                new DbParam("ADDITIONAL_", DbParamType.String),
                new DbParam("CALL_STACK_", DbParamType.String),
                new DbParam("EX_STACK_", DbParamType.String));
            
            DbContext.BulkInsert(insertQuery);*/

            NonSelectQuery insertQuery = new NonSelectQuery(
                "INSERT INTO T_INTEGRATION_LOG(DTM, MSG, LVL, PROCESS_ID, THREAD_ID, APP_ID, HOST_ID, POOL_ID, CODE, ADDITIONAL, CALL_STACK, EX_STACK) VALUES " +
                " (:DTM_, :MSG_, :LVL_, :PROCESS_ID_, :THREAD_ID_, :APP_ID_, :HOST_ID_, :POOL_ID_, :CODE_, :ADDITIONAL_, :CALL_STACK_, :EX_STACK_)", false,
                new DbParam("DTM_", DbParamType.Timestamp),
                new DbParam("MSG_", DbParamType.String),
                new DbParam("LVL_", DbParamType.Char),
                new DbParam("PROCESS_ID_", DbParamType.Integer),
                new DbParam("THREAD_ID_", DbParamType.Integer),
                new DbParam("APP_ID_", DbParamType.Integer),
                new DbParam("HOST_ID_", DbParamType.Integer),
                new DbParam("POOL_ID_", DbParamType.Integer),
                new DbParam("CODE_", DbParamType.Integer),
                new DbParam("ADDITIONAL_", DbParamType.String),
                new DbParam("CALL_STACK_", DbParamType.String),
                new DbParam("EX_STACK_", DbParamType.String));

            foreach (LogEntry entry in records)
            {
                object poolId = null;
                if (entry.Source.PoolId > 0)
                    poolId = entry.Source.PoolId; // box/unbox

                object objectId = null;
                if (entry.object_code > 0)
                    objectId = entry.object_code; // box/unbox

                insertQuery.AddParamValues(
                        entry.Timestamp,
                        entry.Msg,
                        levelCodes[(int)entry.Level],
                        entry.process_id,
                        entry.thread_id,
                        this.appIds[entry.Source.AppPath],
                        this.hostIds[entry.Source.Server],
                        poolId,
                        objectId,
                        entry.additional,
                        entry.call_stack,
                        entry.ex_stack
                        );
            }

            DbContext.Execute(insertQuery);
        }
    }
}