using System.Collections.Generic;

namespace Common.DPL
{
    public interface IThreadListener<out T>
    {
        IEnumerable<T> GetObjects();
    }
}