using System;

namespace AppServer
{
    /// <summary>
    /// Результат выполнения операции
    /// </summary>
    public class OperationResult
    {
        private OperationResult(OperationResultType resultType, Exception ex)
        {
            ResultType = resultType;
            Exception = ex;
        }

        public OperationResult() : this(OperationResultType.Success, null)
        {

        }

        public OperationResult(Exception ex) : this(OperationResultType.Error, ex)
        {

        }

        public OperationResultType ResultType { get; private set; }

        public Exception Exception { get; private set; }
    }
}