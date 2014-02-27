using System;
using System.Collections.Generic;

namespace Common.Logic.XML
{
    [Serializable]
    public class CheckParams : List<Param>
    {
        public void Add(string name, string value)
        {
            Add(new Param(name, value));
        }
    }
}
