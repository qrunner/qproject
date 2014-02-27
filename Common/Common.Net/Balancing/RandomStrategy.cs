using System;
using System.Net;

namespace Common.Net.Balancing
{
    public class RandomStrategy : SimpleStrategyBase
    {
        private readonly Random _rnd = new Random();

        public override DnsEndPoint GetIP()
        {
            if (IPList.Count == 0)
                throw new Exception("IP list is empty");
            return IPList[_rnd.Next(IPList.Count)].ExternalEndpoint;
        }
    }
}