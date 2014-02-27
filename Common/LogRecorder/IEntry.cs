using System;

namespace Provider.Logging
{
    public interface IEntry
    {
        /// <summary>
        /// Статическая информация о месте возникновения
        /// </summary>
        SourceInfo Source { get; }
        /// <summary>
        /// Дата и время события
        /// </summary>
        DateTime Timestamp { get; }
    }
}