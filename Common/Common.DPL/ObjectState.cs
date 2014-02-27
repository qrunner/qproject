namespace Common.DPL
{
    /// <summary>
    /// Состояние объекта
    /// </summary>
    public enum ObjectState
    {
        /// <summary>
        /// В обработке
        /// </summary>
        InProcess,
        /// <summary>
        /// В очереди на обработку
        /// </summary>
        InQueue,
        /// <summary>
        /// Обработка завершена
        /// </summary>
        Commited,
        /// <summary>
        /// Откат в начало очереди
        /// </summary>
        Rollbacked,
        /// <summary>
        /// Обработка отложена
        /// </summary>
        Delayed,
        /// <summary>
        /// Обработка отменена
        /// </summary>
        Cancelled
    }
}