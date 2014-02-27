using System;
using System.Collections.Generic;
using Common.Collections;

namespace Provider
{
    /// <summary>
    /// Базовый класс провайдера. Должен быть унаследован.
    /// </summary>
    public abstract class Provider : IProvider
    {
        protected Provider()
        {
            ConnectionString = null;
            Name = null;
        }

        /// <summary>
        /// Строка подключения к источнику данных для провайдера (задается в файле конфигурации)
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Имя провайдера (задается в файле конфигурации)
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Основной метод инициализации провайдера
        /// </summary>
        /// <param name="providerName">Имя провайдера</param>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="settings">Список настроек</param>
        public void Initialize(string providerName, string connectionString, IDictionary<string, string> settings)
        {
            ConnectionString = connectionString;
            Name = providerName;
            Initialize(settings);
        }

        /// <summary>
        /// Переопределяемый метод инициализации
        /// </summary>
        /// <param name="settings">Список настроек провайдера</param>
        protected abstract void Initialize(IDictionary<string, string> settings);

        /// <summary>
        /// Проверяет подключение к источнику данных провайдера
        /// </summary>
        /// <param name="ex">Исключение при попытке подключения</param>
        /// <returns>Успешность подключения</returns>
        public abstract bool CheckConnection(out Exception ex);

        protected void CheckConfigParametersExists(IDictionary<string, string> settings, params string[] mandatoryParams)
        {
            try
            {
                settings.CheckValuesNotEmpty(mandatoryParams);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("В настройках провайдера <" + GetType() + "> не заданы обязательные параметры.", ex);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}