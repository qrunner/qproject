using System;

namespace Common.Scheduling
{
    /// <summary>
    /// Планировщик
    /// </summary>
    public interface IScheduler
    {
        /// <summary>
        /// Назначает выполнение задачи с заданным расписанием
        /// </summary>
        /// <param name="jobName">Имя задачи</param>
        /// <param name="schedule">Расписание запуска</param>
        /// <param name="action">Задача</param>
        void ScheduleJob(string jobName, ISchedule schedule, Action action);

        /// <summary>
        /// Останавливает назначенное задание и освобождает ресурсы
        /// </summary>
        void Stop(bool waitToComplete);
    }
}