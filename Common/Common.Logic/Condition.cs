using System;

namespace Common.Logic.XML
{
    [Serializable]
    public class Condition
    {
        public CheckParams Checks { get; set; }
        public Attr[] Attributes { get; set; }
        public Condition[] NestedConditions { get; set; }
    }
}
