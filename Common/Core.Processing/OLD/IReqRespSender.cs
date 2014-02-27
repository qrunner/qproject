using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Processing
{
    /// <summary>
    /// Обеспечивает синхронную отправку запроса и получение ответа
    /// </summary>
    /// <typeparam name="Request">Тип объекта запроса</typeparam>
    /// <typeparam name="Response">Тип объекта ответа</typeparam>
    public interface IReqRespSender<in Request, out Response, in ExecContext>
    {
        Response SendRequest(Request obj, ExecContext executionContext);
    }
}
