using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Processing
{
    /// <summary>
    /// Обеспечивает отправку запроса
    /// </summary>
    /// <typeparam name="T">Тип объекта запроса</typeparam>
    public interface IRequestSender<in T, in ExecContext>
    {
        void SendRequest(T obj, ExecContext executionContext);
    }
}