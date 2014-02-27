using System;

namespace Common.Net.Balancing
{
    /// <summary>
    /// Статус обработки запроса
    /// </summary>
    [Serializable]
    public class ResponseStatus
    {
        public ResponseStatus()
        {
        }

        public ResponseStatus(Status status)
        {
            Status = status;
        }

        public ResponseStatus(string errorMessage)
        {
            Status = Status.Error;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}