using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.Logic.XML.Interfaces;

namespace Common.Logic.XML.SimpleLogic
{
   [Serializable]
    public class RuleList : CheckCollectionBase<IRule>, IRuleList
    {
        public OperationType Operation { get; set; }

        public void Add(string propertyName, string checkValue, bool isEqual = true)
        {
            Add(new Rule(){PropertyName = propertyName, CheckValue = checkValue, IsEqual = isEqual});
        }

        public override bool Check(IDictionary<string, string> circs)
        {
            if (this.Any(rule => rule.Check(circs) && Operation == OperationType.Or))
                return true;
            if(this.All(rule => rule.Check(circs)) && Operation == OperationType.And)
                return true;
            return false;
        }

       public override IEnumerable<ICheckable> ChildCheckers
       {
           get { return this; }
       }

       public override string ItemName { get { return Constants.Rules; } }

       public override void DeSerialize(XElement xElem)
       {
           base.DeSerialize(xElem);

           var operAttr = xElem.Attribute(Constants.OperationAttrName);

           Operation = operAttr == null ? OperationType.Or : (OperationType)Enum.Parse(typeof(OperationType), operAttr.Value);
       }

       public override XElement Serialize()
       {var xElem = base.Serialize();

           xElem.Add(new XAttribute(Constants.OperationAttrName, Operation));

           return xElem;
       }
    }
}