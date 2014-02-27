namespace Common.DPL
{
    interface IThreadListenerFactory
    {
        IThreadListener<T> CreateListener<T>();
    }
}