using Provider.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics
{
    /// <summary>
    /// Текущий статус пула
    /// </summary>
    public enum PoolStatus { Working, Idle, Stopped }
    /// <summary>
    /// Статистика работы пула
    /// </summary>
    public struct StatRecord : IEntry
    {
        /// <summary>
        /// Создает новую запись
        /// </summary>
        /// <param name="poolId">Идентификатор пула</param>        
        /// <param name="poolName">Имя пула</param>
        /// <param name="threadsCount">Количество потоков</param>
        /// <param name="inProcessCount">Количество объектов в обработке</param>
        /// <param name="inQueueCount">Количество объектов в очереди на обработку</param>
        /// <param name="status">Текущий статус пула</param>
        public StatRecord(string poolName, int threadsCount, int inProcessCount, int inQueueCount, PoolStatus status)
        {
            source = new SourceInfo(poolName);
            timestamp = DateTime.Now;
            
            ThreadsCount = threadsCount;
            InProcessCount = inProcessCount;
            InQueueCount = inQueueCount;
            Status = status;

            ProcessedCount = 0;
            ErrorsCount = 0;
            ProcessTime = 0;
            ForeignTime = 0;

            PickedCount = 0;
            PickErrors = 0;
            EnqueuedCount = 0;
            PickTime = 0;
            PicksCount = 0;
        }

        public bool IsEmptyProcessing
        {
            get
            {
                return
                    PickErrors == 0 &&
                    EnqueuedCount == 0 &&
                    InQueueCount == 0 &&
                    InProcessCount == 0 &&
                    ProcessedCount == 0 &&
                    ErrorsCount == 0;
            }
        }

        private DateTime timestamp;
        /// <summary>
        /// Дата и время записи
        /// </summary>
        public DateTime Timestamp { get { return timestamp; } }
        // ============= ОБЩИЕ ДАННЫЕ ПУЛА ============= //
        private SourceInfo source;
        /// <summary>
        /// Информация об источнике
        /// </summary>
        public SourceInfo Source { get { return source; } }
        /*/// <summary>
        /// Имя пула
        /// </summary>
        public string PoolName;
        /// <summary>
        /// Идентификатор пула
        /// </summary>
        public int PoolId;
        /// <summary>
        /// Идентификатор сервера
        /// </summary>
        public int ServerId;*/
        /// <summary>
        /// Количество потоков (Тек)
        /// </summary>
        public int ThreadsCount;
        /// <summary>
        /// Количество объектов в обработке (Тек)
        /// </summary>
        public int InProcessCount;
        /// <summary>
        /// Количество объектов в очереди (Тек)
        /// </summary>
        public int InQueueCount;
        /// <summary>
        /// Текущий статус пула (Тек)
        /// </summary>
        public PoolStatus Status;
        // ============= ДАННЫЕ РАБОЧИХ ПОТОКОВ ============= //
        /// <summary>
        /// Количество обработанных объектов (Сумм)
        /// </summary>
        public int ProcessedCount;
        /// <summary>
        /// Количество ошибок в обработке (Сумм)
        /// </summary>
        public int ErrorsCount;
        /// <summary>
        /// Время обработки одного объекта (миллисекунд) (Сумм)
        /// </summary>
        public int ProcessTime;
        /// <summary>
        /// Время обмена с внешней системой (миллисекунд) (Сумм)
        /// </summary>
        public int ForeignTime;
        // ============= ДАННЫЕ ПОТОКА ПОДГРУЗКИ ============= //
        /// <summary>
        /// Количество собранных объектов (Сумм)
        /// </summary>
        public int PickedCount;
        /// <summary>
        /// Количество ошибок при загрузке (Сумм)
        /// </summary>
        public int PickErrors;
        /// <summary>
        /// Количество поставленных в очередь объектов (Сумм)
        /// </summary>
        public int EnqueuedCount;
        /// <summary>
        /// Время, затраченное на загрузку объектов (миллисекунд) (Сумм)
        /// </summary>
        public int PickTime;
        /// <summary>
        /// Количество сборов (Сумм)
        /// </summary>
        public int PicksCount;
    }
}