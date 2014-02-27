using System;

namespace Common.DPL.ThreadParallel
{
    public interface IActionFactory
    {
        Action CreateAction();
    }
}