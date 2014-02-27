using System;
using System.Diagnostics;

namespace Provider.Logging
{
    /// <summary>
    /// Запись журнала (событие)
    /// </summary>
    public struct LogEntry : IEntry
    {
        private SourceInfo source;
        /// <summary>
        /// Информация об источнике события
        /// </summary>
        public SourceInfo Source { get { return source; } }
        /// <summary>
        /// Текстовое описание события
        /// </summary>
        public string Msg;
        /// <summary>
        /// Уровень (тип) события
        /// </summary>
        public LogLevel Level;
        private DateTime timestamp;
        /// <summary>
        /// Время возникновения события
        /// </summary>
        public DateTime Timestamp { get { return timestamp; } }
        /// <summary>
        /// Идентификатор процесса
        /// </summary>
        public int process_id;
        /// <summary>
        /// Идентификатор потока
        /// </summary>
        public int thread_id;
        /// <summary>
        /// Код обрабатываемого объекта
        /// </summary>
        public long object_code;
        /// <summary>
        /// Дополнительная информация об объекте
        /// </summary>
        public string additional;
        /// <summary>
        /// Стек вызовов
        /// </summary>
        public string call_stack;
        /// <summary>
        /// Стек исключений
        /// </summary>
        public string ex_stack;
        /// <summary>
        /// Исключение
        /// </summary>
        private Exception ex;
        /// <summary>
        /// Исключение (в случае ошибки)
        /// </summary>
        public Exception Exception
        {
            get { return ex; }
            set
            {
                ex = value;                
                this.call_stack = ex.StackTrace;
                this.ex_stack = ExceptionStack(ex);
            }
        }

        public LogEntry(LogLevel lvl, string message)
        {
            timestamp = DateTime.Now;
            source = new SourceInfo(null);
            Msg = message;
            Level = lvl;
            process_id = System.Diagnostics.Process.GetCurrentProcess().Id;
            thread_id = System.Threading.Thread.CurrentThread.ManagedThreadId;
            object_code = 0;
            additional = null;
            call_stack = null;
            ex_stack = null;
            ex = null;
        }

        public LogEntry(LogLevel lvl, string message, string poolName, long code)
        {
            timestamp = DateTime.Now;
            source = new SourceInfo(poolName);
            Msg = message;
            Level = lvl;
            process_id = System.Diagnostics.Process.GetCurrentProcess().Id;
            thread_id = System.Threading.Thread.CurrentThread.ManagedThreadId;
            object_code = code;
            additional = null;
            call_stack = null;
            ex_stack = null;
            ex = null;
        }
        
        static string ExceptionStack(Exception ex)
        {
            string retval = ex.Message;
            while (ex.InnerException != null)
            {
                retval += ("\r\n=> " + ex.InnerException.Message);
                ex = ex.InnerException;
            }
            return retval;
        }
        /// <summary>
        /// Возвращает стек вызовов
        /// </summary>
        /// <param name="skipFrames">Пропуск фреймов с начала стека</param>
        /// <returns>Стек вызова</returns>
        static string CallStack(int skipFrames)
        {
            //string retval = "";
            StackTrace stack = new StackTrace(skipFrames);
            return stack.ToString();
            /*foreach (StackFrame frame in stack.GetFrames())
                retval += (frame.GetMethod() + "\r\n");
            return retval;*/
        }

        static string CallStack(Exception ex)
        {
            //string retval = "";
            StackTrace stack = new StackTrace(ex);
            return stack.ToString();            
            /*foreach (StackFrame frame in stack.GetFrames())
                retval += (frame.GetMethod() + "\r\n");
            return retval;*/
        }
    }
}