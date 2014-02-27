using System.Collections.Generic;

namespace Common.Net.Balancing
{
    /// <summary>
    /// Информация о состоянии сервера, участвующего в балансировании
    /// </summary>
    public interface IServerInfo : IEndpointInfo
    {
        /// <summary>
        /// Живой / неживой
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// Словарь параметров, описывающих состояние сервера
        /// </summary>
        IDictionary<string, object> Parameters { get; }
    }
}