using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Common.Logic.XML.Interfaces;

namespace Common.Logic.XML.SimpleLogic
{
    public abstract class CheckCollectionBase<T> : List<T>, ICheckable where T : class, ICheckable 
    {
        public abstract bool Check(IDictionary<string, string> circs);

        public abstract IEnumerable<ICheckable> ChildCheckers { get; }


        public abstract string ItemName { get; }


        public virtual XElement Serialize()
        {
            var xElem = new XElement(ItemName);
            foreach (var condition in this)
            {
                xElem.Add(condition.Serialize());
            }
            return xElem;
        }


        public virtual void DeSerialize(XElement xElem)
        {
            foreach (var xelement in xElem.Elements())
            {
                T elem = Constants.Factory.Deserialize(xelement) as T;
                if (elem == null)
                    throw new Exception(string.Format("Коллекция {0} может включать только элементы {1}, но не {2}.",
                                                      GetType(), typeof (T), elem.GetType()));
                Add(elem);
            }
        }
    }
}