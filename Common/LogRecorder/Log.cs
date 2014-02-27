using System;
using System.Collections.Generic;
using System.Configuration;
using Provider.Configuration;
using Provider.Logging.BatchWrite;
using Provider.Logging.Configuration;

namespace Provider.Logging
{
    public static class Log
    {
        static Log()
        {
            //AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            // ! Вынести в отдельный метод
            emergencyProvider = (BatchWriteProvider)ProviderFactory.GetProvider(Settings.EmergencyProviderName);
        }
        /*
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            FlushAllBuffers();
        }*/

        static object lockObj = new object();

        static LogConfigSection settings = null;
        public static LogConfigSection Settings
        {
            get
            {
                if (settings == null)
                {
                    lock (lockObj)
                    {
                        if (settings == null)
                        {
                            settings = (LogConfigSection)ConfigurationManager.GetSection(LogConfigSection._masterPrefix);
                        }
                    }
                }
                return settings;
            }
        }

        static BatchWriteProvider emergencyProvider = null;
        static Dictionary<LogLevel, HashSet<BatchWriteProvider>> levelProviders = null;

        public static bool IsLevelActive(LogLevel level)
        {
            return LevelProviders.ContainsKey(level) && LevelProviders[level].Count > 0;
        }

        static Dictionary<LogLevel, HashSet<BatchWriteProvider>> LevelProviders
        {
            get
            {
                if (levelProviders == null)
                {
                    lock (lockObj)
                    {
                        if (levelProviders == null)
                        {
                            levelProviders = new Dictionary<LogLevel, HashSet<BatchWriteProvider>>();
                            foreach (LogLevelsConfig lglevel in Settings.Levels)
                            {
                                if (!lglevel.Enabled) continue;

                                HashSet<BatchWriteProvider> listeners = new HashSet<BatchWriteProvider>();

                                foreach (NamedConfigurationElement ll in lglevel.ActiveProviders)
                                {
                                    try
                                    {
                                        BatchWriteProvider lst = (BatchWriteProvider)ProviderFactory.GetProvider(ll.Name, null);
                                        lst.OnError += lst_OnError;
                                        listeners.Add(lst);
                                    }
                                    catch (Exception ex)
                                    {
                                        Exception exception = new Exception("Ошибка инициализации провайдера логирования.", ex);

                                        if (emergencyProvider != null)                                            
                                            emergencyProvider.AddEntry(new LogEntry(LogLevel.Error, "Ошибка инициализации провайдера логирования.") { Exception = exception });
                                            
                                        throw exception;
                                    }
                                }

                                if (listeners.Count > 0)
                                    levelProviders.Add(lglevel.Code, listeners);
                            }
                        }
                    }
                }
                return levelProviders;
            }
        }

        static void lst_OnError(object sender, WriteBatchExceptionEventArgs e)
        {
            if (emergencyProvider != null && sender != emergencyProvider) // при таком условии ошибка в запасном провайдере потеряется
            {
                foreach (IEntry record in e.FlushRecords)
                    emergencyProvider.AddEntry(record);
                emergencyProvider.AddEntry(new LogEntry(LogLevel.Error, "Ошибка при записи в лог. Подробности во вложенном исключении.") { Exception = e.Ex });
                emergencyProvider.FlushAsync();
            }
        }

        static void WriteEntry(LogEntry entry)
        {
            if (LevelProviders.ContainsKey(entry.Level))
            {
                foreach (BatchWriteProvider prv in LevelProviders[entry.Level])
                {
                    prv.AddEntry(entry);
                }
            }

            if (entry.Level == LogLevel.Error || entry.Level == LogLevel.Fatal)
            {
                FlushAllBuffersAsync();
            }
        }

        public static void FlushAllBuffersAsync()
        {
            foreach (LogLevel lvl in Enum.GetValues(typeof(LogLevel)))
            {
                if (LevelProviders.ContainsKey(lvl))
                {
                    foreach (BatchWriteProvider lst in LevelProviders[lvl])
                    {
                        lst.FlushAsync();
                    }
                }
            }
        }

        public static void FlushAllBuffers()
        {
            foreach (LogLevel lvl in Enum.GetValues(typeof(LogLevel)))
            {
                if (LevelProviders.ContainsKey(lvl))
                {
                    foreach (BatchWriteProvider lst in LevelProviders[lvl])
                    {
                        lst.Flush();
                    }
                }
            }
        }
        // ====== INFO ======
        public static void WriteInfo(string message)
        {
            WriteEntry(new LogEntry(LogLevel.Info, message));
        }

        public static void WriteInfo(string message, long code, string poolName)
        {
            WriteEntry(new LogEntry(LogLevel.Info, message, poolName, code));
        }
        // ====== DEBUG ======
        public static void WriteDebug(string message)
        {
            WriteEntry(new LogEntry(LogLevel.Debug, message));
        }

        public static void WriteDebug(string message, long code, string poolName)
        {
            WriteEntry(new LogEntry(LogLevel.Debug, message, poolName, code));
        }

        public static void WriteDebug(string message, long code, string poolName, string additional)
        {
            WriteEntry(new LogEntry(LogLevel.Debug, message, poolName, code) { additional = additional });
        }

        // ====== ERROR ======
        public static void WriteError(string message, Exception ex)
        {
            WriteEntry(new LogEntry(LogLevel.Error, message) { Exception = ex });
        }        

        public static void WriteError(string message, long code, string poolName, Exception ex)
        {
            WriteEntry(new LogEntry(LogLevel.Error, message, poolName, code) { Exception = ex });
        }

        // ====== WARNING ======

        public static void WriteWarning(string message, long code, string poolName)
        {
            WriteEntry(new LogEntry(LogLevel.Warning, message, poolName, code));
        }
        /* 
         /// <summary>
         /// Основной провайдер лога
         /// </summary>
         static ILogWriterProvider DefaultProvider
         {
             get
             {
                 if (defaultProvider == null)
                 {
                     lock (lockObj)
                     {
                         if (defaultProvider == null)
                         {
                             try
                             {
                                 defaultProvider = ProviderFactory.GetProvider(Settings.DefaultListener.ProviderName) as ILogWriterProvider;
                                 defaultProvider.SetEmergencyProvider(EmergencyProvider);
                             }
                             catch (Exception ex)
                             {
                                 throw new Exception(string.Format("Ошибка инициализации основного провайдра журнала <{0}>.", "DefaultLog"), ex);
                             }
                         }
                     }
                 }
                 return defaultProvider;
             }
         }
         /// <summary>
         /// Запасной провайдер лога
         /// </summary>
         static ILogWriterProvider EmergencyProvider
         {
             get
             {
                 if (emergencyProvider == null)
                 {
                     lock (lockObj)
                     {
                         if (emergencyProvider == null)
                         {
                             try
                             {
                                 emergencyProvider = ProviderFactory.GetProvider(Settings.EmergencyListener.ProviderName) as ILogWriterProvider;
                                 emergencyProvider.BufferCapacity = 1;
                             }
                             catch (Exception ex)
                             {
                                 emergencyProvider = new FileLogProvider();
                                 emergencyProvider.ConnectionString = @"~\log\emergency.log";
                                 emergencyProvider.BufferCapacity = 1;
                                 Exception expt = new Exception(string.Format("Ошибка инициализации вспомогательного провайдера журнала <{0}>.", "EmergencyLogProvider"), ex);
                                 emergencyProvider.WriteError("Используем провайдер журнала по умолчанию: " + emergencyProvider.ConnectionString, expt);
                                 throw expt;
                             }
                         }
                     }
                 }
                 return emergencyProvider;
             }
         }

         public static void WriteInfo(int obj_code, string message)
         {            
             DefaultProvider.WriteInfo(obj_code, message);
         }

         public static void WriteInfo(string message)
         {
             DefaultProvider.WriteInfo(message);
         }

         public static void WriteError(string message, Exception ex)
         {
             DefaultProvider.WriteError(message, ex);
         }

         public static void WriteError(int obj_code, string message, Exception ex)
         {
             DefaultProvider.WriteError(obj_code, message, ex);
         }

         public static void WriteDebug(string message)
         {
             DefaultProvider.WriteDebug(message);
         }

         public static void WriteDebug(int obj_code, string message)
         {
             DefaultProvider.WriteDebug(obj_code, message);
         }
         * */
    }
}