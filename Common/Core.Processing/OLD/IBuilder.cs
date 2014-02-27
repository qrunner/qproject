using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Processing
{
    /// <summary>
    /// Обеспечивает создание нового объекта на основе имеющегося
    /// </summary>
    /// <typeparam name="Source">Тип исходного (существующего) объекта</typeparam>
    /// <typeparam name="Dest">Тип создаваемого объекта</typeparam>
    /// <typeparam name="ExecContext">Контекст выполнения</typeparam>
    public interface IBuilder<in Source, out Dest, in ExecContext>
    {
        Dest Transform(Source obj, ExecContext execContext);
    }
}