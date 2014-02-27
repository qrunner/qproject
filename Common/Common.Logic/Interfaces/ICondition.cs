namespace Common.Logic.XML.Interfaces
{
    public interface ICondition :  IChckBase
    {
        string Name { get; set; }

        bool DefaultValue { get; set; }
    }
}