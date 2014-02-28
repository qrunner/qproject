using System;

namespace Common.ContextProcessing
{
    /// <summary>
    /// Исключение, отменяющее обработку текущего контекста
    /// </summary>
    public class CancelException : Exception
    {
        /// <summary>
        /// Создает экземпляр исключения, отменяющего обработку текущего контекста
        /// </summary>
        /// <param name="message">Сообщение о причине отмены</param>
        public CancelException(string message)
            : base(message)
        {

        }
    }
}