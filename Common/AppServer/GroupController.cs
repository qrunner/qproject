using System.Collections.Generic;

namespace Common.ContextProcessing
{
    /// <summary>
    /// Представляет собой группу контроллеров, выполняемых последовательно и вызываемых как один контроллер
    /// </summary>
    /// <typeparam name="TContext">Контекст выполнения контроллеров</typeparam>
    public class GroupController<TContext> : IController<TContext>        
    {
        readonly IEnumerable<IController<TContext>> _innerControllers;

        public GroupController(params IController<TContext>[] innerControllers)
        {
            _innerControllers = innerControllers;
        }

        public GroupController(IEnumerable<IController<TContext>> innerControllers)
        {
            _innerControllers = innerControllers;
        }

        public virtual void Execute(TContext context)
        {
            foreach (var controller in _innerControllers)
                controller.Execute(context);
        }
    }
}