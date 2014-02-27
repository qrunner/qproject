namespace Common.DPL
{
    interface IThreadProcessorFactory
    {
        IThreadProcessor<T> CreateProcessor<T>();
    }
}