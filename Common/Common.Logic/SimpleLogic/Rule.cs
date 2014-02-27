using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Common.Logic.XML.Interfaces;

namespace Common.Logic.XML.SimpleLogic
{
    [Serializable]
    public class Rule : IRule
    {
        private IEnumerable<ICheckable> _childCheckers = new ICheckable[0];

        public string PropertyName { get; set; }

        public string CheckValue { get; set; }

        public bool IsEqual { get; set; }

        public IEnumerable<ICheckable> ChildCheckers
        {
            get { return _childCheckers; }
        }

        public string ItemName { get { return Constants.Rule; } }

        public bool Check(IDictionary<string, string> circs)
        {
            if(!circs.ContainsKey(PropertyName))
                throw new Exception(string.Format("В словаре условий проверки отсутствует ключ {0}", PropertyName));
            if (circs[PropertyName] == CheckValue && IsEqual)
                return true;
            if (circs[PropertyName] != CheckValue && !IsEqual)
                return true;
            return false;
        }

        public XElement Serialize()
        {
            var xElem = new XElement(ItemName);
            xElem.Add(new XAttribute(Constants.PropertyNameAttrName, PropertyName));
            xElem.Add(new XAttribute(Constants.CheckValueAttrName, CheckValue));
            xElem.Add(new XAttribute(Constants.IsEqualAttrName, IsEqual));
            return xElem;
        }

        public void DeSerialize(XElement xElem)
        {
            PropertyName = xElem.Attribute(Constants.PropertyNameAttrName).Value;
            CheckValue = xElem.Attribute(Constants.CheckValueAttrName).Value;
            IsEqual = XmlConvert.ToBoolean(xElem.Attribute(Constants.IsEqualAttrName).Value);
        }
    }
}