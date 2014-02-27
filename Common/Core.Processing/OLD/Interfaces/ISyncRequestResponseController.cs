using OpenBank.SOZ.DataModel.test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Processing.Interfaces
{
    public interface ISyncRequestResponseController<in DataObject, in RequestObject, in ResponseObject, out CtrlContext, ExecContext, in ThreadContext> :
        IController<DataObject, CtrlContext, ExecContext, ThreadContext>
        //where ExecContext : IExecutionContext<CtrlContext, ThreadContext>
        where CtrlContext : class
    {

    }
}