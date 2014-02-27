using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Provider.Logging.BatchWrite;

namespace Provider.Logging.File
{
    public class Writer : BatchWriteProvider
    {
        const string DefaultExt = ".log";
        string _delimeter = "|";
        string _fileName = string.Empty;
        readonly ReaderWriterLockSlim _rwlock = new ReaderWriterLockSlim();

        protected override void Initialize(IDictionary<string, string> settings)
        {
            base.Initialize(settings);

            if (settings.ContainsKey("delimeter") && !string.IsNullOrWhiteSpace(settings["delimeter"]))
                _delimeter = settings["delimeter"];

            if (ConnectionString.Contains("~"))
                _fileName = ConnectionString.Replace("~", AppDomain.CurrentDomain.BaseDirectory);

            Exception ex = null;
            if (CheckConnection(out ex)) return;

            _fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Process.GetCurrentProcess().ProcessName + DefaultExt);
            Exception ex2 = null;
            if (CheckConnection(out ex2))
                throw new Exception(string.Format("Ошибка инициализации провайдера <{0}>. Используем файл по умолчанию '{1}'", GetType().Name, _fileName), ex);

            throw ex;
        }

        public override bool CheckConnection(out Exception ex)
        {
            ex = null;
            try
            {
                string dir = Path.GetDirectoryName(_fileName);

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                _rwlock.EnterWriteLock();
                try
                {
                    using (FileStream fs = System.IO.File.OpenWrite(_fileName))
                    {
                        fs.Flush(true);
                        fs.Close();
                    }
                }
                finally { _rwlock.ExitWriteLock(); }

                return true;
            }
            catch (Exception expt)
            {
                ex = expt;
            }
            return false;
        }

        protected override void WriteBatch(IEnumerable<IEntry> records)
        {
            _rwlock.EnterWriteLock();
            try
            {
                using (StreamWriter fs = System.IO.File.AppendText(_fileName))
                {
                    foreach (LogEntry entry in records)
                        fs.WriteLine(ToString(entry));
                    fs.Flush();
                    fs.Close();
                }
            }
            finally { _rwlock.ExitWriteLock(); }
        }

        string ToString(LogEntry entry)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(entry.Timestamp.ToString("yyyy.MM.dd hh:mm:ss.fff"));
            sb.Append(_delimeter);
            sb.Append(entry.Level);
            sb.Append(_delimeter);
            sb.Append(entry.Msg.Replace('\n', ';'));
            sb.Append(_delimeter);
            sb.Append(entry.process_id);
            sb.Append(_delimeter);

            sb.Append(_delimeter);
            sb.Append(entry.thread_id);
            sb.Append(_delimeter);

            sb.Append(_delimeter);
            sb.Append(entry.object_code);
            sb.Append(_delimeter);
            sb.Append(entry.additional);
            sb.Append(_delimeter);
            sb.Append(entry.call_stack);
            sb.Append(_delimeter);
            sb.Append(entry.ex_stack);
            sb.Append(_delimeter);
            return sb.ToString();
        }
    }
}
