using System.Net;

namespace Common.Net.Balancing
{
    /// <summary>
    /// Информация о точке подключения
    /// </summary>
    public interface IEndpointInfo
    {
        /// <summary>
        /// Внутренний адрес точки подключения
        /// </summary>
        DnsEndPoint Endpoint { get; }

        /// <summary>
        /// Адрес точки подключения, который возвращается клиенту. Если не задан, возвращается внутренний адрес.
        /// </summary>
        DnsEndPoint ExternalEndpoint { get; }
    }
}
