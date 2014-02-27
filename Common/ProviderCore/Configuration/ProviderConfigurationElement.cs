using System.Configuration;

namespace Provider.Configuration
{
    public class ProviderConfigurationElement : NamedConfigurationElement
    {
        const string _providerClass = "providerClass";
        [ConfigurationProperty(_providerClass, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string ProviderClass
        {
            get { return ((string)(base[_providerClass])); }
            set { base[_providerClass] = value; }
        }

        const string _connectionString = "connectionString";
        [ConfigurationProperty(_connectionString, DefaultValue = "", IsKey = false, IsRequired = false)]
        public string ConnectionString
        {
            get { return ((string)(base[_connectionString])); }
            set { base[_connectionString] = value; }
        }

        const string _settings = "settings";
        [ConfigurationProperty(_settings)]
        public KeyValueConfigurationCollection Settings { get { return base[_settings] as KeyValueConfigurationCollection; } }
    }
}