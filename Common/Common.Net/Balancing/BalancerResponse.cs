using System;

namespace Common.Net.Balancing
{
    /// <summary>
    /// Информация о конечной точке для подключения
    /// </summary>
    [Serializable]
    public class BalancerResponse
    {
        public BalancerResponse()
        {
            
        }

        public BalancerResponse(string errorMessage)
        {
            ResponseStatus.Status = Status.Error;
            ResponseStatus.ErrorMessage = errorMessage;
        }

        public BalancerResponse(string host, int port)
        {
            ResponseStatus.Status = Status.Success;
            Host = host;
            Port = port;
        }

        private ResponseStatus _responseStatus = new ResponseStatus();

        /// <summary>
        /// Имя или IP-адрес сервера
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Порт
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Статус выполнения запроса 
        /// </summary>
        public ResponseStatus ResponseStatus
        {
            get { return _responseStatus; }
            set { _responseStatus = value; }
        }
    }
}