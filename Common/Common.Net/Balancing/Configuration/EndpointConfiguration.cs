using System;
using System.Net;
using System.Xml.Serialization;

namespace Common.Net.Balancing.Configuration
{
    [Serializable]
    public class EndpointConfiguration : IEndpointInfo
    {
        [XmlAttribute("host")]
        public string Host { get; set; }

        [XmlAttribute("port")]
        public int Port { get; set; }

        [XmlAttribute("comment")]
        public string Comment { get; set; }

        [XmlAttribute("aliasHost")]
        public string AliasHost { get; set; }

        [XmlAttribute("aliasPort")]
        public int AliasPort { get; set; }

        public DnsEndPoint Endpoint
        {
            get { return new DnsEndPoint(Host, Port); }
        }

        public DnsEndPoint ExternalEndpoint
        {
            get { return new DnsEndPoint(AliasHost, AliasPort); }
        }
    }
}