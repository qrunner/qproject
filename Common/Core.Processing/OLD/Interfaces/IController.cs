using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBank.SOZ.DataModel.test
{
    public interface IController<in DataObject, out CtrlContext, ExecContext, in ThreadContext>
    //where ExecContext : IExecutionContext<CtrlContext, ThreadContext>
    {
        ExecContext CreateExecContext(DataObject obj, ThreadContext threadContext);
        // CtrlContext Context { get; }
        void Execute(DataObject obj, ExecContext execContext);
    }

    public interface IControllerNew<in ExecContext>
    {
        void Execute(ExecContext execContext);
    }
}