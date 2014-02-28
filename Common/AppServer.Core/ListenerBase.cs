namespace AppServer.Core
{
    /// <summary>
    /// Базовый класс загрузчика
    /// </summary>
    /// <typeparam name="T">Тип загружаемого объекта</typeparam>
    public abstract class ListenerBase<T> : IListener<T>
    {
        protected IAcceptor<T> Acceptor;

        public void SetAcceptor(IAcceptor<T> acceptor)
        {
            Acceptor = acceptor;
        }

        public abstract void Start(int threads);

        public abstract void Start();

        public abstract void Stop();

        public abstract int ThreadsCount { get; set; }
    }
}