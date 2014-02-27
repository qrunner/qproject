namespace Common.DPL
{
    interface IThreadProcessor<in T>
    {
        void Initialize();
        void Process(T item);
    }
}