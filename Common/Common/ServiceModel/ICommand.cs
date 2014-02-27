namespace Common.ServiceModel
{
    /// <summary>
    /// Базовый интерфейс команды
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Выполняет команду
        /// </summary>
        void Excecute();
    }
}