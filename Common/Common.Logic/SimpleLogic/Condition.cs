using System;
using System.Xml;
using System.Xml.Linq;
using Common.Logic.XML.Interfaces;

namespace Common.Logic.XML.SimpleLogic
{
    [Serializable]
    public class Condition : CheckBase, ICondition
    {
        public string Name { get; set; }

        public bool DefaultValue { get; set; }

        public override string ItemName
        {
            get { return Constants.Condition; }
        }

        public override bool Check(System.Collections.Generic.IDictionary<string, string> circs)
        {
            var baseRes = base.Check(circs);
            return DefaultValue ? !baseRes : baseRes;
        }

        public override XElement Serialize()
        {
            var xElem = base.Serialize();
            xElem.Add(new XAttribute(Constants.NameAttrName, Name));
            xElem.Add(new XAttribute(Constants.DefaultValueAttrName, DefaultValue));
            return xElem;
        }

        public override void DeSerialize(XElement xElem)
        {
            base.DeSerialize(xElem);
            Name = xElem.Attribute(Constants.NameAttrName).Value;

            var defaultValueAttr = xElem.Attribute(Constants.DefaultValueAttrName);

            DefaultValue = defaultValueAttr==null ? false :  XmlConvert.ToBoolean(xElem.Attribute(Constants.DefaultValueAttrName).Value);
        }
    }
}