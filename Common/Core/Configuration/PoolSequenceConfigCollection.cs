using System.Configuration;
using Provider.Configuration;

namespace Configuration
{
    /// <summary>
    /// Список пулов в последовательности
    /// </summary>
    [ConfigurationCollection(typeof(PoolSequenceConfig), AddItemName = "fromPool")]
    public class PoolSequenceConfigCollection : ConfigurationElementCollectionBase<PoolSequenceConfig> { }
}
