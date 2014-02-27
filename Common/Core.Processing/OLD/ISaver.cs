using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Processing
{
    /// <summary>
    /// Обеспечивает сохранение объекта
    /// </summary>
    /// <typeparam name="T">Тип сохраняемого объекта</typeparam>
    public interface ISaver<in T, in ExecContext>        
    {
        void Save(T obj, ExecContext executionContext);
        //void Save(T obj, object saveInfo, object controllerContext, object threadContext);
    }
}