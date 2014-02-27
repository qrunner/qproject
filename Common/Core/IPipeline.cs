using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationService
{
    /// <summary>
    /// Потокобезопасный конвейер обработки объектов
    /// </summary>
    public interface IPipeline<T>
    {
        /// <summary>
        /// Подтверждает завершение процесса обработки объекта
        /// </summary>
        /// <param name="order">Объект</param>
        void CommitObjectProcess(T obj);
        /// <summary>
        /// Берет объект из очереди и переводит его в работу
        /// </summary>        
        /// <returns>Первый объект из очереди на обработку</returns>
        bool TryGetObjectForProcess(out T obj);
        /// <summary>
        /// Ставит новый объект на конвейер (в случае отсутствия)
        /// </summary>
        /// <param name="obj">Объект</param>
        bool TryPutObject(T obj);
        /// <summary>
        /// Возвращает объект в начало очереди на обработку
        /// </summary>
        /// <param name="order">Объект</param>
        void RollbackObjectProcess(T obj);
        /// <summary>
        /// Количество объектов в очереди на обработку
        /// </summary>
        int ObjectsToProcessCount { get; }
        /// <summary>
        /// Количество объектов в обработке
        /// </summary>
        int ObjectsInProcessCount { get; }
        /// <summary>
        /// Количество отложенных объектов
        /// </summary>
        int ObjectsDelayedCount { get; }
        /// <summary>
        /// Возвращает объект в начало очереди на обработку через заданный интервал времени
        /// </summary>
        /// <param name="order">Объект</param>
        /// <param name="interval">Интервал времени в миллисекундах</param>
        void DelayObjectProcess(T obj, int interval);
        /// <summary>
        /// Возвращает объект в начало очереди на обработку через заданный интервал времени заданное количество попыток. 
        /// Если объект не обработается за указанное количество попаток, то выполнится finalAction.
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="interval">Интервал между попытками</param>
        /// <param name="times">Количество попыток</param>
        /// <param name="finalAction">Действие при исчерпании попыток</param>
        //void DelayObjectProcess(T obj, int interval, int times, Action<T> finalAction);
        /// <summary>
        /// Объект поставлен в очередь
        /// </summary>
        event EventHandler<ObjectEnquedEventArgs> ObjectEnqueued;
    }
}