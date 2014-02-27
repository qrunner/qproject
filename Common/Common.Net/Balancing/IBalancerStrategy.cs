using System.Collections.Generic;
using System.Net;

namespace Common.Net.Balancing
{
    /// <summary>
    /// Предоставляет интерфейс для стратегии получения следующего IP адреса
    /// </summary>
    public interface IBalancerStrategy
    {
        /// <summary>
        /// Получает следующий IP из списка заданных адресов
        /// </summary>
        /// <returns>IP-адрес</returns>
        DnsEndPoint GetIP();

        /// <summary>
        /// Задает список доступных IP-адресов для распределения
        /// </summary>
        /// <param name="endpoints">Список точек подключения</param>
        void SetEndpoints(IEnumerable<IEndpointInfo> endpoints);
    }
}