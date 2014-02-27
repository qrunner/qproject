using System.Configuration;

namespace Provider.Configuration
{
    public class NamedConfigurationElement : ConfigurationElement
    {
        const string _name = "name";
        [ConfigurationProperty(_name, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base[_name]; }
            set { base[_name] = value; }
        }
    }

    public class ConfigurationElementCollectionBase<T> : ConfigurationElementCollection where T : NamedConfigurationElement, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {            
            return ((T)(element)).Name;
        }

        public T this[int idx]
        {
            get { return (T)BaseGet(idx); }
        }

        public T this[string name]
        {
            get { return (T)BaseGet(name); }
        }
    }
}