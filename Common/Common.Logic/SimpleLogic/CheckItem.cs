using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Common.Logic.XML.Interfaces;

namespace Common.Logic.XML.SimpleLogic
{
   [Serializable]
    public class CheckItem : CheckBase, ICheck
   {

       public CheckItem()
       {
           Rules = new RuleList();
       }
       public IRuleList Rules { get; private set; }

       
       public override bool Check(IDictionary<string, string> circs)
       {
           return Rules.Check(circs) && base.Check(circs);
       }


       public override string ItemName
       {
           get { return Constants.Check; }
       }

       public override XElement Serialize()
       {
           var xElem = new XElement(ItemName);
           xElem.Add(Rules.Serialize());
           base.Serialize(xElem);
           return xElem;
       }

       public override void DeSerialize(XElement xElem)
       {
           base.DeSerialize(xElem);
           var xRules = xElem.Element(Rules.ItemName);
           if(xRules == null)
               throw new Exception(string.Format("В элемент {0} не может не включать элемент {1}", ItemName, Rules.ItemName));
           Rules.DeSerialize(xRules);

       }}
}
