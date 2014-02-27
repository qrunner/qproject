using System.Configuration;
using Provider.Configuration;

namespace Configuration
{
    /// <summary>
    /// Список пулов назначения
    /// </summary>
    [ConfigurationCollection(typeof(NamedConfigurationElement), AddItemName = "add")]
    public class ToPoolsCollection : ConfigurationElementCollectionBase<NamedConfigurationElement> { }
}
