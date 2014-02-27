using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.Logic.XML.Interfaces;

namespace Common.Logic.XML.SimpleLogic
{
    [Serializable]
    public abstract class CheckBase : IChckBase
    {
        protected CheckBase()
        {
            Checks = Constants.Factory.ChecksCreate();
        }public IChecks Checks { get; private set; }

        public virtual bool Check(IDictionary<string, string> circs)
        {
            return Checks.Check(circs);
        }

        public IEnumerable<ICheckable> ChildCheckers
        {
            get
            {
                return Checks;
            }
        }

        public abstract string ItemName { get; }


        public virtual XElement Serialize()
        {
            var xItem = new XElement(ItemName);

            Serialize(xItem);

            return xItem;
        }
        
        protected void Serialize(XElement xItem)
        {
            xItem.Add(Checks.Serialize());
        }
        
        public virtual void DeSerialize(XElement xElem)
        {
            var xChecks = xElem.Element(Checks.ItemName);
            if (xChecks != null)
                Checks.DeSerialize(xChecks);
        }
    }
}
