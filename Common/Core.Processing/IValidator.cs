using System.Collections.Generic;

namespace Core.Processing
{
    /// <summary>
    /// Обеспечивает валидацию объекта
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    public interface IValidator<in T>
    {        
        bool Validate(T obj, ref IList<string> messages);
    }
}