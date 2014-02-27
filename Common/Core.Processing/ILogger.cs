using System;

namespace Core.Processing
{
    public interface ILogger<in TExecContext>
    {
        void Write(TExecContext executionContext);
        void WriteError(Exception exeption, TExecContext executionContext);
    }
}