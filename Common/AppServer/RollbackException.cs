using System;

namespace Common.ContextProcessing
{
    /// <summary>
    /// Исключение, возвращающее контекст в начало очереди
    /// </summary>
    public class RollbackException : Exception
    {
        /// <summary>
        /// Таймаут перед постановкой в очередь
        /// </summary>
        public uint Timeout { get; set; }

        /// <summary>
        /// Создает экземпляр исключения, возвращающего контекст в начало очереди
        /// </summary>
        /// <param name="message">Поясняющее сообщение</param>
        /// <param name="timeout">Таймаут перед постановкой в очередь</param>
        public RollbackException(string message, uint timeout)
            : base(message)
        {
            Timeout = timeout;
        }
    }
}
