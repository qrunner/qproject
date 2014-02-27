using System;
using System.Collections.Generic;

namespace Common.ServiceModel.Tasks
{
    /// <summary>
    /// Длительная задача
    /// </summary>
    /// <typeparam name="TContext">Тип контекста выполнения</typeparam>
    public interface ITask<in TContext>
    {
        /// <summary>
        /// Возникает при изменении ходы выполнения
        /// </summary>
        event EventHandler<ProcessEventArgs> OnProcess;

        /// <summary>
        /// Выполняет задачу с указанным контекстом
        /// </summary>
        /// <param name="context">Контекст выполнения</param>
        void Execute(TContext context);

        /// <summary>
        /// Параметры задачи
        /// </summary>
        IDictionary<string, object> Parameters { get; set; }
    }
}