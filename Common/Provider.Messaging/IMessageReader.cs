using System;
using System.Collections.Generic;

namespace Provider.Messaging
{
    /// <summary>
    /// Компонент для чтения сообщений
    /// </summary>
    public interface IMessageReader : IDisposable
    {
        /// <summary>
        /// Читает первое сообщение из очереди
        /// </summary>
        /// <returns>Найденное сообщение, либо null - в случае отсутствия сообщений в очереди.</returns>
        IMessage Read();
        /// <summary>
        /// Получает список всех сообщений в очереди
        /// </summary>
        /// <returns>Список сообщений</returns>
        IEnumerable<IMessage> ReadAll();

        //event EventHandler<MessageRecievedEventArgs> OnMessageRecieved;
    }
}