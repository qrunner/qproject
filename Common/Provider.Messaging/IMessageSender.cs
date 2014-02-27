using System;

namespace Provider.Messaging
{
    /// <summary>
    /// Компонент, отправляющий сообщения
    /// </summary>
    public interface IMessageSender : IDisposable
    {
        /// <summary>
        /// Инициализирует новое сообщение
        /// </summary>
        /// <returns>Экземпляр конкретного класса сообщения</returns>
        IMessage NewMessage();
        /// <summary>
        /// Отправляет сообщение
        /// </summary>
        /// <param name="message">Сообщение для отправки</param>
        void Send(IMessage message);
        /// <summary>
        /// Отправляет сообщение
        /// </summary>
        /// <param name="message">Сообщение для отправки</param>
        /// <param name="sended">Действие, выполняемое при успешной доставке</param>
        void Send(IMessage message, Action<IMessage> sended);        
    }
}