using System;
using System.Collections.Generic;
using System.Configuration;

namespace Common.Configuration
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Проверяет наличие указанных параметров в конфигурации пула. В случае отсутствия какого-либо параметра возникает исключение.
        /// </summary>
        /// <param name="keyValueCollection">/Коллекция параметров ключ-значение.</param>
        /// <param name="mandatoryParams">Список имен параметров</param>
        public static void CheckConfigParametersExists(IDictionary<string, string> keyValueCollection, params string[] mandatoryParams)
        {
            foreach (string parName in mandatoryParams)
            {
                if (!keyValueCollection.ContainsKey(parName) || string.IsNullOrWhiteSpace(keyValueCollection[parName]))
                    throw new ArgumentException(string.Format("В конфигурации отсутствует значение обязательного параметра {0}.", parName), parName);
            }
        }

        public static void Save(string configKey, string value)
        {
            if (ConfigurationManager.AppSettings[configKey] != value)
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                AppSettingsSection asSection = config.AppSettings;
                asSection.Settings.Remove(configKey);
                asSection.Settings.Add(configKey, value);
                config.Save();
                ConfigurationManager.RefreshSection(asSection.SectionInformation.Name);
            }
        }

        public static void UpdateSetting(string key, string value)
        {
            System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}