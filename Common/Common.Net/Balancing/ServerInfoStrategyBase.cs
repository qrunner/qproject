using System.Collections.Generic;

namespace Common.Net.Balancing
{
    public interface IServerInfoStrategyBase : IBalancerStrategy
    {
        IEnumerable<IServerInfo> Servers { get; }
    }
}

/*
 
   var aliveServers = Servers.Where(s => s.IsAlive).ToArray();

            if (!aliveServers.Any())
                throw new Exception("No active servers available");

            return aliveServers.Aggregate((min, testMin) => min.SessionsCount < testMin.SessionsCount ? min : testMin).Endpoint;
 
 */