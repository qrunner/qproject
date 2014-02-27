using System.Configuration;

namespace Provider.Configuration
{
    public class ProviderConfigSection : ConfigurationSection
    {
        public const string _masterPrefix = "providersModel";

        const string _providers = "providers";
        [ConfigurationProperty(_providers)]
        public ProviderConfigurationCollection Providers { get { return ((ProviderConfigurationCollection)(base[_providers])); } }
    }
}
