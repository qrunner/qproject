using Common.ServiceModel;

namespace AppServer.Core
{
    /// <summary>
    /// Пул обработки
    /// </summary>
    /// <typeparam name="T">Тип контекста</typeparam>
    public interface IProcessingPool<in T> : IAcceptor<T>, IService
    {
        /*
        /// <summary>
        /// Устанавливает контроллер обработки
        /// </summary>
        /// <param name="controller"></param>
        void SetController(IController<T> controller);
        */
        /// <summary>
        /// Количество потоков
        /// </summary>
        int ThreadsCount { get; set; }
    }
}