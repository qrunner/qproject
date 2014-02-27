using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Common.Logic.XML.Interfaces
{
    public interface ICheckable : ISerializable
    {
       bool Check(IDictionary<string, string> circs);

       IEnumerable<ICheckable> ChildCheckers { get; }

       string ItemName { get; }

    }
}