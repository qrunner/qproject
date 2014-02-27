namespace Common.Scheduling
{
    /// <summary>
    /// Представляет собой расписание
    /// </summary>
    public interface ISchedule
    {
        /// <summary>
        /// Имя расписания
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Тип расписания
        /// </summary>
        ScheduleType ScheduleType { get; }
        /*
        /// <summary>
        /// Параметры расписания
        /// </summary>
        IDictionary<string, object> Parameters { get; }*/
    }
}