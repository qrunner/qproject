using System;
using Common.ServiceModel;

namespace Common.DPL.ThreadParallel
{
    public class OneActionFactory : IFactory<Action>
    {
        private readonly Action _action;

        public OneActionFactory(Action action)
        {
            _action = action;
        }

        public Action Create()
        {
            return _action;
        }
    }
}