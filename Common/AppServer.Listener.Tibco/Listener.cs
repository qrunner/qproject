using AppServer.Core;
using Common.DPL.ThreadParallel;
using Common.ServiceModel;
using System;

namespace AppServer.Front.Tibco
{
    public class Listener<T> : ListenerBase<T>, IFactory<Action>
    {
        private readonly ActionRunner _runner;

        public Listener()
        {
            _runner = new ActionRunner(this, -1);
        }

        public override void Start(int threads)
        {
            _runner.Start(threads);
        }

        public override void Start()
        {
            _runner.Start();
        }

        public override void Stop()
        {
            _runner.Stop();
        }

        public override int ThreadsCount
        {
            get { return _runner.ThreadsCount; }
            set { _runner.ThreadsCount = value; }
        }

        public Action Create()
        {
            // todo create MessageService

            return () => { };
        }
    }
}