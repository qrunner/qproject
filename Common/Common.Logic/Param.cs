using System;
using System.Xml.Serialization;

namespace Common.Logic.XML
{
    [Serializable]
    public class Param
    {
        public Param()
        {
        }

        public Param(string name, string value)
        {
            Name = name;
            Value = value;
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Value { get; set; }
    }
}
