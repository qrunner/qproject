using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBank.SOZ.DataModel.test
{
    public interface IExecutionContext<out CtrlContext, out ThrdContext>
    {
        CtrlContext ControllerContext { get; }
        ThrdContext ThreadContext { get; }
    }
}