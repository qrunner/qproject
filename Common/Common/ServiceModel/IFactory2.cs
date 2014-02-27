namespace Common.ServiceModel
{
    public interface IFactory
    {
        T Create<T>();
    }
}