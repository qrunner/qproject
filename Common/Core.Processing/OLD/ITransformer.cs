using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Processing
{
    /// <summary>
    /// Интерфейс обеспечивает заполнение объекта данными из исходного
    /// </summary>
    /// <typeparam name="Source">Тип исходного объекта</typeparam>
    /// <typeparam name="Dest">Тип заполняемого объекта</typeparam>
    /// <typeparam name="ExecContext">Контекст выполнения</typeparam>
    public interface ITransformer<in Source, in Dest, in ExecContext>
    {
        void Transform(Source obj, Dest destObj, ExecContext execContext);
    }
}