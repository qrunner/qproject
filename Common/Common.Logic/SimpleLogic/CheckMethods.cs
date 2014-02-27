using System.Collections.Generic;
using System.Linq;
using Common.Logic.XML.Interfaces;

namespace Common.Logic.XML.SimpleLogic
{
    public static class CheckMethods
    {
        public static bool CheckOr(IDictionary<string, string> circs, IEnumerable<ICheckable> checkers)
        {
            return !checkers.Any() || checkers.Any(childChecker => childChecker.Check(circs));
        }

        public static bool CheckAnd(IDictionary<string, string> circs, IEnumerable<ICheckable> checkers)
        {
            return !checkers.Any() || checkers.All(childChecker => childChecker.Check(circs));
        }       
    }
}