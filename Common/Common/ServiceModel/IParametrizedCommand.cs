using System.Collections.Generic;

namespace Common.ServiceModel
{
    /// <summary>
    /// Интерфейс параметризованной команды
    /// </summary>
    public interface IParametrizedCommand : ICommand
    {
        /// <summary>
        /// Параметры команды
        /// </summary>
        IDictionary<string, object> Arguments { get; set; }
    }
}
