namespace Common.Scheduling
{
    /// <summary>
    /// Тип расписания
    /// </summary>
    public enum ScheduleType
    {
        /// <summary>
        /// Запускается с заданным периодом заданное количество раз
        /// </summary>
        Periodic,

        /// <summary>
        /// Запуск по cron-expression
        /// </summary>
        CronBased
    }
}