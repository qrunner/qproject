using OpenBank.SOZ.DataModel.test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Processing.Controllers
{
    public abstract class ControllerBase<T, CtrlContext, ExecContext, ThrdContext> :
        IController<T, CtrlContext, ExecContext, ThrdContext>
        //where ExecContext : IExecutionContext<CtrlContext, ThrdContext>
        where CtrlContext : class
    {
        protected CtrlContext context = null;
        protected IExceptionHandler<ExecContext> exceptionHandler = null;
        protected ILogger<ExecContext> logger = null;

        public ControllerBase(CtrlContext context,
            IExceptionHandler<ExecContext> _exceptionHandler,
            ILogger<ExecContext> _logger)
        {
            this.context = context;
            exceptionHandler = _exceptionHandler;
            logger = _logger;
        }

        public abstract ExecContext CreateExecContext(T obj, ThrdContext threadContext);

        /*public CtrlContext Context
        {
            get { return context; }
        }*/

        public void Execute(T obj, ExecContext execContext)
        {
            try
            {
                ExecuteInternal(obj, execContext);

                if (logger != null)
                    logger.Write(execContext);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError(ex, execContext);

                if (exceptionHandler != null)
                    exceptionHandler.Handle(ex, execContext);
                else throw ex;
            }
        }

        public abstract void ExecuteInternal(T obj, ExecContext execContext);
    }
}