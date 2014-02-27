using System;
namespace Provider.Messaging
{
    /// <summary>
    /// Компонент, работающий со службой сообщений
    /// </summary>
    public interface IMessageService : IDisposable
    {
        /// <summary>
        /// Установка соединения
        /// </summary>
        void Connect();
        /// <summary>
        /// Разрыв соединения
        /// </summary>
        void Disconnect();
        /// <summary>
        /// Является ли подключение открытым
        /// </summary>
        bool Connected { get; }
        /// <summary>
        /// Получает читателя сообщений
        /// </summary>
        /// <returns></returns>
        IMessageReader CreateReader(string queueName);
        /// <summary>
        /// Получает отправщика сообщений
        /// </summary>
        /// <returns></returns>
        IMessageSender CreateSender(string queueName);
    }
}
