using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Processing
{
    /// <summary>
    /// Обеспечивает загрузку объекта
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILoader<out T, in ExecContext>
    {
        T Load(object loadInfo, ExecContext execContext);
    }
}