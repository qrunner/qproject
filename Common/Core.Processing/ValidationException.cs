using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Processing
{
    public class ValidationException : Exception
    {
        public ValidationException(string message, IEnumerable<string> validationMessages) :
            base(message)
        {
            StringBuilder sb = new StringBuilder(message);
            sb.AppendLine("Ошибки валидации:"); //  ?
            foreach (string s in validationMessages)
                sb.AppendLine(s);
        }
    }
}