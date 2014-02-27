using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Provider.Logging
{
    /// <summary>
    /// Уровень сообщения
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Информация
        /// </summary>
        Info = 0,
        /// <summary>
        /// Ошибка
        /// </summary>
        Error = 1,
        /// <summary>
        /// Предупреждение
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Отладка
        /// </summary>
        Debug = 3,
        /// <summary>
        /// Критическая ошибка (продолжение работы невозможно)
        /// </summary>
        Fatal = 4
    }
}
