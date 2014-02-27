namespace Common.ServiceModel
{
    /// <summary>
    /// Предоставляет интерфейс запускаемой службы
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Запускает службу 
        /// </summary>
        void Start();
        /// <summary>
        /// Останавливает службу
        /// </summary>
        void Stop();
        /// <summary>
        /// Задает или возвращает имя службы
        /// </summary>
        string Name { get; set; }
    }
}