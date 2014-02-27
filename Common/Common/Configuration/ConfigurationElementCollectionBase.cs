using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Common.Configuration
{
    /*public class ConfigurationElementCollectionBase<T> : ConfigurationElementCollection where T : ConfigurationElement, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            //return ((T)(element)).Name;
        }

        public T this[int idx]
        {
            get { return (T)BaseGet(idx); }
        }

        public T this[string name]
        {
            get { return (T)BaseGet(name); }
        }
    }*/
}