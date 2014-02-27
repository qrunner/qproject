using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Processing.Interfaces
{
    public interface IExecContextCreator<out TExecContext, in TObject, in TCtrlContext, in TThreadContext>
    {
         TExecContext CreateContext(TObject obj, TCtrlContext ctrlContext, TThreadContext threadContext);
    }
}