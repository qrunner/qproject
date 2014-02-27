using System.Configuration;

namespace Provider.Configuration
{
    [ConfigurationCollection(typeof(ProviderConfigurationElement), AddItemName = "provider")]
    public class ProviderConfigurationCollection : ConfigurationElementCollectionBase<ProviderConfigurationElement> { }
    
    /*public class ProviderConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProviderConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProviderConfigurationElement)(element)).Name;
        }

        public ProviderConfigurationElement this[int idx]
        {
            get { return (ProviderConfigurationElement)BaseGet(idx); }
        }

        public ProviderConfigurationElement this[string name]
        {
            get { return (ProviderConfigurationElement)BaseGet(name); }
        }
    }*/
}