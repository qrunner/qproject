using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Provider.Logging.BatchWrite
{
    /// <summary>
    /// Базовый провайдер пакетной записи
    /// </summary>
    public abstract class BatchWriteProvider : Provider
    {
        protected BatchWriteProvider()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Flush();
        }

        protected ConcurrentQueue<IEntry> Queue = new ConcurrentQueue<IEntry>();
        
        private int _flushInterval = 10000;
        /// <summary>
        /// Интервал в миллисекундах для сброса записей в хранилище
        /// </summary>
        public int FlushInterval
        {
            get { return _flushInterval; }
            set
            {
                if (_flushInterval == value) return;

                _flushInterval = value;
                if (_timer != null)
                    _timer.Change(_flushInterval, _flushInterval);
            }
        }
        
        private int _bufferCapacity = 100;
        /// <summary>
        /// Число записей в очереди, по достижению которого производится сброс в хранилище
        /// </summary>
        public int BufferCapacity
        {
            get { return _bufferCapacity; }
            set { _bufferCapacity = value; }
        }

        Timer _timer = null;

        protected override void Initialize(IDictionary<string, string> settings)
        {
            CheckConfigParametersExists(settings, "flushInterval", "flushCount");

            InitSourceData();

            _flushInterval = int.Parse(settings["flushInterval"]);
            _bufferCapacity = int.Parse(settings["flushCount"]);

            _timer = new Timer(FlushAsync, null, _flushInterval, _flushInterval);
        }

        public virtual void AddEntry(IEntry entry)
        {            
            Queue.Enqueue(entry);
            if (Queue.Count >= _bufferCapacity)
                FlushAsync();
        }
        /// <summary>
        /// Производит массовый сброс записей в хранилище
        /// </summary>
        /// <param name="records"></param>
        protected virtual void WriteBatch(IEnumerable<IEntry> records)
        {

        }
        /// <summary>
        /// Инициализирует данные источника сообщений
        /// </summary>
        protected virtual void InitSourceData()
        {

        }
        /// <summary>
        /// Производит действия по смене хренилища
        /// </summary>
        protected virtual void Swap()
        {

        }

        private void FlushAsync(object cntx)
        {
            FlushAsync();
        }

        public void FlushAsync()
        {
            Task.Factory.StartNew(Flush);
        }

        public void Flush()
        {
            int count = Queue.Count;

            List<IEntry> flushRecords = new List<IEntry>();

            for (int i = 0; i < count; i++)
            {
                IEntry item = null;
                if (Queue.TryDequeue(out item))
                {
                    flushRecords.Add(item);
                }
            }

            try
            {
                lock (Queue) // синхронизируем сброс (? нужно не всегда)
                {
                    WriteBatch(flushRecords);
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError(this, new WriteBatchExceptionEventArgs(flushRecords, new Exception(string.Format("Ошибка при записи в лог. Провайдер: <{0}>. Подробности во вложенном исключении.", this.GetType()), ex)));
            }
        }

        public event EventHandler<WriteBatchExceptionEventArgs> OnError;
    }

    public class WriteBatchExceptionEventArgs : EventArgs
    {
        public WriteBatchExceptionEventArgs(IEnumerable<IEntry> records, Exception ex)
        {
            Ex = ex;
            FlushRecords = records;
        }
        public Exception Ex;
        public IEnumerable<IEntry> FlushRecords;
    }
}