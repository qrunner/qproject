namespace Common.DPL
{
    /// <summary>
    /// Представляет собой задачу, выполняемую циклически в нескольких потоках
    /// </summary>
    public interface IParallelRunner : IRunner
    {
        /// <summary>
        /// Устанавливает количество потоков, а которых выполняется задача
        /// </summary>
        int ThreadsCount { get; set; }

        /// <summary>
        /// Запускает задачу на выполнение в заданном количестве потоков
        /// </summary>
        void Start(int threadsCount);
    }
}