using System;

namespace Common.ContextProcessing
{
    /// <summary>
    /// Обработчик исключения контекста
    /// </summary>
    /// <typeparam name="TExecContext">Контекст выполнения</typeparam>
    public interface IExceptionHandler<in TExecContext>
    {
        /// <summary>
        /// Обрабатывает исключение в контексте
        /// </summary>
        /// <param name="ex">Исключение</param>
        /// <param name="execContext">Контекст выполнения</param>
        void Handle(Exception ex, TExecContext execContext);
    }
}