namespace Provider.Messaging
{
    /// <summary>
    /// Текстовое сообщение
    /// </summary>
    public interface ITextMessage : IMessage
    {
        /// <summary>
        /// Текстовое содержимое сообщения
        /// </summary>
        string Text { get; set; }
    }
}