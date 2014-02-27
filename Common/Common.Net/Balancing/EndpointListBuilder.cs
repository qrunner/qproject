using System.Collections.Generic;
using System.Linq;
using System.Net;
using Common.Strings;

namespace Common.Net.Balancing
{
    public static class EndpointListBuilder
    {
        /// <summary>
        /// Получает список DnsEndPoint из строки вида 192.168.0.1:80; 192.168.0.2:88...;
        /// </summary>
        /// <param name="ipList">Строка со списком точек подключения</param>
        /// <returns>Список DnsEndPoint</returns>
        public static IEnumerable<DnsEndPoint> FromString(string ipList)
        {
            var addresses = StringParser.GetDictionary(ipList, ';', ':');
            return addresses.Select(item => new DnsEndPoint(item.Key, int.Parse(item.Value)));
        }
    }
}