using System;
using System.Collections.Generic;
using System.Text;
using Provider.Logging.BatchWrite;

namespace Provider.Logging.Console
{
    public class Writer : BatchWriteProvider
    {
        private string _delimeter = "|";

        protected override void Initialize(IDictionary<string, string> settings)
        {
            base.Initialize(settings);

            if (settings.ContainsKey("delimeter") && !string.IsNullOrWhiteSpace(settings["delimeter"]))
                _delimeter = settings["delimeter"];
        }

        public override bool CheckConnection(out Exception ex)
        {
            ex = null;
            return true;
        }

        protected override void WriteBatch(IEnumerable<IEntry> records)
        {
            foreach (LogEntry entry in records)
                System.Console.WriteLine(ToString(entry));
        }

        string ToString(LogEntry entry)
        {
            var sb = new StringBuilder();
            sb.Append(entry.Timestamp.ToString("hh:mm:ss.fff"));
            sb.Append(_delimeter);
            sb.Append(entry.Level);
            sb.Append(_delimeter);
            sb.Append(entry.Msg.Replace('\n', ';'));
            if (entry.Exception == null) return sb.ToString();
            sb.Append(_delimeter);
            sb.Append(entry.Exception.Message);
            return sb.ToString();
        }
    }
}
