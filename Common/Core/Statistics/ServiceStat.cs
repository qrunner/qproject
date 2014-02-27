using Configuration;
using Provider;
using Provider.Logging.BatchWrite;
using System.Configuration;

namespace Statistics
{
    public static class ServiceStat
    {
        private static PoolsConfigRoot section = null;
        private static BatchWriteProvider provider = null;
        private static bool saveStat = false;
        private static bool wasInitialized = false;

        static ServiceStat()
        {
            
        }

        public static void WriteStat(StatRecord entry)
        {
            if (saveStat && !entry.IsEmptyProcessing)
                provider.AddEntry(entry);
        }

        public static void Init()
        {
            saveStat = false;
            wasInitialized = true;

            section = (PoolsConfigRoot)ConfigurationManager.GetSection(PoolsConfigRoot.SectionName);
            if (section.SaveStat)
            {
                provider = (BatchWriteProvider)ProviderFactory.GetProvider(section.StatProviderName);
                /*provider.BufferCapacity = section.FlushCount;
                provider.FlushInterval = section.FlushInterval;*/
                saveStat = true;
            }
            /* инициализируется ранее
            if (saveStat)
            {
                Exception ex = null;
                if (provider != null && !provider.CheckConnection(out ex))
                {
                    throw new Exception("Ошибка инициализации статистики.", ex);
                }
            } 
            */
        }
    }
}