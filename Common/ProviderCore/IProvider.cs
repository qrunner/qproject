using System;
using System.Collections.Generic;

namespace Provider
{
    /// <summary>
    /// Провайдер доступа к внешним источникам данных
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// Строка подключения к источнику данных для провайдера (задается в файле конфигурации)
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Имя провайдера (задается в файле конфигурации)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Основной метод инициализации провайдера
        /// </summary>
        /// <param name="providerName">Имя провайдера</param>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="settings">Список настроек</param>
        void Initialize(string providerName, string connectionString, IDictionary<string, string> settings);

        /// <summary>
        /// Проверяет подключение к источнику данных провайдера
        /// </summary>
        /// <param name="ex">Исключение при попытке подключения</param>
        /// <returns>Успешность подключения</returns>
        bool CheckConnection(out Exception ex);
    }
}