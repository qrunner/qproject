namespace Common.ContextProcessing
{
    /// <summary>
    /// Обработчик контекста
    /// </summary>
    /// <typeparam name="TContext">Тип контекста</typeparam>
    public interface IController<in TContext>
    {
        /// <summary>
        /// Выполняет обработку контекста
        /// </summary>
        /// <param name="context">Контекст</param>
        void Execute(TContext context);
    }
}