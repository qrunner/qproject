using System;
using Common.DPL;
using Common.DPL.ThreadParallel;
using Common.ServiceModel;

namespace AppServer.Core
{
    internal class ProcessingPool<TRequest> : IProcessingPool<TRequest>
    {
        private readonly ActionRunner _processor;
        private readonly IPipeline<TRequest> _pipeline;
        public string Name { get; set; }

        public ProcessingPool(IPipeline<TRequest> pipeline, IFactory<Action> actionFactory, int stopTimeout)
        {
            _pipeline = pipeline;
            _processor = new ActionRunner(actionFactory, stopTimeout);
        }

        public void PutObject(TRequest item)
        {
            _pipeline.TryAdd(item);
        }

        public void BlockWhileEmpty(int timeout)
        {
            _pipeline.WaitEmpty(timeout);
        }

        public void Start()
        {
            _processor.Start();
        }

        public void Stop()
        {
            _processor.Stop();
        }

        public int ThreadsCount
        {
            get { return _processor.ThreadsCount; }
            set { _processor.ThreadsCount = value; }
        }
    }
}