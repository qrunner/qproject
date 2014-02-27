using System.Net;

namespace Common.Net.Balancing
{
    public class RoundRobinStrategy : SimpleStrategyBase
    {
        private int _id;

        public override DnsEndPoint GetIP()
        {
            _id = (_id + 1)%IPList.Count;
            return IPList[_id].ExternalEndpoint;
        }
    }
}