namespace AppServer.Core
{
    /// <summary>
    /// Загрузчик запросов в IAcceptor
    /// </summary>
    /// <typeparam name="T">Тип объекта запроса</typeparam>
    public interface IListener<out T>
    {
        /// <summary>
        /// Устанавливает приемник
        /// </summary>
        /// <param name="acceptor">Приемник объектов</param>
        void SetAcceptor(IAcceptor<T> acceptor);

        /// <summary>
        /// Запускает загрузчик с заданным количеством потоков
        /// </summary>
        /// <param name="threads">Количество потоков</param>
        void Start(int threads);

        /// <summary>
        /// Запускает загрузчик
        /// </summary>
        void Start();

        /// <summary>
        /// Останавливает загрузчик
        /// </summary>
        void Stop();

        /// <summary>
        /// Количество потоков
        /// </summary>
        int ThreadsCount { get; set; }
    }
}