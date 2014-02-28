namespace AppServer.Core
{
    /// <summary>
    /// Приемник объектов (запросов)
    /// </summary>
    /// <typeparam name="T">Тип объекта запроса</typeparam>
    public interface IAcceptor<in T>
    {
        /// <summary>
        /// Добавляет объект в обработку
        /// </summary>
        /// <param name="item"></param>
        void PutObject(T item);

        /// <summary>
        /// Блокирует вызывающий поток, если приемник не пуст.
        /// </summary>
        /// <param name="timeout">Таймаут блокировки. Для бесконечного ожидания можно передать Timeout.Infinity</param>
        void BlockWhileEmpty(int timeout);
    }
}