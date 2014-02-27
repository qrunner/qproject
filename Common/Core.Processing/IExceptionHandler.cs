using System;

namespace Core.Processing
{
    public interface IExceptionHandler<in TExecContext>
    {
        void Handle(Exception ex, TExecContext execContext);
    }
}