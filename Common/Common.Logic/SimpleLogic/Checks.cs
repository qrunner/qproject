using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Common.Logic.XML.Interfaces;

namespace Common.Logic.XML.SimpleLogic
{
    public class Checks : CheckCollectionBase<ICheck>, IChecks
    {
        
        public override bool Check(IDictionary<string, string> circs)
        {
            return CheckMethods.CheckOr(circs, this);
        }

        public override IEnumerable<ICheckable> ChildCheckers { get { return this; } }

        public override string ItemName
        {
            get
            {
                return Constants.Checks;
            }
        }
    }
}
