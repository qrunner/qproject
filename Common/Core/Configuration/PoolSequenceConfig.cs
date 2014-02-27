using System.Configuration;
using Provider.Configuration;

namespace Configuration
{
    /// <summary>
    /// Настройка порядка передачи объекта между пулами
    /// </summary>
    public class PoolSequenceConfig : NamedConfigurationElement
    {
        const string _toPools = "toPools";
        [ConfigurationProperty(_toPools)]
        public ToPoolsCollection ToPools { get { return (ToPoolsCollection)base[_toPools]; } }
    }
}
