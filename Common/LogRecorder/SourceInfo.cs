using System.Reflection;

namespace Provider.Logging
{
    /// <summary>
    /// Контейнер статических данных записи
    /// </summary>
    public class SourceInfo
    {
        public SourceInfo(string poolName)
        {
            Pool = poolName;
        }

        private static string appPath = null;
        /// <summary>
        /// Путь исполняемого файла
        /// </summary>
        public string AppPath
        {
            get
            {
                if (SourceInfo.appPath == null)
                {
                    SourceInfo.appPath = Assembly.GetEntryAssembly().Location;
                }
                return SourceInfo.appPath;
            }
        }
        /*
        private int appPathId = -1;
        /// <summary>
        /// Идентификатор пути
        /// </summary>
        public int AppPathId
        {
            get { return SourceInfo.appPathId; }
            set { SourceInfo.appPathId = value; }
        }
        */
        private static string server = null;
        /// <summary>
        /// Сетевое имя компьютера
        /// </summary>
        public string Server
        {
            get
            {
                if (SourceInfo.server == null)
                {
                    SourceInfo.server = System.Net.Dns.GetHostName();
                }
                return SourceInfo.server;
            }
        }
        private int serverId = -1;
       /* /// <summary>
        /// Идентификатор сервера
        /// </summary>
        public int ServerId
        {
            get { return SourceInfo.serverId; }
            set { SourceInfo.serverId = value; }
        }
        * */
        /// <summary>
        /// Имя пула
        /// </summary>
        public string Pool = null;
        /// <summary>
        /// Идентификатор пула
        /// </summary>
        public int PoolId = -1;        
        /*
         * /// <summary>
        /// Инициализированы данные или нет
        /// </summary>
        public bool Initialized { get { return appPathId > 0 && serverId > 0 && PoolId > 0; } }
         * */
    }
}