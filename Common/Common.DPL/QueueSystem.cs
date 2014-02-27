using System;

namespace Common.DPL
{
    public class QueueSystem<T> where T : IEquatable<T>
    {
        private readonly IParallelRunner _listener;
        private readonly IParallelRunner _processor;

        public QueueSystem(IParallelRunner listener, IParallelRunner processor)
        {
            _listener = listener;
            _processor = processor;
        }

        public void Start()
        {
            _listener.Start();
            _processor.Start();
        }

        public void Stop()
        {
            _listener.Stop();
            _processor.Stop();
        }

        public int ListenerParallelizm
        {
            get { return _listener.Parallelizm; }
            set { _listener.Parallelizm = value; }
        }

        public int ProcessorParallelizm
        {
            get { return _processor.Parallelizm; }
            set { _processor.Parallelizm = value; }
        }
    }
}