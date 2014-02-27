using Provider.Database;
using Provider.Logging;
using Provider.Logging.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Statistics
{
    public class StatDbProvider : DbBatchWriteProvider
    {
        protected override void WriteBatch(IEnumerable<IEntry> records)
        {
            if (records.Count() == 0) return;

            NonSelectQuery query = new NonSelectQuery(
                    "INSERT INTO T_INTEGRATION_STAT(POOL_ID, HOST_ID, APP_ID, THREADS, INPROCESS, INQUEUE, STATUS, PROCESSED, ERRORS, PROCESS_TIME, FOREIGN_TIME, PICK_TIME, PICKS_COUNT, PICKED, PICK_ERRORS, ENQUEUED, DTM) " +
                    " VALUES (:POOL_ID, :HOST_ID, :APP_ID, :THREADS, :INPROCESS, :INQUEUE, :STATUS, :PROCESSED, :ERRORS, :PROCESS_TIME, :FOREIGN_TIME, :PICK_TIME, :PICKS_COUNT, :PICKED, :PICK_ERRORS, :ENQUEUED, :DTM)", false,
                    new DbParam(":POOL_ID", DbParamType.Integer),
                    new DbParam(":HOST_ID", DbParamType.Integer),
                    new DbParam(":APP_ID", DbParamType.Integer),
                    new DbParam(":THREADS", DbParamType.Integer),
                    new DbParam(":INPROCESS", DbParamType.Integer),
                    new DbParam(":INQUEUE", DbParamType.Integer),
                    new DbParam(":STATUS", DbParamType.Byte),
                    new DbParam(":PROCESSED", DbParamType.Integer),
                    new DbParam(":ERRORS", DbParamType.Integer),
                    new DbParam(":PROCESS_TIME", DbParamType.Integer),
                    new DbParam(":FOREIGN_TIME", DbParamType.Integer),
                    new DbParam(":PICK_TIME", DbParamType.Integer),
                    new DbParam(":PICKS_COUNT", DbParamType.Integer),
                    new DbParam(":PICKED", DbParamType.Integer),
                    new DbParam(":PICK_ERRORS", DbParamType.Integer),
                    new DbParam(":ENQUEUED", DbParamType.Integer),
                    new DbParam(":DTM", DbParamType.DateTime)
                    );

            foreach (StatRecord record in records)
            {
                query.AddParamValues(
                    record.Source.PoolId,
                    this.hostIds[record.Source.Server],
                    this.appIds[record.Source.AppPath],
                    record.ThreadsCount,
                    record.InProcessCount,
                    record.InQueueCount,
                    (byte)record.Status,
                    record.ProcessedCount,
                    record.ErrorsCount,
                    record.ProcessTime,
                    record.ForeignTime,
                    record.PickTime,
                    record.PicksCount,
                    record.PickedCount,
                    record.PickErrors,
                    record.EnqueuedCount,
                    record.Timestamp
                    );
            }

            DbContext.Execute(query);
        }
    }
}