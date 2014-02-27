namespace Common.Net.Balancing
{
    /// <summary>
    /// Предоствляет методы обновляемого объекта
    /// </summary>
    public interface IRefreshable
    {
        /// <summary>
        /// Обновить состояние
        /// </summary>
        void Refresh();

        /// <summary>
        /// Период обновления состояния, если AutoRefresh = True
        /// </summary>
        int RefreshTimeout { get; set; }

        /// <summary>
        /// Автоматическое обновление статуса с периодом, заданном в RefreshTimeout
        /// </summary>
        bool AutoRefresh { get; set; }
    }
}
