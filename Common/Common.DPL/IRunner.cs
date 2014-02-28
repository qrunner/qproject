namespace Common.DPL
{
    /// <summary>
    /// Представляет собой задачу, выполняемую циклически
    /// </summary>
    public interface IRunner
    {
        /// <summary>
        /// Запускает задачу на выполнение
        /// </summary>
        void Start();

        /// <summary>
        /// Останавливает выполнение задачи
        /// </summary>
        void Stop();
    }
}