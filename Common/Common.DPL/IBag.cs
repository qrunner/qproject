using System;

namespace Common.DPL
{
    /// <summary>
    /// Transport for object and processing management entry point
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBag<T> : IEquatable<IBag<T>>
    {
        /// <summary>
        /// Обрабатываемый объект
        /// </summary>
        T Object { get; }

        /// <summary>
        /// Текущее состояние объекта
        /// </summary>
        ObjectState State { get; set; }

        /// <summary>
        /// Отменяет обработку заявки
        /// </summary>
        void Cancel();

        /// <summary>
        /// Фиксирует обработку заявки
        /// </summary>
        void Commit();

        /// <summary>
        /// Откатывает объект в начало очереди
        /// </summary>
        /// <param name="timeout"></param>
        void Rollback(uint timeout);

        /// <summary>
        /// Таймаут повторного посмещения объекта в очередь
        /// </summary>
        uint RollbackTimeout { get; set; }

        /// <summary>
        /// Количество откатов
        /// </summary>
        uint RollbackedTimes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool IsFree { get; }
    }
}