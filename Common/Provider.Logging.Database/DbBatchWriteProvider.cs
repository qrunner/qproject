using System;
using System.Collections.Generic;
using Provider.Database;
using Provider.Logging.BatchWrite;

namespace Provider.Logging.Database
{
    public class DbBatchWriteProvider : BatchWriteProvider
    {
        DatabaseContext _dbContext = null;
        object sync = new object();
        protected string providerName = "";
        protected string serversTable = "SOZ_STAT_SERVERS";
        protected string poolsTable = "SOZ_STAT_POOLS";
        protected string appPathTable = "SOZ_STAT_APPS";
        protected bool saveStat = true;
        protected Dictionary<string, int> poolIds = new Dictionary<string, int>();
        protected Dictionary<string, int> hostIds = new Dictionary<string, int>();
        protected Dictionary<string, int> appIds = new Dictionary<string, int>();

        protected DatabaseContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    lock (sync)
                    {
                        if (_dbContext == null)
                        {
                            _dbContext = new DatabaseContext(providerName, this.ConnectionString);
                        }
                    }
                }
                return _dbContext;
            }
        }

        protected override void Initialize(IDictionary<string,string> settings)
        {
            CheckConfigParametersExists(settings, "dbProvider");

            try
            {
                providerName = settings["dbProvider"];
                // проверить подключение к БД
                Exception ex = null;
                if (!DbContext.Provider.CheckConnection(out ex))
                {
                    saveStat = false;
                    throw new Exception("Ошибка подключения к базе данных", ex);
                }
            }
            catch (Exception)
            {
                saveStat = false;
                throw;
            }
            
            base.Initialize(settings);            
        }

        protected override void InitSourceData()
        {
            SourceInfo si = new SourceInfo(null);
            hostIds[si.Server] = GetServerId(si.Server);
            appIds[si.AppPath] = GetAppPathId(si.AppPath);            
        }
        
        public override void AddEntry(IEntry entry)
        {
            if (!saveStat) return;

            if (!string.IsNullOrWhiteSpace(entry.Source.Pool))
            {
                // проверить наличие записи в таблице пулов
                if (!poolIds.ContainsKey(entry.Source.Pool))
                {
                    lock (sync) // отладить при многопоточности (есть проблемы при появлении нового пула)
                    {
                        if (!poolIds.ContainsKey(entry.Source.Pool))
                            poolIds.Add(entry.Source.Pool, GetPoolId(entry.Source.Pool));
                    }
                }

                entry.Source.PoolId = poolIds[entry.Source.Pool];
            }
            else
            {
                entry.Source.PoolId = -1;
            }

            base.AddEntry(entry);
        }

        public override bool CheckConnection(out Exception ex)
        {
            return DbContext.Provider.CheckConnection(out ex);
        }

        private int GetServerId(string serverName)
        {
            return Convert.ToInt32(DbContext.GetRecordIdOrInsertIfNotExsists(
                new ScalarQuery("SELECT ID FROM T_INTEGRATION_HOSTS WHERE NAME=:server", false, new DbParamValue(":server", serverName, DbParamType.String)),
                new ScalarQuery("SELECT SQ_INTEGRATION_HOSTS.nextval FROM DUAL", false),
                new NonSelectQuery("INSERT INTO T_INTEGRATION_HOSTS (ID, NAME) VALUES (:ID, :NAME)", false,
                    new DbParamValue(":ID", null, DbParamType.Integer), new DbParamValue(":NAME", serverName, DbParamType.String))));
        }

        private int GetPoolId(string poolName)
        {
            return Convert.ToInt32(DbContext.GetRecordIdOrInsertIfNotExsists(
                new ScalarQuery("SELECT ID FROM T_INTEGRATION_POOLS WHERE NAME=:pool", false, new DbParamValue(":pool", poolName, DbParamType.String)),
                new ScalarQuery("SELECT SQ_INTEGRATION_POOLS.nextval FROM DUAL", false),
                new NonSelectQuery("INSERT INTO T_INTEGRATION_POOLS (ID, NAME) VALUES (:ID, :NAME)", false,
                    new DbParamValue(":ID", null, DbParamType.Integer), new DbParamValue(":NAME", poolName, DbParamType.String))));
        }
        
        private int GetAppPathId(string appPath)
        {
            return Convert.ToInt32(DbContext.GetRecordIdOrInsertIfNotExsists(
                new ScalarQuery("SELECT ID FROM T_INTEGRATION_APPS WHERE NAME=:pool", false, new DbParamValue(":pool", appPath, DbParamType.String)),
                new ScalarQuery("SELECT SQ_INTEGRATION_APPS.nextval FROM DUAL", false),
                new NonSelectQuery("INSERT INTO T_INTEGRATION_APPS (ID, NAME) VALUES (:ID, :NAME)", false,
                    new DbParamValue(":ID", null, DbParamType.Integer), new DbParamValue(":NAME", appPath, DbParamType.String))));
        }
    }
}