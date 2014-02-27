using System.Xml.Linq;

namespace Common.Logic.XML.Interfaces
{
    /// <summary>
    /// Интерфейс, отвечающий за сериализацию/десериализацию
    /// </summary>
    public interface ISerializable
    {
        XElement Serialize();

        void DeSerialize(XElement xElem);
    }
}