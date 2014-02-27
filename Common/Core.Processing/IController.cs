namespace Core.Processing
{
    public interface IController<in TContext>
    {
        void Execute(TContext context);
    }
}