using System.Collections.Generic;
using System.Net;

namespace Common.Net.Balancing
{
    public abstract class SimpleStrategyBase : IBalancerStrategy
    {
        protected IList<IEndpointInfo> IPList = new List<IEndpointInfo>();

        public abstract DnsEndPoint GetIP();

        public virtual void SetEndpoints(IEnumerable<IEndpointInfo> endpoints)
        {
            IPList = new List<IEndpointInfo>(endpoints);
        }
    }
}