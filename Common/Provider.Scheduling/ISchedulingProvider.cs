using System;
using System.Collections.Generic;

namespace Common.Scheduling
{
    public interface ISchedulingProvider
    {
        /// <summary>
        /// Создает планировщик
        /// </summary>
        /// <returns></returns>
        IScheduler CreateScheduler();

        /// <summary>
        /// Назначает выполнение задачи с заданным расписанием и возвращает планировщик
        /// </summary>
        /// <param name="jobName">Имя задачи</param>
        /// <param name="schedule">Расписание запуска</param>
        /// <param name="action">Задача</param>
        IScheduler ScheduleJob(string jobName, ISchedule schedule, Action action);

        /// <summary>
        /// Создает экземпляр расписания
        /// </summary>
        /// <param name="scheduleType">Тип расписания</param>
        /// <param name="parameters">Параметры расписания</param>
        /// <returns>Экземпляр расписания</returns>
        ISchedule CreateSchedule(ScheduleType scheduleType, string name, IDictionary<string, object> parameters);
    }
}