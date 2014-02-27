namespace Common.ServiceModel
{
    /// <summary>
    /// Предоставляет интерфейс запускаемой службы, поддерживающей инициализацию
    /// </summary>
    public interface IInitableService : IService
    {
        /// <summary>
        /// Инициализация службы
        /// </summary>
        void Initialize();
    }
}
