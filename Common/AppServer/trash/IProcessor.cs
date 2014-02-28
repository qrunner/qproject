namespace AppServer
{
    /// <summary>
    /// Обработчик операций
    /// </summary>
    /// <typeparam name="T">Тип обрабатываемого объекта</typeparam>
    public interface IProcessor<in T>
    {
        OperationResult Process(T item);
    }
}