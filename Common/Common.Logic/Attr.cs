using System;
using System.Xml.Serialization;

namespace Common.Logic.XML
{
    [Serializable]
    public class Attr
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool Value { get; set; }
    }
}