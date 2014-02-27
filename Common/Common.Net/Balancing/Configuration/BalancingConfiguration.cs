using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Xml.Serialization;

namespace Common.Net.Balancing.Configuration
{
    [Serializable]
    [XmlRoot(Namespace="")]
    public class BalancingConfiguration
    {
        public EndpointConfiguration[] Servers { get; set; }

        /*public IEnumerable<DnsEndPoint> Endpoints
        {
            get { return Servers != null ? Servers.Select(x => new DnsEndPoint(x.Host, x.Port)) : null; }
        }*/
    }
}