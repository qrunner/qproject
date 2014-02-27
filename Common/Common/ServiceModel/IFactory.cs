namespace Common.ServiceModel
{
    /// <summary>
    /// Предоставляет базовый интерфейс фабрики
    /// </summary>
    /// <typeparam name="T">Тип создаваемого объекта</typeparam>
    public interface IFactory<out T>
    {
        T Create();
    }
}