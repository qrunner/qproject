using Common.Configuration;
using Common.Reflection;
using Provider.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Provider
{
    /// <summary>
    /// Класс для создания экземпляров провайдеров на основе конфигурации
    /// </summary>
    public static class ProviderFactory
    {
        private static readonly Dictionary<string, IProvider> ProvidersCashe = new Dictionary<string, IProvider>();
        /*/// <summary>
        /// Возвращает провайдер по имени строки подключения. Создает или возвращает из кеша.
        /// </summary>
        /// <param name="connection_string_name">Имя строки подключения в </param>
        /// <returns>Экземпляр провайдера</returns>
        public static IProvider GetProvider(string connection_string_name)
        {
            ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[connection_string_name];

            if (cs == null) throw new ArgumentException(string.Format("В файле конфигурации не найдена строка соединения с именем <{0}>", connection_string_name), "connection_string_name");

            IProvider retval = null;
            lock (providersCashe)
            {
                if (providersCashe.ContainsKey(cs.ProviderName))
                    retval = providersCashe[cs.ProviderName];
                else
                {
                    retval = CreateProvider(cs.ProviderName, cs.ConnectionString);
                    providersCashe.Add(cs.ProviderName, retval);
                }
            }
            return retval;
        }*/

        /// <summary>
        /// Возвращает экземпляр провайдера. Если он создавался ранее, то берется из кеша
        /// </summary>
        /// <param name="providerName">Имя провайдера в файле конфигурации</param>        
        /// <returns>Экземпляр заданного провайдера</returns>
        public static IProvider GetProvider(string providerName)
        {
            return GetProvider(providerName, null);
        }

        /// <summary>
        /// Возвращает экземпляр провайдера. Если он создавался ранее, то берется из кеша
        /// </summary>
        /// <param name="providerName">Имя провайдера в файле конфигурации</param>
        /// <param name="connectionString">Имя строки соединения</param>
        /// <returns>Экземпляр заданного провайдера</returns>
        public static IProvider GetProvider(string providerName, string connectionString)
        {
            IProvider retval;
            string key = providerName + connectionString;
            lock (ProvidersCashe)
            {
                if (ProvidersCashe.ContainsKey(key))
                    retval = ProvidersCashe[key];
                else
                {
                    retval = CreateProvider(providerName, connectionString);
                    ProvidersCashe.Add(key, retval);
                }
            }
            return retval;
        }

        /// <summary>
        /// Создает новый экземпляр провайдера
        /// </summary>
        /// <param name="providerName">Имя провайдера в файле конфигурации</param>        
        /// <param name="connectionString">Имя строки соединения</param>        
        /// <returns>Экземпляр заданного провайдера</returns>
        public static IProvider CreateProvider(string providerName, string connectionString)
        {
            ProviderConfigSection pSection = ConfigurationManager.GetSection(ProviderConfigSection._masterPrefix) as ProviderConfigSection;
            
            if (pSection == null) 
                throw new Exception("В файле конфигурации не найдена секция " + ProviderConfigSection._masterPrefix);
            
            ProviderConfigurationElement provider = pSection.Providers[providerName];
            
            if (provider == null) 
                throw new ArgumentException(string.Format("В файле конфигурации не найден провайдер с именем <{0}>.", providerName), "providerName");
            
            ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[string.IsNullOrWhiteSpace(connectionString) ? provider.ConnectionString : connectionString];
            
            if (cs == null)
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    cs = new ConnectionStringSettings(null, connectionString);
                else throw new ArgumentException(string.Format("В файле конфигурации не найдена строка соединения с именем <{0}>", provider.ConnectionString), "connectionString");
            }
            
            Type provType;
            if (!TypeLoader.TryGetType(provider.ProviderClass, out provType))
                throw new ArgumentException(string.Format("Не удается инициализировать провайдер <{1}>. Невозможно найти тип <{0}>.", provider.ProviderClass, providerName), "providerClass");

            IProvider retval = Activator.CreateInstance(provType) as IProvider;
            if (retval == null)
                throw new Exception(string.Format("Класс <{0}> не реализует интерфейс IProvider.", provider.ProviderClass));
            
            retval.Initialize(providerName, cs.ConnectionString, new KeyValueConfigurationCollectionWrapper(provider.Settings));
            
            return retval;
        }
    }
}