namespace Common.DPL
{
    public interface IParallelRunner : IRunner
    {
        int Parallelizm { get; set; }
    }
}